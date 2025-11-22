using System;
using CommunityToolkit.Mvvm.ComponentModel;
using RefTrackSearcher.Core.Models;

namespace RefTrackSearcher.Desktop.ViewModels;

public partial class TrackViewModel : ObservableObject
{
    public Track Track { get; }
        
    [ObservableProperty]
    private bool _isPlaying;
    [ObservableProperty]
    private float _position;

    public TrackViewModel(Track track)
    {
        Track = track;
    }
    
}