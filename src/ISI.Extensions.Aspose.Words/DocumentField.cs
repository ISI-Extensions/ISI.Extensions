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
using ISI.Extensions.Aspose.InternalTryNotToUseExtensions;

namespace ISI.Extensions.Aspose
{
	public partial class Words
	{
		public class DocumentField : ISI.Extensions.Documents.IDocumentField
		{
			internal global::Aspose.Words.Fields.Field _field = null;

			public DocumentField(global::Aspose.Words.Fields.Field field)
			{
				_field = field;
			}

			public string GetFieldCode()
			{
				return _field.GetFieldCode();
			}

			public string GetFieldCode(bool includeChildFieldCodes)
			{
				return _field.GetFieldCode(includeChildFieldCodes);
			}

			public ISI.Extensions.Documents.IDocumentNode Remove()
			{
				throw new NotImplementedException();
			}

			public void Update()
			{
				_field.Update();
			}

			public void Update(bool ignoreMergeFormat)
			{
				_field.Update(ignoreMergeFormat);
			}

			public bool Unlink()
			{
				return _field.Unlink();
			}

			public ISI.Extensions.Documents.FieldType Type => _field.Type.ToFieldType();
			public string Result { get => _field.Result; set => _field.Result = value; }
			public bool IsLocked { get => _field.IsLocked; set => _field.IsLocked = value; }
			public int LocaleId { get => _field.LocaleId; set => _field.LocaleId = value; }
		}
	}
}