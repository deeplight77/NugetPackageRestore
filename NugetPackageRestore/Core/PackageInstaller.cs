using NuGet;
using NuGet.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NugetPackageRestore
{
    public class PackageInstaller
    {
        private static string _flagFileName = "packageInstaller.xst";
        private readonly string _localRepositoryPath;
        private readonly IMSBuildProjectSystem _projectSystem;
        private readonly IProjectManager _projectManager;

        public PackageInstaller(string localRepositoryPath, string projectPath, IConsole console, bool addReferencesToProject)
        {
            _localRepositoryPath                     = localRepositoryPath;
            IPackagePathResolver packagePathResolver = new DefaultPackagePathResolver(localRepositoryPath);
            IPackageRepository localRepository       = new UnzipedLocalPackageRepository(localRepositoryPath, _flagFileName);
            _projectSystem                           = new ModdedMSBuildProjectSystem(projectPath, addReferencesToProject) { Logger = console };
            _projectManager                          = new ProjectManager(localRepository, packagePathResolver, _projectSystem, localRepository) { Logger = console };
        }

        public void InstallPackage(string packageId, string version)
        {
            string folderName = string.Format("{0}.{1}", packageId, version);
            string fullPath = Path.Combine(_localRepositoryPath, folderName, _flagFileName);

            //Create flag file to prevent multiple installations of the same package
            _projectManager.PackageReferenceAdded += (sender, args) => File.Create(fullPath);

            _projectManager.AddPackageReference(packageId, new SemanticVersion(version), false, false);
            _projectSystem.Save();
        }
    }
}
