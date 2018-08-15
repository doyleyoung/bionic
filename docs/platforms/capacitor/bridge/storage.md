# Storage

!!! success "iOS, Android, Electron, PWA"

The Storage API provides a key-value store for simple data.

Mobile OS's may periodically clear data set in ```window.localStorage```, so this API should be used instead of ```window.localStorage```. This API will fall back to using ```localStorage``` when running as a Progressive Web App.

Note: this API is not meant for high-performance data storage applications. Take a look at using SQLite, Realm or a separate data engine if your application will store a lot of items, have high read/write load, or