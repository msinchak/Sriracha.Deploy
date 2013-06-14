﻿using System;
using System.Collections.Generic;
using System.Collections;
using NUnit.Framework;
using Sriracha.Deploy.Data.Dto;
using Sriracha.Deploy.Data.Repository;
using System.Linq;

namespace Sriracha.Deploy.RavenDB.Tests
{
	public class RavenProjectRepositoryTests
	{
		public class CreateProject : RavenTestBase
		{
			[Test]
			public void StoresProject()
			{
				string projectName = Guid.NewGuid().ToString();
				IProjectRepository sut = new RavenProjectRepository(this.DocumentSession);
				var result = sut.CreateProject(projectName);
				Assert.IsNotNull(result);
				Assert.IsNotNullOrEmpty(result.Id);
				Assert.AreEqual(projectName, result.ProjectName);
				var dbItem = this.DocumentSession.Load<DeployProject>(result.Id);
				Assert.AreEqual(projectName, result.ProjectName);
			}

			[Test]
			public void MissingProjectName_ThrowsError()
			{
				string projectName = string.Empty;
				IProjectRepository sut = new RavenProjectRepository(this.DocumentSession);
				Assert.Throws<ArgumentNullException>( ()=> sut.CreateProject(projectName) );
			}
		}

		public class GetProjectList : RavenTestBase
		{
			[Test]
			public void CanRetrieveProjectList()
			{
				var projectList = new List<DeployProject>()
				{
					new DeployProject { Id = Guid.NewGuid().ToString(), ProjectName = Guid.NewGuid().ToString() },
					new DeployProject { Id = Guid.NewGuid().ToString(), ProjectName = Guid.NewGuid().ToString() },
					new DeployProject { Id = Guid.NewGuid().ToString(), ProjectName = Guid.NewGuid().ToString() }
				};
				foreach(var project in projectList)
				{
					this.DocumentSession.Store(project);
				}
				this.DocumentSession.SaveChanges();

				IProjectRepository sut = new RavenProjectRepository(this.DocumentSession);
				var result = sut.GetProjectList().ToList();
				
				Assert.GreaterOrEqual(result.Count, projectList.Count);
				foreach(var project in projectList)
				{
					Assert.IsTrue(result.Any(i=>i.Id == project.Id));
				}
			}
		}

		public class GetProject : RavenTestBase
		{
			[Test]
			public void CanGetProject()
			{
				var project = new DeployProject
				{
					Id = Guid.NewGuid().ToString(),
					ProjectName = Guid.NewGuid().ToString()
				};
				this.DocumentSession.Store(project);
				this.DocumentSession.SaveChanges();

				IProjectRepository sut = new RavenProjectRepository(this.DocumentSession);
				var result = sut.GetProject(project.Id);

				Assert.IsNotNull(result);
				Assert.AreEqual(project.Id, result.Id);
				Assert.AreEqual(project.ProjectName, result.ProjectName);
			}

			[Test]
			public void MissingProjectId_ThrowsError()
			{
				string projectId = string.Empty;
				IProjectRepository sut = new RavenProjectRepository(this.DocumentSession);
				Assert.Throws<ArgumentNullException>( ()=> sut.GetProject(projectId) );
			}

			[Test]
			public void BadProjectId_ThrowsError()
			{
				string projectId = Guid.NewGuid().ToString();
				IProjectRepository sut = new RavenProjectRepository(this.DocumentSession);
				Assert.Throws<KeyNotFoundException>(() => sut.GetProject(projectId));
			}
		}

		public class CreateBranch : RavenTestBase
		{
			[Test]
			public void CanCreateBranch()
			{
				var project = new DeployProject
				{
					Id = Guid.NewGuid().ToString(),
					ProjectName = Guid.NewGuid().ToString()
				};
				this.DocumentSession.Store(project);
				this.DocumentSession.SaveChanges();

				string branchName = Guid.NewGuid().ToString();
				IProjectRepository sut = new RavenProjectRepository(this.DocumentSession);
				var result = sut.CreateBranch(project.Id, branchName);

				Assert.IsNotNull(result);
				Assert.IsNotNullOrEmpty(result.Id);
				Assert.AreEqual(project.Id, result.ProjectId);

				var dbProject = this.DocumentSession.Load<DeployProject>(result.ProjectId);
				Assert.IsNotNull(dbProject);
				Assert.AreEqual(1, dbProject.BranchList.Count);
				Assert.AreEqual(result.Id, dbProject.BranchList[0].Id);
				Assert.AreEqual(project.Id, dbProject.BranchList[0].ProjectId);
				Assert.AreEqual(branchName, dbProject.BranchList[0].BranchName);
			}

