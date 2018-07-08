using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using McMaster.Extensions.CommandLineUtils;

namespace Bionic {
  [Command(Description = "🤖 Bionic - An Ionic CLI clone for Blazor projects")]
  class Program {
    private static List<string> commandOptions = new List<string> {"start", "generate"};
    private static List<string> generateOptions = new List<string> {"component", "page", "provider"};
    private static string APP_CSS_PATH = @"App.scss";


    public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

    [Argument(0, Description = "Project Command (generate)")]
    public string command { get; set; }

    [Argument(1, Description = "Command Option")]
    public string option { get; set; }

    [Argument(2, Description = "Artifact Name")]
    public string artifact { get; set; }

    // Commands
    [Option("-s|--start", Description = "Prepares Blazor project to mimic Ionic structure")]
    public bool start { get; set; } = false;

    [Option("-g|--generate", Description = "Generate components, pages, and providers/services")]
    public bool generate { get; set; } = false;

    private int OnExecute() {
      if (start || command == "start") return SetupBionic();

      if (generate) {
        if (command != null) {
          if (option != null) {
            artifact = option;
          }

          option = command;
        }

        command = "generate";
      }

      if (command == "generate") generate = true;

      if (!commandOptions.Contains(command)) {
        Console.WriteLine("☠  You must provide a valid project command!");
        Console.WriteLine($"   Available Project Commands: {string.Join(", ", commandOptions)}");
        return 1;
      }

      if (IsGenerateCommandComplete()) GenerateArtifact();
      else return 1;

      return 0;
    }

    private bool IsGenerateCommandComplete() {
      if (option != null && !generateOptions.Contains(option)) {
        Console.WriteLine($"☠  Can't generate \"{option}\"");
        Console.WriteLine($"   You can only generate: {string.Join(", ", generateOptions)}");
        return false;
      }

      while (!generateOptions.Contains(option)) {
        option = Prompt.GetString("What would you like to generate?\n (component, page or provider): ",
          promptColor: ConsoleColor.DarkGreen);
      }

      while (artifact == null) {
        artifact = Prompt.GetString($"How would you like to name your {option}?",
          promptColor: ConsoleColor.DarkGreen);
      }

      return true;
    }

    private int SetupBionic() {
      Console.WriteLine($"🤖  Preparing your Bionic Project...");

      // 1. Create App.scss
      var alreadyStarted = InitAppCss();

      if (alreadyStarted) {
        alreadyStarted = Prompt.GetYesNo(
          "Project seems to have been already started. Are you sure you want to continue ?",
          false,
          promptColor: ConsoleColor.DarkGreen
        );
        if (!alreadyStarted) {
          Console.WriteLine("Ok! Bionic start canceled.");
          return 0;
        }
      }

      // 1. Install Bionic Templates
      Process.Start(DotNetExe.FullPathOrDefault(), "new -i BionicTemplates")?.WaitForExit();

      // Steps:
      // 2. Create App.scss (if not available)
      // 3. Add scss compilation target to solution

      return 0;
    }

    private void GenerateArtifact() {
      Console.WriteLine($"🚀  Generating a {option} named {artifact}");
      Process.Start(
        DotNetExe.FullPathOrDefault(),
        $"new bionic.{option} -n {artifact} -o ./{ToCamelCase(option)}s"
      )?.WaitForExit();
      IntroduceAppCssImport($"{ToCamelCase(option)}s", artifact);
    }

    private bool InitAppCss() {
      if (!File.Exists(APP_CSS_PATH)) {
        using (var sw = File.CreateText(APP_CSS_PATH)) {
          sw.WriteLine("// WARNING - This file is automatically updated by Bionic CLI, please do not remove");
          sw.WriteLine("\n// Components\n\n// Pages\n");
        }

        return false;
      }

      return true;
    }

    private void IntroduceAppCssImport(string type, string artifactName) {
      var text = new StringBuilder();

      foreach (string s in File.ReadAllLines(APP_CSS_PATH)) {
        if (s.StartsWith($"// {type}")) {
          text.AppendLine($"{s}\n@import \"{type}/{artifactName}.scss\";");
        }
        else {
          text.AppendLine(s);
        }
      }

      using (var file = new StreamWriter(File.Create(APP_CSS_PATH))) {
        file.Write(text.ToString());
      }
    }

    private static string ToCamelCase(string str) {
      return string.IsNullOrEmpty(str) || str.Length < 1 ? "" : char.ToUpperInvariant(str[0]) + str.Substring(1);
    }
  }
}