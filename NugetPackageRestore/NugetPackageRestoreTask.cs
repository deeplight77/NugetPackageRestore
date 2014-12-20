using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet;


namespace NugetPackageRestore
{
    public class NugetPackageRestoreTask : Task
    {
        private string _configFileFullPath;
        private string _projectFileFullPath;
        private bool? _addReferencesToProject;

        [Required]
        public string SolutionDir { get; set; }

        [Required]
        public string ProjectDir { get; set; }

        [Required]
        public string PackagesDir { get; set; }

        public bool AddContentReferencesToProject
        {
            get
            {
                return _addReferencesToProject.HasValue ? _addReferencesToProject.Value : false;
            }
            set { _addReferencesToProject = value; }
        }

        public string ConfigFileFullPath
        {
            get
            {
                return _configFileFullPath ?? Path.Combine(ProjectDir, "packages.config");
            }
            set { _configFileFullPath = value; }
        }

        public string ProjectFileFullPath
        {
            get
            {
                return _projectFileFullPath ?? Directory.GetFiles(ProjectDir, "*.csproj").FirstOrDefault();
            }
            set { _projectFileFullPath = value; }
        }

        #region Task Implementation
        public override bool Execute()
        {
            //Uncomment the following line for debugging
            System.Diagnostics.Debugger.Launch();
            if (!IsValidInput())
            {
                return false;
            }

            PackageInstaller installer = new PackageInstaller(PackagesDir, ProjectFileFullPath, new MsBuildConsole(Log), AddContentReferencesToProject);

            Log.LogMessage(MessageImportance.Low, "NugetPackageRestore :: SolutionDir='{0}'", SolutionDir);
            Log.LogMessage(MessageImportance.Low, "NugetPackageRestore :: ProjectDir='{0}'", ProjectDir);
            Log.LogMessage(MessageImportance.Low, "NugetPackageRestore :: ConfigFileFullPath='{0}'", ConfigFileFullPath);

            // Get NuGet Package Configuration
            var packages = GetPackages();
            foreach (PackageInfo package in packages)
            {
                String packageFullPath = Path.Combine(PackagesDir, package.FolderName);
                Log.LogMessage(MessageImportance.Low, "NugetPackageRestore :: {0} :: FullPath='{1}'", package.FolderName, packageFullPath);
                
                if (!Directory.Exists(packageFullPath))
                {
                    Log.LogMessage(MessageImportance.Low, "NugetPackageRestore :: Package not found!! :: {0}", package.FolderName);
                    continue;
                }

                installer.InstallPackage(package.Name, package.Version);
            }

            return true;
        }

        #endregion

        #region Private Methods

        private bool IsValidInput()
        {
            if (!Directory.Exists(SolutionDir))
            {
                Log.LogError("NugetPackageRestore :: Could not find directory SolutionDir='{0}'", SolutionDir);
                return false;
            }

            if (!Directory.Exists(ProjectDir))
            {
                Log.LogError("NugetPackageRestore :: Could not find directory ProjectDir='{0}'", ProjectDir);
                return false;
            }

            if (!Directory.Exists(PackagesDir))
            {
                Log.LogError("NugetPackageRestore :: Could not find directory PackagesDir='{0}'", PackagesDir);
                return false;
            }

            if (!File.Exists(ConfigFileFullPath))
            {
                Log.LogError("NugetPackageRestore :: Could not find file packages.config='{0}'", ConfigFileFullPath);
                return false;
            }

            if (!File.Exists(ProjectFileFullPath))
            {
                Log.LogError("NugetPackageRestore :: Could not find project file!! (.csproj)='{0}'", ProjectFileFullPath);
                return false;
            }

            return true;
        }

        private IEnumerable<PackageInfo> GetPackages()
        {
            List<PackageInfo> packages;
            var serializer = new XmlSerializer(typeof(NuGetPackageConfig));
            using (var reader = new StreamReader(ConfigFileFullPath))
            {
                var nugetConfiguration = (NuGetPackageConfig)serializer.Deserialize(reader);
                packages = nugetConfiguration.Packages;
            }
            return packages;
        }
        #endregion

    }
}
