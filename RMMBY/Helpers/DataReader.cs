using System.Collections.Generic;
using System.IO;
using MelonLoader;

namespace RMMBY.Helpers
{
    internal class DataReader
    {
        private static string dataPath;

        public static void SetupDataReader()
        {
            dataPath = Path.Combine(MelonHandler.ModsDirectory, "RMMBY", "data");
        }

        public static string ReadData(string itemType)
        {
            SetupDataReader();

            string result = "";

            try
            {
                StreamReader r = new StreamReader(dataPath);

                string line;
                using (r)
                {
                    do
                    {
                        //Read a line
                        line = r.ReadLine();
                        if (line != null)
                        {
                            //Divide line into basic structure
                            string[] lineData = line.Split(';');
                            //Check the line type
                            if (lineData[0] == itemType)
                            {
                                result = lineData[1];

                                break;
                            }
                        }
                    }
                    while (line != null);
                    //Stop reading the file
                    r.Close();
                }
            } catch { result = "INVALID DATA TYPE"; }

            return result;
        }

        public static string[] ReadDataAll(string itemType)
        {
            SetupDataReader();

            List<string> list = new List<string>();

            try
            {
                StreamReader r = new StreamReader(dataPath);

                string line;
                using (r)
                {
                    do
                    {
                        //Read a line
                        line = r.ReadLine();
                        if (line != null)
                        {
                            //Divide line into basic structure
                            string[] lineData = line.Split(';');
                            //Check the line type
                            if (lineData[0] == itemType)
                            {
                                list.Add(lineData[1]);
                            }
                        }
                    }
                    while (line != null);
                    //Stop reading the file
                    r.Close();
                }
            }
            catch {
                list = new List<string>
                {
                    "INVALID DATA TYPE"
                };
            }

            string[] result = list.ToArray();

            return result;
        }

        public static string[] ReadDataMulti(string itemType)
        {
            SetupDataReader();

            List<string> list = new List<string>();

            try
            {
                StreamReader r = new StreamReader(dataPath);

                string line;
                using (r)
                {
                    do
                    {
                        //Read a line
                        line = r.ReadLine();
                        if (line != null)
                        {
                            //Divide line into basic structure
                            string[] lineData = line.Split(';');
                            //Check the line type
                            if (lineData[0] == itemType)
                            {
                                for (int i = 1; i < lineData.Length; i++)
                                {
                                    list.Add(lineData[i]);
                                }

                                break;
                            }
                        }
                    }
                    while (line != null);
                    //Stop reading the file
                    r.Close();
                }
            } catch {
                list = new List<string>
                {
                    "INVALID DATA TYPE"
                };
            }

            string[] result = list.ToArray();

            return result;
        }

        public static string[] ReadAllData()
        {
            SetupDataReader();

            List<string> list = new List<string>();

            try
            {
                StreamReader r = new StreamReader(dataPath);

                string line;
                using (r)
                {
                    do
                    {
                        //Read a line
                        line = r.ReadLine();
                        if (line != null)
                        {
                            list.Add(line);
                        }
                    }
                    while (line != null);
                    //Stop reading the file
                    r.Close();
                }
            }
            catch {
                list = new List<string>
                {
                    "INVALID DATA TYPE"
                };
            }

            string[] result = list.ToArray();
            return result;
        }
    }
}
