# Modals

!!! success "iOS, Android, Electron, PWA"

The Modals API provides methods for triggering native modal windows for alerts, confirmations, and input prompts, as well as Action Sheets.

## Methods

[ModalsBridge.Alert()](#alert)

[ModalsBridge.Confirm()](#confirm)

[ModalsBridge.Prompt()](#prompt)

[ModalsBridge.ShowActions()](#showactions)

## Example

```c#
    private static async Task ModalsAlert() {
        try {
            var options = new AlertOptions {
                title = "🤖 Bionic Alert",
                message = "Capacitor B