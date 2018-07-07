using System;
using System.Collections.Generic;
using System.Diagnostics;
using McMaster.Extensions.CommandLineUtils;

namespace Bionic {
  
  [Command(Description = "🤖 Bionic - An Ionic CLI clone for Blazor projects")]
  class Program {
    private static List<string> commandOptions = new List<string> {"start", "generate"};
    private static List<string> generateOptions = new List<string> {"component", "page", "provider"};

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
    
    private static int SetupBionic() {
      Console.WriteLine($"🤖  Preparing your Bionic Project...");
      
      // Steps:
      // 1. Create Components directory (if not available)
      // 2. Create App.scss (if not available)
      // 3. Add scss compilation target to solution
      // 4. Install Bionic Templates

      return 0;
    }

    private void GenerateArtifact() {
      Console.WriteLine($"🚀  Generating a {option} named {artifact}");
      
      // Steps:
      // 1. Create Components directory (if not available)
      // 2. Generate Component templates for cshtml and scss
      // 3. Create App.scss (if not available)
      // 4. Add new import to respective App.scss section
      
      Process.Start(DotNetExe.FullPathOrDefault(), $"new bionic.component -n {artifact} -o ./Components")?.WaitForExit();
      // dotnet new bionic.component -n LoginComponent -o ./Components
    }
  }
}