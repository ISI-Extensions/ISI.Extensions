﻿#region Copyright & License
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
using System.Text;

namespace ISI.Extensions.Jenkins
{
	public class Job
	{
		public JobAction[] Actions { get; set; }
		public bool IsConcurrentBuild { get; set; }
		public JobProperty[] Properties { get; set; }
		public bool KeepDependencies { get; set; }
		public HealthReport[] HealthReports { get; set; }
		public string Name { get; set; }
		public string Url { get; set; }
		public string DisplayName { get; set; }
		public Build FirstBuild { get; set; }
		public string Description { get; set; }
		public bool Buildable { get; set; }
		public Build[] Builds { get; set; }
		public bool IsInQueue { get; set; }
		public string Color { get; set; }
		public Build LastBuild { get; set; }
		public Build LastCompletedBuild { get; set; }
		public Build LastFailedBuild { get; set; }
		public Build LastStableBuild { get; set; }
		public Build LastSuccessfulBuild { get; set; }
		public Build LastUnstableBuild { get; set; }
		public Build LastUnsuccessfulBuild { get; set; }
		public int NextBuildNumber { get; set; }

		public string BuildRestUrl => this.Url + "build";

		public string RestJsonUrl => this.Url + "api/json";
	}
}
