using Bionic.Project;
using Bionic.Utils;

namespace Bionic.Commands {
  public class UpdateCommand : ICommand {
    public int Execute() => UpdateBionic();

    private static int UpdateBionic() => DotNetHelper.RunDotNet("tool update -g Bionic");
  }
}