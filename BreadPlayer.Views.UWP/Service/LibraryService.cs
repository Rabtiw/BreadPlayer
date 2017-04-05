﻿using System.Collections.Generic;
using BreadPlayer.Models;
using System.Threading.Tasks;
using BreadPlayer.Common;
using DBreeze.Objects;

namespace BreadPlayer.Service
{
	/// <summary>
	/// Provide services for retrieving and storing Customer information
	/// </summary>
	public class LibraryService : ILibraryService
    {
        private List<DBreezeIndex> TracksIndexes(Mediafile track)
        {
            return new List<DBreezeIndex>()
            {
                new DBreezeIndex(1, track.Path) { PrimaryIndex = true },
                new DBreezeIndex(2, track.FolderPath),
                new DBreezeIndex(3, track.LeadArtist),
                new DBreezeIndex(4, track.Album),
            };
        }
        private List<DBreezeIndex> PlaylistIndexes(Playlist playlist)
        {
            return new List<DBreezeIndex>()
            {
                new DBreezeIndex(1, playlist.Name) { PrimaryIndex = true },
            };
        }
        private IDatabaseService Database
        {
            get;
            set;
        }

        public LibraryService(IDatabaseService service)
        {
            Database = service;
        }

        #region ILibraryService 
        public IEnumerable<Mediafile> Query(string term)
        {
            return Database.QueryRecords<Mediafile>("Tracks", term);
        }
        public IEnumerable<Mediafile> GetAllMediafiles()
        {
            return Database.GetRecords<Mediafile>("Tracks");
        }
        public void AddMediafile(Mediafile data)
        {
            Database.InsertRecord("Tracks", TracksIndexes(data), data);
        }
        public void AddMediafiles(IEnumerable<Mediafile> data)
        {
            Database.InsertTracks(data);
        }
        public void UpdateMediafile(Mediafile data)
        {
            Database.UpdateRecord("Tracks", data.Path, data);
        }
        public void UpdateMediafiles(IEnumerable<Mediafile> data)
        {
            Database.UpdateTracks(data);
        }
        public void RemoveFolder(string folderPath)
        {
            //Database.RemoveTracks(LiteDB.Query.EQ("FolderPath", folderPath));
            // Core.CoreMethods.LibVM.TracksCollection.Elements.RemoveRange(Core.CoreMethods.LibVM.TracksCollection.Elements.Where(t => t.FolderPath == folderPath));
        }
        public void RemoveMediafile(Mediafile data)
        {
            Database.RemoveRecord("Tracks", data.Path);
        }
        public Mediafile GetMediafile(string path)
        {
            return Database.GetRecord<Mediafile>("Tracks", path);
        }
        public void AddPlaylist(Playlist pList)
        {
            Database.InsertRecord("Playlists", PlaylistIndexes(pList), pList);
        }
        public IEnumerable<Playlist> GetPlaylists()
        {
            return Database.GetRecords<Playlist>("Playlists");
        }
        public Playlist GetPlaylist(string name)
        {
            return Database.GetRecord<Playlist>("Playlists", name);
        }
        public bool CheckExists<T>(string table, string path)
        {
            return Database.CheckExists<T>(table, path);
        }
        public void RemovePlaylist(Playlist List)
        {
            Database.RemoveRecord("Playlists", List.Name);
        }
        public int SongCount
        {
            get { return Database.GetRecordsCount("Tracks"); }
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            Database.Dispose();
        }
        #endregion
    }
}
