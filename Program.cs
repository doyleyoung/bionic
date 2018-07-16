using System;
using System.Collections.Generic;
using System.Linq;
using Bionic.Commands;
using McMaster.Extensions.CommandLineUtils;

namespace Bionic {
  static class ExtMethods {
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> me) => !me?.Any() ?? true;
  }

  [Command(Description = "🤖 Bionic - An Ionic CLI clone for Blazor projects")]
  class Program {
    private static readonly List<string> commandOptions = new List<string>
      {"docs", "generate", "info", "serve", "start", "uninstall", "update"};

    [Argument(0, Description = "Project Command (docs, generate, info, serve, start, uninstall, update)")]
    private string command { get; set; }

    [Argument(1, Description = "Command Option")]
    private string option { get; set; }

    [Argument(2, Description = "Artifact Name")]
    private string artifact { get; set; }

    // Commands
    [Option("-s|--start", Description = "Prepares Blazor project to mimic Ionic structure")]
    private bool start { get; set; } = false;

    [Option("-g|--generate", Description = "Generate components, pages, and providers/services")]
    private bool generate { get; set; } = false;

    [Option("-v|--version", Description = "Bionic version")]
    private bool version { get; } = false;

    [Option("-u|--update", Description = "Bionic update")]
    private bool update { get; } = false;

    [Option("-un|--uninstall", Description = "Uninstall Bionic")]
    private bool uninstall { get; } = false;

    public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

    private int OnExecute() {
      if (version) return new VersionCommand().Execute();

      if (command == "docs") return new DocsCommand().Execute();
      if (command == "info") return new InfoCommand().Execute();
      if (command == "serve") return new ServeCommand().Execute();
      if (start || command == "start") return new StartCommand().Execute();
      if (update || command == "update") return new UpdateCommand().Execute();
      if (uninstall || command == "uninstall") return new UninstallCommand().Execute();

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

      return new GenerateCommand(option, artifact).Execute();
    }
  }
}