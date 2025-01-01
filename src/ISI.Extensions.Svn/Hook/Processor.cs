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
using ISI.Extensions.Extensions;

namespace ISI.Extensions.Svn
{
	public delegate bool OnPostCommit(object sender, string repositoryPath, long revision);
	public delegate bool OnPostLock(object sender, string repositoryPath, string user);
	public delegate bool OnPostRevisionPropertyChange(object sender, string repositoryPath, long revision, string user, string propertyName, Action action);
	public delegate bool OnPostUnlock(object sender, string repositoryPath, string user);
	public delegate bool OnPreCommit(object sender, string repositoryPath, long transaction);
	public delegate bool OnPreLock(object sender, string repositoryPath, string path, string user, string comment, bool stealLock);
	public delegate bool OnPreRevisionPropertyChange(object sender, string repositoryPath, long revision, string user, string propertyName, Action action);
	public delegate bool OnPreUnlock(object sender, string repositoryPath, string path, string user, string token, bool breakUnlock);
	public delegate bool OnStartCommit(object sender, string repositoryPath, string user, string[] capabilities);

	public class Processor
	{
		public event OnPostCommit OnPostCommit = null;
		public event OnPostLock OnPostLock = null;
		public event OnPostRevisionPropertyChange OnPostRevisionPropertyChange = null;
		public event OnPostUnlock OnPostUnlock = null;
		public event OnPreCommit OnPreCommit = null;
		public event OnPreLock OnPreLock = null;
		public event OnPreRevisionPropertyChange OnPreRevisionPropertyChange = null;
		public event OnPreUnlock OnPreUnlock = null;
		public event OnStartCommit OnStartCommit = null;


		public bool Process(string[] args)
		{
			var command = args[0].ToLower();

			switch (command)
			{
				case "post-commit":
					return OnPostCommit?.Invoke(this, args[1].Trim(), args[2].ToLong()) ?? true;

				case "post-lock":
					return OnPostLock?.Invoke(this, args[1].Trim(), args[2].Trim()) ?? true;

				case "post-revprop-change":
					return OnPostRevisionPropertyChange?.Invoke(this, args[1].Trim(), args[2].ToLong(), args[3].Trim(), args[4].Trim(), ISI.Extensions.Enum<Action>.Parse(args[5])) ?? true;

				case "post-unlock":
					return OnPostUnlock?.Invoke(this, args[1].Trim(), args[2].Trim()) ?? true;

				case "pre-commit":
					return OnPreCommit?.Invoke(this, args[1].Trim(), args[2].ToLong()) ?? true;

				case "pre-lock":
					return OnPreLock?.Invoke(this, args[1].Trim(), args[2].Trim(), args[3].Trim(), args[4].Trim(), args[5].ToBoolean()) ?? true;

				case "pre-revprop-change":
					return OnPreRevisionPropertyChange?.Invoke(this, args[1].Trim(), args[2].ToLong(), args[3].Trim(), args[4].Trim(), ISI.Extensions.Enum<Action>.Parse(args[5])) ?? true;

				case "pre-unlock":
					return OnPreUnlock?.Invoke(this, args[1].Trim(), args[2].Trim(), args[3].Trim(), args[4].Trim(), args[5].ToBoolean()) ?? true;

				case "start-commit":
					return OnStartCommit?.Invoke(this, args[1].Trim(), args[2].Trim(), args[3].Split([','], StringSplitOptions.RemoveEmptyEntries, value => value.Trim())) ?? true;
			}

			return true;
		}
	}
}
