# Status Bar

!!! success "iOS, Android"

The StatusBar API Provides methods for configuring the style of the Status Bar, along with showing or hiding it.

## Methods

[StatusBarBridge.Show()](#show)

[StatusBarBridge.Hide()](#hide)

[StatusBarBridge.SetBackgroundColor()](#setbackgroundcolor)

[StatusBarBridge.SetStyle()](#setstyle)

## Example

```c#
    // Show Status Bar
    StatusBarBridge.Show();
    
    // Hide Status Bar
    StatusBarBridge.Hide();
    
    // Set Status Bar bg color
    StatusBarBridge.SetBackgroundColor("blue");

    // Set Statu