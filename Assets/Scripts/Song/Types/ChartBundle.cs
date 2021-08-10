using Assets.Scripts.Song.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Song.Types
{
    [Serializable]
    public class ChartBundle
    {
        public string Title;
        public string Artist;
        public string Genre;
        public string Source;
        public DateTime ReleaseDate;

        public string CoverArtFile;
        public string BGFile;
        public Dictionary<Difficulty, string> ChartFiles;

        public static ChartBundle DeserialiseChartBundleFile(string chartBundleJSON)
        {
            return JsonConvert.DeserializeObject<ChartBundle>(chartBundleJSON);
        }
    }
}
