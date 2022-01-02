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
		public class DocumentNode : ISI.Extensions.Documents.IDocumentNode
		{
			internal global::Aspose.Words.Node _node = null;

			public DocumentNode(global::Aspose.Words.Node node)
			{
				_node = node;
			}

			public ISI.Extensions.Documents.NodeType NodeType => _node.NodeType.ToNodeType();
			public ISI.Extensions.Documents.IDocumentCompositeNode ParentNode => new DocumentCompositeNode(_node.ParentNode);
			public ISI.Extensions.Documents.IDocumentNode PreviousSibling => new DocumentNode(_node.PreviousSibling);
			public ISI.Extensions.Documents.IDocumentNode NextSibling => new DocumentNode(_node.NextSibling);
			public bool IsComposite => _node.IsComposite;
			public ISI.Extensions.Documents.IDocumentRange Range => new DocumentRange(_node.Range);
			public ISI.Extensions.Documents.IDocumentNode Clone(bool isCloneChildren)
			{
				return new DocumentNode(_node.Clone(isCloneChildren));
			}

			public string GetText()
			{
				return _node.GetText();
			}

			public void Remove()
			{
				_node.Remove();
			}

			public ISI.Extensions.Documents.IDocumentNode NextPreOrder(ISI.Extensions.Documents.IDocumentNode rootNode)
			{
				return new DocumentNode(_node.NextPreOrder(rootNode.GetAsposeNode()));
			}

			public ISI.Extensions.Documents.IDocumentNode PreviousPreOrder(ISI.Extensions.Documents.IDocumentNode rootNode)
			{
				return new DocumentNode(_node.PreviousPreOrder(rootNode.GetAsposeNode()));
			}
		}
	}
}