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

namespace ISI.Extensions.SpreadSheets
{
	/// <summary>
	/// ColumnNumber = 0 => A
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	public class ColumnAttribute : Attribute
	{
		public string ColumnName { get; set; }
		public int ColumnOffset { get; }

		public string HeaderCaption { get; set; }
		public string HeaderStyleName { get; set; }

		public string StyleName { get; set; }

		private double? _width = null;

		public double Width
		{
			get => _width.GetValueOrDefault();
			set => _width = value;
		}

		[global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
		public bool HasWidthValue => _width.HasValue;

		public string Formula { get; set; }

		public string FooterCaption { get; set; }
		public string FooterStyleName { get; set; }
		public string FooterFormula { get; set; }

		/// <summary>
		/// columnNumber = 0 => A
		/// </summary>
		public ColumnAttribute(int columnOffset)
		{
			ColumnOffset = columnOffset;
		}

		public ColumnAttribute()
		{
			ColumnOffset = -1;
		}
	}

	[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	public class ColumnImageAttribute : ColumnAttribute
	{
		public ColumnImageAttribute(int columnOffset) 
			: base(columnOffset)
		{
		}

		public ColumnImageAttribute()
			: base()
		{
		}
	}
}