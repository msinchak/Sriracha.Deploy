﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MMDB.Shared;
using NUnit.Framework;
using Raven.Client;
using Sriracha.Deploy.Data.Dto;
using Sriracha.Deploy.Data.Exceptions;

namespace Sriracha.Deploy.RavenDB.Tests
{
	public class RavenDBBuildRepositoryTests
    {
		public class StoreBuild : RavenTestBase
		{
			private class TestData
			{
				public string ProjectId { get; set; }
				public string ProjectBranchId { get; set; }
				public string FileId { get; set; }
				public Version Version { get; set; }
				public RavenDBBuildRepository Sut { get; set; }

				public static TestData Create(IDocumentSession session)
				{
					var testData = new TestData
					{
						ProjectId = Guid.NewGuid().ToString(),
						ProjectBranchId = Guid.NewGuid().ToString(),
						FileId = Guid.NewGuid().ToString(),
						Version = TempTestDataHelper.RandomVersion(),
						Sut = new RavenDBBuildRepository(session)
					};
					return testData;
				}

			}
			[Test]
			public void ActuallyStoresBuild()
			{
				var testData = TestData.Create(this.DocumentSession);

				var result = testData.Sut.StoreBuild(testData.ProjectId, testData.ProjectBranchId, testData.FileId, testData.Version);

				Assert.IsNotNull(result);
				Assert.AreNotEqual(0, result.Id);

				var dbResult = this.DocumentSession.Load<DeployBuild>(result.Id);
				Assert.AreEqual(result, dbResult);
			}

			[Test]
			public void DuplicateData_ThrowsError()
			{
				var testData = TestData.Create(this.DocumentSession);

				var result1 = testData.Sut.StoreBuild(testData.ProjectId, testData.ProjectBranchId, testData.FileId, testData.Version);

				Assert.Throws<DuplicateObjectException<DeployBuild>>(delegate { testData.Sut.StoreBuild(testData.ProjectId, testData.ProjectBranchId, testData.FileId, testData.Version); });
			}
		}
    }
}
