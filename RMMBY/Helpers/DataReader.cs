using System;
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

            if (result == "") result = "INVALID DATA TYPE";
            return result;
        }

        public static string[] ReadDataAll(string itemType)
        {
            List<string> list = new List<string>();

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

            string[] result = list.ToArray();

            return result;
        }

        public static string[] ReadDataMulti(string itemType)
        {
            List<string> list = new List<string>();

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

            string[] result = list.ToArray();

            return result;
        }
    }
}
