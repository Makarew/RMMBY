using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RMMBYGB
{
    public class ModData
    {
        [JsonIgnore]
        public string DownloadURL { get; set; }

        [JsonIgnore]
        public string ItemType { get; set; }

        [JsonIgnore]
        public int ItemID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("Owner().name")]
        public string OwnerName { get; set; }

        [JsonProperty("Updates().bSubmissionsHasUpdates()")]
        public bool? HasUpdates { get; set; }

        [JsonProperty("Files().aFiles()")]
        public Dictionary<string, ModFile> Files { get; set; } = new Dictionary<string, ModFile>();

        public ModData(string downloadURL, string itemType, int itemID)
        {
            this.DownloadURL = downloadURL;
            this.ItemType = itemType;
            this.ItemID = itemID;
        }
    }
}
