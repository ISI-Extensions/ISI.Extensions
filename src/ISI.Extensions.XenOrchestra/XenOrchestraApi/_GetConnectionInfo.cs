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
using DTOs = ISI.Extensions.XenOrchestra.DataTransferObjects.XenOrchestraApi;

namespace ISI.Extensions.XenOrchestra
{
	public partial class XenOrchestraApi
	{
		private (Renci.SshNet.ConnectionInfo ConnectionInfo, string Password) GetConnectionInfo(DTOs.IRequest request)
		{
			var server = (string)null;
			var userName = (string)null;
			var password = (string)null;

			if (!string.IsNullOrWhiteSpace(request.XenOrchestraServer))
			{
				server = request.XenOrchestraServer;
				userName = request.XenOrchestraUserName;
				password = request.XenOrchestraPassword;
			}

			if (!string.IsNullOrWhiteSpace(Configuration.XenOrchestraServer))
			{
				server = Configuration.XenOrchestraServer;
				userName = Configuration.XenOrchestraUserName;
				password = Configuration.XenOrchestraPassword;
			}

			if (!string.IsNullOrWhiteSpace(server))
			{
				return (ConnectionInfo: ISI.Extensions.SshNet.ConnectionManager.GetConnectionInfo(server, userName, password), Password: password);
			}

			throw new Exception("No Server available");
		}
	}
}