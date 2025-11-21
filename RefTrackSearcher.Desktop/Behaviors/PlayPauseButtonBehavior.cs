using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Xaml.Interactivity;

namespace RefTrackSearcher.Desktop.Behaviors;
public class PlayPauseButtonBehavior : Behavior<Button>
{
    public static readonly StyledProperty<bool> IsPlayingProperty =
        AvaloniaProperty.Register<PlayPauseButtonBehavior, bool>(nameof(IsPlaying));

    public bool IsPlaying
    {
        get => GetValue(IsPlayingProperty);
        set => SetValue(IsPlayingProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        UpdateButtonAppearance();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == IsPlayingProperty)
        {
            UpdateButtonAppearance();
        }
    }

    private void UpdateButtonAppearance()
    {
        if (AssociatedObject == null) return;

        AssociatedObject.Classes.Set("playing", IsPlaying);
        AssociatedObject.Classes.Set("paused", !IsPlaying);
    }
}
