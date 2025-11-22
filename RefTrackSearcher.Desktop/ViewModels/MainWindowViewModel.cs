using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibVLCSharp.Shared;
using RefTrackSearcher.Core.Interfaces.Services;
using RefTrackSearcher.Core.Models;

namespace RefTrackSearcher.Desktop.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly IJamendoService _jamendoService;
        private readonly IJamendoTagsService _jamendoTagsService;
        private string _searchText;
        private readonly AudioPlayer.AudioPlayer _player = new AudioPlayer.AudioPlayer();
        private TrackViewModel _currentTrack;

        [ObservableProperty] private string _currentTrackUrl;

        public MainWindowViewModel(IJamendoService jamendoService, IJamendoTagsService jamendoTagsService)
        {
            _jamendoService = jamendoService;
            _jamendoTagsService = jamendoTagsService;

            SearchCommand = new AsyncRelayCommand(SearchAsync);
            PlayPauseCommand = new RelayCommand<TrackViewModel>(PlayPause);
            StopCommand = new RelayCommand(Stop);
            Genres = new ObservableCollection<SelectableTag>();
            SearchResultList = new ObservableCollection<TrackViewModel>();

            LoadGenresData(_jamendoTagsService.GetGenres());
            _player.PositionChanged += OnTrackPositionChanged;
        }

        public ICommand SearchCommand { get; }
        public ICommand PlayPauseCommand { get; }
        public ICommand StopCommand { get; }
        public ObservableCollection<TrackViewModel> SearchResultList { get; }
        public ObservableCollection<SelectableTag> Genres { get; }

        public string SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        private void LoadGenresData(List<string> tagList)
        {
            Genres.Clear();

            foreach (var tag in tagList)
            {
                Genres.Add(new SelectableTag { Name = tag });
            }
        }

        private async Task SearchAsync()
        {
            var trackQueryParams = new TrackQueryParams
            {
                Tags = GetSelectedTags(),
                Name = _searchText
            };

            var apiResponse = await _jamendoService.GetTracksAsync(trackQueryParams);

            if (apiResponse == null || !string.IsNullOrEmpty(apiResponse.Headers?.ErrorMessage))
                return;

            LoadSearchResultListData(apiResponse.Results);
        }

        private string GetSelectedTags() => string.Join("+", Genres.Where(g => g.IsSelected).Select(g => g.Name));

        private void LoadSearchResultListData(List<Track> tracks)
        {
            SearchResultList.Clear();

            foreach (var track in tracks)
            {
                SearchResultList.Add(new TrackViewModel(track));
            }
        }

        private void OnTrackPositionChanged(object? sender, MediaPlayerPositionChangedEventArgs args)
        {
            _currentTrack.Position = args.Position == -1 ? 0 : args.Position;
        }

        private void PlayPause(TrackViewModel track)
        {
            string url = track.Track.Audio;
            if (url != CurrentTrackUrl)
            {
                _currentTrack = track;
                _player.Play(url);
                CurrentTrackUrl = url;
                ResetCurrentlyPlayingTrack();
                track.IsPlaying = true;
            }
            else
            {
                _player.Pause();
                track.IsPlaying = !track.IsPlaying;
            }
        }

        private void Stop()
        {
            _player.Stop();
            CurrentTrackUrl = null;
            ResetCurrentlyPlayingTrack();
        }

        private void ResetCurrentlyPlayingTrack()
        {
            var currentlyPlayingTrack = SearchResultList.FirstOrDefault(t => t.IsPlaying);
            if (currentlyPlayingTrack != null)
            {
                currentlyPlayingTrack.IsPlaying = false;
            }
        }

        public partial class SelectableTag : ObservableObject
        {
            [ObservableProperty] private string _name;

            [ObservableProperty] private bool _isSelected;
        }
    }
}