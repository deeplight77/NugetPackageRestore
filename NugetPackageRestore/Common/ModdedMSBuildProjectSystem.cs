using Microsoft.Build.Construction;
using NuGet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NugetPackageRestore
{
    /// <summary>
    /// This class allows to add Content references to the project of all installed files
    /// </summary>
    public class ModdedMSBuildProjectSystem : MSBuildProjectSystem
    {
        public ModdedMSBuildProjectSystem(string projectFile, bool addReferenceToProject)
            : base(projectFile)
        {
            ProjectPath = projectFile;
            AddReferenceToProject = addReferenceToProject;
        }
        public string ProjectPath { get; private set; }
        public bool AddReferenceToProject { get; private set; }

        /// <summary>
        /// Adds a file to the project, and optionally adds a reference to the project file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="stream"></param>
        public override void AddFile(string path, System.IO.Stream stream)
        {
            base.AddFile(path, stream);

            if (AddReferenceToProject)
            {
                ProjectRootElement rootElement = ProjectRootElement.Open(ProjectPath);
                rootElement.AddItem("Content", path);
            }
        }

    }
}
