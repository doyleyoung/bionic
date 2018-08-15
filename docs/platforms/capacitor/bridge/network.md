# Network

!!! success "iOS, Android, Electron, PWA"

The Network API provides events for monitoring network status changes, along with querying the current state of the network.

## Methods

[NetworkBridge.GetStatus()](#getstatus)

[NetworkBridge.AddListener()](#addlistener)

[NetworkBridge.RemoveListener()](#removelistener)

## Example

```c#
    private NetworkStatus CurrentNetworkStatus;
    private async Task NetworkCurrentStatus() {
        try {
            CurrentNetworkStatus = await NetworkBridge.GetStatus();
        }
        catch (Exception