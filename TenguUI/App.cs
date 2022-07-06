using Downla.Models;
using Tengu.Business.API.DTO;
using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;
using TenguUI.Controllers.Interfaces;

namespace TenguUI
{
    public partial class App : Form
    {
        private DownloadMonitor? currentDownload;
        public TenguHosts[] Hosts { get; set; } = new TenguHosts[] { TenguHosts.AnimeSaturn, TenguHosts.AnimeUnity };
        
        private readonly ITenguController _tenguController;
        
        
        public App(ITenguController commandsController)
        {
            InitializeComponent();

            _tenguController = commandsController;

            _tenguController.SetHosts(Hosts);

            GetEpisodesComboBox.DataSourceChanged += GetEpisodesSourceChangedHandler;
            VideoComboBox.DataSourceChanged += VideoSourceChangedHandler;
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            TenguSearchFilter tenguSearchFilter = new()
            {
                Genres = new TenguGenres[] { TenguGenres.ArtiMarziali }
            };

            var result = await _tenguController.SearchAnimesAsync(SearchTitleTextBox.Text, tenguSearchFilter);
            
            GetEpisodesComboBox.DataSource = result;
        }

        private async void GetEpisodesButton_Click(object sender, EventArgs e)
        {
            var anime = (AnimeModel)GetEpisodesComboBox.SelectedItem;
            var result = await _tenguController.GetEpisodesAsync(anime.Id, anime.Host, 0, 24);

            VideoComboBox.DataSource = result;
        }
        private async void LoadMoreEpisodesButton_Click(object sender, EventArgs e)
        {
            var anime = (AnimeModel)GetEpisodesComboBox.SelectedItem;

            var currentEpisodes = (EpisodeModel[])VideoComboBox.DataSource;
            var lastIndex = int.Parse(currentEpisodes[^1].EpisodeNumber);

            var result = await _tenguController.GetEpisodesAsync(anime.Id, anime.Host, lastIndex, lastIndex + 24);

            VideoComboBox.DataSource = result;
        }

        private void StartDownloadButton_Click(object sender, EventArgs e)
        {
            var episode = (EpisodeModel)VideoComboBox.SelectedItem;

            currentDownload = _tenguController.DownloadAsync(episode.Url, episode.Host);

            VideoProgressBarUpdater.RunWorkerAsync();
        }

        private void VideoProgressBarUpdater_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if(currentDownload != null)
            {
                while(currentDownload.Status == Downla.DownloadStatuses.Pending || currentDownload.Status == Downla.DownloadStatuses.Downloading)
                {
                    //VideoProgressBar.Value = currentDownload.Percentage;
                    Thread.Sleep(500);
                }
            }

        }


        #region Delegates
        private void GetEpisodesSourceChangedHandler(object? sender, EventArgs e)
        {
            var comboBox = (ComboBox) sender!;
            var dataSource = (AnimeModel[])comboBox.DataSource!;

            if(dataSource.Any())
            {
                GetEpisodesComboBox.Enabled = true;
                GetEpisodesButton.Enabled = true;

                LoadMoreEpisodesButton.Enabled = true;
            }
            else
            {
                GetEpisodesComboBox.Text = "No anime founds";

                GetEpisodesComboBox.Enabled = false;
                GetEpisodesButton.Enabled = false;
            }
        }
        private void VideoSourceChangedHandler(object? sender, EventArgs e)
        {
            var comboBox = (ComboBox)sender!;
            var dataSource = (EpisodeModel[])comboBox.DataSource!;

            if (dataSource.Any())
            {
                VideoComboBox.Enabled = true;
                StartDownloadButton.Enabled = true;
            }
            else
            { 
                VideoComboBox.Text = "No episode founds";
    
                VideoComboBox.Enabled = false;
                StartDownloadButton.Enabled = false;
            }
        }
        #endregion

    }
}