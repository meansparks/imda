using CodeGenEng.ConfigParser;
using CodeGenEng.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.util;

namespace CodeGenEng
{
    class ModelProcess
    {
        private StringBuilder contentBuffer = new StringBuilder();

        private EA.Repository m_Repository;

        private GenJobData genjob;
        private TemplateData gentemplate;

        private ConfigData config_data;

        public ModelProcess(EA.Repository m_Repository, GenJobData genjob, TemplateData gentemplate, ConfigData config_data)
        {
            this.m_Repository = m_Repository;

            this.genjob = genjob;
            this.gentemplate = gentemplate;
            this.config_data = config_data;
        }

        public bool toProcess()
        {
            try
            {
                EA.Package currPackage = m_Repository.GetTreeSelectedPackage();

                if (m_Repository.GetTreeSelectedElements().Count > 0)
                {
                    foreach (EA.Element theElement in m_Repository.GetTreeSelectedElements())
                    {
                        
                        processElement(currPackage, theElement);
                    }

                    /* classInOneFile:所有选中的类生成在一个文件里，如果选中的是包则按包划分文件，以包命名文件 */
                    if (gentemplate.ClassInOneFile && contentBuffer.Length > 0)
                    {
                        genDataCodes4AllInOneFile(gentemplate, currPackage, contentBuffer.ToString());
                        contentBuffer = new StringBuilder();
                    }
                }
                else
                {
                    processInPackage(currPackage);
                }

                return true;
            }
            catch (IMDAException)
            {
                throw;
            }
        }

        private void processInPackage(EA.Package aPackage)
        {

            foreach (EA.Element theElement in aPackage.Elements)
            {
                this.processElement(aPackage, theElement);
            }

            /* classInOneFile:所有选中的类生成在一个文件里，如果选中的是包则按包划分文件，以包命名文件 */
            if (gentemplate.ClassInOneFile && contentBuffer.Length > 0)
            {
                genDataCodes4AllInOneFile(gentemplate, aPackage, contentBuffer.ToString());
                contentBuffer = new StringBuilder();
            }

            /* 遍历子包 */
            if (genjob.IsSubPackage && aPackage.Packages.Count > 0)
            {
                foreach (EA.Package thePackage in aPackage.Packages)
                {
                    this.processInPackage(thePackage);
                }
            }
        }

        private void processElement(EA.Package currPackage, EA.Element theElement)
        {
            try {
                if ("Class" == theElement.Type)
                {
                    String file_content = getContentInElement(currPackage, theElement);

                    /* classInOneFile:所有选中的类生成在一个文件里，如果选中的是包则按包划分文件，以包命名文件 */
                    if (!gentemplate.ClassInOneFile)
                    {
                        genDataCodes4SingleFile(gentemplate, currPackage, theElement, file_content);
                    }
                    else {
                        contentBuffer.AppendLine(file_content).AppendLine("");
                    }
                }
            }
            catch (IMDAException)
            {
                throw;
            }
        }

        private String getContentInElement(EA.Package currPackage, EA.Element theElement)
        {
            String model_name = getModelName(currPackage);
            VelocityGenCode vgc = new VelocityGenCode(theElement, genjob, gentemplate, model_name, config_data);
            return vgc.getContent();
        }

        private String getModelName(EA.Package thePackage)
        {
            String model_name = thePackage.Name;
            if (GenJobData.MODELTYPE_ALIAS.ToLower() == genjob.ModelType.ToLower())
            {
                model_name = thePackage.Alias;
            }
            else if (GenJobData.MODELTYPE_STEREOTYPE.ToLower() == genjob.ModelType.ToLower())
            {
                model_name = thePackage.StereotypeEx;
            }
            return model_name;
        }

        private void genDataCodes4SingleFile(TemplateData theTemplate, EA.Package thePackage, EA.Element theClass, String file_content)
        {
            String model_name = getModelName(thePackage);

            StringBuilder dir_name = new StringBuilder();
            dir_name.Append(genjob.BaseNamespace).Append(".").Append(model_name.ToLower()).Append(".").Append(theTemplate.CodeSortName.ToLower());

            StringBuilder file_path = new StringBuilder();
            file_path.Append(theTemplate.TargetDir).Append("/").Append(dir_name);

            String file_name = theTemplate.FileNamePrefix + theClass.Name + theTemplate.FileNameSuffix;

            if (theTemplate.FileNameDelimiter != null && theTemplate.FileNameDelimiter != "")
            {
                StringBuilder file_name_buffer = new StringBuilder();
                String[] file_name_arry = file_name.Split();
                for (int i = 0; i < file_name_arry.Length; i++)
                {
                    if (Char.IsUpper(file_name_arry[i], 0))
                    {
                        if (i > 0)
                        {
                            file_name_buffer.Append(theTemplate.FileNameDelimiter);
                        }
                        file_name_buffer.Append(file_name_arry[i].ToLower());
                    }
                    else {
                        file_name_buffer.Append(file_name_arry[i]);
                    }
                }
                file_name = file_name_buffer.ToString();
            }

            if (theTemplate.FileNameLowerCase)
            {
                file_name = file_name.ToLower();
            }

            Tools.writerFile(file_path.ToString(), file_name, theTemplate.FileNameEx, file_content);
        }

        private void genDataCodes4AllInOneFile(TemplateData theTemplate, EA.Package thePackage, String file_content)
        {
            String model_name = thePackage.Name;
            if (GenJobData.MODELTYPE_ALIAS.ToLower() == genjob.ModelType.ToLower())
            {
                model_name = thePackage.Alias;
            }
            else if (GenJobData.MODELTYPE_STEREOTYPE.ToLower() == genjob.ModelType.ToLower())
            {
                model_name = thePackage.StereotypeEx;
            }

            StringBuilder file_path = new StringBuilder();
            file_path.Append(theTemplate.TargetDir);

            String file_name = theTemplate.FileNamePrefix + model_name + theTemplate.FileNameSuffix;

            if (theTemplate.FileNameDelimiter != null && theTemplate.FileNameDelimiter != "")
            {
                StringBuilder file_name_buffer = new StringBuilder();
                String[] file_name_arry = file_name.Split();
                for (int i = 0; i < file_name_arry.Length; i++)
                {
                    if (Char.IsUpper(file_name_arry[i], 0))
                    {
                        if (i > 0)
                        {
                            file_name_buffer.Append(theTemplate.FileNameDelimiter);
                        }
                        file_name_buffer.Append(file_name_arry[i].ToLower());
                    }
                    else
                    {
                        file_name_buffer.Append(file_name_arry[i]);
                    }
                }
                file_name = file_name_buffer.ToString();
            }

            if (theTemplate.FileNameLowerCase)
            {
                file_name = file_name.ToLower();
            }

            Tools.writerFile(file_path.ToString(), file_name, theTemplate.FileNameEx, file_content);
        }
    }
}
 