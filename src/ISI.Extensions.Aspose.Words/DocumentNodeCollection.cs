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
using ISI.Extensions.Extensions;
using ISI.Extensions.Aspose.InternalTryNotToUseExtensions;

namespace ISI.Extensions.Aspose
{
	public partial class Words
	{
		public class DocumentNodeCollection : ISI.Extensions.Documents.IDocumentNodeCollection
		{
			internal global::Aspose.Words.NodeCollection _nodes = null;

			public DocumentNodeCollection(global::Aspose.Words.NodeCollection nodes)
			{
				_nodes = nodes;
			}

			public void Add(ISI.Extensions.Documents.IDocumentNode node)
			{
				_nodes.Add(node.GetAsposeNode());
			}

			public void Insert(int index, ISI.Extensions.Documents.IDocumentNode node)
			{
				_nodes.Insert(index, node.GetAsposeNode());
			}

			public void Remove(ISI.Extensions.Documents.IDocumentNode node)
			{
				_nodes.Remove(node.GetAsposeNode());
			}

			public void RemoveAt(int index)
			{
				_nodes.RemoveAt(index);
			}

			public void Clear()
			{
				_nodes.Clear();
			}

			public bool Contains(ISI.Extensions.Documents.IDocumentNode node)
			{
				return _nodes.Contains(node.GetAsposeNode());
			}

			public int IndexOf(ISI.Extensions.Documents.IDocumentNode node)
			{
				return _nodes.IndexOf(node.GetAsposeNode());
			}

			public ISI.Extensions.Documents.IDocumentNode[] ToArray()
			{
				return _nodes.ToArray().ToNullCheckedArray(node => new DocumentNode(node));
			}

			public ISI.Extensions.Documents.IDocumentNode this[int index] => new DocumentNode(_nodes[index]);

			public int Count => _nodes.Count;
		}
	}
}