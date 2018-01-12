// -------------------------------------------------------------------------------
// AppInfo.cs
// -------------------------------------------------------------------------------
namespace NugetPackageLister
{
  /// <summary>
  /// Allows access to static application information like the command line options.
  /// </summary>
  internal static class AppInfo
  {
    /// <summary>
    /// Gets or sets the object representing the command line options.
    /// </summary>
    public static CommandLineOptions Options { get; set; }
  }
}