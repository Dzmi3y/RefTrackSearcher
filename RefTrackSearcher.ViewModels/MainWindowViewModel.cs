using CommunityToolkit.Mvvm.Input;
using RefTrackSearcher.Core.Interfaces.Services;
using RefTrackSearcher.Core.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RefTrackSearcher.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly IJamendoService _jamendoService;
        public MainWindowViewModel(IJamendoService jamendoService)
        {
            _jamendoService = jamendoService;
            SearchCommand = new RelayCommand(async () => await SearchAsync());
        }

        public ICommand SearchCommand { get; }

        public string Greeting { get; } = "Welcome to Avalonia!";

        public ObservableCollection<Track> TestList { get; set; } = new ObservableCollection<Track>();

        private async Task SearchAsync()
        {
            string clientId = "";
            TrackQueryParams trackQueryParams = new TrackQueryParams()
            {
                Tags = "rock"
            };
            var apiResponce = await _jamendoService.GetTracksAsync(clientId, trackQueryParams);

            if (apiResponce == null) return;
            if (!string.IsNullOrEmpty(apiResponce.Headers.ErrorMessage)) return;

            LoadListData(apiResponce.Results);
        }

        private void LoadListData(List<Track> newTrackList)
        {
            TestList.Clear();

            foreach (var item in newTrackList)
            {
                TestList.Add(item);
            }
        }

    }
}
