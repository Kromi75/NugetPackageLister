// -------------------------------------------------------------------------------
// Program.cs
// -------------------------------------------------------------------------------
namespace NugetPackageLister
{
  using System;
  using System.Diagnostics;
  using System.IO;
  using System.Threading.Tasks;
  using Akka.Actor;
  using NugetPackageLister.Actors;
  using NugetPackageLister.Messages;

  internal class Program
  {
    private static void Main(string[] args)
    {
      CommandLineOptions options = new CommandLineOptions();

      // Note: The help information is automatically printed out if the command line parameters were specified incorrectly.
      if (!CommandLine.Parser.Default.ParseArguments(args, options))
      {
        Program.EndProgram();
        return;
      }

      AppInfo.Options = options;
      string workingFolder = options.WorkingFolder;
      if (string.IsNullOrEmpty(workingFolder))
      {
        workingFolder = Environment.CurrentDirectory;
      }

      if (!Directory.Exists(workingFolder))
      {
        Console.Error.WriteLine($"Folder <{workingFolder}> not found.");
        Program.EndProgram();
        return;
      }

      using (ActorSystem system = ActorSystem.Create("NpsActorSystem"))
      {
        IActorRef workingFolderActor = system.ActorOf(Props.Create<WorkingFolderActor>());
        Task task = workingFolderActor.Ask(new AnalyzeFolderMessage(workingFolder, options.TargetFile));

        task.Wait();
      }

      Program.EndProgram();
    }

    private static void EndProgram()
    {
      if (Debugger.IsAttached)
      {
        Console.WriteLine("Press <Enter>.");
        Console.ReadLine();
      }
    }
  }
}