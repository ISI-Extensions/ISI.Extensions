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

namespace ISI.Extensions
{
	public class LatitudeLongitude : ILatitudeLongitude, IEquatable<LatitudeLongitude>
	{
		public bool Equals(LatitudeLongitude other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((LatitudeLongitude) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Latitude.GetHashCode() * 397) ^ Longitude.GetHashCode();
			}
		}

		public double Latitude { get; set; }

		public double Longitude { get; set; }

		public LatitudeLongitude()
		{

		}
		public LatitudeLongitude(double latitude, double longitude)
		{
			Latitude = latitude;
			Longitude = longitude;
		}
		public LatitudeLongitude(string latitude, string longitude)
		{
			var latitudeLongitude = new LatitudeLongitudeNullable(latitude, longitude);

			if (!latitudeLongitude.HasValue)
			{
				throw new($"\"{latitude}, {longitude}\" not parseable to a Latitude Longitude");
			}

			Latitude = latitudeLongitude.Latitude.GetValueOrDefault();
			Longitude = latitudeLongitude.Longitude.GetValueOrDefault();
		}
		public LatitudeLongitude(string value)
		{
			var latitudeLongitude = new LatitudeLongitudeNullable(value);

			if (!latitudeLongitude.HasValue)
			{
				throw new($"\"{value}\" not parseable to a Latitude Longitude");
			}

			Latitude = latitudeLongitude.Latitude.GetValueOrDefault();
			Longitude = latitudeLongitude.Longitude.GetValueOrDefault();
		}

		public override string ToString()
		{
			return $"Latitude: {Latitude} Longitude: {Longitude}";
		}

		public static ILatitudeLongitude Parse(string value)
		{
			var latitudeLongitude = new LatitudeLongitudeNullable(value);

			if (!latitudeLongitude.HasValue)
			{
				throw new($"\"{value}\" not parseable to a Latitude Longitude");
			}

			return latitudeLongitude.GetValueOrDefault();
		}

		public static LatitudeLongitude Empty => new(0, 0);

		public static bool operator ==(LatitudeLongitude x, LatitudeLongitude y)
		{
			if ((((object)x) == null) && (((object)y) == null))
			{
				return true;
			}

			return ((((object)x) != null) && (((object)y) != null) && (x.Latitude == y.Latitude) && (x.Longitude == y.Longitude));
		}

		public static bool operator !=(LatitudeLongitude x, LatitudeLongitude y)
		{
			return !(x == y);
		}
	}
}
