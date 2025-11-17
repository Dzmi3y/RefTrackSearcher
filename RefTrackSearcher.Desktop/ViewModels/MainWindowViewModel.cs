using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using RefTrackSearcher.Core.Interfaces.Services;
using RefTrackSearcher.Core.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RefTrackSearcher.Desktop.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _searchText;
        private readonly IJamendoService _jamendoService;
        private readonly IJamendoTagsService _jamendoTagsService;

        public MainWindowViewModel(IJamendoService jamendoService, IJamendoTagsService jamendoTagsService)
        {
            _jamendoService = jamendoService;
            _jamendoTagsService = jamendoTagsService;
            SearchCommand = new RelayCommand(async () => await SearchAsync());
            LoadGanrestData(_jamendoTagsService.GetGenres());
        }

        public ICommand SearchCommand { get; }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public ObservableCollection<Track> SearchResultList { get; set; } = new ObservableCollection<Track>();
        public ObservableCollection<SelectableTag> Genres { get; set; } = new ObservableCollection<SelectableTag>();

        private void LoadGanrestData(List<string> tagList)
        {
            SearchResultList.Clear();

            foreach (var tag in tagList)
            {
                Genres.Add(new SelectableTag() { Name = tag });
            }
        }

        private async Task SearchAsync()
        {
            // string clientId = "";
            // TrackQueryParams trackQueryParams = new TrackQueryParams()
            // {
            //     Tags = "rock"
            // };
            // var apiResponce = await _jamendoService.GetTracksAsync(clientId, trackQueryParams);
            //
            // if (apiResponce == null) return;
            // if (!string.IsNullOrEmpty(apiResponce.Headers.ErrorMessage)) return;
            //
            // LoadSearchResultListData(apiResponce.Results);
        }

        private void LoadSearchResultListData(List<Track> newTrackList)
        {
            SearchResultList.Clear();

            foreach (var track in newTrackList)
            {
                SearchResultList.Add(track);
            }
        }

        public class SelectableTag : INotifyPropertyChanged
        {
            private string _name;
            private bool _isSelected;

            public string Name
            {
                get => _name;
                set
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }

            public bool IsSelected
            {
                get => _isSelected;
                set
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
                => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}