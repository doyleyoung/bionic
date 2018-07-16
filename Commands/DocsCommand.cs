using Bionic.Project;
using Bionic.Utils;

namespace Bionic.Commands {
  public class DocsCommand : ICommand {
    public int Execute() => OpenBlazorDocs();
    
    private static int OpenBlazorDocs() {
      var browser = UrlHelper.OpenUrl("https://blazor.net");
      browser?.WaitForExit();
      return browser?.ExitCode ?? 1;
    }
  }
}