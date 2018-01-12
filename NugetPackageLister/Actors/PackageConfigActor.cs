// -------------------------------------------------------------------------------
// PackageConfigActor.cs
// -------------------------------------------------------------------------------
namespace NugetPackageLister.Actors
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Xml.Linq;
  using Akka.Actor;
  using NugetPackageLister.Messages;

  /// <summary>
  /// Analyzes a packages.config file regarding contained package information and reports back after all package information
  /// has been written to the output target.
  /// </summary>
  internal sealed class PackageConfigActor : ReceiveActor
  {
    private List<IActorRef> childActors;

    private IActorRef initiator;

    public PackageConfigActor()
    {
      this.Receive<AnalyzePackageConfigMessage>(m => this.ProcessAnalyzePackageConfigMessage(m));

      // All child actors need to report back after they are done in order to have the process running
      // until everything is written to the output target.
      this.Receive<PackageWrittenMessage>(m => this.ProcessPackageWrittenMessage());
    }

    private void ProcessPackageWrittenMessage()
    {
      this.childActors.Remove(UntypedActor.Context.Sender);
      if (!this.childActors.Any())
      {
        this.initiator.Tell(new PackageConfigProcessedMessage());
      }
    }

    private void ProcessAnalyzePackageConfigMessage(AnalyzePackageConfigMessage message)
    {
      this.initiator = UntypedActor.Context.Sender;
      string packageConfigPath = message.PackageConfigPath;
      if (!File.Exists(packageConfigPath))
      {
        Console.Error.WriteLine($"PackageConfigActor: File {packageConfigPath} not found.");
        this.initiator.Tell(new PackageConfigProcessedMessage());
        return;
      }

      string projectPath = Path.GetDirectoryName(packageConfigPath);
      string solutionPath = Path.GetDirectoryName(projectPath);
      string projectName = Path.GetFileName(projectPath);
      XDocument document = XDocument.Load(packageConfigPath);
      IEnumerable<NugetPackageInfo> packages = from XElement element in document.Descendants()
                                               where element.Name == "package"
                                               select new NugetPackageInfo(
                                                 packageConfigPath,
                                                 solutionPath,
                                                 projectName,
                                                 element.Attribute("id")?.Value,
                                                 element.Attribute("version")?.Value,
                                                 element.Attribute("targetFramework")?.Value,
                                                 element.Attribute("developDependency")?.Value);

      this.childActors = new List<IActorRef>();
      foreach (NugetPackageInfo nugetPackageInfo in packages)
      {
        IActorRef packageActor = UntypedActor.Context.ActorOf(Props.Create<PackageActor>());
        this.childActors.Add(packageActor);
        packageActor.Tell(new WritePackageMessage(nugetPackageInfo, message.ProcessResultActor));
      }
    }
  }
}