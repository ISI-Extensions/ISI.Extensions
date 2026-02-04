using System;
using System.Collections.Generic;
using System.Text;

namespace ISI.Extensions.S3
{
  public class BlobInfo
  {
	  public string FullName { get; set; }
	  public string LastModified { get; set; }

	  public string ETag { get; set; }

		public ulong Size { get; set; }

	  public bool IsDirectory { get; set; }

	  public string VersionId { get; set; }
	  public string ContentType { get; set; }
	  public string Expires { get; set; }
	  public IDictionary<string, string> UserMetadata { get; set; }

	  public bool IsLatest { get; set; }

	  public DateTime? LastModifiedDateTime { get; set; }

	  public override string ToString() => $"{(IsDirectory ? "dir => " : string.Empty)}{FullName}";
  }
}
