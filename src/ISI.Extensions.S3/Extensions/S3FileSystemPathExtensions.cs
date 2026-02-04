using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.S3.Extensions
{
  public static class S3FileSystemPathExtensions
  {
	  public static string GetFullName(this ISI.Extensions.S3.S3FileSystem.IS3FileSystemPath s3FileSystemPath) => (!string.IsNullOrWhiteSpace(s3FileSystemPath.Directory) ? $"{s3FileSystemPath.Directory}{s3FileSystemPath.DirectorySeparator}{s3FileSystemPath.PathName}" : s3FileSystemPath.PathName);
  }
}
