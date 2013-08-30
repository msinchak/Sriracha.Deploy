﻿ngSriracha.config(function ($routeProvider, SrirachaNavigatorProvider) {
	var navigator = SrirachaNavigatorProvider.$get();
	$routeProvider
		.when("/", {
			templateUrl: "/templates/home-template.html",
			controller: "HomeController"
		})

		//Projects
		.when(navigator.project.list.url, {
			templateUrl: "templates/project-list-template.html",
			controller: "ProjectController",
		})
		.when(navigator.project.create.url, {
			templateUrl: "templates/project-edit-template.html",
			controller: "ProjectController"
		})
		.when(navigator.project.view.url, {
			templateUrl: "templates/project-view-template.html",
			controller: "ProjectController"
		})
		.when(navigator.project.edit.url, {
			templateUrl: "templates/project-edit-template.html",
			controller: "ProjectController"
		})
		.when(navigator.project.remove.url, {
			templateUrl: "templates/project-delete-template.html",
			controller: "ProjectController"
		})

		//Components
		.when(navigator.component.create.url, {
			templateUrl: "templates/component-edit-template.html",
			controller: "ProjectController"
		})
		.when(navigator.component.view.url, {
			templateUrl: "templates/component-view-template.html",
			controller: "ProjectController"
		})
		.when(navigator.component.edit.url, {
			templateUrl: "templates/component-edit-template.html",
			controller: "ProjectController"
		})
		.when(navigator.component.remove.url, {
			templateUrl: "templates/component-delete-template.html",
			controller: "ProjectController"
		})

		//Deployment Steps
		.when(navigator.deploymentStep.create.url, {
			templateUrl: "templates/project/deploymentStep/deploymentstep-edit-template.html",
			controller: "ProjectController"
		})
		.when(navigator.deploymentStep.edit.url, {
			templateUrl: "templates/project/deploymentStep/deploymentstep-edit-template.html",
			controller: "ProjectController"
		})
		.when(navigator.deploymentStep.remove.url, {
			templateUrl: "templates/project/deploymentStep/deploymentstep-delete-template.html",
			controller: "ProjectController"
		})

		//Branches
		.when(navigator.branch.create.url, {
			templateUrl: "templates/project/branch/branch-edit-template.html",
			controller: "ProjectController"
		})
		.when(navigator.branch.edit.url, {
			templateUrl: "templates/project/branch/branch-edit-template.html",
			controller: "ProjectController"
		})
		.when(navigator.branch.remove.url, {
			templateUrl: "templates/project/branch/branch-delete-template.html",
			controller: "ProjectController"
		})

		//Environments
		.when(Sriracha.Navigation.Environment.CreateUrl, {
			templateUrl: "templates/environment-edit-template.html",
			controller: "ProjectController"
		})
		.when(Sriracha.Navigation.Environment.EditUrl, {
			templateUrl: "templates/environment-edit-template.html",
			controller: "ProjectController"
		})
		.when(Sriracha.Navigation.Environment.DeleteUrl, {
			templateUrl: "templates/environment-delete-template.html",
			controller: "ProjectController"
		})

		//Builds
		.when(navigator.build.list.url, {
			templateUrl: "templates/builds/build-list-template.html",
			controller: "BuildController"
		})
		.when(navigator.build.submit.url, {
			templateUrl: "templates/builds/build-submit-template.html",
			controller: "BuildController"
		})
		.when(navigator.build.view.url, {
			templateUrl: "templates/builds/build-view-template.html",
			controller: "BuildController"
		})
		.when(navigator.build.remove.url, {
			templateUrl: "templates/builds/build-delete-template.html",
			controller: "BuildController"
		})

		//Deployments
		.when(navigator.deployment.batchRequest.url, {
			templateUrl: "templates/deployment-batchRequest-template.html",
			controller: "deployBatchRequestController"
		})
		.when(navigator.deployment.batchList.urlList[0], {
			templateUrl: "templates/deployment-batchList-template.html",
			controller: "deployBatchListController"
		})
		.when(navigator.deployment.batchList.urlList[1], {
			templateUrl: "templates/deployment-batchList-template.html",
			controller: "deployBatchListController"
		})
		.when(navigator.deployment.batchList.urlList[2], {
			templateUrl: "templates/deployment-batchList-template.html",
			controller: "deployBatchListController"
		})
		.when(navigator.deployment.batchList.urlList[3], {
			templateUrl: "templates/deployment-batchList-template.html",
			controller: "deployBatchListController"
		})
		.when(navigator.deployment.batchList.urlList[4], {
			templateUrl: "templates/deployment-batchList-template.html",
			controller: "deployBatchListController"
		})
		.when(navigator.deployment.batchStatus.url, {
			templateUrl: "templates/deployment-batchStatus-template.html",
			controller: "deployBatchStatusController"
		})
		.when(Sriracha.Navigation.Deployment.SubmitUrl, {
			templateUrl: "templates/deployment-submit-template.html",
			controller: "DeployController"
		})
		.when(Sriracha.Navigation.Deployment.ViewUrl, {
			templateUrl: "templates/deployment-view-template.html",
			controller: "DeployController"
		})

		//System Log
		.when(Sriracha.Navigation.SystemLog.ListUrl, {
			templateUrl: "templates/systemlog-list-template.html",
			controller: "SystemLogController"
		})

		.otherwise({
			template: "<h1>Not Found</h1>"
		})

	;
});

