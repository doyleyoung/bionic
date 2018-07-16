using System;
using Bionic.Project;
using Bionic.Utils;

namespace Bionic.Commands {
  public class InfoCommand : ICommand {
    public int Execute() => Info();
    
    private static int Info() {
      new VersionCommand().Execute();
      Console.WriteLine();
      return DotNetHelper.RunDotNet("--info");
    }
  }
}