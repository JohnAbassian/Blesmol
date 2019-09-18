using System;
using System.Collections.Generic;

namespace Blesmol.Core {
	public interface ILogger {
		Boolean Log(Exception e, DateTime occurredOn);
		void Delete(Guid ErrorID);
		void DeleteAll();
		List<LoggedError> GetLoggedErrors();
	}

	public class LoggedError {
		public String Message { get; set; }
		public DateTime OccurredOn { get; set; }
		public Guid ID { get; set; }

		public void Delete(ILogger logger) => logger.Delete(ID);
	}
}
