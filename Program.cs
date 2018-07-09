using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using McMaster.Extensions.CommandLineUtils;

namespace Bionic {
  [Command(Description = "🤖 Bionic - An Ionic CLI clone for Blazor projects")]
  class Program {
    private static readonly List<string> commandOptions = new List<string> {"start", "generate"};
    private static readonly List<string> generateOptions = new List<string> {"component", "page", "provider"};
    private static readonly string AppCssPath = @"App.scss";

    [Argument(0, Description = "Project Command (start, generate)")]
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

    public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

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

      // 1. Get project file name
      var projectFileName = GetProjectFileName();
      if (projectFileName == null) {
        Console.WriteLine($"☠ No C# project found. Please make sure you are in the root of a C# project.");
        return 1;
      }

      // 2. Create App.scss
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

      // 3. Inject App.css in index.html
      InjectAppCssInIndexHtml();

      // 4. Inject targets in .csproj
      IntroduceProjectTargets(projectFileName);

      // 5. Install Bionic Templates
      Process.Start(DotNetExe.FullPathOrDefault(), "new -i BionicTemplates")?.WaitForExit();

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

    private static bool InitAppCss() {
      if (File.Exists(AppCssPath)) return true;

      using (var sw = File.CreateText(AppCssPath)) {
        sw.WriteLine("// WARNING - This file is automatically updated by Bionic CLI, please do not remove");
        sw.WriteLine("\n// Components\n\n// Pages\n");
      }

      return false;
    }

    private static void IntroduceAppCssImport(string type, string artifactName) {
      SeekForLineStartingWithAndInsert(AppCssPath, $"// {type}", $"@import \"{type}/{artifactName}.scss\";");
    }

    private static void IntroduceProjectTargets(string projectFileName) {
      const string content = @"
    <!-- dotnet watch: https://github.com/aspnet/Docs/blob/master/aspnetcore/tutorials/dotnet-watch.md -->
    <ItemGroup>
        <DotNetCliToolReference Include=""Microsoft.DotNet.Watcher.Tools"" Version=""2.0.0"" />
        <Watch Include=""**\*.scss"" />
    </ItemGroup>

    <Target Name=""CompileSCSS"" BeforeTargets=""Build"" Condition=""Exists('App.scss')"">
        <Message Importance=""high"" Text=""Compiling SCSS"" />
        <Exec Command=""scss --no-cache --update ./App.scss:./wwwroot/css/App.css"" />
    </Target>";

      SeekForLineStartingWithAndInsert(projectFileName, "</Project>", content, false);
    }

    private static void InjectAppCssInIndexHtml() {
      SeekForLineStartingWithAndInsert(
        "wwwroot/index.html",
        "    <link href=\"css/site.css",
        "    <link href=\"css/App.css\" rel=\"stylesheet\" />",
        false
      );
    }

    private static string ToCamelCase(string str) {
      return string.IsNullOrEmpty(str) || str.Length < 1 ? "" : char.ToUpperInvariant(str[0]) + str.Substring(1);
    }

    private static void SeekForLineStartingWithAndInsert(string fileName, string startsWith,
      string contentToIntroduce, bool insertAfter = true) {
      var text = new StringBuilder();

      foreach (var s in File.ReadAllLines(fileName)) {
        text.AppendLine(s.StartsWith(startsWith) ? (insertAfter ? $"{s}\n{contentToIntroduce}" : $"{contentToIntroduce}\n{s}") : s);
      }

      using (var file = new StreamWriter(File.Create(fileName))) {
        file.Write(text.ToString());
      }
    }

    private static string GetProjectFileName() {
      return Directory.GetFiles("./", "*.csproj", SearchOption.TopDirectoryOnly).FirstOrDefault();
    }
  }
}