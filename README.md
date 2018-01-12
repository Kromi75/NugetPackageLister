# NugetPackageLister

## Purpose
A command line tool that parses all Nuget package configuration files in a folder and lists all packages that are referenced,
regardless of how many solutions and projects are in that folder.

The output is either printed to std-out or written to a CSV file in form of a simple list that can be processed further to find 
package version mismatches across solutions.

## Remarks
I created this tool to learn about and test two libraries I was interested in:

 * https://github.com/commandlineparser/commandline ([license](https://github.com/commandlineparser/commandline/blob/master/License.md))
 * https://github.com/akkadotnet/akka.net ([license](https://github.com/akkadotnet/akka.net/blob/master/LICENSE))

In the current version I accept that the output is in no particular order because the data can easily be sorted, filtered and grouped
by other tools. My focus was on leveraging Akka.NET to, on the one hand, process all config files more or less simultaneously, and
on the other hand not having to care about concurrency issues while writing all results to a single file.

## Usage

```
Usage: npl [-i|--include-header] [-s|--separator <string>] [-e|--excel]
[-t|--target-file <file path>] [source]

  source                  The folder path to search for packaged files in.
                          If not specified the current folder is used.

  -t, --target-file       Name of the file to write the package information
                          into. If not specified all output is written to the
                          std-out stream.

  -i, --include-header    If this option and a target file are specified a
                          header line is output as first line.

  -s, --separator         Character(s) for separating values in CSV file.
                          Default: ";"

  -e, --excel             If specified version values are formatted in a way
                          that prevents Excel from converting them to dates.

  --help                  Display this help screen.
  ```
  