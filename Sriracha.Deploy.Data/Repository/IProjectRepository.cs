﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sriracha.Deploy.Data.Dto;

namespace Sriracha.Deploy.Data.Repository
{
	public interface IProjectRepository
	{
		IEnumerable<DeployProject> GetProjectList(string[] idList=null);
		DeployProject CreateProject(string projectName);
		DeployProject GetProject(string projectId);
		DeployProject UpdateProject(string projectId, string projectName);
		DeployProjectBranch CreateBranch(string projectId, string branchName);
		void DeleteProject(string projectId);

		DeployComponent CreateComponent(string projectId, string componentName);
		DeployComponent UpdateComponent(string projectId, string componentId, string componentName);

		DeployComponentDeploymentStep CreateDeploymentStep(string projectId, string componentId, string stepName, string taskTypeName, dynamic taskOptions);
		DeployComponentDeploymentStep UpdateDeploymentStep(string projectId, string componentId, string deploymentStepId, string stepName, string taskTypeName, dynamic taskOptions);
	}
}
