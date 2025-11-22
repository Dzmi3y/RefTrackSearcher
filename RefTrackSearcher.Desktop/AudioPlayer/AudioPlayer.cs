
using System;
using LibVLCSharp.Shared;

namespace RefTrackSearcher.Desktop.AudioPlayer;

public class AudioPlayer
{
    private LibVLC _libVLC;
    private MediaPlayer _mediaPlayer;
    
    public event EventHandler<MediaPlayerPositionChangedEventArgs> PositionChanged;
    public event EventHandler<MediaPlayerLengthChangedEventArgs> LengthChanged;


    public AudioPlayer()
    {
        _libVLC = new LibVLC();
        _mediaPlayer = new MediaPlayer(_libVLC);
        _mediaPlayer.PositionChanged += (s, e) => PositionChanged?.Invoke(this, e);
        _mediaPlayer.LengthChanged += (s, e) => LengthChanged?.Invoke(this, e);

    }

    public void Play(string url)
    {
        var media = new Media(_libVLC, url,FromType.FromLocation);
        _mediaPlayer.Play(media);
    }

    public void Pause() => _mediaPlayer.Pause();
    public void Stop() => _mediaPlayer.Stop();
    
    public float Position 
    { 
        get => _mediaPlayer.Position;
        set => _mediaPlayer.Position = value;
    }

    public long Length => _mediaPlayer.Length;
    public bool IsSeekable => _mediaPlayer.IsSeekable;

}
