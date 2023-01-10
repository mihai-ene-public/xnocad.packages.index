using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexBuilder.Models;

public class RepositoryInfo
{
    public string PackageId { get; set; }
    public string UserName { get; set; }
    public string RepositoryName { get; set; }
    public string BranchName { get; set; }
    public string FolderName { get; set; }
}
