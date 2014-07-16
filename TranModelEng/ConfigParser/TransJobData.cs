using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranModelEng.ConfigParser
{
    public class TransJobData
    {
        public const String CREATMODE_NEW = "new";
        public const String CREATMODE_UPDATE = "update";
        public const String CREATMODE_FORCE = "force";

        public TransJobData()
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

        private Boolean isSubPackage;
        public Boolean IsSubPackage
        {
            get { return isSubPackage; }
            set { isSubPackage = value; }
        }

        private Boolean isStructure;
        public Boolean IsStructure
        {
            get { return isStructure; }
            set { isStructure = value; }
        }

        private String creatMode;
        public String CreatMode
        {
            get { return creatMode; }
            set { creatMode = value; }
        }

        private List<TaskData> tasks;
        public List<TaskData> Tasks
        {
            get
            {
                return tasks;
            }

            set
            {
                tasks = value;
            }
        }

    }
}
