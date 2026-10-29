using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;

namespace MusicService.Views;

public partial class MainWindow : Window
{
    private bool _allowClose;
    private bool _closeAnimationStarted;
    private bool _minimizeAnimationStarted;

    public MainWindow()
    {
        InitializeComponent();
        Closing += OnWindowClosing;
    }


    private async void OnMinimizeClick(
        object? sender,
        RoutedEventArgs e)
    {
        if (_minimizeAnimationStarted || _closeAnimationStarted)
            return;

        _minimizeAnimationStarted = true;

        try
        {
            await AnimateFadeOutAsync(180);

            WindowState = WindowState.Minimized;
            Opacity = 1;
        }
        finally
        {
            _minimizeAnimationStarted = false;
        }
    }


    private void OnCloseClick(
        object? sender,
        RoutedEventArgs e)
    {
        Close();
    }


    private async void OnWindowClosing(
        object? sender,
        WindowClosingEventArgs e)
    {
        if (_allowClose)
            return;

        e.Cancel = true;

        if (_closeAnimationStarted)
            return;

        _closeAnimationStarted = true;

        try
        {
            await AnimateFadeOutAsync(250);
            _allowClose = true;
            Close();
        }
        catch
        {
            _allowClose = true;
            Close();
        }
    }


    private async Task AnimateFadeOutAsync(double durationMs)
    {
        var animation = new Animation
        {
            Duration = TimeSpan.FromMilliseconds(durationMs),
            Easing = new CubicEaseIn(),
            FillMode = FillMode.Forward,
            Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(0d),
                    Setters =
                    {
                        new Setter(Visual.OpacityProperty, 1d)
                    }
                },
                new KeyFrame
                {
                    Cue = new Cue(1d),
                    Setters =
                    {
                        new Setter(Visual.OpacityProperty, 0d)
                    }
                }
            }
        };

        await animation.RunAsync(this);
    }
}