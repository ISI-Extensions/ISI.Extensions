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

namespace ISI.Extensions.S3
{
  public static class S3FileSystemPathExtensions
  {
	  public static string BucketName(this ISI.Extensions.S3.S3FileSystem.IS3FileSystemPath s3FileSystemPath)
	  {
			return (s3FileSystemPath.Directory ?? string.Empty).Split([s3FileSystemPath.DirectorySeparator], StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
	  }

	  public static string DirectoryWithoutBucketName(this ISI.Extensions.S3.S3FileSystem.IS3FileSystemPath s3FileSystemPath)
	  {
		  var directoryPieces = new List<string>((s3FileSystemPath.Directory ?? string.Empty).Split([s3FileSystemPath.DirectorySeparator], StringSplitOptions.RemoveEmptyEntries));
		  directoryPieces.RemoveAt(0);
		  return string.Join(s3FileSystemPath.DirectorySeparator, directoryPieces);
	  }

		public static string FullPathWithoutBucketName(this ISI.Extensions.S3.S3FileSystem.IS3FileSystemPath s3FileSystemPath)
	  {
		  var fullPathBuilder = new System.Text.StringBuilder();

		  if (s3FileSystemPath.IsRoot)
		  {
			  fullPathBuilder.Append(s3FileSystemPath.DirectorySeparator);
		  }

		  var directory = s3FileSystemPath.DirectoryWithoutBucketName();
		  if (!string.IsNullOrWhiteSpace(directory))
		  {
			  fullPathBuilder.Append(directory);

			  if (!directory.EndsWith(s3FileSystemPath.DirectorySeparator))
			  {
				  fullPathBuilder.Append(s3FileSystemPath.DirectorySeparator);
			  }
		  }

		  if (!string.IsNullOrWhiteSpace(s3FileSystemPath.PathName))
		  {
			  fullPathBuilder.Append(s3FileSystemPath.PathName);
		  }

		  return fullPathBuilder.ToString();
	  }

	}
}
