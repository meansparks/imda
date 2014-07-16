using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenEng.ConfigParser
{
    class ConfigData
    {
        public ConfigData()
        {

        }

        private string plugin_name;
        public string Plugin_name
        {
            get { return plugin_name; }
            set { plugin_name = value; }
        }

        private string plugin_version;
        public string Plugin_version
        {
            get { return plugin_version; }
            set { plugin_version = value; }
        }

        private string plugin_copyrigth;
        public string Plugin_copyrigth
        {
            get { return plugin_copyrigth; }
            set { plugin_copyrigth = value; }
        }
    }
}
