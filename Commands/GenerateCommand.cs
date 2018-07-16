using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Bionic.Project;
using Bionic.Utils;
using McMaster.Extensions.CommandLineUtils;

namespace Bionic.Commands {
  public class GenerateCommand : ICommand {
    private static readonly List<string> GenerateOptions = new List<string>
      {"component", "page", "provider", "service"};

    private const string ProgramPath = "Program.cs";

    private static readonly Regex ServiceRegEx =
      new Regex(@"BrowserServiceProvider[^(]*\([\s]*(.*?)=>[\s]*{([^}]*)}", RegexOptions.Compiled);

    private string _option;
    private string _artifact;

    public GenerateCommand(string option, string artifact) {
      this._option = option;
      this._artifact = artifact;
    }

    public int Execute() => IsGenerateCommandComplete() ? GenerateArtifact() : 1;

    private bool IsGenerateCommandComplete() {
      if (_option != null && !GenerateOptions.Contains(_option)) {
        Console.WriteLine($"☠  Can't generate \"{_option}\"");
        Console.WriteLine($"   You can only generate: {string.Join(", ", GenerateOptions)}");
        return false;
      }

      while (!GenerateOptions.Contains(_option)) {
        _option = Prompt.GetString("What would you like to generate?\n (component, page or provider): ",
          promptColor: ConsoleColor.DarkGreen);
      }

      while (_artifact == null) {
        _artifact = Prompt.GetString($"How would you like to name your {_option}?",
          promptColor: ConsoleColor.DarkGreen);
      }

      return true;
    }

    private int GenerateArtifact() {
      Console.WriteLine($"🚀  Generating a {_option} named {_artifact}");

      if (_option == "page") {
        Process.Start(
          DotNetExe.FullPathOrDefault(),
          $"new bionic.{_option} -n {_artifact} -p /{ToPageName(_artifact)} -o ./{ToCamelCase(_option)}s"
        )?.WaitForExit();
        return IntroduceAppCssImport($"{ToCamelCase(_option)}s", _artifact);
      }
      
      if (_option == "component") {
        Process.Start(
          DotNetExe.FullPathOrDefault(),
          $"new bionic.{_option} -n {_artifact} -o ./{ToCamelCase(_option)}s"
        )?.WaitForExit();
        return IntroduceAppCssImport($"{ToCamelCase(_option)}s", _artifact);
      }
      
      if (_option == "provider" || _option == "service") {
        Process.Start(
          DotNetExe.FullPathOrDefault(),
          $"new bionic.{_option} -n {_artifact} -o ./{ToCamelCase(_option)}s"
        )?.WaitForExit();
        return IntroduceServiceInBrowser(_artifact);
      }

      return 1;
    }

    private static int IntroduceAppCssImport(string type, string artifactName) {
      return FileHelper.SeekForLineStartingWithAndInsert(ProjectHelper.AppCssPath, $"// {type}",
        $"@import \"{type}/{artifactName}.scss\";");
    }

    private static int IntroduceServiceInBrowser(string serviceName) {
      try {
        var text = new StringBuilder();

        var all = File.ReadAllText(ProgramPath);

        var matches = ServiceRegEx.Matches(all);
        var browserName = matches[0].Groups[1].Value.Trim().Trim(Environment.NewLine.ToCharArray());
        var currentServices = matches[0].Groups[2].Value;
        var currentServicesList = currentServices.Split("\n");
        var lastEntry = currentServicesList.Last();
        var newServices =
          $"{currentServices}    {browserName}.AddSingleton<I{serviceName}, {serviceName}>();\n{lastEntry}";

        using (var file = new StreamWriter(File.Create(ProgramPath))) {
          file.Write(all.Replace(currentServices, newServices));
        }
      }
      catch (Exception e) {
        return 1;
      }

      return 0;
    }

    private static string ToPageName(string artifact) {
      var rx = new Regex("[pP]age");
      var name = rx.Replace(artifact, "").ToLower();
      return string.IsNullOrEmpty(name) ? artifact.ToLower() : name;
    }

    private static string ToCamelCase(string str) {
      return string.IsNullOrEmpty(str) || str.Length < 1 ? "" : char.ToUpperInvariant(str[0]) + str.Substring(1);
    }
  }
}