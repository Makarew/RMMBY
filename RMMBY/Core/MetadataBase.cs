using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace RMMBY
{
    public class MetadataBase
    {
        public string Title { get; set; } = "N/A";
        public string Description { get; set; } = "N/A";
        public string Version { get; set; } = "N/A";
        public string Author { get; set; } = "N/A";
        public List<string> Modules { get; set; }
        public string SettingsFile { get; set; } = "N/A";
        public string ConfigFile { get; set; } = "N/A";
        public bool Restart { get; set; } = false;
        public int GamebananaID { get; set; } = -1;
        public int GamebananaIndex { get; set; } = 0;
        public string Location { get; private set; }
        public MetadataState State { get; private set; } = MetadataState.Success;

        public static T Load<T>(string path) where T : MetadataBase
        {
            T t;
            try
            {
                t = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
                t.Location = Path.GetDirectoryName(path);
                for (int i = 0; i < t.Modules.Count; i++)
                {
                    string text = t.Modules[i] = Path.Combine(t.Location, t.Modules[i]);
                    bool flag = !File.Exists(text);
                    if (flag)
                    {
                        t.State = MetadataState.NoModule;
                    }
                }
            }
            catch (JsonReaderException exception)
            {
                t = default(T);
                t.Title = Path.GetFileName(Path.GetDirectoryName(path));
                t.State = MetadataState.BadJson;
            }

            return t;
        }

        public static MetadataBase Load(string path)
        {
            return MetadataBase.Load<MetadataBase>(path);
        }
    }
}
