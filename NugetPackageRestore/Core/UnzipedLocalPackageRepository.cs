using NuGet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NugetPackageRestore
{
    /* The problem with the default LocalPackageRepository class is that it returns true for its Exists method whenever a package physically exists in a folder
        So, if a package exists on disk, but isn't referenced by the project, ProjectManager skips it and doesn't add references to the project */

    /* This class makes sure that we skip a package only if it exists on disk and its flagged as installed by this task, if a file its uninstalled using Nuget Console or the VS Package Manager
           the folder and thus the flagging file will be removed, allowing us to reinstall on the next Task run. Only case not covered would be manual uninstalls of files */

    /// <summary>
    /// Allows to skip a package only if its flagged as installed by this Task
    /// </summary>
    public class UnzipedLocalPackageRepository : LocalPackageRepository
    {
        public UnzipedLocalPackageRepository(string physicalPath, string flagFileName) : base(physicalPath) 
        {
            FlagFileName = flagFileName;
            LocalRepositoryPath = physicalPath;
        }
        public string LocalRepositoryPath { get; private set; }
        public string FlagFileName { get; private set; }

        /// <summary>
        /// Checks if a package has been installed in our project by looking at the flag file .xst
        /// </summary>
        /// <param name="packageId"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public override bool Exists(string packageId, SemanticVersion version)
        {
            string folderName = string.Format("{0}.{1}", packageId, version);
            string fullPath = Path.Combine(LocalRepositoryPath, folderName, FlagFileName);

            //Here we return false for all packages flagged as non-existant, or true for allready installed packages
            if (File.Exists(fullPath))
            {
                return true;
            }

            return false;
        }
    }
}
