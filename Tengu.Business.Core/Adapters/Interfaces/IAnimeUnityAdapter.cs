﻿using Tengu.Business.Commons;

namespace Tengu.Business.Core
{
    public interface IAnimeUnityAdapter
    {
        Task Download(string downloadPath, string animeUrl, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetLatestEpisode(int count, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchByFilters(AnimeSaturnSearchFilterInput searchFilter, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchByTitle(string title, CancellationToken cancellationToken = default);
    }
}