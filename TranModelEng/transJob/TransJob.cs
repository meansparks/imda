using BaseUMLModel.umlelements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranModelEng.ConfigParser;
using TranModelEng.Properties;
using Util.util;

namespace TranModelEng.transJob
{
    class TransJob
    {
        private EA.Repository m_Repository;
        private TransJobData trans_job;
        private ConfigData config_data;

        private EA.Package targetPackage;

        public TransJob(EA.Repository m_Repository, TransJobData trans_job, ConfigData config_data)
        {
            this.m_Repository = m_Repository;
            this.trans_job = trans_job;
            this.config_data = config_data;
        }

        public bool runJob()
        {
            try
            {
                if (trans_job != null && trans_job.Tasks != null && trans_job.Tasks.Count() > 0)
                {
                    List<TaskData> main_tasks = this.getMainTaskList();
                    foreach (TaskData main_task in main_tasks)
                    {
                        List<TaskData> sub_tasks = this.getSubTaskList(main_task.Id);
                        this.taskExecution(main_task, sub_tasks);
                    }

                    Tools.writerOutput(m_Repository, trans_job.Id + IMDAResources.job_complete);
                    return true;
                }

                Tools.writerOutput(m_Repository, trans_job.Id + IMDAResources.job_failed);
                return false;
            }
            catch (IMDAException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void taskExecution(TaskData main_task, List<TaskData> sub_tasks)
        {
            try
            {
                Tools.writerOutput(m_Repository, main_task.Id + IMDAResources.task_begin);
                /* 解析目标路径 */
                this.targetPackage = parseTargetPackagePath(main_task.TargetPackagePath);

                /* 递归获得满足条件的源模型元素 */
                EA.Package currPackage = m_Repository.GetTreeSelectedPackage();

                if (m_Repository.GetTreeSelectedElements().Count > 0)
                {//选中了单个元素
                    foreach (EA.Element theElement in m_Repository.GetTreeSelectedElements())
                    {
                        this.taskProcess(main_task, sub_tasks, theElement);
                        //仅支持第二级
                        foreach (EA.Element subElement in theElement.Elements)
                        {
                            this.taskProcess(main_task, sub_tasks, subElement);
                            //主任务支持三级
                            foreach (EA.Element ssubElement in subElement.Elements)
                            {
                                this.taskProcess(main_task, sub_tasks, ssubElement);
                            }
                        }
                    }
                }
                else
                {//选中了一个包 
                    //包转换
                    this.taskProcess(main_task, sub_tasks, currPackage.Element);

                    /* 遍历子包 */
                    if (trans_job.IsSubPackage)
                    {
                        this.processInPackage(currPackage, main_task, sub_tasks);
                    }
                    else
                    {
                        foreach (EA.Element theElement in currPackage.Elements)
                        {
                            this.taskProcess(main_task, sub_tasks, theElement);
                            //仅支持第二级
                            foreach (EA.Element subElement in theElement.Elements)
                            {
                                this.taskProcess(main_task, sub_tasks, subElement);
                                //主任务支持三级
                                foreach (EA.Element ssubElement in subElement.Elements)
                                {
                                    this.taskProcess(main_task, sub_tasks, ssubElement);
                                }
                            }
                        }
                    }
                }

                Tools.writerOutput(m_Repository, main_task.Id + IMDAResources.task_complete);
            }
            catch (IMDAException)
            {
                Tools.writerOutput(m_Repository, main_task.Id + IMDAResources.task_failed);
            }
            catch (Exception)
            {
                Tools.writerOutput(m_Repository, main_task.Id + IMDAResources.task_failed);
            }
        }

        private void processInPackage(EA.Package aPackage, TaskData main_task, List<TaskData> sub_tasks)
        {
            /* 更新子包结构 */
            if (trans_job.IsStructure)
            {
                this.targetPackage = findPackageByName(this.targetPackage, aPackage.Name);
            }

            /* 获得包下所有元素 */
            foreach (EA.Element theElement in aPackage.Elements)
            {
                /* 执行主任务 */
                this.taskProcess(main_task, sub_tasks, theElement);
                //仅支持第二级
                foreach (EA.Element subElement in theElement.Elements)
                {
                    this.taskProcess(main_task, sub_tasks, subElement);
                    //主任务支持三级
                    foreach (EA.Element ssubElement in subElement.Elements)
                    {
                        this.taskProcess(main_task, sub_tasks, ssubElement);
                    }
                }
            }
            /* 获得包下所有包 */
            foreach (EA.Package thePackage in aPackage.Packages)
            {
                /* 执行主任务 */
                this.taskProcess(main_task, sub_tasks, thePackage.Element);
            }

            /* 遍历子包 */
            if (trans_job.IsSubPackage && aPackage.Packages.Count > 0)
            {
                foreach (EA.Package thePackage in aPackage.Packages)
                {
                    this.processInPackage(thePackage, main_task, sub_tasks);
                }
            }
        }

        private void taskProcess(TaskData main_task, List<TaskData> sub_tasks, EA.Element sourceEl)
        {
            try
            {
                /* 判断任务的源对象是否与当前选中对象一致 */
                if (main_task.SourceType.ToLower().Equals(sourceEl.Type.ToLower()))
                {
                    /* 判断过滤属性 */
                    Boolean is_pass = ModelTrans.filterAttrCheck(sourceEl, main_task.FilterAttr, main_task.FilterValue);
                    if (is_pass)
                    {
                        /* 执行主任务 */
                        if (ClassEl.EA_TYPE.ToLower().Equals(main_task.SourceType.ToLower()) || ClassEl.EA_TYPE.ToLower().Equals(main_task.TargetType.ToLower()))
                        {
                            IModelTrans mt = new ModelTrans4Class(m_Repository, trans_job, main_task, sub_tasks, sourceEl, targetPackage);
                            mt.toTrans();
                        }
                        else
                        {
                            IModelTrans mt = new ModelTrans4El(m_Repository, trans_job, main_task, sub_tasks, sourceEl, targetPackage);
                            mt.toTrans();
                        }
                    }
                }
            }
            catch (IMDAException)
            {
                Tools.writerOutput(m_Repository, main_task.Id + IMDAResources.task_failed);
                throw;
            }
            catch (Exception)
            {
                Tools.writerOutput(m_Repository, main_task.Id + IMDAResources.task_failed);
                throw;
            }
        }

        private List<TaskData> getMainTaskList()
        {
            List<TaskData> main_tasks = new List<TaskData>();
            if (trans_job != null && trans_job.Tasks != null && trans_job.Tasks.Count() > 0)
            {
                foreach (TaskData task in trans_job.Tasks)
                {
                    if (task.MainTask == null || "".Equals(task.MainTask))
                    {
                        main_tasks.Add(task);
                    }
                }
            }

            return main_tasks;
        }

        private List<TaskData> getSubTaskList(String task_id)
        {
            List<TaskData> sub_tasks = new List<TaskData>();
            if (trans_job != null && trans_job.Tasks != null && trans_job.Tasks.Count() > 0)
            {
                foreach (TaskData task in trans_job.Tasks)
                {
                    if (task_id.Equals(task.MainTask))
                    {
                        sub_tasks.Add(task);
                    }
                }
            }

            return sub_tasks;
        }

        private EA.Package parseTargetPackagePath(String targetPackagePath)
        {
            EA.Package target_path = null;
            try
            {
                if (!TaskData.VAR_CURR_MAIN_OBJECT.Equals(targetPackagePath))
                {
                    if (targetPackagePath != null && !"".Equals(targetPackagePath))
                    {
                        String[] mpoins = targetPackagePath.Split('/');
                        foreach (String mpoin in mpoins)
                        {
                            target_path = findPackageByName(target_path, mpoin);
                        }
                    }
                    if (target_path == null)
                    {
                        Tools.writerOutput(m_Repository, targetPackagePath + IMDAResources.task_not_find_target_path);
                    }
                }
            }
            catch (Exception e)
            {
                Tools.writerOutput(m_Repository, e.Message);
            }
            return target_path;
        }

        private EA.Package findPackageByName(EA.Package parent_package, String package_name)
        {
            EA.Package thePackage = null;
            if (parent_package == null)
            {
                foreach (EA.Package p in m_Repository.Models)
                {
                    if (package_name.Equals(p.Name))
                    {
                        return p;
                    }
                }
            }
            else
            {
                foreach (EA.Package p in parent_package.Packages)
                {
                    if (package_name.Equals(p.Name))
                    {
                        thePackage = p;
                        break;
                    }
                }
                if (thePackage == null)
                {
                    thePackage = parent_package.Packages.AddNew(package_name, PackageEl.EA_TYPE);
                    thePackage.Update();
                }
            }

            return thePackage;
        }


    }
}
