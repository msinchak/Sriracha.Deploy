﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sriracha.Deploy.Data.Tasks
{
	public class TaskMetadata
	{
		public string TaskTypeName { get; set; }

		public IDeployTask CreateTask()
		{
			throw new NotImplementedException();
		}
	}
}