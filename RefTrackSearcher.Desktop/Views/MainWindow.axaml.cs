using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using LibVLCSharp.Shared;

namespace RefTrackSearcher.Desktop.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private AudioPlayer.AudioPlayer _player = new AudioPlayer.AudioPlayer();

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            _player.Play("https://prod-1.storage.jamendo.com/?trackid=1204669&format=mp31&from=LX4NAEWkWiF0r1bYb9vDTg%3D%3D%7C7oFQzDzSX%2BAA7CvEtVryUg%3D%3D");

        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            _player.Pause();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            _player.Stop();
        }
    }
}