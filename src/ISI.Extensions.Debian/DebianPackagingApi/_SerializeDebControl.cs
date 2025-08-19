#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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
using DTOs = ISI.Extensions.Debian.DataTransferObjects.DebianPackagingApi;

namespace ISI.Extensions.Debian
{
	public partial class DebianPackagingApi
	{
		private string SerializeDebControl(DebControl debControl)
		{
			var content = new StringBuilder();

			content.AppendLine($"Package: {debControl.Package}");

			if (!string.IsNullOrWhiteSpace(debControl.Source))
			{
				content.AppendLine($"Source: {debControl.Source}");
			}

			content.AppendLine($"Version: {debControl.Version}");

			content.AppendLine($"Architecture: {debControl.Architecture.TrimStart("binary-", StringComparison.InvariantCultureIgnoreCase)}");

			if (debControl.Depends.NullCheckedAny())
			{
				content.AppendLine($"Depends: {string.Join(", ", debControl.Depends)}");
			}

			if (debControl.PreDepends.NullCheckedAny())
			{
				content.AppendLine($"Pre-Depends: {string.Join(", ", debControl.PreDepends)}");
			}

			if (debControl.Recommends.NullCheckedAny())
			{
				content.AppendLine($"Recommends: {string.Join(", ", debControl.Recommends)}");
			}

			if (debControl.Suggests.NullCheckedAny())
			{
				content.AppendLine($"Suggests: {string.Join(", ", debControl.Suggests)}");
			}

			if (debControl.Enhances.NullCheckedAny())
			{
				content.AppendLine($"Enhances: {string.Join(", ", debControl.Enhances)}");
			}

			if (debControl.Breaks.NullCheckedAny())
			{
				content.AppendLine($"Breaks: {string.Join(", ", debControl.Breaks)}");
			}

			if (debControl.Conflicts.NullCheckedAny())
			{
				content.AppendLine($"Conflicts: {string.Join(", ", debControl.Conflicts)}");
			}
			
			if (debControl.InstalledSize.HasValue)
			{
				content.AppendLine($"Installed-Size: {debControl.InstalledSize}");
			}

			if (!string.IsNullOrWhiteSpace(debControl.Maintainer))
			{
				content.AppendLine($"Maintainer: {debControl.Maintainer}");
			}

			if (!string.IsNullOrWhiteSpace(debControl.Homepage))
			{
				content.AppendLine($"Homepage: {debControl.Homepage}");
			}

			if (!string.IsNullOrWhiteSpace(debControl.Description))
			{
				content.AppendLine($"Description: {debControl.Description}");
			}

			if (!string.IsNullOrWhiteSpace(debControl.Source))
			{
				content.AppendLine($"Source: {debControl.Source}");
			}

			return content.ToString();
		}
	}
}