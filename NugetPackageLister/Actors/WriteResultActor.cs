// -------------------------------------------------------------------------------
// WriteResultActor.cs
// -------------------------------------------------------------------------------
namespace NugetPackageLister.Actors
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using Akka.Actor;
  using NugetPackageLister.Messages;

  /// <summary>
  /// Writes package information either to a CSV file or to the std-out stream.
  /// </summary>
  internal sealed class WriteResultActor : ReceiveActor
  {
    private StreamWriter writer;

    private bool writeToStdOut;

    public WriteResultActor()
    {
      this.Initialize();
    }

    protected override void PostStop()
    {
      this.TearDown();
      base.PostStop();
    }

    private void Initialize()
    {
      string outputFile = AppInfo.Options?.TargetFile;
      if (!string.IsNullOrEmpty(outputFile))
      {
        if (!outputFile.Contains(Path.PathSeparator.ToString()))
        {
          outputFile = Path.Combine(Environment.CurrentDirectory, outputFile);
        }

        this.writer = new StreamWriter(outputFile);
        this.writer.AutoFlush = true;

        if (AppInfo.Options.IncludeHeader)
        {
          this.WriteHeaderToFile();
        }
      }
      else
      {
        this.writeToStdOut = true;
      }

      this.Receive<WriteResultMessage>(m => this.ProcessResultMessage(m));
      this.Receive<TearDownMessage>(m => this.TearDown());
    }

    private void TearDown()
    {
      this.writer?.Dispose();
      this.writer = null;
    }

    private void ProcessResultMessage(WriteResultMessage message)
    {
      if (this.writeToStdOut)
      {
        Console.WriteLine(message.NugetPackageInfo);
      }
      else
      {
        this.WriteLineToFile(message.NugetPackageInfo);
      }

      UntypedActor.Context.Sender.Tell(new PackageWrittenMessage());
    }

    private void WriteLineToFile(NugetPackageInfo nugetPackageInfo)
    {
      // Version information is prefixed with a tab character in order to prevent Excel from automatically converting them to date values.
      string version = AppInfo.Options.ExcelCompatibility ? $"\t{nugetPackageInfo.Version}" : nugetPackageInfo.Version;

      List<string> columns = new List<string> { nugetPackageInfo.SolutionPath, nugetPackageInfo.ProjectName, nugetPackageInfo.Id, version, nugetPackageInfo.TargetFramework };
      string joined = this.JoinColumns(columns);
      this.writer.WriteLine(joined);
    }

    private void WriteHeaderToFile()
    {
      List<string> columns = new List<string> { "Solution Path", "Project Name", "ID", "Version", "Target Framework" };
      string joined = this.JoinColumns(columns);
      this.writer.WriteLine(joined);
    }

    private string JoinColumns(List<string> columns)
    {
      string separatorWithQuote = $"\"{AppInfo.Options.Separator}\"";
      string joined = $"\"{string.Join(separatorWithQuote, columns)}\"";
      return joined;
    }
  }
}