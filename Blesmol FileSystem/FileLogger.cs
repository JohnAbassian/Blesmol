using Blesmol.Core;
using Blesmol.Registry;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Blesmol_FileSystem {
	public class FileLogger : ILogger {
		private readonly ISettings Settings = new Settings(true);
		private readonly String LogFileName = "/log.json";

		public void Delete(Guid ErrorID) {

			Settings.Load();
			String path = Settings.FileSystemLog.FileSystemPath + "/" + LogFileName;

			List<LoggedError> errors = ReadLog(path);
			errors.Remove(errors.Find(x => x.ID == ErrorID));

			WriteLog(path, errors);
		}

		public List<LoggedError> GetLoggedErrors() {

			Settings.Load();
			String path = Settings.FileSystemLog.FileSystemPath + "/" + LogFileName;

			return ReadLog(path);
		}

		public Boolean Log(Exception e, DateTime occurredOn) {


			Settings.Load();
			String path = Settings.FileSystemLog.FileSystemPath + "/" + LogFileName;

			List<LoggedError> errors = ReadLog(path);

			if(errors.Count == 1000)
				errors.RemoveAt(999);

			errors.Insert(0,new LoggedError() { ID = Guid.NewGuid(), Message = e.Message, OccurredOn = occurredOn });

			WriteLog(path, errors);

			return true;

		}


		private void WriteLog(String path, List<LoggedError> errors) {
			using (StreamWriter w = File.CreateText(path)) {
				w.Write(JsonConvert.SerializeObject(errors));
			}
		}

		private List<LoggedError> ReadLog(String path) {
			List<LoggedError> errors;

			if (File.Exists(path)) {
				using (StreamReader r = new StreamReader(path)) {
					String json = r.ReadToEnd();
					errors = JsonConvert.DeserializeObject<List<LoggedError>>(json);
				}
			} else errors = new List<LoggedError>();

			return errors;
		}

		public void DeleteAll() {
			Settings.Load();
			File.Delete(Settings.FileSystemLog.FileSystemPath + "/" + LogFileName);
		}
	}
}
