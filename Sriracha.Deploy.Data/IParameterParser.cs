﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sriracha.Deploy.Data
{
	//\$\{((((MACHINE:)|(ENV:)).+?))\}
	public interface IParameterParser
	{
		List<string> FindMachineParameters(string value);
		List<string> FindEnvironmentParameters(string value);
	}
}
