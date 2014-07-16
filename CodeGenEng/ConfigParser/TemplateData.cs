using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenEng.ConfigParser
{
    class TemplateData
    {
        public TemplateData()
        {

        }

        private string targetDir;
        public string TargetDir
        {
            get { return targetDir; }
            set { targetDir = value; }
        }

        private string template_name;
        public string Template_name
        {
            get { return template_name; }
            set { template_name = value; }
        }

        private Boolean classInOneFile;
        public Boolean ClassInOneFile
        {
            get { return classInOneFile; }
            set { classInOneFile = value; }
        }

        private string fileNamePrefix;
        public string FileNamePrefix
        {
            get { return fileNamePrefix; }
            set { fileNamePrefix = value; }
        }

        private string fileNameSuffix;
        public string FileNameSuffix
        {
            get { return fileNameSuffix; }
            set { fileNameSuffix = value; }
        }

        private string fileNameEx;
        public string FileNameEx
        {
            get { return fileNameEx; }
            set { fileNameEx = value; }
        }

        private Boolean fileNameLowerCase;
        public Boolean FileNameLowerCase
        {
            get { return fileNameLowerCase; }
            set { fileNameLowerCase = value; }
        }

        private string fileNameDelimiter;
        public string FileNameDelimiter
        {
            get { return fileNameDelimiter; }
            set { fileNameDelimiter = value; }
        }

        private string codeSortName;
        public string CodeSortName
        {
            get { return codeSortName; }
            set { codeSortName = value; }
        }

    }
}
