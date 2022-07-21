using Downla;
using Downla.Models;
using Tengu.Business.API.DTO;
using Tengu.Business.API.Interfaces;
using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;
using TenguUI.Controllers.Interfaces;

namespace TenguUI
{
    public partial class App : Form
    {
        public List<TenguHosts> Hosts { get; set; } = new List<TenguHosts>() { TenguHosts.AnimeSaturn, TenguHosts.AnimeUnity };
        
        private readonly ITenguController _tenguController;

        private readonly ITenguApi _tenguApi;

        private DownloadMonitor? currentDownload;

        private CancellationTokenSource cts = new CancellationTokenSource();

        public App(ITenguController commandsController, ITenguApi tenguApi)
        {
            InitializeComponent();

            _tenguController = commandsController;
            _tenguApi = tenguApi;

            _tenguController.SetHosts(Hosts.ToArray());

            DownlaMaxConnectionsTextBox.Text = "10";

            DownlaMaxPacketSizeTextBox.Text = "40000000";

            DownlaDownloadPathTextBox.Text = _tenguApi.DownloadPath = $"{Environment.CurrentDirectory}\\DownloadedAnime";

            GetEpisodesComboBox.DataSourceChanged += GetEpisodesSourceChangedHandler;
            VideoComboBox.DataSourceChanged += VideoSourceChangedHandler;
        }

        private void Infos_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            DownloadMonitorInfos infos = (DownloadMonitorInfos)sender!;
            switch (e.PropertyName)
            {
                case nameof(DownloadMonitor.Infos.DownloadedPackets):
                    BeginInvoke( () =>
                    {
                        LogBox.Text = $"Packet Downloaded: {infos.DownloadedPackets}\r\n" + LogBox.Text;
                    });
                    break;
                case nameof(DownloadMonitor.Infos.Percentage):
                    BeginInvoke(() =>
                    {
                        VideoProgressBar.Value = infos.Percentage;
                        if(infos.Percentage == 100)
                        {
                            StartDownloadButton.Enabled = true;
                            StopDownloadButton.Enabled = false;
                        }
                    });
                    break;
            }
        }

        private void CurrentDownload_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            DownloadMonitor monitor = (DownloadMonitor)sender!;
            switch (e.PropertyName)
            {
                case nameof(DownloadMonitor.Status):
                    BeginInvoke(() =>
                    {
                        LogBox.Text = $"Status Changed: {monitor.Status}\r\n" + LogBox.Text;
                    });
                    break;
            }
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {

            TenguSearchFilter tenguSearchFilter = new()
            {
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

        private async void StartDownloadButton_Click(object sender, EventArgs e)
        {
            cts.TryReset();

            var episode = (EpisodeModel)VideoComboBox.SelectedItem;

            currentDownload = await _tenguController.StartDownloadAsync(episode.DownloadUrl, episode.Host, cts.Token);

            currentDownload.PropertyChanged += CurrentDownload_PropertyChanged;
            currentDownload.Infos.PropertyChanged += Infos_PropertyChanged;
            StopDownloadButton.Enabled = true;
            StartDownloadButton.Enabled = false;
        }

        private void StopDownloadButton_Click(object sender, EventArgs e)
        {
            cts.Cancel();

            StopDownloadButton.Enabled = false;
            StartDownloadButton.Enabled = true;
        }

        private void DownlaMaxConnectionsTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(DownlaMaxConnectionsTextBox.Text))
            {
                _tenguApi.MaxConnections = int.Parse(DownlaMaxConnectionsTextBox.Text);
            }
        }

        private void DownlaMaxPacketSizeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(DownlaMaxPacketSizeTextBox.Text))
            {
                _tenguApi.MaxPacketSize = int.Parse(DownlaMaxPacketSizeTextBox.Text);
            }
        }

        private void AnimeSaturnCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (AnimeSaturnCheckBox.Checked)
            {
                Hosts.Add(TenguHosts.AnimeSaturn);
            }
            else
            {
                Hosts.Remove(TenguHosts.AnimeSaturn);
            }

            _tenguApi.CurrentHosts = Hosts.ToArray();
        }

        private void AnimeUnityCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (AnimeUnityCheckBox.Checked)
            {
                Hosts.Add(TenguHosts.AnimeUnity);
            }
            else
            {
                Hosts.Remove(TenguHosts.AnimeUnity);
            }

            _tenguApi.CurrentHosts = Hosts.ToArray();
        }

        private void ChoosePathButton_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK)
                {
                    DownlaDownloadPathTextBox.Text = _tenguApi.DownloadPath = fbd.SelectedPath;
                }
            }
        }

        #region Delegates
        private void GetEpisodesSourceChangedHandler(object? sender, EventArgs e)
        {
            var comboBox = (ComboBox)sender!;
            var dataSource = (AnimeModel[])comboBox.DataSource!;

            if (dataSource.Any())
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