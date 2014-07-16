using BaseUMLModel.umlelements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TranModelEng.Properties;
using Util.util;

namespace TranModelEng.importJob
{
    class ImportFileParse
    {
        private static String rootNode = "objects";
        private static String classNode = "class";
        private static String attrNode = "attr";
        private static String optNode = "opt";

        private static String class_name = "name";
        private static String class_alias = "alias";
        private static String class_notes = "notes";
        private static String class_persistence = "persistence";
        private static String class_scope = "scope";
        private static String class_stereotype = "stereotype";

        private static String attr_name = "name";
        private static String attr_alias = "alias";
        private static String attr_notes = "notes";
        private static String attr_initial = "initial";
        private static String attr_scope = "scope";
        private static String attr_stereotype = "stereotype";
        private static String attr_type = "type";

        private static String opt_name = "name";
        private static String opt_alias = "alias";
        private static String opt_notes = "notes";
        private static String opt_return_type = "return_type";
        private static String opt_scope = "scope";
        private static String opt_stereotype = "stereotype";

        private static XmlDocument doc;
        private static XmlReader reader;

        public static List<ClassEl> parseClassEl(String file_path)
        {
            List<ClassEl> classes = new List<ClassEl>();
            openConfigDoc(file_path);
            try
            {
                //得到class节点
                StringBuilder select_path = new StringBuilder();
                select_path.Append("/").Append(rootNode).Append("/").Append(classNode);

                XmlNodeList xnl = doc.SelectNodes(select_path.ToString());

                if (xnl != null && xnl.Count > 0)
                {
                    foreach (XmlElement xe in xnl)
                    {
                        ClassEl c = new ClassEl();
                        c.Name = (xe.GetAttribute(class_name) != null) ? xe.GetAttribute(class_name) : "";
                        c.Alias = (xe.GetAttribute(class_alias) != null) ? xe.GetAttribute(class_alias) : "";
                        c.Notes = (xe.GetAttribute(class_notes) != null) ? xe.GetAttribute(class_notes) : "";
                        c.Persistence = (xe.GetAttribute(class_persistence) != null) ? xe.GetAttribute(class_persistence) : "";
                        c.Scope = (xe.GetAttribute(class_scope) != null) ? xe.GetAttribute(class_scope) : "";
                        c.Stereotype = (xe.GetAttribute(class_stereotype) != null) ? xe.GetAttribute(class_stereotype) : "";
                        c.Attributes = parseAttrs(xe);
                        c.Options = parseOpts(xe);
                        classes.Add(c);
                    }
                }
            }
            catch (Exception e)
            {
                throw new IMDAException(IMDAResources.parse_job_error, e);
            }
            closeConfigDoc();
            return classes;
        }

        private static List<AttributeEl> parseAttrs(XmlElement classNode)
        {
            List<AttributeEl> attrs = new List<AttributeEl>();
            try
            {
                XmlNodeList xnl = classNode.GetElementsByTagName(attrNode);
                if (xnl != null && xnl.Count > 0)
                {
                    foreach (XmlNode node in xnl)
                    {
                        AttributeEl attr = new AttributeEl();
                        XmlElement e = (XmlElement)node;
                        attr.Name = (e.GetAttribute(attr_name) != null) ? e.GetAttribute(attr_name) : "";
                        attr.Alias = (e.GetAttribute(attr_alias) != null) ? e.GetAttribute(attr_alias) : "";
                        attr.Notes = (e.GetAttribute(attr_notes) != null) ? e.GetAttribute(attr_notes) : "";
                        attr.Initial = (e.GetAttribute(attr_initial) != null) ? e.GetAttribute(attr_initial) : "";
                        attr.Scope = (e.GetAttribute(attr_scope) != null) ? e.GetAttribute(attr_scope) : "";
                        attr.Stereotype = (e.GetAttribute(attr_stereotype) != null) ? e.GetAttribute(attr_stereotype) : "";
                        attr.Type = (e.GetAttribute(attr_type) != null) ? e.GetAttribute(attr_type) : "";

                        attrs.Add(attr);
                    }
                }
            }
            catch (Exception e)
            {
                throw new IMDAException(IMDAResources.task_parse_failed, e);
            }
            return attrs;
        }

        private static List<OptionEl> parseOpts(XmlElement classNode)
        {
            List<OptionEl> opts = new List<OptionEl>();
            try
            {
                XmlNodeList xnl = classNode.GetElementsByTagName(optNode);
                if (xnl != null && xnl.Count > 0)
                {
                    foreach (XmlNode node in xnl)
                    {
                        OptionEl opt = new OptionEl();
                        XmlElement e = (XmlElement)node;
                        opt.Name = (e.GetAttribute(opt_name) != null) ? e.GetAttribute(opt_name) : "";
                        opt.Alias = (e.GetAttribute(opt_alias) != null) ? e.GetAttribute(opt_alias) : "";
                        opt.Notes = (e.GetAttribute(opt_notes) != null) ? e.GetAttribute(opt_notes) : "";
                        opt.Scope = (e.GetAttribute(opt_scope) != null) ? e.GetAttribute(opt_scope) : "";
                        opt.Stereotype = (e.GetAttribute(opt_stereotype) != null) ? e.GetAttribute(opt_stereotype) : "";
                        opt.Return_type = (e.GetAttribute(opt_return_type) != null) ? e.GetAttribute(opt_return_type) : "";

                        opts.Add(opt);
                    }
                }

            }
            catch (Exception e)
            {
                throw new IMDAException(IMDAResources.task_parse_failed, e);
            }
            return opts;
        }

        private static void openConfigDoc(String curr_path)
        {
            try
            {
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
