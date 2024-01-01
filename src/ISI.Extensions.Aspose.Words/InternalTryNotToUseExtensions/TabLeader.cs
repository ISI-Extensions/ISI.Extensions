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

namespace ISI.Extensions.Aspose.InternalTryNotToUseExtensions
{
	public static partial class WordsExtensions
	{
		public static ISI.Extensions.Documents.TabLeader ToTabLeader(this global::Aspose.Words.TabLeader tabLeader)
		{
			switch (tabLeader)
			{
				case global::Aspose.Words.TabLeader.None: return ISI.Extensions.Documents.TabLeader.None;
				case global::Aspose.Words.TabLeader.Dots: return ISI.Extensions.Documents.TabLeader.Dots;
				case global::Aspose.Words.TabLeader.Dashes: return ISI.Extensions.Documents.TabLeader.Dashes;
				case global::Aspose.Words.TabLeader.Line: return ISI.Extensions.Documents.TabLeader.Line;
				case global::Aspose.Words.TabLeader.Heavy: return ISI.Extensions.Documents.TabLeader.Heavy;
				case global::Aspose.Words.TabLeader.MiddleDot: return ISI.Extensions.Documents.TabLeader.MiddleDot;
				default:
					throw new ArgumentOutOfRangeException(nameof(tabLeader), tabLeader, null);
			}
		}

		public static global::Aspose.Words.TabLeader ToTabLeader(this ISI.Extensions.Documents.TabLeader tabLeader)
		{
			switch (tabLeader)
			{
				case ISI.Extensions.Documents.TabLeader.None: return global::Aspose.Words.TabLeader.None;
				case ISI.Extensions.Documents.TabLeader.Dots: return global::Aspose.Words.TabLeader.Dots;
				case ISI.Extensions.Documents.TabLeader.Dashes: return global::Aspose.Words.TabLeader.Dashes;
				case ISI.Extensions.Documents.TabLeader.Line: return global::Aspose.Words.TabLeader.Line;
				case ISI.Extensions.Documents.TabLeader.Heavy: return global::Aspose.Words.TabLeader.Heavy;
				case ISI.Extensions.Documents.TabLeader.MiddleDot: return global::Aspose.Words.TabLeader.MiddleDot;
				default:
					throw new ArgumentOutOfRangeException(nameof(tabLeader), tabLeader, null);
			}
		}
	}
}