using Bionic.Project;
using Bionic.Utils;

namespace Bionic.Commands {
  public class ServeCommand : ICommand {
    public int Execute() => ServeBlazor();
    
    private static int ServeBlazor() => DotNetHelper.RunDotNet("watch run");
  }
}