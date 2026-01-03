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

namespace ISI.Extensions.Aspose.Extensions
{
	public static partial class CellsExtensions
	{
		public static global::Aspose.Cells.LookAtType? ToLookAt(this ISI.Extensions.SpreadSheets.LookAt? pattern)
		{
			return (pattern.HasValue ? ToLookAt(pattern.GetValueOrDefault()) : (global::Aspose.Cells.LookAtType?)null);
		}

		public static global::Aspose.Cells.LookAtType ToLookAt(this ISI.Extensions.SpreadSheets.LookAt pattern)
		{
			switch (pattern)
			{
				case ISI.Extensions.SpreadSheets.LookAt.Contains: return global::Aspose.Cells.LookAtType.Contains;
				case ISI.Extensions.SpreadSheets.LookAt.StartsWith: return global::Aspose.Cells.LookAtType.StartWith;
				case ISI.Extensions.SpreadSheets.LookAt.EndsWith: return global::Aspose.Cells.LookAtType.EndWith;
			}

			return global::Aspose.Cells.LookAtType.EntireContent;
		}

		public static ISI.Extensions.SpreadSheets.LookAt? ToLookAt(this global::Aspose.Cells.LookAtType? pattern)
		{
			return (pattern.HasValue ? ToLookAt(pattern.GetValueOrDefault()) : (ISI.Extensions.SpreadSheets.LookAt?)null);
		}

		public static ISI.Extensions.SpreadSheets.LookAt ToLookAt(this global::Aspose.Cells.LookAtType pattern)
		{
			switch (pattern)
			{
				case global::Aspose.Cells.LookAtType.Contains: return ISI.Extensions.SpreadSheets.LookAt.Contains;
				case global::Aspose.Cells.LookAtType.StartWith: return ISI.Extensions.SpreadSheets.LookAt.StartsWith;
				case global::Aspose.Cells.LookAtType.EndWith: return ISI.Extensions.SpreadSheets.LookAt.EndsWith;
			}

			return ISI.Extensions.SpreadSheets.LookAt.EntireContent;
		}
	}
}
