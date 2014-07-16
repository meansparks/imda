using CodeGenEng.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Util.util;

namespace CodeGenEng.ConfigParser
{
    class ConfigParse
    {
        private static String configFile = "config.xml";

        private static String rootNode = "configs";
        private static String configNode = "config";
        private static String genJobsNode = "genJobs";
        private static String genJobNode = "genjob";
        private static String templateNode = "template";

        private static String c_plugin_name = "plugin_name";
        private static String c_plugin_version = "plugin_version";
        private static String c_plugin_copyrigth = "plugin_copyrigth";

        private static String job_id = "id";
        private static String job_menuName = "menuName";
        private static String job_isSubPackage = "isSubPackage";
        private static String job_haveSType = "haveSType";
        private static String job_tRoot = "tRoot";
        private static String job_baseNamespace = "baseNamespace";
        private static String job_modelType = "modelType";

        private static String t_targetDir = "targetDir";
        private static String t_classInOneFile = "classInOneFile";
        private static String t_fileNamePrefix = "fileNamePrefix";
        private static String t_fileNameSuffix = "fileNameSuffix";
        private static String t_fileNameEx = "fileNameEx";
        private static String t_fileNameLowerCase = "fileNameLowerCase";
        private static String t_fileNameDelimiter = "fileNameDelimiter";
        private static String t_codeSortName = "codeSortName";

        private static XmlDocument doc;
        private static XmlReader reader;

        public static ConfigData parseConfig()
        {
            ConfigData c = new ConfigData();
            openConfigDoc();
            try
            {
                //得到config节点
                XmlNode xn = doc.SelectSingleNode(rootNode).SelectSingleNode(configNode);
                c.Plugin_name = ((XmlElement)xn).GetElementsByTagName(c_plugin_name).Item(0).InnerText;
                c.Plugin_version = ((XmlElement)xn).GetElementsByTagName(c_plugin_version).Item(0).InnerText;
                c.Plugin_copyrigth = ((XmlElement)xn).GetElementsByTagName(c_plugin_copyrigth).Item(0).InnerText;
            }
            catch (Exception e)
            {
                throw new IMDAException(IMDAResources.parse_config_error, e);
            }
            closeConfigDoc();
            return c;
        }

        public static GenJobData parseGenJob(String jobID)
        {
            GenJobData genJob = new GenJobData(); 
            openConfigDoc();
            try
            {
                //得到config节点
                StringBuilder select_path = new StringBuilder();
                select_path.Append("/").Append(rootNode).Append("/").Append(genJobsNode).Append("/").Append(genJobNode).Append("[@id='").Append(jobID).Append("']");

                XmlElement xn = (XmlElement)(doc.SelectNodes(select_path.ToString()).Item(0));

                genJob.Id = xn.GetAttribute(job_id);
                genJob.MenuName = xn.GetAttribute(job_menuName);

                if ("false".Equals(xn.GetAttribute(job_isSubPackage)))
                {
                    genJob.IsSubPackage = false;
                }
                else
                {
                    genJob.IsSubPackage = true;
                }

                if ("false".Equals(xn.GetAttribute(job_haveSType)))
                {
                    genJob.HaveSType = false;
                }
                else
                {
                    genJob.HaveSType = true;
                }

                genJob.TRoot = (xn.GetAttribute(job_tRoot) != null)?xn.GetAttribute(job_tRoot):"";
                genJob.BaseNamespace = (xn.GetAttribute(job_baseNamespace) != null)? xn.GetAttribute(job_baseNamespace):"";
                genJob.ModelType = (xn.GetAttribute(job_modelType) != null)?xn.GetAttribute(job_modelType):"";

                List<TemplateData> templates = new List<TemplateData>();
                XmlNodeList xnl = xn.GetElementsByTagName(templateNode);
                if (xnl != null && xnl.Count > 0)
                {
                    foreach (XmlNode node in xnl)
                    {
                        templates.Add(parseTemplate((XmlElement)node));
                    }
                }

                genJob.Templates = templates;
            }
            catch (Exception e)
            {
                throw new IMDAException(IMDAResources.parse_job_error, e);
            }
            closeConfigDoc();
            return genJob;
        }

        private static TemplateData parseTemplate(XmlElement xe)
        {
            TemplateData t = new TemplateData();
            try
            {
                t.TargetDir = xe.GetAttribute(t_targetDir);
                t.Template_name = xe.InnerText;
                if ("true".Equals(xe.GetAttribute(t_classInOneFile)))
                {
                    t.ClassInOneFile = true;
                }
                else
                {
                    t.ClassInOneFile = false;
                }
                t.FileNamePrefix = (xe.GetAttribute(t_fileNamePrefix) != null)?xe.GetAttribute(t_fileNamePrefix):"";
                t.FileNameSuffix = (xe.GetAttribute(t_fileNameSuffix) != null)?xe.GetAttribute(t_fileNameSuffix):"";
                t.FileNameEx = (xe.GetAttribute(t_fileNameEx) != null)?xe.GetAttribute(t_fileNameEx):"";
                if ("true".Equals(xe.GetAttribute(t_fileNameLowerCase)))
                {
                    t.FileNameLowerCase = true;
                }
                else
                {
                    t.FileNameLowerCase = false;
                }
                t.FileNameDelimiter = (xe.GetAttribute(t_fileNameDelimiter) != null)?xe.GetAttribute(t_fileNameDelimiter):"";
                t.CodeSortName = (xe.GetAttribute(t_codeSortName) != null)?xe.GetAttribute(t_codeSortName):"";

            }
            catch (Exception e)
            {
                throw new IMDAException(IMDAResources.parse_template_error, e);
            }
            return t;
        }

        public static Hashtable parseJobMenu()
        {
            Hashtable jobMenu = new Hashtable();
            openConfigDoc();
            try
            {
                //得到config节点
                StringBuilder select_path = new StringBuilder();
                select_path.Append("/").Append(rootNode).Append("/").Append(genJobsNode).Append("/").Append(genJobNode);

                XmlNodeList xnl = doc.SelectNodes(select_path.ToString());
                if (xnl != null && xnl.Count > 0)
                {
                    XmlElement xe;
                    foreach (XmlNode node in xnl)
                    {
                        xe = (XmlElement)node;
                        jobMenu.Add(xe.GetAttribute(job_id), xe.GetAttribute(job_menuName));
                    }
                }

            }
            catch (Exception e)
            {
                throw new IMDAException(IMDAResources.parse_job_menu_error, e);
            }
            closeConfigDoc();
            return jobMenu;
        }

        private static void openConfigDoc()
        {
            try
            {
                String curr_path = System.Reflection.Assembly.GetExecutingAssembly().CodeBase.ToString();
                curr_path = curr_path.Substring(0, curr_path.LastIndexOf("/") + 1) + configFile;

                doc = new XmlDocument();
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                reader = XmlReader.Create(@curr_path, settings);
                doc.Load(reader);
            }
            catch (Exception e) 
            {
                //throw new IMDAException(IMDAResources.load_error, e);
                throw new IMDAException(e.Message, e);
            }

        }

        private static void closeConfigDoc()
        {
            try
            {
                reader.Close();
                doc = null;
            }
            catch { }
        }
    }
}
