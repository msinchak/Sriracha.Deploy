﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sriracha.Deploy.Data.Dto;

namespace Sriracha.Deploy.Data
{
	public interface IFileManager
	{
		IEnumerable<DeployFile> GetFileList();
		DeployFile CreateFile(string fileName, byte[] fileData);
		DeployFile UpdateFile(string fileId, string fileName, byte[] fileData);
		DeployFile GetFile(string fileId);
		void DeleteFile(string fileId);

		void ExportFile(string fileId, string targetFilePath);
	}
}
