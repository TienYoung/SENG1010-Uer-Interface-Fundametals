using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Id3;
using Microsoft.Win32;

namespace Task_3
{
    class SoundModel
    {
        public string? Artist { get; set; }

        public string? Album { get; set; }

        public int Track { get; set; }

        public string? Title { get; set; }

        public string? Filename { get; set; }

        public SoundModel(string? artist, string? album, int track, string? title, string? filename)
        {
            Artist = artist;
            Album = album;
            Track = track;
            Title = title;
            Filename = filename;
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
            for (int i = 0; i < openFileDialog.FileNames.Length; i++)
            {
                string filename = openFileDialog.FileNames[i];
                string safename = openFileDialog.SafeFileNames[i];

                using (var mp3 = new Mp3(filename))
                {
                    Id3Tag tag = mp3.GetTag(Id3TagFamily.Version2X);
                    smList.Add(new SoundModel(tag.Artists, tag.Album, tag.Track, tag.Title, safename));
                }
            }

            return smList.ToArray();
        }
    }
}
