using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranModelEng.ConfigParser
{
    public class TaskData
    {
        public const String TYPE_PACKAGE = "Package";
        public const String TYPE_USECASE = "UseCase";
        public const String TYPE_ACTIVITY = "Activity";
        public const String TYPE_ACTION = "Action";
        public const String TYPE_OBJECT = "Object";
        public const String TYPE_CLASS = "Class";
        public const String TYPE_ATTRIBUTE = "Attribute";
        public const String TYPE_METHOD = "Method";

        public const String TYPE_SORT_PACKAGE = "package";
        public const String TYPE_SORT_ELEMENT = "element";

        public const String FILTERATTR_NAME = "name";
        public const String FILTERATTR_STEREOTYPE = "stereotype";

        public const String VAR_CURR_MAIN_OBJECT = "$CURR_MAIN_OBJECT";

        public TaskData()
        {

        }

        private string id;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        private string mainTask;
        public string MainTask
        {
            get { return mainTask; }
            set { mainTask = value; }
        }

        private string sourceType;
        public string SourceType
        {
            get { return sourceType; }
            set { sourceType = value; }
        }

        private string targetType;
        public string TargetType
        {
            get { return targetType; }
            set { targetType = value; }
        }

        private string targetPackagePath;
        public string TargetPackagePath
        {
            get { return targetPackagePath; }
            set { targetPackagePath = value; }
        }

        private string filterAttr;
        public string FilterAttr
        {
            get { return filterAttr; }
            set { filterAttr = value; }
        }

        private string filterValue;
        public string FilterValue
        {
            get { return filterValue; }
            set { filterValue = value; }
        }

        private Hashtable attrs;
        public Hashtable Attrs
        {
            get { return attrs; }
            set { attrs = value; }
        }

    }
}
