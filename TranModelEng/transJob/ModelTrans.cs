using BaseUMLModel;
using BaseUMLModel.umlelements;
using EA;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TranModelEng.ConfigParser;
using TranModelEng.Properties;
using Util.util;

namespace TranModelEng.transJob
{
    public abstract class ModelTrans : IModelTrans
    {
        protected EA.Repository m_Repository;

        protected TransJobData trans_job;
        protected TaskData main_task;
        protected List<TaskData> sub_tasks;
        protected EA.Element sourceEl;
        protected EA.Package targetPackage;

        public ModelTrans(EA.Repository m_Repository, TransJobData trans_job, TaskData main_task, List<TaskData> sub_tasks, EA.Element sourceEl, EA.Package targetPackage)
        {
            this.m_Repository = m_Repository;
            this.trans_job = trans_job;
            this.main_task = main_task;
            this.sub_tasks = sub_tasks;
            this.sourceEl = sourceEl;
            this.targetPackage = targetPackage;
        }

        public virtual void toTrans() { 
        
        }

        protected void transEl(TaskData curr_task, IUMLElement source_el, IUMLElement target_el)
        {
            Type st = source_el.GetType();
            Type tt = target_el.GetType();

            if (curr_task.Attrs != null && curr_task.Attrs.Count > 0)
            {
                foreach (String key in curr_task.Attrs.Keys)
                {
                    try
                    {
                        tt.GetProperty(Tools.realFristU(curr_task.Attrs[key].ToString())).SetValue(target_el, st.GetProperty(Tools.realFristU(key)).GetValue(source_el).ToString());
                    }
                    catch (Exception e)
                    {
                        Tools.writerOutput(m_Repository, key + ":" + curr_task.Attrs[key] + " " + e.Message);
                    }
                }
            }
            else
            {
                foreach (PropertyInfo t_property in tt.GetProperties())
                {
                    try
                    {
                        t_property.SetValue(target_el, st.GetProperty(t_property.Name).GetValue(source_el).ToString());
                    }
                    catch (Exception e)
                    {
                        Tools.writerOutput(m_Repository, t_property.Name + " " + e.Message);
                    }
                }
            }
        }

        public static Boolean filterAttrCheck(EA.Element curr_e, String filter_attr, String filter_value)
        {
            Boolean is_pass = true;
            if (filter_attr == null || "".Equals(filter_attr))
            {
                return is_pass;
            }
            if (TaskData.FILTERATTR_NAME.ToLower().Equals(filter_attr.ToLower()))
            {
                if ("".Equals(curr_e.Name))
                {
                    is_pass = false;
                }
                else if (!"".Equals(filter_value) && !filter_value.Equals(curr_e.Name))
                {
                    is_pass = false;
                }
            }
            else if(TaskData.FILTERATTR_STEREOTYPE.ToLower().Equals(filter_attr.ToLower())){
                if ("".Equals(curr_e.Stereotype))
                {
                    is_pass = false;
                }
                else if (!"".Equals(filter_value) && !filter_value.Equals(curr_e.Stereotype))
                {
                    is_pass = false;
                }
            }
            return is_pass;
        }
        public static Boolean filterAttrCheck(EA.Attribute curr_a, String filter_attr, String filter_value)
        {
            Boolean is_pass = true;
            if (filter_attr == null || "".Equals(filter_attr))
            {
                return is_pass;
            }
            if (TaskData.FILTERATTR_NAME.ToLower().Equals(filter_attr.ToLower()))
            {
                if ("".Equals(curr_a.Name))
                {
                    is_pass = false;
                }
                else if (!"".Equals(filter_value) && !filter_value.Equals(curr_a.Name))
                {
                    is_pass = false;
                }
            }
            else if (TaskData.FILTERATTR_STEREOTYPE.ToLower().Equals(filter_attr.ToLower()))
            {
                if ("".Equals(curr_a.Stereotype))
                {
                    is_pass = false;
                }
                else if (!"".Equals(filter_value) && !filter_value.Equals(curr_a.Stereotype))
                {
                    is_pass = false;
                }
            }
            return is_pass;
        }
        public static Boolean filterAttrCheck(EA.Method curr_m, String filter_attr, String filter_value)
        {
            Boolean is_pass = true;
            if (filter_attr == null || "".Equals(filter_attr))
            {
                return is_pass;
            }
            if (TaskData.FILTERATTR_NAME.ToLower().Equals(filter_attr.ToLower()))
            {
                if ("".Equals(curr_m.Name))
                {
                    is_pass = false;
                }
                else if (!"".Equals(filter_value) && !filter_value.Equals(curr_m.Name))
                {
                    is_pass = false;
                }
            }
            else if (TaskData.FILTERATTR_STEREOTYPE.ToLower().Equals(filter_attr.ToLower()))
            {
                if ("".Equals(curr_m.Stereotype))
                {
                    is_pass = false;
                }
                else if (!"".Equals(filter_value) && !filter_value.Equals(curr_m.Stereotype))
                {
                    is_pass = false;
                }
            }
            return is_pass;
        }

    }
}
