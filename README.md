# 🤖 Bionic

An Ionic CLI clone for Blazor projects

## Quick Start

1. Create a [Blazor App](https://blazor.net/docs/get-started.html).
2. Install Bionic: ```dotnet tool install --global Bionic``` (Required once)
3. Prepare Blazor project for Bionic: ```bionic start```
4. Run project: ```dotnet watch run```
5. Create a new component: ```bionic generate component CounterComponent```
6. Edit component and reuse it anywhere you want...

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