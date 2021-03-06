﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sriracha.Deploy.Data.Dto
{
	public class DeployBatchRequestItem
	{
		public string Id { get; set; }
		public DeployBuild Build { get; set; }
		public List<DeployMachine> MachineList { get; set; }
	}
}
