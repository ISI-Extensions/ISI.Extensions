#region Copyright & License
/*
Copyright (c) 2025, Integrated Solutions, Inc.
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

namespace ISI.Extensions.UnitOfMeasure
{
	public delegate void OnChange();

	public class Distance
	{
		public DistanceUnitOfMeasure UnitOfMeasure { get; private set; }
		public float Value { get; private set; }
		internal bool HasValue { get; private set; }

		public event OnChange OnChange = null;

		public Distance()
		{
			UnitOfMeasure = DistanceUnitOfMeasure.Millimeter;
			Value = 0;
			HasValue = false;
		}

		public Distance(DistanceUnitOfMeasure unitOfMeasure, float value)
		{
			SetValue(unitOfMeasure, value);
		}

		public int Pixels
		{
			get => (int) GetValue(DistanceUnitOfMeasure.Pixel);
			set => SetValue(DistanceUnitOfMeasure.Pixel, value);
		}

		public int Points
		{
			get => (int) GetValue(DistanceUnitOfMeasure.Point);
			set => SetValue(DistanceUnitOfMeasure.Point, value);
		}

		public float Inches
		{
			get =>  GetValue(DistanceUnitOfMeasure.Inch);
			set => SetValue(DistanceUnitOfMeasure.Inch, value);
		}

		public int Document
		{
			get => (int) GetValue(DistanceUnitOfMeasure.Document);
			set => SetValue(DistanceUnitOfMeasure.Document, value);
		}

		public int Millimeters
		{
			get => (int) GetValue(DistanceUnitOfMeasure.Millimeter);
			set => SetValue(DistanceUnitOfMeasure.Millimeter, value);
		}


		public void SetValue(DistanceUnitOfMeasure unitOfMeasure, float value)
		{
			UnitOfMeasure = unitOfMeasure;
			Value = value;
			HasValue = true;

			OnChange?.Invoke();
		}


		public float GetValue(DistanceUnitOfMeasure unitOfMeasure)
		{
			if (UnitOfMeasure == unitOfMeasure)
			{
				return Value;
			}

			float inches = Value;

			switch (UnitOfMeasure)
			{
				case DistanceUnitOfMeasure.Point:
					inches = Value / 72f;
					break;
				case DistanceUnitOfMeasure.Inch:
					break;
				case DistanceUnitOfMeasure.Document:
					inches = Value / 300f;
					break;
				case DistanceUnitOfMeasure.Millimeter:
					inches = Value / 25.4f;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			switch (unitOfMeasure)
			{
				case DistanceUnitOfMeasure.Point:
					return inches * 72f;
				case DistanceUnitOfMeasure.Inch:
					return inches;
				case DistanceUnitOfMeasure.Document:
					return inches * 300f;
				case DistanceUnitOfMeasure.Millimeter:
					return inches * 25.4f;
				default:
					throw new ArgumentOutOfRangeException(nameof(unitOfMeasure), unitOfMeasure, null);
			}
		}
	}
}
