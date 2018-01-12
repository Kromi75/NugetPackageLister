// -------------------------------------------------------------------------------
// AnalyzePackageConfigMessage.cs
// -------------------------------------------------------------------------------
namespace NugetPackageLister.Messages
{
  using Akka.Actor;

  internal sealed class AnalyzePackageConfigMessage
  {
    public AnalyzePackageConfigMessage(string packageConfigPath, IActorRef processResultActor)
    {
      this.PackageConfigPath = packageConfigPath;
      this.ProcessResultActor = processResultActor;
    }

    public string PackageConfigPath { get; }

    public IActorRef ProcessResultActor { get; }
  }
}