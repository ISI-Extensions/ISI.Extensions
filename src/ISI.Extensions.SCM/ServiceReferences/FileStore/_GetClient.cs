#region Copyright & License
/*
Copyright (c) 2022, Integrated Solutions, Inc.
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

namespace ISI.Extensions.Scm.ServiceReferences.FileStore
{
	public partial class FileStoreClient
	{
		public static FileStoreClient GetClient(string webServiceUrl)
		{
			System.Net.ServicePointManager.Expect100Continue = true;
			System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls;

			var securityMode = ((new Uri(webServiceUrl)).Scheme == Uri.UriSchemeHttps ? System.ServiceModel.SecurityMode.Transport : System.ServiceModel.SecurityMode.None);

			var binding = new System.ServiceModel.WSHttpBinding(securityMode)
			{
				Name = "WSHttpBinding",
				OpenTimeout = new(0, 0, 10, 0),
				SendTimeout = new(0, 0, 10, 0),
				ReceiveTimeout = new(0, 0, 10, 0),
				CloseTimeout = new(0, 0, 10, 0),
				MaxBufferPoolSize = 2147483647,
				MaxReceivedMessageSize = 2147483647,
				ReaderQuotas =
				{
					MaxDepth = 2147483647,
					MaxStringContentLength = 2147483647,
					MaxArrayLength = 2147483647,
					MaxBytesPerRead = 2147483647,
					MaxNameTableCharCount = 2147483647
				}
			};

			var endpoint = new System.ServiceModel.EndpointAddress(webServiceUrl);

			return new(binding, endpoint);
		}
	}
}