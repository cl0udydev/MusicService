using System.Collections.Generic;
using MusicService.Domain;

namespace MusicService.Infrastructure.Storage.DataModels;

public interface IPlaylistsDataModel: IDataModel<Dictionary<int, Playlist>> {}