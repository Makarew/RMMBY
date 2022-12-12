using System.Collections.Generic;
using System.IO;

namespace RMMBY.Helpers
{
    internal class WriteToFile
    {
        public static void WriteFile(string file, string[] lines, bool append)
        {
            if (append)
            {
                StreamReader r = new StreamReader(file);

                List<string> newLines = new List<string>();

                string line;
                using (r)
                {
                    do
                    {
                        //Read a line
                        line = r.ReadLine();
                        if (line != null)
                        {
                            if (line != "" && line != " " && line != "  ")
                                newLines.Add(line);
                        }
                    }
                    while (line != null);
                    //Stop reading the file
                    r.Close();
                }

                for (int i = 0; i < lines.Length; i++)
                {
                    newLines.Add(lines[i]);
                }

                StreamWriter sw = new StreamWriter(file, false);

                for (int i = 0; i < newLines.Count; i++)
                {
                    sw.WriteLine(newLines[i]);
                }
                sw.Close();
            }
            else
            {
                StreamWriter sw = new StreamWriter(file, false);

                for (int i = 0; i < lines.Length; i++)
                {
                    sw.WriteLine(lines[i]);
                }
                sw.Close();
            }

        }

        public static void ReplaceLine(string file, string oldLine, string newLine, int linePosition, bool remove)
        {
            StreamReader r = new StreamReader(file);

            List<string> lines = new List<string>();

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

                        string modLine = "";
                        //Check the line type
                        if (remove && lineData[linePosition] == oldLine)
                        {
                        }
                        else if (!remove && lineData[linePosition] == oldLine)
                        {
                            lineData[linePosition] = newLine;

                            for (int i = 0; i < lineData.Length; i++)
                            {
                                modLine += lineData[i];

                                if (i != lineData.Length - 1)
                                {
                                    modLine += ";";
                                }
                            }
                        }
                        else if (lineData[linePosition] != oldLine)
                        {
                            for (int i = 0; i < lineData.Length; i++)
                            {
                                modLine += lineData[i];

                                if (i != lineData.Length - 1)
                                {
                                    modLine += ";";
                                }
                            }
                        }
                        if (lineData.Length > 1)
                            lines.Add(modLine);
                    }
                }
                while (line != null);
                //Stop reading the file
                r.Close();
            }

            WriteFile(file, lines.ToArray(), false);
        }
    }
}