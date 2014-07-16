using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TranModelEng.Properties;
using Util.util;

namespace TranModelEng.ConfigParser
{
    class ConfigParse
    {
        private static String configFile = "config.xml";

        private static String rootNode = "configs";
        private static String configNode = "config";
        private static String transJobsNode = "transJobs";
        private static String transJobNode = "transJob";
        private static String taskNode = "task";
        private static String attrNode = "attr";
        private static String importJobsNode = "importJobs";
        private static String importJobNode = "importJob";
        private static String modelFileNode = "modelFile";
        private static String modelDirNode = "modelDir";

        private static String c_plugin_name = "plugin_name";
        private static String c_plugin_version = "plugin_version";
        private static String c_plugin_copyrigth = "plugin_copyrigth";

        private static String tjob_id = "id";
        private static String tjob_menuName = "menuName";
        private static String tjob_isSubPackage = "isSubPackage";
        private static String tjob_isStructure = "isStructure";
        private static String tjob_creatMode = "creatMode";

        private static String task_id = "id";
        private static String task_mainTask = "mainTask";
        private static String task_sourceType = "sourceType";
        private static String task_targetType = "targetType";
        private static String task_targetPackagePath = "targetPackagePath";
        private static String task_filterAttr = "filterAttr";
        private static String task_filterValue = "filterValue";

        private static String attr_name = "name";
        private static String attr_targetName = "targetName";

        private static String ijob_id = "id";
        private static String ijob_menuName = "menuName";
        private static String ijob_targetPackagePath = "targetPackagePath";
        private static String ijob_creatMode = "creatMode";

        private static String mdir_isSubPackage = "isSubPackage";
        private static String mdir_extName = "extName";

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

        public static TransJobData parseTransJob(String jobID)
        {
            TransJobData transJob = new TransJobData(); 
            openConfigDoc();
            try
            {
                //得到config节点
                StringBuilder select_path = new StringBuilder();
                select_path.Append("/").Append(rootNode).Append("/").Append(transJobsNode).Append("/").Append(transJobNode).Append("[@id='").Append(jobID).Append("']");

                XmlElement xn = (XmlElement)(doc.SelectNodes(select_path.ToString()).Item(0));

                transJob.Id = xn.GetAttribute(tjob_id);
                transJob.MenuName = xn.GetAttribute(tjob_menuName);

                if ("false".Equals(xn.GetAttribute(tjob_isSubPackage)))
                {
                    transJob.IsSubPackage = false;
                }
                else
                {
                    transJob.IsSubPackage = true;
                }

                if ("false".Equals(xn.GetAttribute(tjob_isStructure)))
                {
                    transJob.IsStructure = false;
                }
                else
                {
                    transJob.IsStructure = true;
                }

                transJob.CreatMode = (xn.GetAttribute(tjob_creatMode) != null) ? xn.GetAttribute(tjob_creatMode) : "";

                List<TaskData> tasks = new List<TaskData>();
                XmlNodeList xnl = xn.GetElementsByTagName(taskNode);
                if (xnl != null && xnl.Count > 0)
                {
                    foreach (XmlNode node in xnl)
                    {
                        tasks.Add(parseTask((XmlElement)node));
                    }
                }

                transJob.Tasks = tasks;
            }
            catch (Exception e)
            {
                throw new IMDAException(IMDAResources.parse_job_error, e);
            }
            closeConfigDoc();
            return transJob;
        }

        private static TaskData parseTask(XmlElement xe)
        {
            TaskData t = new TaskData();
            try
            {
                t.Id = (xe.GetAttribute(task_id) != null) ? xe.GetAttribute(task_id) : "";
                t.MainTask = (xe.GetAttribute(task_mainTask) != null) ? xe.GetAttribute(task_mainTask) : "";
                t.SourceType = (xe.GetAttribute(task_sourceType) != null) ? xe.GetAttribute(task_sourceType) : "";
                t.TargetType = (xe.GetAttribute(task_targetType) != null) ? xe.GetAttribute(task_targetType) : "";
                t.TargetPackagePath = (xe.GetAttribute(task_targetPackagePath) != null) ? xe.GetAttribute(task_targetPackagePath) : "";
                t.FilterAttr = (xe.GetAttribute(task_filterAttr) != null) ? xe.GetAttribute(task_filterAttr) : "";
                t.FilterValue = (xe.GetAttribute(task_filterValue) != null) ? xe.GetAttribute(task_filterValue) : "";

                Hashtable attrs = new Hashtable();
                XmlNodeList xnl = xe.GetElementsByTagName(attrNode);
                if (xnl != null && xnl.Count > 0)
                {
                    foreach (XmlNode node in xnl)
                    {
                        XmlElement e = (XmlElement)node;
                        String key = (e.GetAttribute(attr_name) != null) ? e.GetAttribute(attr_name) : "";
                        String value = (e.GetAttribute(attr_targetName) != null) ? e.GetAttribute(attr_targetName) : "";
                        if (key != null && !"".Equals(key) && value != null && !"".Equals(value))
                        {
                            attrs.Add(key, value);
                        }
                        
                    }
                }

                t.Attrs = attrs;

            }
            catch (Exception e)
            {
                throw new IMDAException(IMDAResources.task_parse_failed, e);
            }
            return t;
        }


