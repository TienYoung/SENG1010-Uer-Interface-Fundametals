using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TagLib;
using Microsoft.Win32;
using System.ComponentModel;

namespace Task_3
{
    class SoundModel : INotifyPropertyChanged
    {
        public string? Artist { get; }

        public string? Album { get; }

        public uint? Track { get; }

        public string? Title { get; }

        private string? filename;
        public string? Filename
        {
            get
            {
                return Path.GetFileNameWithoutExtension(filename);
            }
            private set
            {
                filename = value;
                OnPropertyChanged(nameof(Filename));
            }
        }

        private string? newName;
        public string? NewName
        {
            get => newName;
            private set
            {
                newName = value;
                OnPropertyChanged(nameof(NewName));
            }
        }

        public SoundModel(string? artist, string? album, uint? track, string? title, string? filename)
        {
            Artist = artist;
            Album = album;
            Track = track;
            Title = title;
            Filename = filename;
        }

        public void UpdateNewName(string pattern)
        {
            NewName = pattern.Replace("<Artist>", Artist)
                .Replace("<Album>", Album)
                .Replace("<Track>", Track.ToString())
                .Replace("<Title>", Title);
        }

        public void Rename(string newName)
        {
            if (String.IsNullOrEmpty(newName)) return;

            string path = Path.GetDirectoryName(filename) ?? String.Empty;
            string extension = Path.GetExtension(filename ?? String.Empty);
            newName = Path.Combine(path, newName + extension);
            System.IO.File.Move(filename ?? String.Empty, newName);
            Filename = newName;
        }

        public static SoundModel[] LoadSoundsViaDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".mp3";
            openFileDialog.Filter = "MP3 files (.mp3)|*.mp3";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() != true)
            {
                return Array.Empty<SoundModel>();
            }

            List<SoundModel> smList = new List<SoundModel>();
            foreach (var filename in openFileDialog.FileNames)
            {
                using (var mp3 = TagLib.File.Create(filename))
                {
                    var tag = mp3.Tag;
                    smList.Add(new SoundModel(tag.FirstPerformer, tag.Album, tag.Track, tag.Title, filename));
                }
            }

            return smList.ToArray();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
