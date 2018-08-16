# Electron

[Electron](https://electronjs.org/) allows for the development of desktop GUI applications using front and back end components originally developed for web applications.
This Bionic platform plugin uses a direct template as provided by Electron ans wraps it up for convenient development with Bionic apps.

!!! info
    Bionic will eventually also provide another way of deploying Electron apps through Bionic Capacitor Plugin.

## Requirements

### NodeJS

Capacitor depends on NodeJS. Please [install](https://nodejs.org/en/download/) or ensure that you are using a current NodeJS version:

```text
> node --version
v9.5.0
```

## Prepare your Blazor app for Electron

### Ensure that index.html is explicitly defined

Electron will fail to route to index.html if page is not explicitly defined. To fix this issue find your ```index.cshtml``` page and, if not present, add ```@page "/index.html"```.
Here's how the Blazor Standalone template ```Pages/index.cshtml``` should look like in order to make it work with Electron:

```html
@page "/"
@page "/index.html"

<h1>Hello, world!</h1>

Welcome to your new app.

<SurveyPrompt Title="How is Blazor working for you?" />
```

### Ensure that document base URL is set to look into current directory

Edit ```wwwroot/index.html``` and change ```<base href="/" />``` to be ```<base href="./" />```.

## Initializing Electron

First, we need to download and install Bionic's Electron Plugin. This step is only required once per project. From your project (or Blazor Client) directory do: 

```text
> bionic platform add electron
🔍  Looking for electron platform plugin
☕  Found it! Adding electron plugin...
🚀  electron platform successfully added
```

This will create the necessary assets under ```platforms/electron```.

The next step is to initialize Electron, and similarly, you only need to execute this step once:

```text
> bionic platform electron init
☕  Initializing Electron...
...
🚀  Electron is ready to go! - try: bionic platform electron serve
```

## Building and Serving Electron

The following steps are to be executed everytime you want to deploy the latest changes in Electron.

First, rebuild your Blazor/Bionic project to ensure compiled assets are up-to-date:
```text
> dotnet build
```

Then prepare all assets in Electron platform directory:

```text
> bionic platform electron build
☕  Building Electron...
🚀  Electron successfully built. Try: bionic platform electron serve
```

Finally serve Electron:

```text
> bionic platform electron serve
☕  Serving Electron...
```
