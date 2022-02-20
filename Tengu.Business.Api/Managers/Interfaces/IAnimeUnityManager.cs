﻿using Tengu.Business.Commons;

namespace Tengu.Business.API
{
    public interface IAnimeUnityManager
    {
        Task Download(string downloadPath, string episodeId, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetLatestEpisodes(int offset ,int limit, CancellationToken cancellationToken = default);
        Task<EpisodeModel[]> GetEpisodes(string animeId, int offset = 0, int limit = 0, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnime(SearchFilter filter, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnime(string title, CancellationToken cancellationToken = default);
        Task<AnimeModel[]> SearchAnime(string title, SearchFilter filter, CancellationToken cancellationToken = default);
    }
}
