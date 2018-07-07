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
dotnet build -c Release /t:pack
```

## Production

```bash
nuget install Bionic
```

```bash
bionic -g component CounterComponent
```