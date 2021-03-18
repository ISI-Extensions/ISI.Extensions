#region Copyright & License
/*
Copyright (c) 2021, Integrated Solutions, Inc.
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
using ISI.Extensions.Aspose.Extensions;

namespace ISI.Extensions.Aspose
{
	public partial class Cells
	{
		public class WorksheetComment : ISI.Extensions.SpreadSheets.IWorksheetComment
		{
			internal readonly ISI.Extensions.Aspose.Cells.WorksheetCommentCollection _worksheetComments = null;
			internal readonly global::Aspose.Cells.Comment _comment = null;

			internal WorksheetComment(ISI.Extensions.Aspose.Cells.WorksheetCommentCollection worksheetComments, global::Aspose.Cells.Comment comment)
			{
				_worksheetComments = worksheetComments;
				_comment = comment;
			}

			public string Author
			{
				get => _comment.Author;
				set => _comment.Author = value;
			}

			public int Row => _comment.Row;

			public int Column => _comment.Column;

			public string Note 
			{
				get => _comment.Note;
				set => _comment.Note = value;
			}

			public string HtmlNote
			{
				get => _comment.HtmlNote;
				set => _comment.HtmlNote = value;
			}

			public bool IsVisible
			{
				get => _comment.IsVisible;
				set => _comment.IsVisible = value;
			}

			public bool AutoSize
			{
				get => _comment.AutoSize;
				set => _comment.AutoSize = value;
			}

			public ISI.Extensions.UnitOfMeasure.Distance Width
			{
				get
				{
					var value = new ISI.Extensions.UnitOfMeasure.Distance(ISI.Extensions.UnitOfMeasure.DistanceUnitOfMeasure.Inch, (float)_comment.WidthInch);
					value.OnChange += () => _comment.WidthInch = value.Inches;
					return value;
				}
				set
				{
					_comment.Width = value.Pixels;
					value.OnChange += () => _comment.WidthInch = value.Inches;
				}
			}

			public ISI.Extensions.UnitOfMeasure.Distance Height
			{
				get
				{
					var value = new ISI.Extensions.UnitOfMeasure.Distance(ISI.Extensions.UnitOfMeasure.DistanceUnitOfMeasure.Inch, (float)_comment.HeightInch);
					value.OnChange += () => _comment.HeightInch = value.Inches;
					return value;
				}
				set
				{
					_comment.Height = value.Pixels;
					value.OnChange += () => _comment.HeightInch = value.Inches;
				}
			}
		}
	}
}