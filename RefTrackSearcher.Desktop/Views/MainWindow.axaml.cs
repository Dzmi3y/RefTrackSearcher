using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using RefTrackSearcher.Desktop.ViewModels;

namespace RefTrackSearcher.Desktop.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private async Task DownloadTrack(TrackViewModel track)
        {
            string url = track.Track.AudioDownload;
    
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    
                    var saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Title = "Save Track As";
                    saveFileDialog.InitialFileName = $"{track.Track.ArtistName} - {track.Track.Name}.mp3";
                    saveFileDialog.Filters = new List<FileDialogFilter>()
                    {
                        new FileDialogFilter()
                        {
                            Name = "Audio files",
                            Extensions = new List<string> { ".mp3", ".wave" }
                        }
                    };
            
                    if (await saveFileDialog.ShowAsync(this) is string filePath)
                    {
                        await File.WriteAllBytesAsync(filePath, content);
                    }
                }
            }
        }

        private async void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is TrackViewModel track)
            {
                await DownloadTrack(track);
            }
        }
    }
}