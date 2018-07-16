using Bionic.Project;
using Bionic.Utils;

namespace Bionic.Commands {
  public class UninstallCommand : ICommand {
    public int Execute() => UninstallBionic();

    private static int UninstallBionic() => DotNetHelper.RunDotNet("tool uninstall -g Bionic");
  }
}