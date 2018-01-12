// -------------------------------------------------------------------------------
// CommandLineOptions.cs
// -------------------------------------------------------------------------------
namespace NugetPackageLister
{
  using CommandLine;
  using CommandLine.Text;

  /// <summary>
  /// Represents and describes all command line options.
  /// </summary>
  internal class CommandLineOptions
  {
    [Option('t', "target-file", HelpText = "Name of the file to write the package information into. If not specified all output is written to the std-out stream.")]
    public string TargetFile { get; set; }

    [Option('i', "include-header", HelpText = "If this option and a target file are specified a header line is output as first line.")]
    public bool IncludeHeader { get; set; }

    [Option('s', "separator", HelpText = "Character(s) for separating values in CSV file. Default: \";\"")]
    public string Separator { get; set; }

    [Option('e', "excel", HelpText = "If specified version values are formatted in a way that prevents Excel from converting them to dates.")]
    public bool ExcelCompatibility { get; set; }

    [ValueOption(0)]
    public string WorkingFolder { get; set; }

    [HelpOption]
    public string GetUsage()
    {
      HelpText help = new HelpText
                        {
                          Heading = new HeadingInfo("NugetPackageLister", "1.0"),
                          AdditionalNewLineAfterOption = true,
                          AddDashesToOption = true
                        };
      help.AddPreOptionsLine("Parses all Nuget package configuration files in a folder and lists all packages.");
      help.AddPreOptionsLine(string.Empty);
      help.AddPreOptionsLine("Usage: npl [-i|--include-header] [-s|--separator <string>] [-e|--excel] [-t|--target-file <file path>] [source]");
      help.AddPreOptionsLine(string.Empty);
      help.AddPreOptionsLine("  source                  The folder path to search for packaged files in.");
      help.AddPreOptionsLine("                          If not specified the current folder is used.");
      help.AddOptions(this);
      return help;
    }
  }
}