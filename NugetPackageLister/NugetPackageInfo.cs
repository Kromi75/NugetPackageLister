// -------------------------------------------------------------------------------
// NugetPackageInfo.cs
// -------------------------------------------------------------------------------
namespace NugetPackageLister
{
  using System.Collections.Generic;
  using System.Diagnostics;

  /// <summary>
  /// Contains information about a Nuget package as it is listed in a package config file plus some context properties.
  /// </summary>
  [DebuggerDisplay("{DebuggerInfo}")]
  internal sealed class NugetPackageInfo
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:NugetPackageInfo"/> class.
    /// </summary>
    /// <param name="fullPath">The full path of the package config file.</param>
    /// <param name="solutionPath">The path of the solution, considering that the project using this package is part of a Visual Studio solution.</param>
    /// <param name="projectName">The project name.</param>
    /// <param name="id">The package id.</param>
    /// <param name="version">The package version.</param>
    /// <param name="targetFramework">The target framework moniker (TFM) to apply when installing the package.</param>
    /// <param name="developDependency">A value indicating whether the package is included in the Nuget package of the consuming project.</param>
    public NugetPackageInfo(string fullPath, string solutionPath, string projectName, string id, string version, string targetFramework, string developDependency)
    {
      this.Id = id;
      this.Version = version;
      this.TargetFramework = targetFramework;
      this.DevelopDependency = developDependency;
      this.FullPath = fullPath;
      this.SolutionPath = solutionPath;
      this.ProjectName = projectName;
    }

    /// <summary>
    /// Gets or sets the package id.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Gets or sets the package version.
    /// </summary>
    public string Version { get; }

    /// <summary>
    /// Gets or sets the target framework moniker (TFM) to apply when installing the package.
    /// </summary>
    public string TargetFramework { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the package is included in the Nuget package of the consuming project.
    /// Is ignored if the consuming project doesn't create a package itself.
    /// </summary>
    public string DevelopDependency { get; }

    /// <summary>
    /// Gets or sets the full path of the package config file.
    /// </summary>
    public string FullPath { get; }

    /// <summary>
    /// Gets or sets the path of the solution, considering that the project using this package is part of a Visual Studio solution.
    /// </summary>
    public string SolutionPath { get; }

    /// <summary>
    /// Gets or sets the project name.
    /// </summary>
    public string ProjectName { get; }

    /// <summary>
    /// Gets a string that is displayed in debugger windows for an instance of this type.
    /// </summary>
    public string DebuggerInfo
    {
      get
      {
        return $"{{FullPath = {this.FullPath}, Id = {this.Id}, Version = {this.Version}, TargetFramework = {this.TargetFramework} }}";
      }
    }

    /// <summary>
    /// Gets a string representation of the current object.
    /// </summary>
    public override string ToString()
    {
      return string.Join(";", new List<string> { this.SolutionPath, this.ProjectName, this.Id, this.Version, this.TargetFramework });
    }
  }
}