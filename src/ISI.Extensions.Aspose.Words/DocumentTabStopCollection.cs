#region Copyright & License
/*
Copyright (c) 2023, Integrated Solutions, Inc.
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
		public class DocumentTabStopCollection : ISI.Extensions.Documents.IDocumentTabStopCollection
		{
			internal global::Aspose.Words.TabStopCollection _tabStops = null;

			public DocumentTabStopCollection(global::Aspose.Words.TabStopCollection tabStops)
			{
				_tabStops = tabStops;
			}

			public void Clear()
			{
				_tabStops.Clear();
			}

			public double GetPositionByIndex(int index)
			{
				return _tabStops.GetPositionByIndex(index);
			}

			public int GetIndexByPosition(double position)
			{
				return _tabStops.GetIndexByPosition(position);
			}

			public void Add(ISI.Extensions.Documents.IDocumentTabStop tabStop)
			{
				_tabStops.Add(tabStop.GetAsposeTabStop());
			}

			public void Add(double position, ISI.Extensions.Documents.TabAlignment alignment, ISI.Extensions.Documents.TabLeader leader)
			{
				_tabStops.Add(position, alignment.ToTabAlignment(), leader.ToTabLeader());
			}

			public void RemoveByPosition(double position)
			{
				_tabStops.RemoveByPosition(position);
			}

			public void RemoveByIndex(int index)
			{
				_tabStops.RemoveByIndex(index);
			}

			public ISI.Extensions.Documents.IDocumentTabStop After(double position)
			{
				return new DocumentTabStop(_tabStops.After(position));
			}

			public ISI.Extensions.Documents.IDocumentTabStop Before(double position)
			{
				return new DocumentTabStop(_tabStops.Before(position));
			}

			public int Count => _tabStops.Count;

			public ISI.Extensions.Documents.IDocumentTabStop this[int index] => new DocumentTabStop(_tabStops[index]);
			public ISI.Extensions.Documents.IDocumentTabStop this[double position] => new DocumentTabStop(_tabStops[position]);
		}
	}
}