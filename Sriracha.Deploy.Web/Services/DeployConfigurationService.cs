﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.ServiceInterface;
using Sriracha.Deploy.Data;
using Sriracha.Deploy.Data.Dto;

namespace Sriracha.Deploy.Web.Services
{
	public class DeployConfigurationService : Service
	{
		private readonly IProjectManager _projectManager;
		public DeployConfigurationService(IProjectManager projectManager)
		{
			_projectManager = DIHelper.VerifyParameter(projectManager);
		}

		public object Get(DeployConfiguration request)
		{
			if(request == null || 
				(string.IsNullOrEmpty(request.Id) && string.IsNullOrEmpty(request.ProjectId)))
			{
				throw new ArgumentNullException();
			}
			if (!string.IsNullOrEmpty(request.Id))
			{
				return _projectManager.GetConfiguration(request.Id);
			}
			else 
			{
				return _projectManager.GetConfigurationList(request.ProjectId);
			}

		}

		public object Post(DeployConfiguration configuration)
		{
			if (string.IsNullOrEmpty(configuration.Id))
			{
				return _projectManager.CreateConfiguration(configuration.ProjectId, configuration.ConfigurationName);
			}
			else
			{
				return _projectManager.UpdateConfiguration(configuration.Id, configuration.ProjectId, configuration.ConfigurationName);
			}
		}

		public void Delete(DeployConfiguration configuration)
		{
			_projectManager.DeleteConfiguration(configuration.Id);
		}
	}
}