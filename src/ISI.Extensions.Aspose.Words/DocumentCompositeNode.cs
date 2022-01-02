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
using ISI.Extensions.Aspose.InternalTryNotToUseExtensions;

namespace ISI.Extensions.Aspose
{
	public partial class Words
	{
		public class DocumentCompositeNode : ISI.Extensions.Documents.IDocumentCompositeNode
		{
			internal global::Aspose.Words.CompositeNode _compositeNode = null;

			public DocumentCompositeNode(global::Aspose.Words.CompositeNode compositeNode)
			{
				_compositeNode = compositeNode;
			}

			public string GetText()
			{
				return _compositeNode.GetText();
			}

			public ISI.Extensions.Documents.IDocumentNodeCollection GetChildNodes(ISI.Extensions.Documents.NodeType nodeType, bool isDeep)
			{
				return new DocumentNodeCollection(_compositeNode.GetChildNodes(nodeType.ToNodeType(), isDeep));
			}

			public ISI.Extensions.Documents.IDocumentNode GetChild(ISI.Extensions.Documents.NodeType nodeType, int index, bool isDeep)
			{
				return new DocumentNode(_compositeNode.GetChild(nodeType.ToNodeType(), index, isDeep));
			}

			public ISI.Extensions.Documents.IDocumentNode AppendChild(ISI.Extensions.Documents.IDocumentNode newChild)
			{
				return new DocumentNode(_compositeNode.AppendChild(newChild.GetAsposeNode()));
			}

			public ISI.Extensions.Documents.IDocumentNode PrependChild(ISI.Extensions.Documents.IDocumentNode newChild)
			{
				return new DocumentNode(_compositeNode.PrependChild(newChild.GetAsposeNode()));
			}

			public ISI.Extensions.Documents.IDocumentNode InsertAfter(ISI.Extensions.Documents.IDocumentNode newChild, ISI.Extensions.Documents.IDocumentNode refChild)
			{
				return new DocumentNode(_compositeNode.InsertAfter(newChild.GetAsposeNode(), refChild.GetAsposeNode()));
			}

			public ISI.Extensions.Documents.IDocumentNode InsertBefore(ISI.Extensions.Documents.IDocumentNode newChild, ISI.Extensions.Documents.IDocumentNode refChild)
			{
				return new DocumentNode(_compositeNode.InsertBefore(newChild.GetAsposeNode(), refChild.GetAsposeNode()));
			}

			public ISI.Extensions.Documents.IDocumentNode RemoveChild(ISI.Extensions.Documents.IDocumentNode oldChild)
			{
				return new DocumentNode(_compositeNode.RemoveChild(oldChild.GetAsposeNode()));
			}

			public void RemoveAllChildren()
			{
				_compositeNode.RemoveAllChildren();
			}

			public void RemoveSmartTags()
			{
				_compositeNode.RemoveSmartTags();
			}

			public int IndexOf(ISI.Extensions.Documents.IDocumentNode child)
			{
				return _compositeNode.IndexOf(child.GetAsposeNode());
			}

			public bool IsComposite => _compositeNode.IsComposite;
			public bool HasChildNodes => _compositeNode.HasChildNodes;
			public ISI.Extensions.Documents.IDocumentNodeCollection ChildNodes => new DocumentNodeCollection(_compositeNode.ChildNodes);
			public ISI.Extensions.Documents.IDocumentNode FirstChild => new DocumentNode(_compositeNode.FirstChild);
			public ISI.Extensions.Documents.IDocumentNode LastChild => new DocumentNode(_compositeNode.LastChild);
			public int Count => _compositeNode.Count;
		}
	}
}