#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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
		public static Guid SetDocumentStream(this ISI.Extensions.IDocumentStorage documentStorage, ISI.Extensions.DocumentStorage.IDocument document)
		{
			return documentStorage.SetDocument(document, null);
		}

		public static Guid SetDocumentStream(this ISI.Extensions.IDocumentStorage documentStorage, ISI.Extensions.DocumentStorage.IDocument document, Guid userUuid)
		{
			return documentStorage.SetDocument(document, userUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens));
		}

		public static Guid SetDocumentStream(this ISI.Extensions.IDocumentStorage documentStorage, ISI.Extensions.DocumentStorage.IDocument document, int? userId)
		{
			return documentStorage.SetDocument(document, string.Format("{0}", userId));
		}






		public static void SetDocumentStream(this ISI.Extensions.IDocumentStorage documentStorage, Guid documentUuid, ISI.Extensions.DocumentStorage.IDocument document)
		{
			documentStorage.SetDocument(documentUuid, document, null);
		}

		public static void SetDocumentStream(this ISI.Extensions.IDocumentStorage documentStorage, Guid documentUuid, ISI.Extensions.DocumentStorage.IDocument document, Guid userUuid)
		{
			documentStorage.SetDocument(documentUuid, document, userUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens));
		}

		public static void SetDocumentStream(this ISI.Extensions.IDocumentStorage documentStorage, Guid documentUuid, ISI.Extensions.DocumentStorage.IDocument document, int? userId)
		{
			documentStorage.SetDocument(documentUuid, document, string.Format("{0}", userId));
		}






		public static ISI.Extensions.DocumentStorage.IDocument GetDocumentStream(this ISI.Extensions.IDocumentStorage documentStorage, Guid documentUuid)
		{
			return documentStorage.GetDocument(documentUuid, null);
		}

		public static ISI.Extensions.DocumentStorage.IDocument GetDocumentStream(this ISI.Extensions.IDocumentStorage documentStorage, Guid documentUuid, Guid userUuid)
		{
			return documentStorage.GetDocument(documentUuid, userUuid.Formatted(GuidExtensions.GuidFormat.WithHyphens));
		}

		public static ISI.Extensions.DocumentStorage.IDocument GetDocumentStream(this ISI.Extensions.IDocumentStorage documentStorage, Guid documentUuid, int? userId)
		{
			return documentStorage.GetDocument(documentUuid, string.Format("{0}", userId));
		}

	}
}
