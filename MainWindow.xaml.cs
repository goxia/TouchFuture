using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;

namespace TouchFuture;

/// <summary>
/// TouchFuture - 智能桌面助手
/// </summary>
public partial class MainWindow : Window
{
    #region Win32 API
    [DllImport("user32.dll")]
    private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
    
    // Win32 API 常量
    private const uint KEYEVENTF_KEYUP = 0x0002;
    private const byte VK_LWIN = 0x5B;
    private const byte VK_CONTROL = 0x11;
    private const byte VK_MENU = 0x12; // Alt 键
    #endregion

    private bool _isMenuExpanded = false;
    private DispatcherTimer? _autoHideTimer;
    private DispatcherTimer? _winCTimer;
    private bool _isDragging = false;
    private System.Windows.Point _lastMousePosition;
    private System.Windows.Point _lastTouchPosition;
    private bool _isWinCPressed = false;

    public MainWindow()
    {
        try
        {
            InitializeComponent();
            InitializeAutoHideTimer();
            this.Topmost = true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"MainWindow constructor error: {ex.Message}");
            MessageBox.Show($"初始化错误: {ex.Message}", "TouchFuture", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void InitializeAutoHideTimer()
    {
        _autoHideTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(5)
        };
        _autoHideTimer.Tick += (s, e) =>
        {
            if (_isMenuExpanded)
            {
                CollapseMenu();
            }
        };

        // 初始化Win+C长按计时器
        _winCTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(2) // 2秒检测
        };
        _winCTimer.Tick += WinCTimer_Tick;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        // 窗口已经通过WindowStartupLocation="CenterScreen"居中显示
        // 这里可以添加其他初始化逻辑
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        // 清理资源
    }

