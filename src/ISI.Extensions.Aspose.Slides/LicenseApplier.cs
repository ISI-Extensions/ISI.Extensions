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
using ISI.Extensions.Extensions;
using ISI.Extensions.TypeLocator.Extensions;

namespace ISI.Extensions.Aspose.Slides
{
	[ISI.Extensions.LicenseManager.LicenseApplier]
	public class LicenseApplier : ISI.Extensions.LicenseManager.ILicenseApplier
	{
		private static bool _IsLicensed = false;

		static LicenseApplier()
		{
			if (!_IsLicensed)
			{
				var localContainer = ISI.Extensions.TypeLocator.Container.LocalContainer;

				var licenseManagers = localContainer.GetImplementations<ISI.Extensions.Aspose.ICellsLicense>().Cast<ISI.Extensions.LicenseManager.ILicenseStream>();
				
				if (!licenseManagers.Any())
				{
					licenseManagers = localContainer.GetImplementations<ISI.Extensions.Aspose.ITotalLicense>().Cast<ISI.Extensions.LicenseManager.ILicenseStream>();
				}

				if (!licenseManagers.Any())
				{
					throw new("Aspose License not found");
				}

				var licenseManager = licenseManagers.First();

				(new global::Aspose.Slides.License()).SetLicense(licenseManager.GetLicenseStream());

				_IsLicensed = true;
			}
		}

		public void ApplyLicense()
		{
			if (!_IsLicensed)
			{
				throw new("Did not get licensed");
			}
		}

		public bool IsLicensed => _IsLicensed;
	}
}
