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

namespace ISI.Extensions.Aspose
{
	public partial class Pdf
	{
		public partial class PdfDocumentHelper
		{
			public ISI.Extensions.Documents.Pdf.PdfFieldInfo[] GetFieldInfos(ISI.Extensions.Documents.IDocument document)
			{
				document.Stream.Rewind();

				var docDocument = new global::Aspose.Pdf.Document(document.Stream);

				document.Stream.Rewind();

				var form = new global::Aspose.Pdf.Facades.Form(document.Stream);

				var pdfFieldInfos = new List<ISI.Extensions.Documents.Pdf.PdfFieldInfo>();

				foreach (var field in docDocument.Form.Fields)
				{
					var pdfFieldInfo = new ISI.Extensions.Documents.Pdf.PdfFieldInfo()
					{
						FieldName = field.Name,
						Contents = field.Contents,
					};

					if (field is global::Aspose.Pdf.Forms.RadioButtonOptionField radioButtonOptionField)
					{
						var radioButtonOptionFieldIndex = (string.IsNullOrWhiteSpace(radioButtonOptionField.Name) ? $"\"{radioButtonOptionField.OptionName}\"" : radioButtonOptionField.Name);

						pdfFieldInfo.FieldName = $"{radioButtonOptionField.FullName}[{radioButtonOptionFieldIndex}]";
					}

					switch (form.GetFieldType(pdfFieldInfo.FieldName))
					{
						case global::Aspose.Pdf.Facades.FieldType.Text:
							pdfFieldInfo.FieldType = ISI.Extensions.Documents.Pdf.FieldType.Text;
							break;
						case global::Aspose.Pdf.Facades.FieldType.ComboBox:
							pdfFieldInfo.FieldType = ISI.Extensions.Documents.Pdf.FieldType.DropDown;
							break;
						case global::Aspose.Pdf.Facades.FieldType.ListBox:
							pdfFieldInfo.FieldType = ISI.Extensions.Documents.Pdf.FieldType.ListBox;
							break;
						case global::Aspose.Pdf.Facades.FieldType.Radio:
							pdfFieldInfo.FieldType = ISI.Extensions.Documents.Pdf.FieldType.RadioButton;
							break;
						case global::Aspose.Pdf.Facades.FieldType.CheckBox:
							pdfFieldInfo.FieldType = ISI.Extensions.Documents.Pdf.FieldType.CheckBox;
							break;
						case global::Aspose.Pdf.Facades.FieldType.PushButton:
							pdfFieldInfo.FieldType = ISI.Extensions.Documents.Pdf.FieldType.Button;
							break;
						case global::Aspose.Pdf.Facades.FieldType.MultiLineText:
							pdfFieldInfo.FieldType = ISI.Extensions.Documents.Pdf.FieldType.MultiLineText;
							break;
						case global::Aspose.Pdf.Facades.FieldType.Barcode:
							pdfFieldInfo.FieldType = ISI.Extensions.Documents.Pdf.FieldType.Barcode;
							break;
						case global::Aspose.Pdf.Facades.FieldType.InvalidNameOrType:
							pdfFieldInfo.FieldType = ISI.Extensions.Documents.Pdf.FieldType.Unknown;
							break;
						case global::Aspose.Pdf.Facades.FieldType.Signature:
							pdfFieldInfo.FieldType = ISI.Extensions.Documents.Pdf.FieldType.Signature;
							break;
						case global::Aspose.Pdf.Facades.FieldType.Image:
							pdfFieldInfo.FieldType = ISI.Extensions.Documents.Pdf.FieldType.Image;
							break;
						case global::Aspose.Pdf.Facades.FieldType.Numeric:
							pdfFieldInfo.FieldType = ISI.Extensions.Documents.Pdf.FieldType.Numeric;
							break;
						case global::Aspose.Pdf.Facades.FieldType.DateTime:
							pdfFieldInfo.FieldType = ISI.Extensions.Documents.Pdf.FieldType.DateTime;
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					pdfFieldInfos.Add(pdfFieldInfo);
				}

				return pdfFieldInfos.ToArray();
			}
		}
	}
}