﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sriracha.Deploy.Data.Tasks;

namespace Sriracha.Deploy.Data.Impl
{
	public class TaskManager : ITaskManager
	{
		private readonly IModuleInspector _moduleInspector;

		public TaskManager(IModuleInspector moduleInspector)
		{
			this._moduleInspector = DIHelper.VerifyParameter(moduleInspector);
		}

		public List<TaskMetadata> GetAvailableTaskList()
		{
			var typeList = this._moduleInspector.FindTypesImplementingInterfaces(typeof(IDeployTaskDefinition));
			List<TaskMetadata> returnList = new List<TaskMetadata>();
			foreach(var type in typeList)
			{
				if(type.IsClass && !type.IsAbstract)
				{
					var item = new TaskMetadata
					{
						TaskTypeName = type.FullName,
						TaskDisplayName = type.Name
					};
					returnList.Add(item);
				}
			}
			return returnList;
		}
	}
}
