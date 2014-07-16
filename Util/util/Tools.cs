using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.util
{
    public class Tools
    {
        public static void writerFile(String file_path, String file_name, String suffix_name, String text)
        {
            TextWriter tw;
            String full_file_path = file_path + "\\" + file_name + "." + suffix_name;

            if (!Directory.Exists(file_path))
            {
                Directory.CreateDirectory(file_path);
            }

            if (!File.Exists(full_file_path))
            {
                tw = File.CreateText(full_file_path);
            }
            else
            {
                // create a writer and open the file
                tw = new StreamWriter(full_file_path, false, Encoding.UTF8);
            }

            // write a line of text to the file
            tw.WriteLine(text);

            // close the stream
            tw.Close();
        }

        public static void writerOutput(EA.Repository m_Repository, String message)
        {
            String now_str = DateTime.Now.ToString("HH:mm:ss");
            m_Repository.CreateOutputTab("IMDA");
            m_Repository.EnsureOutputVisible("IMDA");
            m_Repository.WriteOutput("IMDA", "[" + now_str + "] " + message, 1);
        }

        public static String FristU(String text)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
        }
        public static String realFristU(String text)
        {
            string str1 = text.Substring(0, 1);
            string str2 = text.Substring(1, text.Length - 1);
            return str1.ToUpper() + str2;
        }

    }
}
