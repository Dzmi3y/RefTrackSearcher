using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace RefTrackSearcher.Desktop.Behaviors;

public class SliderValueChangedBehavior : Behavior<Slider>
{
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<SliderValueChangedBehavior, ICommand>(nameof(Command));

    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly StyledProperty<object> CommandParameterProperty =
        AvaloniaProperty.Register<SliderValueChangedBehavior, object>(nameof(CommandParameter));

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    private bool _isUserInteraction;

    protected override void OnAttached()
    {
        base.OnAttached();
        
        AssociatedObject.AddHandler(InputElement.PointerPressedEvent, OnPointerPressed, RoutingStrategies.Tunnel);
        AssociatedObject.PropertyChanged += OnSliderPropertyChanged;
    }

    protected override void OnDetaching()
    {
        AssociatedObject.RemoveHandler(InputElement.PointerPressedEvent, OnPointerPressed);
        AssociatedObject.PropertyChanged -= OnSliderPropertyChanged;
        base.OnDetaching();
    }

    private void OnPointerPressed(object sender, PointerPressedEventArgs e)
    {
        _isUserInteraction = true;
    }

    private void OnSliderPropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == Slider.ValueProperty && _isUserInteraction)
        {
            ExecuteCommand();
            _isUserInteraction = false; 
        }
    }

    private void ExecuteCommand()
    {
        if (Command?.CanExecute(CommandParameter) == true)
        {
            Command.Execute(CommandParameter);
        }
    }
}
