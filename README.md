# TouchFuture

一个为Windows 11设计的智能桌面助手工具，专注在触控屏幕，也支持手写笔以及传统鼠标操作。

> **📖 Language / 语言**: [English](README_EN.md) | 中文

> **🖥️ 演示**

![Demo-TouchFuture](Demo-TouchFuture.jpg)

## 🚀 功能特性

### 🎯 核心功能
- **常驻桌面**: 始终置顶显示，可拖拽到屏幕任意位置
- **圆形UI**: 美观的圆形助手界面，默认显示为机器人图标
- **触控支持**: 完全支持手指触摸和手写笔操作
- **展开菜单**: 点击后展开圆形功能菜单

### 🛠️ 集成功能
1. **🎤 语音输入** - 调用Windows 11内置语音输入 (Win+H)
2. **📝 实时字幕** - 启用Windows 11实时字幕功能 (Win+Ctrl+L)  
3. **🌐 Copilot调用** - 快速启动Copilot (Win+C)
4. **💬 Copilot语音对话** - 启动Copilot并保持2秒语音模式 (Win+C长按)

## 技术规格

- **框架**: .NET 8.0 WPF
- **目标平台**: Windows 11 24H2 (x64/ARM64)
- **运行要求**: 无需预装runtime或环境，采用独立部署方式
- **UI技术**: WPF with XAML，支持高DPI显示
- **依赖包**: 仅需.NET 8.0基础库，无外部依赖

## 使用方法

1. 运行 `TouchFuture.exe`
2. 应用程序会显示为屏幕右上角的圆形按钮
3. 点击或触碰按钮展开功能菜单
4. 选择所需功能或点击关闭按钮收起菜单
5. 可以拖拽按钮到屏幕任意位置

## 构建说明

```bash
# 克隆或下载项目到本地
cd TouchFuture

# 构建项目
dotnet build TouchFuture.csproj

# 运行应用程序
dotnet run --project TouchFuture.csproj
```

## 项目结构

```
TouchFuture/
├── TouchFuture.csproj  # 项目文件
├── App.xaml                     # 应用程序资源定义
├── App.xaml.cs                  # 应用程序入口点
├── MainWindow.xaml              # 主窗口UI定义
├── MainWindow.xaml.cs           # 主窗口逻辑
├── app.manifest                 # 应用程序清单
├── AssemblyInfo.cs              # 程序集信息
└── README.md                    # 项目说明文档
```

## 开发状态

- ✅ 基础UI框架完成
- ✅ 触控交互完成
- ✅ 语音输入功能完成
- ✅ 实时字幕功能完成
- ✅ AI助手调用完成
- 🔄 其他功能开发中

## 许可证

此项目仅供学习和个人使用，禁止用于其他商业场景，如有特殊需求请与本人联系。

---

*TouchFuture - 让Windows 11触控体验更智能* 🚀