        public static Hashtable parseTransJobMenu()
        {
            Hashtable jobMenu = new Hashtable();
            openConfigDoc();
            try
            {
                //得到config节点
                StringBuilder select_path = new StringBuilder();
                select_path.Append("/").Append(rootNode).Append("/").Append(transJobsNode).Append("/").Append(transJobNode);

                XmlNodeList xnl = doc.SelectNodes(select_path.ToString());
                if (xnl != null && xnl.Count > 0)
                {
                    XmlElement xe;
                    foreach (XmlNode node in xnl)
                    {
                        xe = (XmlElement)node;
                        jobMenu.Add(xe.GetAttribute(tjob_id), xe.GetAttribute(tjob_menuName));
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

        public static ImportJobData parseImportJob(String jobID)
        {
            ImportJobData importJob = new ImportJobData();
            openConfigDoc();
            try
            {
                //得到config节点
                StringBuilder select_path = new StringBuilder();
                select_path.Append("/").Append(rootNode).Append("/").Append(importJobsNode).Append("/").Append(importJobNode).Append("[@id='").Append(jobID).Append("']");

                XmlElement xn = (XmlElement)(doc.SelectNodes(select_path.ToString()).Item(0));

                importJob.Id = xn.GetAttribute(ijob_id);
                importJob.MenuName = xn.GetAttribute(ijob_menuName);
                importJob.TargetPackagePath = (xn.GetAttribute(ijob_targetPackagePath) != null) ? xn.GetAttribute(ijob_targetPackagePath) : "";
                importJob.CreatMode = (xn.GetAttribute(ijob_creatMode) != null) ? xn.GetAttribute(ijob_creatMode) : "";

                List<String> modelFiles = new List<String>();
                XmlNodeList fxnl = xn.GetElementsByTagName(modelFileNode);
                if (fxnl != null && fxnl.Count > 0)
                {
                    foreach (XmlNode node in fxnl)
                    {
                        XmlElement e = (XmlElement)node;
                        String fm_path = e.InnerText;
                        if (fm_path != null && !"".Equals(fm_path))
                        {
                            modelFiles.Add(fm_path);
                        }
                    }
                }
                importJob.ModelFiles = modelFiles;

                List<ModelDirData> modelDirs = new List<ModelDirData>();
                XmlNodeList dxnl = xn.GetElementsByTagName(modelDirNode);
                if (dxnl != null && dxnl.Count > 0)
                {
                    foreach (XmlNode node in dxnl)
                    {
                        XmlElement e = (XmlElement)node;
                        ModelDirData md = new ModelDirData();
                        md.DirPath = e.InnerText;
                        md.ExtName = (e.GetAttribute(mdir_extName) != null) ? e.GetAttribute(mdir_extName) : "";
                        if ("".Equals(md.ExtName))
                        {
                            md.ExtName = ModelDirData.DEF_EXTNAME;
                        }

                        if ("true".Equals(e.GetAttribute(mdir_isSubPackage)))
                        {
                            md.IsSubPackage = true;
                        }
                        else
                        {
                            md.IsSubPackage = false;
                        }
                        modelDirs.Add(md);
                    }
                }
                importJob.ModelDirs = modelDirs;
            }
            catch (Exception e)
            {
                throw new IMDAException(IMDAResources.parse_job_error, e);
            }
            closeConfigDoc();
            return importJob;
        }

        public static Hashtable parseImportJobMenu()
        {
            Hashtable jobMenu = new Hashtable();
            openConfigDoc();
            try
            {
                //得到config节点
                StringBuilder select_path = new StringBuilder();
                select_path.Append("/").Append(rootNode).Append("/").Append(importJobsNode).Append("/").Append(importJobNode);

                XmlNodeList xnl = doc.SelectNodes(select_path.ToString());
                if (xnl != null && xnl.Count > 0)
                {
                    XmlElement xe;
                    foreach (XmlNode node in xnl)
                    {
                        xe = (XmlElement)node;
                        jobMenu.Add(xe.GetAttribute(ijob_id), xe.GetAttribute(ijob_menuName));
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
