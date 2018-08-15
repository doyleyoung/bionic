# Share

!!! success "iOS, Android, PWA"

The Share API provides methods for sharing content in any sharing-enabled apps the user may have installed.

The Share API works on iOS, Android, and the Web (using the new [Web Share API](https://developers.google.com/web/updates/2016/09/navigator-share)), though web support is currently spotty.

## Methods

[ShareBridge.share()](#share)

## Example

```c#
    private static async void Share() {
        try {
            var options = new ShareOptions {
                title = "Blazor Framework",
            