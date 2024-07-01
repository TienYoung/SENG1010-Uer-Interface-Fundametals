using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TagLib;
using Microsoft.Win32;

namespace Task_3
{
    class SoundModel
    {
        public string? Artist { get; set; }

        public string? Album { get; set; }

        public uint Track { get; set; }

        public string? Title { get; set; }

        public string? Filename { get; set; }

        public SoundModel(string? artist, string? album, uint track, string? title, string? filename)
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

                using (var mp3 = TagLib.File.Create(filename))
                {
                    var tag = mp3.Tag;
                    smList.Add(new SoundModel(tag.FirstPerformer, tag.Album, tag.Track, tag.Title, safename));
                }
            }

            return smList.ToArray();
        }
    }
}
