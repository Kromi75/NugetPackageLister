// -------------------------------------------------------------------------------
// WritePackageMessage.cs
// -------------------------------------------------------------------------------
namespace NugetPackageLister.Messages
{
  using Akka.Actor;

  internal sealed class WritePackageMessage
  {
    public WritePackageMessage(NugetPackageInfo nugetPackageInfo, IActorRef processResultActor)
    {
      this.NugetPackageInfo = nugetPackageInfo;
      this.ProcessResultActor = processResultActor;
    }

    public NugetPackageInfo NugetPackageInfo { get; }

    public IActorRef ProcessResultActor { get; }
  }
}