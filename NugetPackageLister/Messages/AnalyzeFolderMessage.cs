// -------------------------------------------------------------------------------
// AnalyzeFolderMessage.cs
// -------------------------------------------------------------------------------
namespace NugetPackageLister.Messages
{
  internal sealed class AnalyzeFolderMessage
  {
    public AnalyzeFolderMessage(string path, string targetFile)
    {
      this.Path = path;
      this.TargetFile = targetFile;
    }

    public string Path { get; }

    public string TargetFile { get; }
  }
}