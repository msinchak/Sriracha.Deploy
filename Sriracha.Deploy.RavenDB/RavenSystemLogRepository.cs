﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Indexes;
using Sriracha.Deploy.Data;
using Sriracha.Deploy.Data.Dto;
using Sriracha.Deploy.Data.Repository;

namespace Sriracha.Deploy.RavenDB
{
	public class RavenSystemLogRepository : ISystemLogRepository
	{
		public class SystemLogByDateIndex : AbstractIndexCreationTask<SystemLog>
		{
			public SystemLogByDateIndex()
			{
				Map = messages => from i in messages
								  select new { i.MessageDateTimeUtc, i.EnumSystemLogTypeID };
				Index(x => x.MessageDateTimeUtc, Raven.Abstractions.Indexing.FieldIndexing.Analyzed);
			}
		}

		private readonly IDocumentSession _documentSession;
		private readonly Logger _logger;

		public RavenSystemLogRepository(IDocumentSession documentSession, Logger logger)
		{
			_documentSession = DIHelper.VerifyParameter(documentSession);
			_logger = DIHelper.VerifyParameter(logger);
		}

		public SystemLog LogMessage(EnumSystemLogType logType, string userName, DateTime messageDateTime, string message, string loggerName)
		{
			var systemLog = new SystemLog
			{
				Id = Guid.NewGuid().ToString(),
				EnumSystemLogTypeID = logType,
				UserName = userName,
				MessageDateTimeUtc = messageDateTime.ToUniversalTime(),
				MessageText = message,
				LoggerName = loggerName
			};
			this._documentSession.Store(systemLog);
			this._documentSession.SaveChanges();
			return systemLog;
		}


		public void PurgeLogMessages(DateTime utcNow, EnumSystemLogType systemLogType, int? ageMinutes)
		{
			if (ageMinutes.HasValue)
			{
				DateTime maxDate = utcNow.AddMinutes(0 - ageMinutes.Value);
				this._logger.Trace(string.Format("Deleting {0} messages older than {1}", systemLogType, maxDate));
				var query = this._documentSession.Advanced.LuceneQuery<SystemLog, SystemLogByDateIndex>()
												.WhereEquals("EnumSystemLogTypeID", systemLogType)
												.AndAlso().WhereLessThan("MessageDateTimeUtc", maxDate);
				var queryString = query.ToString();
				this._documentSession.Advanced.DocumentStore.DatabaseCommands.DeleteByIndex
				(
					typeof(SystemLogByDateIndex).Name, new IndexQuery
					{
						Query = queryString
					},
					allowStale: true
				);
				this._documentSession.SaveChanges();
			}

		}
	}
}
