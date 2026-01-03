#region Copyright & License
/*
Copyright (c) 2026, Integrated Solutions, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

		* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
		* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
		* Neither the name of the Integrated Solutions, Inc. nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Debian.DataTransferObjects.DebianPackagingApi
{
	public interface ICreateDebRequest
	{
		Version DebSpecVersion { get; }
		DebControl DebControl { get; }

		string PreInstallScript { get; }
		string PostInstallScript { get; }
		string PreRemovalScript { get; }
		string PostRemovalScript { get; }

		string DefaultOwner { get; }
		string DefaultGroup { get; }

		IEnumerable<ICreateDebRequestDataEntry> DataEntries { get; }
	}

	public class CreateDebRequestWithDebFullName : AbstractCreateDebRequest, ICreateDebRequest
	{
		public string DebFullName { get; set; }
	}

	public class CreateDebRequestWithDebStream : AbstractCreateDebRequest, ICreateDebRequest
	{
		public System.IO.Stream DebStream { get; set; }
	}

	public abstract class AbstractCreateDebRequest
	{
		public Version DebSpecVersion { get; set; } = new("2.0");
		public DebControl DebControl { get; set; }

		public string PreInstallScript { get; set; }
		public string PostInstallScript { get; set; }
		public string PreRemovalScript { get; set; }
		public string PostRemovalScript { get; set; }

		public string DefaultOwner { get; set; } = "root";
		public string DefaultGroup { get; set; } = "root";

		public IEnumerable<ICreateDebRequestDataEntry> DataEntries { get; set; }
	}

	public interface ICreateDebRequestDataEntry
	{

	}

	public interface ICreateDebRequestDataEntryFile : ICreateDebRequestDataEntry
	{
		bool IsAscii { get; }
		bool IsExecutable { get; }
		bool DoNotRemove { get; }
		string TargetPath { get; }
	}

	public class CreateDebRequestEntryFile : ICreateDebRequestDataEntryFile
	{
		public string SourceFullName { get; set; }

		public bool IsAscii { get; set; }
		public bool IsExecutable { get; set; }
		public bool DoNotRemove { get; set; }

		public string TargetPath { get; set; }
	}

	public class CreateDebRequestEntryStream : ICreateDebRequestDataEntryFile
	{
		public System.IO.Stream SourceStream { get; set; }

		public bool IsAscii { get; set; }
		public bool IsExecutable { get; set; }
		public bool DoNotRemove { get; set; }

		public string TargetPath { get; set; }
	}

	public class CreateDebRequestEntryFileWildCard : ICreateDebRequestDataEntry
	{
		public string SourceDirectory { get; set; }

		public string TargetPathDirectory { get; set; }
	}
}