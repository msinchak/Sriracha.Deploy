﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using Raven.Client;
using Sriracha.Deploy.Data;
using Sriracha.Deploy.Data.Dto;
using Sriracha.Deploy.Data.Exceptions;
using Sriracha.Deploy.Data.Repository;

namespace Sriracha.Deploy.RavenDB
{
	public class RavenBuildRepository : IBuildRepository
	{
		private readonly IDocumentSession _documentSession;
		private readonly IUserIdentity _userIdentity;
		private readonly Logger _logger;
		public RavenBuildRepository(IDocumentSession documentSession, IUserIdentity userIdentity, Logger logger)
		{
			_documentSession = DIHelper.VerifyParameter(documentSession);
			_userIdentity = DIHelper.VerifyParameter(userIdentity);
			_logger = DIHelper.VerifyParameter(logger);
		}

		public IEnumerable<DeployBuild> GetBuildList(string projectId = null, string branchId = null, string componentId = null)
		{
			var query = this._documentSession.Query<DeployBuild>().AsQueryable();
			if(!string.IsNullOrEmpty(projectId))
			{
				query = query.Where(i=>i.ProjectId == projectId);
			}
			if(!string.IsNullOrEmpty(branchId)) 
			{
				query = query.Where(i=>i.ProjectBranchId == branchId);
			}
			if(!string.IsNullOrEmpty(componentId))
			{
				query = query.Where(i=>i.ProjectComponentId == componentId);
			}
			return query.ToList();
		}

		public PagedSortedList<DeployBuild> GetBuildList(ListOptions listOptions)
		{
			var pagedList = _documentSession.QueryPageAndSort<DeployBuild>(listOptions, "UpdatedDateTimeUtc", false);
			return new PagedSortedList<DeployBuild>(pagedList, listOptions.SortField, listOptions.SortAscending.Value);
		}

		public DeployBuild CreateBuild(string projectId, string projectName, string projectComponentId, string projectComponentName, string projectBranchId, string projectBranchName, string fileId, string version)
		{
			var existingItem = (from i in this._documentSession.Query<DeployBuild>()
											.Customize(x=>x.WaitForNonStaleResultsAsOfLastWrite())
								where i.ProjectId == projectId
									&& i.ProjectBranchId == projectBranchId
									&& i.ProjectComponentId == projectComponentId
									&& i.FileId == fileId
									&& i.Version == version
								select i).FirstOrDefault();
			if(existingItem != null)
			{
				throw new DuplicateObjectException<DeployBuild>(existingItem);
			}
			var item = new DeployBuild
			{
				Id = Guid.NewGuid().ToString(),
				ProjectId = projectId,
				ProjectName = projectName,
				ProjectComponentId = projectComponentId,
				ProjectComponentName = projectComponentName,
				ProjectBranchId = projectBranchId,
				ProjectBranchName = projectBranchName,
				FileId = fileId,
				Version = version,
				CreatedDateTimeUtc = DateTime.UtcNow,
				CreatedByUserName = _userIdentity.UserName,
				UpdatedDateTimeUtc = DateTime.UtcNow,
				UpdatedByUserName = _userIdentity.UserName
			};
			this._documentSession.Store(item);
			this._documentSession.SaveChanges();
			return item;
		}

		public DeployBuild GetBuild(string buildId)
		{
			if(string.IsNullOrEmpty(buildId))
			{
				throw new ArgumentNullException("Missing build ID");
			}
			return this._documentSession.Load<DeployBuild>(buildId);
		}

		public DeployBuild UpdateBuild(string buildId, string projectId, string projectName, string projectComponentId, string projectComponentName, string projectBranchId, string projectBranchName, string fileId, string version)
		{
			if (string.IsNullOrEmpty(buildId))
			{
				throw new ArgumentNullException("Missing build ID");
			}
			if (string.IsNullOrEmpty(projectId))
			{
				throw new ArgumentNullException("Missing project ID");
			}
			if(string.IsNullOrEmpty(projectName))
			{
				throw new ArgumentNullException("Missing project name");
			}
			if (string.IsNullOrEmpty(projectComponentId))
			{
				throw new ArgumentNullException("Missing component ID");
			}
			if (string.IsNullOrEmpty(projectComponentName)) 
			{
				throw new ArgumentNullException("Missing component name");
			}
			if (string.IsNullOrEmpty(projectBranchId))
			{
				throw new ArgumentNullException("Missing branch ID");
			}
			if (string.IsNullOrEmpty(projectBranchName))
			{
				throw new ArgumentNullException("Missing branch name");
			}
			if (string.IsNullOrEmpty(fileId))
			{
				throw new ArgumentNullException("Missing file ID");
			}
			if (string.IsNullOrEmpty(version))
			{
				throw new ArgumentNullException("Missing version");
			}
			var build = this.GetBuild(buildId);
			build.ProjectId = projectId;
			build.ProjectComponentId = projectComponentId;
			build.ProjectComponentName = projectComponentName;
			build.ProjectBranchId = projectBranchId;
			build.ProjectBranchName = projectBranchName;
			build.FileId = fileId;
			build.Version = version;
			build.UpdatedDateTimeUtc = DateTime.UtcNow;
			build.UpdatedByUserName = _userIdentity.UserName;
			this._documentSession.SaveChanges();
			return build;
		}

		public void DeleteBuild(string buildId)
		{
			_logger.Info("User {0} deleting build {1}", _userIdentity.UserName, buildId);
			var build = GetBuild(buildId);
			this._documentSession.Delete(build);
			this._documentSession.SaveChanges();

		}
	}
}
