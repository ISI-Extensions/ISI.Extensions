#region Copyright & License
/*
Copyright (c) 2024, Integrated Solutions, Inc.
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
using System.Runtime.Serialization;
using LOCALENTITIES = ISI.Extensions.VisualStudioCode;

namespace ISI.Extensions.VisualStudioCode.SerializableModels
{
	[DataContract]
	public class SolutionPackage
	{
		public static SolutionPackage ToSerializable(LOCALENTITIES.SolutionPackage source)
		{
			return new SolutionPackage()
			{
				Name = source.Name,
				Version = source.Version,
				Description = source.Description,
				ProductName = source.ProductName,
				Author = source.Author,
				LockfileVersion = source.LockfileVersion,
				Requires = source.Requires,
				Scripts = source.Scripts?.NullCheckedConvert(SolutionPackageScripts.ToSerializable),
				Engines = source.Engines?.NullCheckedConvert(SolutionPackageEngines.ToSerializable),
			};
		}

		public LOCALENTITIES.SolutionPackage Export()
		{
			return new LOCALENTITIES.SolutionPackage()
			{
				Name = Name,
				Version = Version,
				Description = Description,
				ProductName = ProductName,
				Author = Author,
				LockfileVersion = LockfileVersion,
				Requires = Requires,
				Scripts = Scripts?.Export(),
				Engines = Engines?.Export(),
			};
		}

		[DataMember(Name = "name", EmitDefaultValue = false)]
		public string Name { get; set; }

		[DataMember(Name = "version", EmitDefaultValue = false)]
		public string Version { get; set; }

		[DataMember(Name = "description", EmitDefaultValue = false)]
		public string Description { get; set; }

		[DataMember(Name = "productName", EmitDefaultValue = false)]
		public string ProductName { get; set; }

		[DataMember(Name = "author", EmitDefaultValue = false)]
		public string Author { get; set; }

		[DataMember(Name = "lockfileVersion", EmitDefaultValue = false)]
		public int LockfileVersion { get; set; }

		[DataMember(Name = "requires", EmitDefaultValue = false)]
		public bool Requires { get; set; }

		[DataMember(Name = "scripts", EmitDefaultValue = false)]
		public SolutionPackageScripts Scripts { get; set; }

		[DataMember(Name = "engines", EmitDefaultValue = false)]
		public SolutionPackageEngines Engines { get; set; }
	}

	[DataContract]
	public class SolutionPackageScripts
	{
		public static SolutionPackageScripts ToSerializable(LOCALENTITIES.SolutionPackageScripts source)
		{
			return new SolutionPackageScripts()
			{
				Lint = source.Lint,
				Format = source.Format,
				Test = source.Test,
				Dev = source.Dev,
				Build = source.Build,
			};
		}

		public LOCALENTITIES.SolutionPackageScripts Export()
		{
			return new LOCALENTITIES.SolutionPackageScripts()
			{
				Lint = Lint,
				Format = Format,
				Test = Test,
				Dev = Dev,
				Build = Build,
			};
		}

		[DataMember(Name = "lint", EmitDefaultValue = false)]
		public string Lint { get; set; }

		[DataMember(Name = "format", EmitDefaultValue = false)]
		public string Format { get; set; }

		[DataMember(Name = "test", EmitDefaultValue = false)]
		public string Test { get; set; }

		[DataMember(Name = "dev", EmitDefaultValue = false)]
		public string Dev { get; set; }

		[DataMember(Name = "build", EmitDefaultValue = false)]
		public string Build { get; set; }
	}

	[DataContract]
	public class SolutionPackageEngines
	{
		public static SolutionPackageEngines ToSerializable(LOCALENTITIES.SolutionPackageEngines source)
		{
			return new SolutionPackageEngines()
			{
				Node = source.Node,
				Npm = source.Npm,
				Yarn = source.Yarn,
			};
		}

		public LOCALENTITIES.SolutionPackageEngines Export()
		{
			return new LOCALENTITIES.SolutionPackageEngines()
			{
				Node = Node,
				Npm = Npm,
				Yarn = Yarn,
			};
		}

		[DataMember(Name = "node", EmitDefaultValue = false)]
		public string Node { get; set; }

		[DataMember(Name = "npm", EmitDefaultValue = false)]
		public string Npm { get; set; }

		[DataMember(Name = "yarn", EmitDefaultValue = false)]
		public string Yarn { get; set; }
	}
}
