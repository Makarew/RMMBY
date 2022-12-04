using System;
using Newtonsoft.Json;

namespace RMMBYGB
{
    public class ModFile
    {
        [JsonProperty("_idRow")]
        public string Id { get; set; }

        [JsonProperty("_sFile")]
        public string FileName { get; set; }

        [JsonProperty("_sDownloadUrl")]
        public string DownloadUrl { get; set; }

        [JsonProperty("_sDescription")]
        public string Description { get; set; }

        [JsonProperty("_bContainsExe")]
        public bool ContainsExe { get; set; }

        [JsonProperty("_tsDateAdded")]
        public long DateAddedLong { get; set; }

        [JsonIgnore]
        public DateTime DateAdded
        {
            get
            {
                return ModFile.Epoch.AddSeconds((double)this.DateAddedLong);
            }
        }

        private static readonly DateTime Epoch = new DateTime(1970, 1, 1);
    }
}
