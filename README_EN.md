# TouchFuture

An intelligent desktop assistant tool designed for Windows 11, focused on touch screen interaction while also supporting stylus and traditional mouse operations.

> **📖 Language / 语言**: English | [中文](README.md)

> **🖥️ Demo**
![Demo-TouchFuture](Demo-TouchFuture.jpg)

## 🚀 Features

### 🎯 Core Functionality
- **Always on Desktop**: Always stays on top, draggable to any position on screen
- **Circular UI**: Beautiful circular assistant interface with default robot icon
- **Touch Support**: Full support for finger touch and stylus operations
- **Expandable Menu**: Click to expand circular function menu

### 🛠️ Integrated Functions
1. **🎤 Voice Input** - Invoke Windows 11 built-in voice input (Win+H)
2. **📝 Live Captions** - Enable Windows 11 live captions (Win+Ctrl+L)
3. **🌐 Copilot Launcher** - Quick launch Copilot (Win+C)
4. **💬 Copilot Voice Chat** - Launch Copilot with 2-second voice mode (Win+C long press)

## Technical Specifications

- **Framework**: .NET 8.0 WPF
- **Target Platform**: Windows 11 24H2 (x64/ARM64)
- **System Requirements**: No runtime or environment pre-installation required, uses self-contained deployment
- **UI Technology**: WPF with XAML, supports high DPI displays
- **Dependencies**: Only requires .NET 8.0 base libraries, no external dependencies

## Usage Instructions

1. Run `TouchFuture.exe`
2. The application will appear as a circular button in the top-right corner of the screen
3. Click or touch the button to expand the function menu
4. Select the desired function or click the close button to collapse the menu
5. Drag the button to any position on the screen

## Build Instructions

```bash
# Clone or download the project locally
cd TouchFuture

# Build the project
dotnet build TouchFuture.csproj

# Run the application
dotnet run --project TouchFuture.csproj
```

## Publishing Options

### Framework-Dependent Deployment (FDD)
```bash
dotnet publish -c Release -r win-x64 --self-contained false -o publish-fdd
```
- **Size**: ~0.18 MB (5 files)
- **Requirements**: Requires .NET 8.0 runtime installed on target machine
- **Use Case**: Development, testing, or environments with existing .NET runtime

### Self-Contained Deployment (SCD) - Single File
```bash
dotnet publish -c Release --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o publish-extract-single
```
- **Size**: ~154 MB (1 exe file)
- **Requirements**: No runtime installation needed
- **Use Case**: Distribution to end users, production deployment

## Project Structure

```
TouchFuture/
├── TouchFuture.csproj           # Project file
├── App.xaml                     # Application resource definitions
├── App.xaml.cs                  # Application entry point
├── MainWindow.xaml              # Main window UI definition
├── MainWindow.xaml.cs           # Main window logic
├── app.manifest                 # Application manifest
├── AssemblyInfo.cs              # Assembly information
└── README.md                    # Project documentation
```

## Development Status

- ✅ Basic UI framework completed
- ✅ Touch interaction completed
- ✅ Voice input functionality completed
- ✅ Live captions functionality completed
- ✅ AI assistant integration completed
- 🔄 Additional features in development

## Button Layout

The circular interface uses a clock-position layout:
- **12 o'clock (🎤)**: Voice Input - Windows voice recognition
- **3 o'clock (📝)**: Live Captions - Real-time speech-to-text
- **6 o'clock (💬)**: Copilot Voice Chat - AI conversation mode
- **9 o'clock (🌐)**: Copilot Launcher - Quick AI assistant access
- **Center (🤖)**: Main toggle button

## License

This project is for educational and personal use only. Commercial use is prohibited without explicit permission. Please contact the author for special requirements.

---


*TouchFuture - Making Windows 11 touch experience smarter* 🚀
