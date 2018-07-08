# 🤖 Bionic

An Ionic CLI clone for Blazor projects

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
dotnet tool install --global Bionic --version 1.0.0
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