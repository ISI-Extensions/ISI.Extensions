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

namespace ISI.Extensions.SpreadSheets
{
	public enum FileFormat
	{
		[ISI.Extensions.EnumGuid("00000000-0000-0000-0000-000000000000", "Default", "default")] Default,
		[ISI.Extensions.EnumGuid("4049b630-75c8-4825-9b8f-883c98b9f614", "Comma Separated Variable (.csv)", "csv")] CommaSeparatedValues,
		[ISI.Extensions.EnumGuid("3ad24a23-8af4-4fcc-ad5d-f5c4b6fe3161", "Tab Delimited (.tab)", "tab")] TabDelimited,
		[ISI.Extensions.EnumGuid("7f6bf900-a63b-42bd-bc24-3ec642e42989", "Portable Document Format (.pdf)", "pdf")] Pdf,
		[ISI.Extensions.EnumGuid("9f574086-590f-4f97-b44f-9fd5df7080b4", "HTML", "html")] Html,
		[ISI.Extensions.EnumGuid("8952fb98-3bcb-488a-8a4f-a3e848b2905a", "Microsoft Excel 97-2003 (.xls)", "xls")] Xls,
		[ISI.Extensions.EnumGuid("98e8168b-c454-4d79-b609-202d8f6cfda3", "Microsoft Excel (.xlsx)", "xlsx")] Xlsx,
	}
}
