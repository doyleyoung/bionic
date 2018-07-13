using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using McMaster.Extensions.CommandLineUtils;

namespace Bionic {
  [Command(Description = "🤖 Bionic - An Ionic CLI clone for Blazor projects")]
  class Program {
    private static readonly List<string> commandOptions = new List<string> {"start", "generate"};
    private static readonly List<string> generateOptions = new List<string> {"component", "page", "provider", "service"};
    private static readonly string AppCssPath = "App.scss";
    private static readonly string ProgramPath = "Program.cs";
    private static readonly Regex ServiceRegEx = new Regex(@"BrowserServiceProvider[^(]*\([\s]*(.*?)=>[\s]*{([^}]*)}", RegexOptions.Compiled);

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

    [Option("-v|--version", Description = "Bionic version")]
    private bool version { get; } = false;

    public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

    private int OnExecute() {
      if (version) return Version();

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
          "Project seems to have already been started. Are you sure you want to continue?",
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

      if (option == "page") {
        Process.Start(
          DotNetExe.FullPathOrDefault(),
          $"new bionic.{option} -n {artifact} -p /{ToPageName(artifact)} -o ./{ToCamelCase(option)}s"
        )?.WaitForExit();
        IntroduceAppCssImport($"{ToCamelCase(option)}s", artifact);
      }
      else if (option == "component") {
        Process.Start(
          DotNetExe.FullPathOrDefault(),
          $"new bionic.{option} -n {artifact} -o ./{ToCamelCase(option)}s"
        )?.WaitForExit();
        IntroduceAppCssImport($"{ToCamelCase(option)}s", artifact);
      } else if (option == "provider" || option == "service") {
        Process.Start(
          DotNetExe.FullPathOrDefault(),
          $"new bionic.{option} -n {artifact} -o ./{ToCamelCase(option)}s"
        )?.WaitForExit();
        IntroduceServiceInBrowser(artifact);
      }
    }

    private static bool InitAppCss() {
      if (File.Exists(AppCssPath)) return true;

      using (var sw = File.CreateText(AppCssPath)) {
        sw.WriteLine("// WARNING - This file is automatically updated by Bionic CLI, please do not remove");
        sw.WriteLine("\n// Components\n\n// Pages\n");
      }

      return false;
    }

    private static string ToPageName(string artifact) {
      var rx = new Regex("[pP]age");
      var name = rx.Replace(artifact, "").ToLower();
      return string.IsNullOrEmpty(name) ? artifact.ToLower() : name;
    }

    private static void IntroduceAppCssImport(string type, string artifactName) {
      SeekForLineStartingWithAndInsert(AppCssPath, $"// {type}", $"@import \"{type}/{artifactName}.scss\";");
    }

    private static void IntroduceProjectTargets(string projectFileName) {
      const string content = @"
    <ItemGroup>
        <Watch Include=""**/*.cshtml;**/*.scss"" Visible=""false""/>
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

    private static void IntroduceServiceInBrowser(string serviceName) {
      var text = new StringBuilder();

      string all = File.ReadAllText(ProgramPath);

      var matches = ServiceRegEx.Matches(all);
      var browserName = matches[0].Groups[1].Value.Trim().Trim(Environment.NewLine.ToCharArray());
      var currentServices = matches[0].Groups[2].Value;
      var currentServicesList = currentServices.Split("\n");
      var lastEntry = currentServicesList.Last();
      var newServices = $"{currentServices}    {browserName}.AddSingleton<I{serviceName}, {serviceName}>();\n{lastEntry}";

      using (var file = new StreamWriter(File.Create(ProgramPath))) {
        file.Write(all.Replace(currentServices, newServices));
      }
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

    private static int Version() {
      var informationlVersion = ((AssemblyInformationalVersionAttribute)Attribute.GetCustomAttribute(
          Assembly.GetExecutingAssembly(), typeof(AssemblyInformationalVersionAttribute), false))
        .InformationalVersion;
      Console.WriteLine($"🤖 Bionic v{informationlVersion}");
      return 0;
    }
  }
}