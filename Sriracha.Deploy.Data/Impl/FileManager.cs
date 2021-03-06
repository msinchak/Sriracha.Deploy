﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sriracha.Deploy.Data.Dto;
using Sriracha.Deploy.Data.Repository;

namespace Sriracha.Deploy.Data.Impl
{
	public class FileManager : IFileManager
	{
		private readonly IFileRepository _fileRepository;
		private readonly IFileWriter _fileWriter;

		public FileManager(IFileRepository fileRepository, IFileWriter fileWriter)
		{
			this._fileRepository = DIHelper.VerifyParameter(fileRepository);
			this._fileWriter = DIHelper.VerifyParameter(fileWriter);
		}

		public IEnumerable<DeployFile> GetFileList()
		{
			return _fileRepository.GetFileList();
		}

		public DeployFile CreateFile(string fileName, byte[] fileData)
		{
			return _fileRepository.CreateFile(fileName, fileData);
		}

		public DeployFile UpdateFile(string fileId, string fileName, byte[] fileData)
		{
			return _fileRepository.UpdateFile(fileId, fileName, fileData);
		}

		public DeployFile GetFile(string fileId)
		{
			return _fileRepository.GetFile(fileId);
		}

		public void DeleteFile(string fileId)
		{
			_fileRepository.DeleteFile(fileId);
		}

		public void ExportFile(string fileId, string targetFilePath)
		{
			var file = this.GetFile(fileId);
			var data = _fileRepository.GetFileData(fileId);
			_fileWriter.WriteBytes(targetFilePath, data);
		}
	}
}
