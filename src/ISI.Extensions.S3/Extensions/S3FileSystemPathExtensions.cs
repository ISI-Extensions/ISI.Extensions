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
