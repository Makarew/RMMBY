using Newtonsoft.Json;
using System.Collections;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Globalization;

namespace RMMBYGB
{
    class Program
    {
        private static string point = "";
        private static string modName = "";
        private static string fileName = "NoModBetterUseThisExactStringForAFolderOrSomething";

        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CultureInfo.DefaultThreadCurrentCulture = (CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US"));

            if(args.Length != 0)
            {
                try
                {
                    point = "'Get Mod Data'";
                    ModData modData = GetModData(args[0]);

                    point = "'Display Mod Data'";
                    if (modData != null && modData.Name == "" && MessageBox.Show("Couldn't parse mod info. Download unknown mod?", "RMMBY 1-Click", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        CheckDirectory();
                        Run(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mods"), modData.DownloadURL);
                    } else if (modData != null && modData.Name != "" && MessageBox.Show(string.Concat(new string[]
                    {
                        "Download ",
                        modData.Name,
                        " by ",
                        modData.OwnerName,
                        "?"
                    }), DataReader.ReadData("RMMBYVariant") + " 1-Click", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        modName = modData.Name;
                        CheckDirectory();
                        Run(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mods"), modData.DownloadURL);
                    }

                } catch
                {
                    Cleanup(1);
                }
            } else
            {
                MessageBox.Show("This is the " + DataReader.ReadData("RMMBYVariant") + " GameBanana 1-Click installer. Launch the game once, and this will enable 1-Click installs for your game.",
                    DataReader.ReadData("RMMBYVariant") + " 1-Click", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                Application.ExitThread();
            }
        }

        public static void Run(string root, string url)
        {
            point = "'Get File Locations'";

            string from = Path.Combine(root, $"tmpdownload-{DateTime.Now:yyyyMMdd-HHmmss}.zip.part");
            fileName = from;

            try
            {
                if (File.Exists(from))
                    File.Delete(from);

                Console.WriteLine("Check File");

                point = "'Get GBID'";
                uint gbid = 0;
                if ((url.StartsWith("http://gamebanana.com/dl/") && !uint.TryParse(url.Substring("http://gamebanana.com/dl/".Length), out gbid)) ||
                    (url.StartsWith("https://gamebanana.com/dl/") && !uint.TryParse(url.Substring("https://gamebanana.com/dl/".Length), out gbid)) ||
                    (url.StartsWith("http://gamebanana.com/mmdl/") && !uint.TryParse(url.Substring("http://gamebanana.com/mmdl/".Length), out gbid)) ||
                    (url.StartsWith("https://gamebanana.com/mmdl/") && !uint.TryParse(url.Substring("https://gamebanana.com/mmdl/".Length), out gbid)))
                    gbid = 0;

                Console.WriteLine("gbid " + gbid);

                if (gbid != 0)
                {
                    point = "'Downloading Mod'";

                        using (var client = new WebClient())
                        {
                            File.Create(from);

                        File.WriteAllBytes(from, client.DownloadData(url));
                        }
                }
                else
                {
                    Cleanup(2);
                }

                Extract(from, root);
            }
            finally
            {
                Cleanup(0);
            }
        }

        private static void Extract(string source, string destination)
        {
            string toZip = source.Replace(".part", "");
            File.Move(source, toZip);
            if(!point.Contains("Secondary"))
                point = "'Extract Zip'";

            ZipFile.ExtractToDirectory(toZip, destination);

            if (modName != "")
            {
                MessageBox.Show(string.Concat(new string[]
                {
                modName,
                " has installed successfully."
                }),
                DataReader.ReadData("RMMBYVariant") + " 1-Click", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            } else
            {
                MessageBox.Show(string.Concat(new string[]
                {
                "Unknown mod",
                " has installed successfully."
                }),
                DataReader.ReadData("RMMBYVariant") + " 1-Click", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            Application.ExitThread();
        }

        public static void CheckDirectory()
        {
            string tempDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mods");

            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }
        }

        public static ModData GetModData(string url)
        {
            string[] array = url.Split(new char[]
            {
                ','
            });

            if (array.Length < 3)
                return null;

            int num = DataReader.ReadData("GBSchema").Length + 3;

            string downloadUrl = array[0].Substring(num, array[0].Length - num);
            string itemType = array[1];
            int itemId;

            int.TryParse(array[2], out itemId);

            return GetItemData(downloadUrl, itemType, itemId);
        }

        public static ModData GetItemData(string downloadUrl, string itemType, int itemId)
        {
            ModData itemData = new ModData(downloadUrl, itemType, itemId);
            string requestUri = CreateRequestUrl<ModData>(itemData, itemType, itemId);

            ModData itemData2 = JsonConvert.DeserializeObject<ModData>(Uri.UnescapeDataString(Client.Get().GetStringAsync(requestUri).Result));
            if (itemData2 != null)
            {
                itemData = itemData2;
            }

            itemData.DownloadURL = downloadUrl;
            itemData.ItemType = itemType;
            itemData.ItemID = itemId;
            return itemData;
        }

        public static string CreateRequestUrl<T>(T item, string itemType, int itemID)
        {
            List<string> supportedFields = GetSupportedFields(itemType);
            string text = string.Format("https://api.gamebanana.com/Core/Item/Data?itemtype={0}&itemid={1}&fields=", itemType, itemID);
            PropertyInfo[] properties = item.GetType().GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                JsonPropertyAttribute jsonPropertyAttribute = (JsonPropertyAttribute)properties[i].GetCustomAttribute(typeof(JsonPropertyAttribute));

                if (jsonPropertyAttribute != null && supportedFields.Contains(jsonPropertyAttribute.PropertyName)){
                    if(text.Last<char>() != '=')
                    {
                        text += ",";
                    }
                    text += jsonPropertyAttribute.PropertyName;
                }
            }

            return text + "&format=json&return_keys=1";
        }

        public static List<string> GetSupportedFields(string itemType)
        {
            List<string> result;
            try
            {
                result = JsonConvert.DeserializeObject<List<string>>(Client.Get().GetStringAsync("https://api.gamebanana.com/Core/Item/Data/AllowedFields?itemtype=" + itemType).Result);
            }
            catch
            {
                result = new List<string>();
            }
            return result;
        }

        public static void Cleanup(int error)
        {
            if (error == 1)
            {
                MessageBox.Show(string.Concat(new string[]
                        {
                        "Error installing ",
                        modName,
                        " at point: ",
                        point
                        }),
                        DataReader.ReadData("RMMBYVariant") + " 1-Click", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            } else if (error == 2)
            {
                MessageBox.Show(string.Concat(new string[]
                        {
                        "Error installing ",
                        modName,
                        ". Couldn't access GameBanana file."
                        }),
                        DataReader.ReadData("RMMBYVariant") + " 1-Click", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            if (File.Exists(fileName))
                File.Delete(fileName);

            if (File.Exists(fileName.Replace(".part", "")))
                File.Delete(fileName.Replace(".part", ""));

            Application.ExitThread();
        }
    }
}