    #region UI Event Handlers

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (!_isMenuExpanded)
        {
            _isDragging = false;
            _lastMousePosition = e.GetPosition(this);
        }
    }

    private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (!_isMenuExpanded && !_isDragging)
        {
            // 这是一个点击而不是拖拽，展开菜单
            ToggleMenu();
        }
        
        _isDragging = false;
    }

    protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
    {
        base.OnMouseMove(e);
        
        if (e.LeftButton == MouseButtonState.Pressed && !_isMenuExpanded)
        {
            var currentPosition = e.GetPosition(this);
            var diff = currentPosition - _lastMousePosition;
            
            // 如果移动距离超过阈值，开始拖拽
            if (!_isDragging && (Math.Abs(diff.X) > 5 || Math.Abs(diff.Y) > 5))
            {
                _isDragging = true;
                // 开始拖拽操作
                this.DragMove();
            }
        }
    }

    private void AssistantButton_Touch(object sender, TouchEventArgs e)
    {
        if (!_isMenuExpanded)
        {
            ToggleMenu();
        }
    }

    private void Window_TouchDown(object sender, TouchEventArgs e)
    {
        if (!_isMenuExpanded)
        {
            _isDragging = false;
            _lastTouchPosition = e.GetTouchPoint(this).Position;
        }
    }

    private void Window_TouchUp(object sender, TouchEventArgs e)
    {
        if (!_isMenuExpanded && !_isDragging)
        {
            // 这是一个点击而不是拖拽，展开菜单
            ToggleMenu();
        }
        
        _isDragging = false;
    }

    private void Window_TouchMove(object sender, TouchEventArgs e)
    {
        if (!_isMenuExpanded)
        {
            var currentPosition = e.GetTouchPoint(this).Position;
            var diff = new System.Windows.Vector(
                currentPosition.X - _lastTouchPosition.X, 
                currentPosition.Y - _lastTouchPosition.Y);
            
            // 如果移动距离超过阈值，开始拖拽
            if (!_isDragging && (Math.Abs(diff.X) > 5 || Math.Abs(diff.Y) > 5))
            {
                _isDragging = true;
                // 开始拖拽操作
                this.DragMove();
            }
        }
    }

    private void CloseMenuButton_Click(object sender, RoutedEventArgs e)
    {
        CollapseMenu();
    }

    #endregion

    #region Menu Functions

    private void ToggleMenu()
    {
        if (_isMenuExpanded)
        {
            CollapseMenu();
        }
        else
        {
            ExpandMenu();
        }
    }

    private void ExpandMenu()
    {
        try
        {
            _isMenuExpanded = true;
            
            // 保存当前位置
            double currentLeft = this.Left;
            double currentTop = this.Top;
            
            // 扩大窗口以容纳圆形菜单
            this.Width = 220;
            this.Height = 220;
            
            // 调整位置，使原来的小圆圈保持在中心
            this.Left = currentLeft - 70; // 向左移动70像素
            this.Top = currentTop - 70;   // 向上移动70像素
            
            // 显示菜单背景和按钮
            var menuBackground = this.FindName("MenuBackground") as Ellipse;
            var menuCanvas = this.FindName("MenuCanvas") as Canvas;
            
            if (menuBackground != null)
                menuBackground.Visibility = Visibility.Visible;
                
            if (menuCanvas != null)
                menuCanvas.Visibility = Visibility.Visible;
            
            // 启动自动隐藏计时器
            _autoHideTimer?.Start();
        }
        catch (Exception ex)
        {
            _isMenuExpanded = false;
            System.Diagnostics.Debug.WriteLine($"ExpandMenu error: {ex.Message}");
        }
    }

    private void CollapseMenu()
    {
        try
        {
            _isMenuExpanded = false;
            _autoHideTimer?.Stop();
            
            // 保存当前位置
            double currentLeft = this.Left;
            double currentTop = this.Top;
            
            // 隐藏菜单背景和按钮
            var menuBackground = this.FindName("MenuBackground") as Ellipse;
            var menuCanvas = this.FindName("MenuCanvas") as Canvas;
            
            if (menuBackground != null)
                menuBackground.Visibility = Visibility.Collapsed;
                
            if (menuCanvas != null)
                menuCanvas.Visibility = Visibility.Collapsed;
            
            // 恢复小窗口尺寸
            this.Width = 80;
            this.Height = 80;
            
            // 调整位置，使小圆圈回到原来的位置
            this.Left = currentLeft + 70; // 向右移动70像素
            this.Top = currentTop + 70;   // 向下移动70像素
        }
        catch (Exception ex)
        {
            _isMenuExpanded = false;
            System.Diagnostics.Debug.WriteLine($"CollapseMenu error: {ex.Message}");
        }
    }

    #endregion

    #region Feature Implementations

    private void VoiceInputButton_Click(object sender, RoutedEventArgs e)
    {
        CollapseMenu();
        // 模拟 Win+H 快捷键
        SimulateKeyPress(0x5B, 0x48); // Win + H
    }

    private void LiveCaptionButton_Click(object sender, RoutedEventArgs e)
    {
        CollapseMenu();
        // 模拟 Win+Ctrl+L 快捷键
        SimulateKeyCombo(new byte[] { 0x5B, 0x11, 0x4C }); // Win + Ctrl + L
    }

    private void CopilotButton_Click(object sender, RoutedEventArgs e)
    {
        CollapseMenu();
        try
        {
            // 执行 Win+C 长按2秒
            StartWinCLongPress();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"CopilotButton_Click error: {ex.Message}");
        }
    }

    #endregion

    #region Helper Methods

    private void SimulateKeyPress(byte key1, byte key2)
    {
        // 按下按键
        keybd_event(key1, 0, 0, UIntPtr.Zero);
        keybd_event(key2, 0, 0, UIntPtr.Zero);
        
        // 释放按键
        keybd_event(key2, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        keybd_event(key1, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
    }

    private void SimulateKeyCombo(byte[] keys)
    {
        // 按下所有按键
        foreach (var key in keys)
        {
            keybd_event(key, 0, 0, UIntPtr.Zero);
        }
        
        // 释放所有按键（逆序）
        for (int i = keys.Length - 1; i >= 0; i--)
        {
            keybd_event(keys[i], 0, KEYEVENTF_KEYUP, UIntPtr.Zero);
        }
    }

    #endregion
    
    #region New UI Event Handlers - 新UI事件处理
    
    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            CollapseMenu();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"CloseButton_Click error: {ex.Message}");
        }
    }
    
    private void VoiceButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            CollapseMenu();
            SimulateKeyCombo(new byte[] { VK_LWIN, 0x48 }); // Win+H 语音输入
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"VoiceButton_Click error: {ex.Message}");
        }
    }
    
    private void CaptionButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            CollapseMenu();
            SimulateKeyCombo(new byte[] { VK_LWIN, VK_CONTROL, 0x4C }); // Win+Ctrl+L 实时字幕
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"CaptionButton_Click error: {ex.Message}");
        }
    }

    private void AltSpaceButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            CollapseMenu();
            // 执行 Win+C 单击快捷键 - 使用明确的按键顺序
            keybd_event(VK_LWIN, 0, 0, UIntPtr.Zero);      // 按下Win键
            System.Threading.Thread.Sleep(50);             // 短暂延迟确保按键被识别
            keybd_event(0x43, 0, 0, UIntPtr.Zero);         // 按下C键
            System.Threading.Thread.Sleep(50);             // 短暂延迟
            keybd_event(0x43, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);         // 释放C键
            keybd_event(VK_LWIN, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);      // 释放Win键
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"AltSpaceButton_Click error: {ex.Message}");
        }
    }

    private void StartWinCLongPress()
    {
        try
        {
            _isWinCPressed = true;
            
            // 开始按住Win+C
            keybd_event(VK_LWIN, 0, 0, UIntPtr.Zero);   // 按下Win键
            keybd_event(0x43, 0, 0, UIntPtr.Zero);      // 按下C键 (0x43)
            
            // 启动2秒计时器
            _winCTimer?.Start();
            
            // 静默执行，不显示提示消息
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"StartWinCLongPress error: {ex.Message}");
            ReleaseWinC();
        }
    }

    private void WinCTimer_Tick(object? sender, EventArgs e)
    {
        try
        {
            _winCTimer?.Stop();
            ReleaseWinC();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"WinCTimer_Tick error: {ex.Message}");
        }
    }

    private void ReleaseWinC()
    {
        try
        {
            if (_isWinCPressed)
            {
                // 释放按键
                keybd_event(0x43, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);      // 释放C键
                keybd_event(VK_LWIN, 0, KEYEVENTF_KEYUP, UIntPtr.Zero);   // 释放Win键
                
                _isWinCPressed = false;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ReleaseWinC error: {ex.Message}");
        }
    }
    
    private void MainButton_MouseDown(object sender, MouseButtonEventArgs e)
    {
        try
        {
            _isDragging = false;
            _lastMousePosition = e.GetPosition(this);
            Mouse.Capture((IInputElement)sender);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"MainButton_MouseDown error: {ex.Message}");
        }
    }
    
    private void MainButton_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
    {
        try
        {
            if (Mouse.Captured == sender && e.LeftButton == MouseButtonState.Pressed)
            {
                var currentPosition = e.GetPosition(this);
                var deltaX = currentPosition.X - _lastMousePosition.X;
                var deltaY = currentPosition.Y - _lastMousePosition.Y;
                
                if (Math.Abs(deltaX) > 5 || Math.Abs(deltaY) > 5)
                {
                    _isDragging = true;
                    this.Left += deltaX;
                    this.Top += deltaY;
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"MainButton_MouseMove error: {ex.Message}");
        }
    }
    
    private void MainButton_MouseUp(object sender, MouseButtonEventArgs e)
    {
        try
        {
            Mouse.Capture(null);
            
            if (!_isDragging)
            {
                // 这是点击，不是拖拽 - 切换菜单
                if (_isMenuExpanded)
                    CollapseMenu();
                else
                    ExpandMenu();
            }
            
            _isDragging = false;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"MainButton_MouseUp error: {ex.Message}");
        }
    }
    
    private void MainButton_TouchDown(object sender, TouchEventArgs e)
    {
        try
        {
            _isDragging = false;
            _lastTouchPosition = e.GetTouchPoint(this).Position;
            this.CaptureTouch(e.TouchDevice);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"MainButton_TouchDown error: {ex.Message}");
        }
    }
    
    private void MainButton_TouchMove(object sender, TouchEventArgs e)
    {
        try
        {
            if (this.AreAnyTouchesCaptured)
            {
                var currentPosition = e.GetTouchPoint(this).Position;
                var deltaX = currentPosition.X - _lastTouchPosition.X;
                var deltaY = currentPosition.Y - _lastTouchPosition.Y;
                
                if (Math.Abs(deltaX) > 10 || Math.Abs(deltaY) > 10)
                {
                    _isDragging = true;
                    this.Left += deltaX;
                    this.Top += deltaY;
                    _lastTouchPosition = currentPosition;
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"MainButton_TouchMove error: {ex.Message}");
        }
    }
    
    private void MainButton_TouchUp(object sender, TouchEventArgs e)
    {
        try
        {
            this.ReleaseTouchCapture(e.TouchDevice);
            
            if (!_isDragging)
            {
                // 这是点击，不是拖拽 - 切换菜单
                if (_isMenuExpanded)
                    CollapseMenu();
                else
                    ExpandMenu();
            }
            
            _isDragging = false;
            e.Handled = true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"MainButton_TouchUp error: {ex.Message}");
        }
    }
    
    #endregion
}