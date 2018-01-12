// -------------------------------------------------------------------------------
// WriteResultMessage.cs
// -------------------------------------------------------------------------------
namespace NugetPackageLister.Messages
{
  internal sealed class WriteResultMessage
  {
    public WriteResultMessage(NugetPackageInfo nugetPackageInfo)
    {
      this.NugetPackageInfo = nugetPackageInfo;
    }

    public NugetPackageInfo NugetPackageInfo { get; }
  }
}