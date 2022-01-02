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

namespace ISI.Extensions.Extensions
{
	public static class DocumentStorageExtensions
	{
		public static async Task<Guid> SetDocumentStreamAsync(this ISI.Extensions.IDocumentStorage documentStorage, ISI.Extensions.DocumentStorage.IDocument document)
		{
			return await documentStorage.SetDocumentAsync(document, null);
		}

		public static async Task<Guid> SetDocumentStreamAsync(this ISI.Extensions.IDocumentStorage documentStorage, ISI.Extensions.DocumentStorage.IDocument document, Guid userUuid)
		{
			return await documentStorage.SetDocumentAsync(document, userUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens));
		}

		public static async Task<Guid> SetDocumentStreamAsync(this ISI.Extensions.IDocumentStorage documentStorage, ISI.Extensions.DocumentStorage.IDocument document, int? userId)
		{
			return await documentStorage.SetDocumentAsync(document, string.Format("{0}", userId));
		}






		public static async Task SetDocumentStreamAsync(this ISI.Extensions.IDocumentStorage documentStorage, Guid documentUuid, ISI.Extensions.DocumentStorage.IDocument document)
		{
			await documentStorage.SetDocumentAsync(documentUuid, document, null);
		}

		public static async Task SetDocumentStreamAsync(this ISI.Extensions.IDocumentStorage documentStorage, Guid documentUuid, ISI.Extensions.DocumentStorage.IDocument document, Guid userUuid)
		{
			await documentStorage.SetDocumentAsync(documentUuid, document, userUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens));
		}

		public static async Task SetDocumentStreamAsync(this ISI.Extensions.IDocumentStorage documentStorage, Guid documentUuid, ISI.Extensions.DocumentStorage.IDocument document, int? userId)
		{
			await documentStorage.SetDocumentAsync(documentUuid, document, string.Format("{0}", userId));
		}






		public static async Task<ISI.Extensions.DocumentStorage.IDocument> GetDocumentStreamAsync(this ISI.Extensions.IDocumentStorage documentStorage, Guid documentUuid)
		{
			return await documentStorage.GetDocumentAsync(documentUuid, null);
		}

		public static async Task<ISI.Extensions.DocumentStorage.IDocument> GetDocumentStreamAsync(this ISI.Extensions.IDocumentStorage documentStorage, Guid documentUuid, Guid userUuid)
		{
			return await documentStorage.GetDocumentAsync(documentUuid, userUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens));
		}

		public static async Task<ISI.Extensions.DocumentStorage.IDocument> GetDocumentStreamAsync(this ISI.Extensions.IDocumentStorage documentStorage, Guid documentUuid, int? userId)
		{
			return await documentStorage.GetDocumentAsync(documentUuid, string.Format("{0}", userId));
		}
	}
}
