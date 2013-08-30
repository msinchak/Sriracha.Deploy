﻿ngSriracha.provider("SrirachaNavigator", function () {
	this.$get = function () {
		var root = {
			getUrl: function (clientUrl, parameters) {
				var url = clientUrl;
				if (parameters) {
					for (var paramName in parameters) {
						url = url.replace(":" + paramName, parameters[paramName]);
					}
				}
				return "/#" + url;
			},
			goTo: function (clientUrl, parameters) {
				var url = this.getUrl(clientUrl, parameters);
				window.location.href = url;
			}
		};
		root.project = {
			list: {
				url: "/project",
				clientUrl: function () { return root.getUrl(this.url) },
				go: function() { root.goTo(this.url) }
			},
			create: {
				url: "/project/create",
				clientUrl: function () { return root.getUrl(this.url) },
				go: function() { root.goTo(this.url) }
			},
			view: {
				url: "/project/:projectId",
				clientUrl: function (projectId) { return root.getUrl(this.url, { projectId: projectId }) },
				go: function(projectId) { root.goTo(this.url, { projectId: projectId}) }
			},
			edit: {
				url: "/project/edit/:projectId",
				clientUrl: function (projectId) { return root.getUrl(this.url, { projectId: projectId }) },
				go: function (projectId) { root.goTo(this.url, { projectId: projectId }) }
			},
			"delete": {
				url: "/project/delete/:projectId",
				clientUrl: function (projectId) { return root.getUrl(this.url, { projectId: projectId }) },
				go: function (projectId) { root.goTo(this.url, { projectId: projectId }) }
			}
		};
		root.environment = {
			create: {
				url: "/project/:projectId/environment/create",
				clientUrl: function (projectId) { return root.getUrl(this.url, { projectId: projectId }) },
				go: function (projectId) { root.goTo(this.url, { projectId: projectId }) }
			},
			edit: {
				url: "/project/:projectId/environment/edit/:environmentId",
				clientUrl: function (projectId, environmentId) { return root.getUrl(this.url, { projectId: projectId, environmentId: environmentId }) },
				go: function (projectId, environmentId) { root.goto(this.url, { projectId: projectId, environmentId: environmentId }) }
			},
			"delete": {
				url: "/project/:projectId/environment/delete/:environmentId",
				clientUrl: function (projectId, environmentId) { return root.getUrl(this.url, { projectId: projectId, environmentId: environmentId }) },
				go: function (projectId, environmentId) { root.goto(this.url, { projectId: projectId, environmentId: environmentId }) }
			},
		};
		root.component = {
			create: {
				url: "/project/:projectId/component/create",
				clientUrl: function(projectId) { return root.getUrl(this.url, { projectId: projectId }) },
				go: function(projectId) { root.goTo(this.url, { projectId: projectId}) }
			},
			view: {
				url: "/project/:projectId/component/:componentId",
				clientUrl: function(projectId, componentId) { return root.getUrl(this.url, { projectId: projectId, componentId: componentId }) },
				go: function(projectId, componentId) { root.goto(this.url, { projectId: projectId, componentId: componentId}) }
			},
			edit: {
				url: "/project/:projectId/component/edit/:componentId",
				clientUrl: function(projectId, componentId) { return root.getUrl(this.url, { projectId: projectId, componentId: componentId }) },
				go: function(projectId, componentId) { root.goto(this.url, { projectId: projectId, componentId: componentId}) }
			},
			"delete": {
				url: "/project/:projectId/component/delete/:componentId",
				clientUrl: function(projectId, componentId) { return root.getUrl(this.url, { projectId: projectId, componentId: componentId }) },
				go: function(projectId, componentId) { root.goto(this.url, { projectId: projectId, componentId: componentId}) }
			}
		}
		root.build = {
			list: {
				url: "/build",
				clientUrl: function() { return root.getUrl(this.url) },
				go: function() { root.goTo(this.url) }
			},
			submit: {
				url: "/build/submit",
				clientUrl: function () { return root.getUrl(this.url) },
				go: function () { root.goTo(this.url) }
			},
			view: {
				url: "/build/:buildId",
				clientUrl: function (buildId) { return root.getUrl(this.url, { buildId: buildId }); },
				go: function (buildId) { root.goTo(this.url, { buildId: buildId }); }
			},
			remove: {
				url: "/build/delete/:buildId",
				clientUrl: function (buildId) { return root.getUrl(this.url, { buildId: buildId }); },
				go: function (buildId) { root.goTo(this.url, { buildId: buildId }); }
			}
		};
		root.deployment = {
			batchRequest: {
				url: "/deploy/batchRequest",
				clientUrl: function () { return root.getUrl(this.url) },
				go: function() { root.goTo(this.url) }
			},
			batchList: {
				urlList: [
					"/deploy/batchList",
					"/deploy/batchList/:pageNumber",
					"/deploy/batchList/:pageNumber/:pageSize",
					"/deploy/batchList/:pageNumber/:pageSize/:sortField",
					"/deploy/batchList/:pageNumber/:pageSize/:sortField/:sortAscending"
				],
				url: "/deploy/batchList/:pageNumber/:pageSize/:sortField/:sortAscending",
				clientUrl: function (pageNumber, pageSize, sortField, sortAscending) 
				{ 
					return root.getUrl(this.url, { pageNumber: pageNumber, pageSize: pageSize, sortField: sortField, sortAscending: sortAscending}); 
				},
				go: function (pageNumber, pageSize, sortField, sortAscending)  
				{ 
					root.goTo(this.url, { pageNumber: pageNumber, pageSize: pageSize, sortField: sortField, sortAscending: sortAscending}); 
				}
			},
			batchStatus: {
				url: "/deploy/batchStatus/:deployBatchRequestId",
				clientUrl: function (deployBatchRequestId) { return root.getUrl(this.url, { deployBatchRequestId: deployBatchRequestId }); },
				go: function (deployBatchRequestId) { root.goTo(this.url, { deployBatchRequestId: deployBatchRequestId }); }
			},
			submit: {
				url: "/deploy/submit/:buildId/:environmentId",
				clientUrl: function (buildId, environmentId) { return root.getUrl(this.url, { buildId: buildId, environmentId: environmentId }); },
				go: function (buildId, environmentId) { root.goTo(this.url, { buildId: buildId, environmentId: environmentId }); }
			},
			view: {
				url: "/deploy/:deployStateId",
				clientUrl: function (deployStateId) { return this.getUrl(this.url, { deployStateId: deployStateId }) },
				go: function (deployStateId) { root.goTo(this.url, { deployStateId: deployStateId }) }
			}
		};
		return root;
	}
});
