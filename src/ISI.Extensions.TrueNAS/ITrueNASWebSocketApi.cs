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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using SerializableDTOs = ISI.Extensions.TrueNAS.SerializableModels;

namespace ISI.Extensions.TrueNAS
{
	internal interface ITrueNASWebSocketApi
	{
		[StreamJsonRpc.JsonRpcMethod("auth.login_ex")]
		Task<SerializableDTOs.LoginResponse> LoginAsync(SerializableDTOs.LoginRequest login_data);

		[StreamJsonRpc.JsonRpcMethod("system.info")]
		Task<SerializableDTOs.GetSystemInfoResponse> GetSystemInfoAsync();

		[StreamJsonRpc.JsonRpcMethod("system.general.ui_certificate_choices")]
		Task<Dictionary<string, string>> ListCertificateChoicesAsync();

		[StreamJsonRpc.JsonRpcMethod("system.general.config")]
		Task<SerializableDTOs.GetSystemConfigGeneralResponse> GetSystemConfigGeneralAsync();

		[StreamJsonRpc.JsonRpcMethod("system.general.update")]
		Task<SerializableDTOs.SetSystemConfigGeneralResponse> SetSystemConfigGeneralAsync(SerializableDTOs.SetSystemConfigGeneralRequest general_settings);

		[StreamJsonRpc.JsonRpcMethod("certificate.create")]
		Task<object> CreateCertificateAsync(SerializableDTOs.CreateCertificateRequest certificate_create);
		//Task<SerializableDTOs.CreateCertificateResponse> CreateCertificateAsync(SerializableDTOs.CreateCertificateRequest certificate_create);

		[StreamJsonRpc.JsonRpcMethod("certificate.delete")]
		Task<bool> DeleteCertificateAsync(int id, bool force);

		[StreamJsonRpc.JsonRpcMethod("system.general.ui_restart")]
		Task<object> RestartUiAsync(int delay = 3);
	}
}
