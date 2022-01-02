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

namespace ISI.Extensions.Documents
{
	public enum FileFormat
	{
		[ISI.Extensions.EnumGuid("00000000-0000-0000-0000-000000000000", "Default", "default")] Default,
		Unknown,
		[ISI.Extensions.EnumGuid("caa25d87-4c1e-4b97-bd1f-24cb04191b5d", "Microsoft Word 97-2003 (.doc)", "doc")] Doc,
		Dot,
		[ISI.Extensions.EnumGuid("3f61e5b6-8e9a-4481-af2f-14ff14f65a07", "Microsoft Word (.docx)", "docx")] Docx,
		Docm,
		Dotx,
		Dotm,
		[ISI.Extensions.EnumGuid("75c4bee1-7ad2-4b23-9520-4bc277a34a46", "Rich Text Format (.rtf)", "rtf")] Rtf,
		[ISI.Extensions.EnumGuid("7f6bf900-a63b-42bd-bc24-3ec642e42989", "Portable Document Format (.pdf)", "pdf")] Pdf,
		[ISI.Extensions.EnumGuid("089b0400-d205-4cbe-ad15-348bba5f6d8f", "XPS Document (.xps)", "xps")] Xps,
		[ISI.Extensions.EnumGuid("9f574086-590f-4f97-b44f-9fd5df7080b4", "HTML", "html")] Html,
		[ISI.Extensions.EnumGuid("a870fcb1-b9ad-4e25-8b39-1604a07635f1", "TIFF Image (.tif)", "tiff")] Tiff,
		[ISI.Extensions.EnumGuid("f9cf3ef5-65dd-45bd-8fd6-aace6af7251b", "PNG Image", "png")] Png,
		Bmp,
		[ISI.Extensions.EnumGuid("94ba666b-d919-4a24-a339-8b409141b716", "JPEG Image", "jpg")] Jpeg,
		[ISI.Extensions.EnumGuid("8952fb98-3bcb-488a-8a4f-a3e848b2905a", "Microsoft Excel 97-2003 (.xls)", "xls")] Xls,
		[ISI.Extensions.EnumGuid("98e8168b-c454-4d79-b609-202d8f6cfda3", "Microsoft Excel (.xlsx)", "xlsx")] Xlsx,
		[ISI.Extensions.EnumGuid("4049b630-75c8-4825-9b8f-883c98b9f614", "Comma Separated Variable (.csv)", "csv")] CommaSeparatedValues,
		[ISI.Extensions.EnumGuid("3ad24a23-8af4-4fcc-ad5d-f5c4b6fe3161", "Tab Delimited (.tab)", "tab")] TabDelimited,
		[ISI.Extensions.EnumGuid("5536ef91-9ea7-42a3-993f-dfda6cc3a759", "Pipe Delimited (.pipe)", "pipe")] PipeDelimited,
		[ISI.Extensions.EnumGuid("e3b67f6a-2f85-4711-812b-388b5e8d5a15", "Microsoft PowerPoint (.pptx)", "pptx")] Pptx,
		[ISI.Extensions.EnumGuid("35d65faa-a219-4595-a8b9-0c412168d20e", "Microsoft PowerPoint 97-2003 (.ppt)", "ppt")] Ppt,
		Ppsx,
		Pps,
	}
}
