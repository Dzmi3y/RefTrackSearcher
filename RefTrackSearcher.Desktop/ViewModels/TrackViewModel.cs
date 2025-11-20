using CommunityToolkit.Mvvm.ComponentModel;
using RefTrackSearcher.Core.Models;

namespace RefTrackSearcher.Desktop.ViewModels;

public partial class TrackViewModel : ObservableObject
{
    public Track Track { get; }
        
    [ObservableProperty]
    private bool _isPlaying;

    public TrackViewModel(Track track)
    {
        Track = track;
    }
}