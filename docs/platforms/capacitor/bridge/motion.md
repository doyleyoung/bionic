# Motion

!!! success "iOS, Android, Electron, PWA"

The Motion API tracks accelerometer and device orientation (compass heading, etc.)

## Methods

[MotionBridge.AddAccelListener()](#addaccellistener)

[MotionBridge.AddOrientationListener()](#addorientationlistener)

[MotionBridge.RemoveListener()](#removelistener)

## Example

```c#
    private MotionEventResult CurrentAccel;
    private string MotionAccelWatcherID;
    private async Task MotionAccel() {
        if (MotionAccelWatcherID == null) {
            MotionAccelWatcherID = "my-motion-accel-l