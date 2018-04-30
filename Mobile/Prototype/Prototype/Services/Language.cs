using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;

namespace Prototype.Services
{
    public class Language
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Short { get; set; }

        public String GetLanguageResult(string test)
        {
            //String[] directory = Directory.GetFiles("/data/user/0/com.Aveva.Eva/");
            //String root = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory());
            //System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)
            //"/data/user/0/com.Aveva.Eva/files/.__override__"
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Language)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("Prototype.Droid." + this.Short + ".yml");
            //StreamReader file = new StreamReader(Directory.GetCurrentDirectory() + "/Language/" + this.Short + ".yml");
            string line = "";
            StreamReader reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                if (line.StartsWith(test))
                {
                    line = line.Split(':')[1].Trim().Trim('"');
                    break;
                }
            }
            return line;
        }
    }
}
