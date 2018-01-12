// -------------------------------------------------------------------------------
// PackageActor.cs
// -------------------------------------------------------------------------------
namespace NugetPackageLister.Actors
{
  using Akka.Actor;
  using NugetPackageLister.Messages;

  /// <summary>
  /// Pipes package information to the write actor to have it written to the output target and reports back after writing is done.
  /// </summary>
  internal sealed class PackageActor : ReceiveActor
  {
    private IActorRef initiator;

    public PackageActor()
    {
      this.Receive<WritePackageMessage>(m => this.ProcessWritePackageMessage(m));

      // The write actor needs to report back after it is done with this package in order to have the process running
      // until everything is written to the output target.
      this.Receive<PackageWrittenMessage>(m => this.ProcessPackageWrittenMessage());
    }

    private void ProcessPackageWrittenMessage()
    {
      this.initiator.Tell(new PackageWrittenMessage());
    }

    private void ProcessWritePackageMessage(WritePackageMessage message)
    {
      this.initiator = UntypedActor.Context.Sender;
      message.ProcessResultActor.Tell(new WriteResultMessage(message.NugetPackageInfo));
    }
  }
}