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
        private string? _nextPageUrl;
        private TrackQueryParams _lastTrackQueryParams;
        private readonly int _pageSize = 10;


        [ObservableProperty] private bool _isNextPageExist;
        [ObservableProperty] private string _currentTrackUrl;

        public MainWindowViewModel(IJamendoService jamendoService, IJamendoTagsService jamendoTagsService)
        {
            _jamendoService = jamendoService;
            _jamendoTagsService = jamendoTagsService;

            SearchCommand = new AsyncRelayCommand(SearchAsync);
            PlayPauseCommand = new RelayCommand<TrackViewModel>(PlayPause);
            StopCommand = new RelayCommand<TrackViewModel>(Stop);
            PositionChangedCommand = new RelayCommand<TrackViewModel>(PositionChanged);
            LoadNextPageCommand = new AsyncRelayCommand(LoadPageAsync);
            Genres = new ObservableCollection<SelectableTag>();
            SearchResultList = new ObservableCollection<TrackViewModel>();

            LoadGenresData(_jamendoTagsService.GetGenres());
            _player.PositionChanged += OnTrackPositionChanged;
        }

        public ICommand SearchCommand { get; }
        public ICommand PlayPauseCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand PositionChangedCommand { get; }

        public ICommand LoadNextPageCommand { get; }
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
            SearchResultList.Clear();

            _lastTrackQueryParams = new TrackQueryParams
            {
                Tags = GetSelectedTags(),
                Name = _searchText,
                Offset = 0,
                Limit = _pageSize
            };
            await LoadPageAsync();
        }

        private async Task LoadPageAsync()
        {
            var apiResponse = await _jamendoService.GetTracksAsync(_lastTrackQueryParams);

            if (apiResponse == null || !string.IsNullOrEmpty(apiResponse.Headers?.ErrorMessage))
                return;

            LoadSearchResultListData(apiResponse);
            _lastTrackQueryParams.Offset += _pageSize;
        }

        private string GetSelectedTags() => string.Join("+", Genres.Where(g => g.IsSelected).Select(g => g.Name));

        private void LoadSearchResultListData(ApiResponse apiResponse)
        {
            _nextPageUrl = apiResponse.Headers?.Next;
            IsNextPageExist = !string.IsNullOrEmpty(_nextPageUrl);

            foreach (var track in apiResponse.Results)
            {
                SearchResultList.Add(new TrackViewModel(track));
            }
        }

        private void OnTrackPositionChanged(object? sender, MediaPlayerPositionChangedEventArgs args)
        {
            if (args.Position != -1)
            {
                _currentTrack.Position = args.Position;
            }
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
                _player.Position = track.Position;
            }
            else
            {
                _player.Pause();
                track.IsPlaying = !track.IsPlaying;
            }
        }

        private void Stop(TrackViewModel track)
        {
            if (track.IsPlaying)
            {
                _player.Stop();
                CurrentTrackUrl = null;
                ResetCurrentlyPlayingTrack();
            }

            track.Position = 0;
        }

        private void ResetCurrentlyPlayingTrack()
        {
            var currentlyPlayingTrack = SearchResultList.FirstOrDefault(t => t.IsPlaying);
            if (currentlyPlayingTrack != null)
            {
                currentlyPlayingTrack.IsPlaying = false;
            }
        }

        private void PositionChanged(TrackViewModel track)
        {
            if (track.IsPlaying)
            {
                _player.Position = track.Position;
            }
        }

        public partial class SelectableTag : ObservableObject
        {
            [ObservableProperty] private string _name;

            [ObservableProperty] private bool _isSelected;
        }
    }
}