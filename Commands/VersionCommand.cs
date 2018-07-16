using System;
using System.Reflection;
using Bionic.Project;

namespace Bionic.Commands {
  public class VersionCommand : ICommand {
    public int Execute() => PrintVersion();

    private static int PrintVersion() {
      var informationalVersion = ((AssemblyInformationalVersionAttribute) Attribute.GetCustomAttribute(
          Assembly.GetExecutingAssembly(), typeof(AssemblyInformationalVersionAttribute), false))
        .InformationalVersion;
      Console.WriteLine($"🤖 Bionic v{informationalVersion}");
      return 0;
    }
  }
}