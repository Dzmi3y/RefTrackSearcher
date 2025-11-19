
using LibVLCSharp.Shared;

namespace RefTrackSearcher.Desktop.AudioPlayer;

public class AudioPlayer
{
    private LibVLC _libVLC;
    private MediaPlayer _mediaPlayer;

    public AudioPlayer()
    {
        _libVLC = new LibVLC();
        _mediaPlayer = new MediaPlayer(_libVLC);
    }

    public void Play(string url)
    {
        var media = new Media(_libVLC, url,FromType.FromLocation);
        _mediaPlayer.Play(media);
    }

    public void Pause() => _mediaPlayer.Pause();
    public void Stop() => _mediaPlayer.Stop();
}
