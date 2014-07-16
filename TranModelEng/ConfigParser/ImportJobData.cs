using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranModelEng.ConfigParser
{
    class ImportJobData
    {
        public const String CREATMODE_NEW = "new";
        public const String CREATMODE_UPDATE = "update";
        public const String CREATMODE_FORCE = "force";

        public ImportJobData()
        {

        }

        private string id;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        private string menuName;
        public string MenuName
        {
            get { return menuName; }
            set { menuName = value; }
        }

        private string targetPackagePath;
        public string TargetPackagePath
        {
            get { return targetPackagePath; }
            set { targetPackagePath = value; }
        }

        private String creatMode;
        public String CreatMode
        {
            get { return creatMode; }
            set { creatMode = value; }
        }

        private List<String> modelFiles;
        public List<String> ModelFiles
        {
            get
            {
                return modelFiles;
            }

            set
            {
                modelFiles = value;
            }
        }

        private List<ModelDirData> modelDirs;
        public List<ModelDirData> ModelDirs
        {
            get
            {
                return modelDirs;
            }

            set
            {
                modelDirs = value;
            }
        }

    }
}
