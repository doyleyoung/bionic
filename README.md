# 🤖 Bionic

An Ionic CLI clone for Blazor projects

[![YouTube Video](https://img.youtube.com/vi/NONCv-i4Q34/0.jpg)](https://youtu.be/NONCv-i4Q34)

## Quick Start

Before we start, make sure that the following tools are available in your system:

- ### SASS is installed and available in your terminal path

You can install sass from [here](https://sass-lang.com/install).
Ensure availability by executing scss command:
```bash
scss --version
Ruby Sass 3.5.6
```

- ### NodeJS is installed and available in your terminal path

You can install node from [here](https://nodejs.org/).
Ensure availability by executing node command:
```bash
node --version
v9.5.0
```

```bash
npm --version
5.6.0
```

The following steps are only required to be executed once:

1. Create a [Blazor App](https://blazor.net/docs/get-started.html)
2. Install Bionic from [NuGet](https://www.nuget.org/packages/Bionic): ```dotnet tool install --global Bionic```
3. Prepare Blazor project for Bionic: ```bionic start```

The next steps are part of your day-to-day development:

4. Run project: ```bionic serve```
5. In a secondary terminal, cd into your project root directory
6. Create a new component: ```bionic generate component CounterComponent```
7. Edit component and reuse it anywhere you want...

## Sample Bionic commands

### Version
```bash
bionic version
```

### Updating Bionic

```bash
bionic update
```

### Uninstalling Bionic

```bash
bionic uninstall
```

### Using Electron platform plugin

```bash
bionic platform add electron
bionic platform electron init
bionic platform electron build
bionic platform electron serve
```

### Using Bionic Blast scripts

Blast scripts are easy to use and organize.
They are most useful to easily set build sequences.

In your Blazor project Client or Standalone directory, use your favorite text editor or IDE to create or edit ```.bionic/bionic.blast```
Add the following content and save it:

```text
:electron
>electron-init
>electron-build

:electron-init
bionic platform add electron
bionic platform electron init

:electron-build
bionic platform electron build
bionic platform electron serve
```

Lines starting with:
```text
: - targets
> - sub-targets. Make sure that there are no spaces after it.
```
Any other type of line is a cli command.
 
# Blast scripts not executing?

There's a bug in [dotnet tools](https://github.com/dotnet/cli/issues/9321) that is preventing bionic tool from being found in the system path.
There several solition. If you are in OSX, just edit ```/etc/paths.d/dotnet-cli-tools``` to be ```$HOME/.dotnet/tools```.
Did not try in Linux, but you may have to do the same or just edit your shell init script accordingly.


Then have bionic blast it away: ```bionic blast electron```