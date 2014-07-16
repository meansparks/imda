using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenEng.ConfigParser
{
    class GenJobData
    {
        public const String MODELTYPE_NAME = "name";
        public const String MODELTYPE_STEREOTYPE = "stereotype";
        public const String MODELTYPE_ALIAS = "alias";

        public GenJobData()
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

        private Boolean haveSType;
        public Boolean HaveSType
        {
            get { return haveSType; }
            set { haveSType = value; }
        }

        private String tRoot;
        public String TRoot
        {
            get { return tRoot; }
            set { tRoot = value; }
        }

        private String baseNamespace;
        public String BaseNamespace
        {
            get { return baseNamespace; }
            set { baseNamespace = value; }
        }

        private String modelType;
        public String ModelType
        {
            get { return modelType; }
            set { modelType = value; }
        }

        private List<TemplateData> templates;
        public List<TemplateData> Templates
        {
            get {
                return templates; 
            }

            set {
                templates = value; 
            }
        }

    }
}
