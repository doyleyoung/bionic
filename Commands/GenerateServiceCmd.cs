using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using McMaster.Extensions.CommandLineUtils;

namespace Bionic.Commands {
  public class GenerateServiceCmd : CommandBase, ICommand {
    private const string ProgramPath = "Program.cs";

    private static readonly Regex ServiceRegEx =
      new Regex(@"BrowserServiceProvider[^(]*\([\s]*(.*?)=>[\s]*{([^}]*)}", RegexOptions.Compiled);

    [Argument(0, Description = "Artifact Name"), Required]
    private string Artifact { get; set; }

    public GenerateCommand Parent { get; }

    public GenerateServiceCmd() {}

    public GenerateServiceCmd(string artifact) => this.Artifact = artifact;

    protected override int OnExecute(CommandLineApplication app) => GenerateService();

    public int Execute() => GenerateService();

    private int GenerateService() {
      Console.WriteLine($"🚀  Generating a service named {Artifact}");
      Process.Start(
        DotNetExe.FullPathOrDefault(),
        $"new bionic.service -n {Artifact} -o ./Services"
      )?.WaitForExit();
      return IntroduceServiceInBrowser(Artifact);
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
  }
}