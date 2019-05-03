using Win32 = Microsoft.Win32;
using System;
using Blesmol.Core;
using System.Collections.Generic;
using System.Linq;

namespace Blesmol.Registry {
	public abstract class SettingsBase : ISettingsLifecycle {
		private readonly SettingsBase Parent;
		private readonly Boolean Writeable;
		private readonly String FullPath;
		private readonly Func<String, Win32.RegistryKey> GetKey;
		private readonly List<ISetting> Settings = new List<ISetting>();

		protected abstract String Path { get; }

		protected void AddSetting<T>(String name, out ISetting setting) {
			setting = Setting<T>.Build<T>(name);
			Settings.Add(setting);
		}

		protected virtual void Load(Win32.RegistryKey key) => Settings.ForEach(setting => setting.Value = key.GetValue(setting.Name));

		protected virtual void Save(Win32.RegistryKey key) => Settings.ForEach(setting => key.SetValue(setting.Name, setting.Value));

		void ISettingsLifecycle.Load() => OpenCloseRegistry(Load);

		void ISettingsLifecycle.Save() => OpenCloseRegistry(Save);

		private void OpenCloseRegistry(Action<Win32.RegistryKey> action) {
			using (Win32.RegistryKey key = GetKey(FullPath)) {
				action(key);

				key.Close();
			}
		}

		public SettingsBase(Boolean writeable) : this(null, writeable) { }

		public SettingsBase(SettingsBase parent) : this(parent, parent.Writeable) { }

		private SettingsBase(SettingsBase parent, Boolean writeable) {
			Parent = parent;
			Writeable = writeable;
			FullPath = BuildFullPath(this);

			if (Writeable) GetKey = Win32.Registry.LocalMachine.CreateSubKey;
			else GetKey = Win32.Registry.LocalMachine.OpenSubKey;

			String BuildFullPath(SettingsBase settings) => settings.Parent == null ? settings.Path : $@"{BuildFullPath(settings.Parent)}\{settings.Path}";
		}

		protected interface ISetting {
			String Name { get; }
			Object Value { get; set; }
		}

		protected abstract class Setting<T> : ISetting {
			private readonly String Name;
			String ISetting.Name => Name;

			private T Value { get; set; }
			Object ISetting.Value {
				get => Value;
				set => Value = Convert(value);
			}

			protected abstract T Convert(Object value);

			protected Setting(String name) => Name = name;

			private delegate ISetting Builder(String name);
			private static Dictionary<Type, Builder> TypeMap = new Dictionary<Type, Builder>() {
				{ typeof(String), name => new StringSetting(name) },
				{ typeof(Int32?), name => new Int32NSetting(name) },
				{ typeof(Units.Unit), name => new UnitSetting(name) }
			};
			internal static ISetting Build<T>(String name) => TypeMap.TryGetValue(typeof(T), out Builder builder) ? builder(name) : throw new NotSupportedException($"type {typeof(T).Name} does not have a Settings<T> implementation.");
		}

		protected class StringSetting : Setting<String> {
			protected override String Convert(Object value) => value as String;

			internal StringSetting(String name) : base(name) { }
		}

		protected class Int32NSetting : Setting<Int32?> {
			protected override Int32? Convert(Object value) {
				if (Int32.TryParse(value?.ToString(), out Int32 _value)) return _value;
				return null;
			}

			internal Int32NSetting(String name) : base(name) { }
		}

		protected class UnitSetting : Setting<Units.Unit> {
			protected override Units.Unit Convert(Object value) => Enum.TryParse(value?.ToString(), out Units.Unit _value) ? _value : default;

			internal UnitSetting(String name) : base(name) { }
		}
	}
}
