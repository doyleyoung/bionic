# 🤖 Bionic

An Ionic CLI clone for Blazor projects

## Quick Start

Before we start, make sure sass is installed and available in your terminal path.
You can install sass from [here](https://sass-lang.com/install).
Ensure availability by executing scss command:
```bash
scss --version
Ruby Sass 3.5.6
```

The following steps are only required to be executed once:

1. Create a [Blazor App](https://blazor.net/docs/get-started.html)
2. Install Bionic: ```dotnet tool install --global Bionic```
3. Prepare Blazor project for Bionic: ```bionic start```

The next steps are part of your day-to-day development:

4. Run project: ```dotnet watch run```
5. In a secondary terminal, cd into your project root directory
6. Create a new component: ```bionic generate component CounterComponent```
7. Edit component and reuse it anywhere you want...


## Updating Bionic

1. Remove current Bionic version: ```dotnet tool uninstall --global Bionic```
2. Install latest version: ```dotnet tool install --global Bionic```

For specific version, add ```--version x.x.x``` to line 2 above.
Look in [NuGet](https://www.nuget.org/packages/Bionic) for available versions. 


## Development

### Build (Debug)

```bash
dotnet build
```

### Install Templates

```bash
dotnet new -i ./
```

## Test Bionic CLI

```bash
dotnet ../bin/Debug/netcoreapp2.1/Bionic.dll generate component CounterComponent
```

## Releasing

### Build and Pack (Release)

```bash
dotnet pack -o ./
```

or

```bash
dotnet build -c Release /t:pack
```

## Production

### Install Bionic CLI
```bash
dotnet tool install --global Bionic --version 1.0.3
```

### Before you start, enhance your existing Blazor project (required once) 
```bash
bionic start
```

### Create a new Blazor component
```bash
bionic -g component CounterComponent
```

### Don't like Bionic enhancements! How can I remove Bionic templates from my Blazor project?
```bash
dotnet new -u BionicTemplates
```
