// -------------------------------------------------------------------------------
// WorkingFolderActor.cs
// -------------------------------------------------------------------------------
namespace NugetPackageLister.Actors
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using Akka.Actor;
  using NugetPackageLister.Messages;

  /// <summary>
  /// The entry actor. There's only one instance of this one and it creates child actors to
  /// search a folder and its subfolders for Nuget package configuration files.
  /// </summary>
  internal sealed class WorkingFolderActor : ReceiveActor
  {
    private List<IActorRef> childActors;

    private IActorRef initiator;

    private readonly IActorRef processResultActor;

    public WorkingFolderActor()
    {
      // There's only one WriteResultActor to avoid concurrency issues.
      this.processResultActor = UntypedActor.Context.ActorOf(Props.Create<WriteResultActor>());

      this.Receive<AnalyzeFolderMessage>(m => this.ProcessAnalyzeFolderMessage(m));

      // All child actors need to report back after they are done in order to have the process running
      // until everything is written to the output target.
      this.Receive<PackageConfigProcessedMessage>(m => this.ProcessPackageConfigProcessedMessage());
    }

    private void ProcessPackageConfigProcessedMessage()
    {
      if (this.childActors.Contains(UntypedActor.Context.Sender))
      {
        this.childActors.Remove(UntypedActor.Context.Sender);
      }

      if (!this.childActors.Any())
      {
        this.processResultActor.Tell(new TearDownMessage());
        this.initiator.Tell(new WorkCompletedMessage());
      }
    }

    private void ProcessAnalyzeFolderMessage(AnalyzeFolderMessage message)
    {
      if (message == null)
      {
        return;
      }

      this.initiator = UntypedActor.Context.Sender;
      this.childActors = new List<IActorRef>();
      List<string> packageConfigFiles = this.FindPackageConfigFiles(message.Path);
      if (!packageConfigFiles.Any())
      {
        Console.WriteLine($"No packages.config files found in folder <{message.Path}>.");
        this.initiator.Tell(new WorkCompletedMessage());
        return;
      }

      foreach (string packageConfigPath in packageConfigFiles)
      {
        AnalyzePackageConfigMessage configMessage = new AnalyzePackageConfigMessage(packageConfigPath, this.processResultActor);
        IActorRef actor = UntypedActor.Context.ActorOf(Props.Create<PackageConfigActor>());
        this.childActors.Add(actor);
        actor.Tell(configMessage);
      }
    }

    private List<string> FindPackageConfigFiles(string workingFolder)
    {
      string[] repositoryFiles = Directory.GetFiles(workingFolder, "packages.config", SearchOption.AllDirectories);
      return repositoryFiles.ToList();
    }
  }
}