			[Test]
			public void MissingBranchName_ThrowsError()
			{
				string projectId = Guid.NewGuid().ToString();
				string branchName = null;
				IProjectRepository sut = new RavenProjectRepository(this.DocumentSession);
				Assert.Throws<ArgumentNullException>(() => sut.CreateBranch(projectId, branchName));
			}

			[Test]
			public void MissingProjectId_ThrowsError()
			{
				string projectId = null;
				string branchName = Guid.NewGuid().ToString(); 
				IProjectRepository sut = new RavenProjectRepository(this.DocumentSession);
				Assert.Throws<ArgumentNullException>(() => sut.CreateBranch(projectId, branchName));
			}

			[Test]
			public void BadProjectId_ThrowsError()
			{
				string projectId = Guid.NewGuid().ToString();
				string branchName = Guid.NewGuid().ToString();
				IProjectRepository sut = new RavenProjectRepository(this.DocumentSession);
				Assert.Throws<KeyNotFoundException>(() => sut.CreateBranch(projectId, branchName));
			}
		}

		public class DeleteProject : RavenTestBase
		{
			[Test]
			public void CanDeleteProject()
			{
				var project = new DeployProject
				{
					Id = Guid.NewGuid().ToString(),
					ProjectName = Guid.NewGuid().ToString()
				};
				this.DocumentSession.Store(project);
				this.DocumentSession.SaveChanges();

				IProjectRepository sut = new RavenProjectRepository(this.DocumentSession);
				sut.DeleteProject(project.Id);

				var dbItem = this.DocumentSession.Load<DeployProject>(project.Id);
				Assert.IsNull(dbItem);
			}

			[Test]
			public void MissingProjectId_ThrowsError()
			{
				string projectId = string.Empty;
				IProjectRepository sut = new RavenProjectRepository(this.DocumentSession);
				Assert.Throws<ArgumentNullException>(()=> sut.DeleteProject(projectId));
			}

			[Test]
			public void BadProjectId_ThrowsError()
			{
				string projectId = Guid.NewGuid().ToString();
				IProjectRepository sut = new RavenProjectRepository(this.DocumentSession);
				Assert.Throws<KeyNotFoundException>(() => sut.DeleteProject(projectId));
			}
		}

		public class UpdateProject : RavenTestBase
		{
			[Test]
			public void CanUpdateProject()
			{
				var project = new DeployProject
				{
					Id = Guid.NewGuid().ToString(),
					ProjectName = Guid.NewGuid().ToString()
				};
				this.DocumentSession.Store(project);
				this.DocumentSession.SaveChanges();

				string newProjectName = Guid.NewGuid().ToString();
				IProjectRepository sut = new RavenProjectRepository(this.DocumentSession);
				sut.UpdateProject(project.Id, newProjectName);

				var dbItem = this.DocumentSession.Load<DeployProject>(project.Id);
				Assert.IsNotNull(dbItem);
				Assert.AreEqual(newProjectName, dbItem.ProjectName);
			}

			[Test]
			public void MissingProjectId_ThrowsError()
			{
				string projectId = string.Empty;
				string projectName = Guid.NewGuid().ToString();
				IProjectRepository sut = new RavenProjectRepository(this.DocumentSession);
				Assert.Throws<ArgumentNullException>(()=>sut.UpdateProject(projectId, projectName));
			}

			[Test]
			public void MissingProjectName_ThrowsError()
			{
				string projectId = Guid.NewGuid().ToString();
				string projectName = string.Empty;
				IProjectRepository sut = new RavenProjectRepository(this.DocumentSession);
				Assert.Throws<ArgumentNullException>(() => sut.UpdateProject(projectId, projectName));
			}
		}
	}
}