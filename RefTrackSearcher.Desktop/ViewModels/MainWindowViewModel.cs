using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RefTrackSearcher.Core.Interfaces.Services;
using RefTrackSearcher.Core.Models;

namespace RefTrackSearcher.Desktop.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly IJamendoService _jamendoService;
        private readonly IJamendoTagsService _jamendoTagsService;
        private string _searchText;

        public MainWindowViewModel(IJamendoService jamendoService, IJamendoTagsService jamendoTagsService)
        {
            _jamendoService = jamendoService;
            _jamendoTagsService = jamendoTagsService;
            
            SearchCommand = new AsyncRelayCommand(SearchAsync);
            Genres = new ObservableCollection<SelectableTag>();
            SearchResultList = new ObservableCollection<Track>();
            
            LoadGenresData(_jamendoTagsService.GetGenres());
        }

        public ICommand SearchCommand { get; }
        public ObservableCollection<Track> SearchResultList { get; }
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
            const string clientId = "";
            var trackQueryParams = new TrackQueryParams
            {
                Tags = GetSelectedTags(),
                Name = _searchText
            };
            
            var apiResponse = await _jamendoService.GetTracksAsync(clientId, trackQueryParams);
            
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
                SearchResultList.Add(track);
            }
        }

        public partial class SelectableTag : ObservableObject
        {
            [ObservableProperty]
            private string _name;
            
            [ObservableProperty]
            private bool _isSelected;
        }
    }
}
