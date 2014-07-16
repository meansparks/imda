using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranModelEng.ConfigParser
{
    class ModelDirData
    {
        public const String DEF_EXTNAME = "xml";

        public ModelDirData()
        {

        }

        private string dirPath;
        public string DirPath
        {
            get { return dirPath; }
            set { dirPath = value; }
        }

        private Boolean isSubPackage;
        public Boolean IsSubPackage
        {
            get { return isSubPackage; }
            set { isSubPackage = value; }
        }
        
        private string extName;
        public string ExtName
        {
            get { return extName; }
            set { extName = value; }
        }

    }
}
