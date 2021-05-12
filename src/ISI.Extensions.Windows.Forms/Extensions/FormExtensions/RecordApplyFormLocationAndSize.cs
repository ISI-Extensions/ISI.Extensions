﻿#region Copyright & License
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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Windows.Forms.Extensions
{
	public static partial class FormExtensions
	{
		public static ISI.Extensions.Windows.Forms.FormLocationAndSize[] RecordApplyFormLocationAndSize(this System.Windows.Forms.Form form, IEnumerable<ISI.Extensions.Windows.Forms.FormLocationAndSize> formLocationAndSizes)
		{
			return RecordApplyFormLocationAndSize(form, form.Name, formLocationAndSizes);
		}

		public static ISI.Extensions.Windows.Forms.FormLocationAndSize[] RecordApplyFormLocationAndSize(this System.Windows.Forms.Form form, string formName, IEnumerable<ISI.Extensions.Windows.Forms.FormLocationAndSize> formLocationAndSizes)
		{
			var formLocationAndSize = formLocationAndSizes.FirstOrDefault(formLocationAndSize => string.Equals(formLocationAndSize.FormName, formName, StringComparison.InvariantCultureIgnoreCase));

			if (formLocationAndSize == null)
			{
				formLocationAndSize = new ISI.Extensions.Windows.Forms.FormLocationAndSize()
				{
					FormName = formName,
					Left = form.Left,
					Top = form.Top,
					Width = form.Width,
					Height = form.Height,
				};

				if (!(formLocationAndSizes is List<ISI.Extensions.Windows.Forms.FormLocationAndSize>))
				{
					formLocationAndSizes = formLocationAndSizes.ToNullCheckedList(NullCheckCollectionResult.Empty);
				}

				((List<ISI.Extensions.Windows.Forms.FormLocationAndSize>)formLocationAndSizes).Add(formLocationAndSize);
			}
			else
			{
				RecordApplyFormLocationAndSize(form, formLocationAndSize);
			}

			return formLocationAndSizes.ToArray();
		}

		public static void RecordApplyFormLocationAndSize(this System.Windows.Forms.Form form, ISI.Extensions.Windows.Forms.FormLocationAndSize formLocationAndSize)
		{
			formLocationAndSize.Left = form.Left;
			formLocationAndSize.Top = form.Top;
			formLocationAndSize.Width = form.Width;
			formLocationAndSize.Height = form.Height;
		}
	}
}