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
    class ModelTrans4Class : ModelTrans
    {
        public ModelTrans4Class(EA.Repository m_Repository, TransJobData trans_job, TaskData main_task, List<TaskData> sub_tasks, EA.Element sourceEl, EA.Package targetPackage)
            : base(m_Repository, trans_job, main_task, sub_tasks, sourceEl, targetPackage)
        {

        }

        public override void toTrans()
        {
            try
            {
                /* 转换主任务对象 */
                IUMLElement target_el = mainTaskTrans();
                /* 转换子任务对象 */
                EA.Element targetEl = findTargetEl(target_el.getElName(), main_task.TargetType);
                subTaskTrans(targetEl);
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

        private IUMLElement mainTaskTrans()
        {
            /* 解析源对象 */
            ElFactory elFactory = new ElFactory();
            IUMLElement source_el = elFactory.createElement(m_Repository, main_task.SourceType, sourceEl);

            /* 创建目标对象 */
            IUMLElement target_el = elFactory.createElement(m_Repository, main_task.TargetType);

            /* 对象转换 */
            this.transEl(main_task, source_el, target_el);

            /* 生成目标模型 */
            this.genTargetModel(target_el);

            return target_el;
        }

        private void subTaskTrans(EA.Element mainTargetEl)
        {
            if (sub_tasks != null && sub_tasks.Count > 0)
            {
                foreach (TaskData sub_task in sub_tasks)
                {
                    if (PackageEl.EA_TYPE.ToLower().Equals(sub_task.SourceType.ToLower()))
                    {
                        EA.Package cp = m_Repository.GetPackageByGuid(sourceEl.ElementGUID);
                        foreach (EA.Package thePackage in cp.Packages)
                        {
                            this.subTaskProcess(sub_task, thePackage.Element, mainTargetEl);
                        }
                    }
                    else if (AttributeEl.EA_TYPE.ToLower().Equals(sub_task.SourceType.ToLower()))
                    {
                        foreach (EA.Attribute theAttribute in sourceEl.Attributes)
                        {
                            this.subTaskProcess(sub_task, theAttribute, mainTargetEl);
                        }
                    }
                    else if (OptionEl.EA_TYPE.ToLower().Equals(sub_task.SourceType.ToLower()))
                    {
                        foreach (EA.Method theMethod in sourceEl.Methods)
                        {
                            this.subTaskProcess(sub_task, theMethod, mainTargetEl);
                        }

                    }
                    else
                    {
                        foreach (EA.Element theElement in sourceEl.Elements)
                        {
                            this.subTaskProcess(sub_task, theElement, mainTargetEl);
                            //仅支持第二级
                            foreach (EA.Element subElement in theElement.Elements)
                            {
                                this.subTaskProcess(sub_task, subElement, mainTargetEl);
                            }
                        }
                    }
                }
            }
        }

        /* 主任务方法 */
        private void genTargetModel(IUMLElement target_el)
        {
            EA.Element targetEl = findTargetEl(target_el.getElName(), main_task.TargetType);
            if (TransJobData.CREATMODE_NEW.ToLower().Equals(trans_job.CreatMode))
            {
                if (targetEl == null)
                {
                    this.createTargetEl2Model(target_el, main_task.TargetType);
                }
            }
            else if (TransJobData.CREATMODE_UPDATE.ToLower().Equals(trans_job.CreatMode))
            {
                if (targetEl == null)
                {
                    this.createTargetEl2Model(target_el, main_task.TargetType);
                }
                else
                {
                    this.updateTargetEl2Model(target_el, targetEl);
                }
            }
            else
            {
                if (targetEl != null)
                {
                    //删除当前元素
                    this.deleteTargetEl2Model(targetEl);
                    //创建新的元素
                    this.createTargetEl2Model(target_el, main_task.TargetType);
                }
                else
                {
                    this.createTargetEl2Model(target_el, main_task.TargetType);
                }
            }
        }

        private void createTargetEl2Model(IUMLElement target_el, String target_element_type)
        {
            if (PackageEl.EA_TYPE.ToLower().Equals(target_element_type.ToLower()))
            {
                EA.Package targetEl = (EA.Package)this.targetPackage.Packages.AddNew(target_el.getElName(), target_element_type);
                targetEl.Update();
                target_el.setEl(targetEl.Element);
                targetEl.Update();
            }
            else
            {
                EA.Element targetEl = this.targetPackage.Elements.AddNew(target_el.getElName(), target_element_type);
                targetEl.Update();
                target_el.setEl(targetEl);
                targetEl.Update();
                targetEl.Refresh();
            }
        }

        private void updateTargetEl2Model(IUMLElement target_el, EA.Element targetEl)
        {
            target_el.setEl(targetEl);
            targetEl.Update();
        }

        private void deleteTargetEl2Model(EA.Element targetEl)
        {
            if (PackageEl.EA_TYPE.ToLower().Equals(targetEl.Type.ToLower()))
            {
                this.targetPackage.Packages.Refresh();
                short p_index = 0;
                foreach (EA.Package p in this.targetPackage.Packages)
                {
                    if (targetEl.Name.Equals(p.Name))
                    {
                        break;
                    }
                    p_index++;
                }
                this.targetPackage.Packages.Delete(p_index);
                this.targetPackage.Packages.Refresh();
            }
            else
            {
				this.targetPackage.Elements.Refresh();
                short p_index = 0;
                foreach (EA.Element e in this.targetPackage.Elements)
                {
                    if (targetEl.Name.Equals(e.Name))
                    {
                        break;
                    }
                    p_index++;
                }
                this.targetPackage.Elements.Delete(p_index);
                this.targetPackage.Elements.Refresh();
            }
        }

        private EA.Element findTargetEl(String target_element_name, String target_element_type)
        {
            if (this.targetPackage != null)
            {
                if (PackageEl.EA_TYPE.ToLower().Equals(target_element_type.ToLower()))
                {
					this.targetPackage.Packages.Refresh();
                    foreach (EA.Package p in this.targetPackage.Packages)
                    {
                        if (target_element_name.Equals(p.Name))
                        {
                            return p.Element;
                        }
                    }
                }
                else
                {
                    this.targetPackage.Elements.Refresh();
                    foreach (EA.Element e in this.targetPackage.Elements)
                    {
                        if (target_element_name.Equals(e.Name))
                        {
                            return e;
                        }
                    }
                }
            }

            return null;
        }

        /* 子任务方法 */
        private void subTaskProcess(TaskData sub_task, EA.Element source_element, EA.Element mainTargetEl)
        {
            /* 判断任务的源对象是否与当前选中对象一致 */
            if (sub_task.SourceType.ToLower().Equals(source_element.Type.ToLower()))
            {
                /* 判断过滤属性 */
                Boolean is_pass = ModelTrans.filterAttrCheck(source_element, sub_task.FilterAttr, sub_task.FilterValue);
                if (is_pass)
                {
                    /* 解析源对象 */
                    ElFactory elFactory = new ElFactory();
                    IUMLElement source_el = elFactory.createElement(m_Repository, sub_task.SourceType, source_element);

                    /* 创建目标对象 */
                    IUMLElement target_el = elFactory.createElement(m_Repository, sub_task.TargetType);

                    /* 对象转换 */
                    this.transEl(sub_task, source_el, target_el);

                    /* 生成目标模型 */
                    if (AttributeEl.EA_TYPE.ToLower().Equals(sub_task.TargetType.ToLower()))
                    {
                        this.genSubTargetModel4Attr(sub_task, target_el, mainTargetEl);
                    }
                    else if (OptionEl.EA_TYPE.ToLower().Equals(sub_task.TargetType.ToLower()))
                    {
                        this.genSubTargetModel4Method(sub_task, target_el, mainTargetEl);
                    }
                    else
                    {
                        this.genSubTargetModel4El(sub_task, target_el, mainTargetEl);
                    }
                }
            }
        }

        private void subTaskProcess(TaskData sub_task, EA.Attribute source_attribute, EA.Element mainTargetEl)
        {
            /* 判断过滤属性 */
            Boolean is_pass = ModelTrans.filterAttrCheck(source_attribute, sub_task.FilterAttr, sub_task.FilterValue);
            if (is_pass)
            {
                /* 解析源对象 */
                ElFactory elFactory = new ElFactory();
                IUMLElement source_el = elFactory.createElement(m_Repository, sub_task.SourceType, source_attribute);

                /* 创建目标对象 */
                IUMLElement target_el = elFactory.createElement(m_Repository, sub_task.TargetType);

                /* 对象转换 */
                this.transEl(sub_task, source_el, target_el);

                /* 生成目标模型 */
                if (AttributeEl.EA_TYPE.ToLower().Equals(sub_task.TargetType.ToLower()))
                {
                    this.genSubTargetModel4Attr(sub_task, target_el, mainTargetEl);
                }
                else if (OptionEl.EA_TYPE.ToLower().Equals(sub_task.TargetType.ToLower()))
                {
                    this.genSubTargetModel4Method(sub_task, target_el, mainTargetEl);
                }
                else
                {
                    this.genSubTargetModel4El(sub_task, target_el, mainTargetEl);
                }
            }
        }

        private void subTaskProcess(TaskData sub_task, EA.Method source_method, EA.Element mainTargetEl)
        {
            /* 判断过滤属性 */
            Boolean is_pass = ModelTrans.filterAttrCheck(source_method, sub_task.FilterAttr, sub_task.FilterValue);
            if (is_pass)
            {
                /* 解析源对象 */
                ElFactory elFactory = new ElFactory();
                IUMLElement source_el = elFactory.createElement(m_Repository, sub_task.SourceType, source_method);

                /* 创建目标对象 */
                IUMLElement target_el = elFactory.createElement(m_Repository, sub_task.TargetType);

                /* 对象转换 */
                this.transEl(sub_task, source_el, target_el);

                /* 生成目标模型 */
                if (AttributeEl.EA_TYPE.ToLower().Equals(sub_task.TargetType.ToLower()))
                {
                    this.genSubTargetModel4Attr(sub_task, target_el, mainTargetEl);
                }
                else if (OptionEl.EA_TYPE.ToLower().Equals(sub_task.TargetType.ToLower()))
                {
                    this.genSubTargetModel4Method(sub_task, target_el, mainTargetEl);
                }
                else
                {
                    this.genSubTargetModel4El(sub_task, target_el, mainTargetEl);
                }
            }
        }

        private void genSubTargetModel4El(TaskData sub_task, IUMLElement target_el, EA.Element mainTargetEl)
        {
            EA.Element targetEl = (EA.Element)findTargetEl4SubTask(target_el.getElName(), sub_task.TargetType, mainTargetEl);
            if (TransJobData.CREATMODE_NEW.ToLower().Equals(trans_job.CreatMode))
            {
                if (targetEl == null)
                {
                    this.createTargetEl2Model4SubTask(target_el, sub_task.TargetType, mainTargetEl);
                }
            }
            else if (TransJobData.CREATMODE_UPDATE.ToLower().Equals(trans_job.CreatMode))
            {
                if (targetEl == null)
                {
                    this.createTargetEl2Model4SubTask(target_el, sub_task.TargetType, mainTargetEl);
                }
                else
                {
                    this.updateTargetEl2Model4SubTask(target_el, targetEl, mainTargetEl);
                }
            }
            else
            {
                if (targetEl != null)
                {
                    //删除当前元素
                    this.deleteTargetEl2Model4SubTask(targetEl, mainTargetEl);
                    //创建新的元素
                    this.createTargetEl2Model4SubTask(target_el, sub_task.TargetType, mainTargetEl);
                }
                else
                {
                    this.createTargetEl2Model4SubTask(target_el, sub_task.TargetType, mainTargetEl);
                }
            }
        }

        private void genSubTargetModel4Attr(TaskData sub_task, IUMLElement target_el, EA.Element mainTargetEl)
        {
            EA.Attribute targetAttribute = (EA.Attribute)findTargetEl4SubTask(target_el.getElName(), sub_task.TargetType, mainTargetEl);
            if (TransJobData.CREATMODE_NEW.ToLower().Equals(trans_job.CreatMode))
            {
                if (targetAttribute == null)
                {
                    this.createTargetAttr2Model4SubTask(target_el, sub_task.TargetType, mainTargetEl);
                }
            }
            else if (TransJobData.CREATMODE_UPDATE.ToLower().Equals(trans_job.CreatMode))
            {
                if (targetAttribute == null)
                {
                    this.createTargetAttr2Model4SubTask(target_el, sub_task.TargetType, mainTargetEl);
                }
                else
                {
                    this.updateTargetEl2Model4SubTask(target_el, targetAttribute, mainTargetEl);
                }
            }
            else
            {
                if (targetAttribute != null)
                {
                    //删除当前元素
                    this.deleteTargetAttr2Model4SubTask(targetAttribute, mainTargetEl);
                    //创建新的元素
                    this.createTargetAttr2Model4SubTask(target_el, sub_task.TargetType, mainTargetEl);
                }
                else
                {
                    this.createTargetAttr2Model4SubTask(target_el, sub_task.TargetType, mainTargetEl);
                }
            }
        }

        private void genSubTargetModel4Method(TaskData sub_task, IUMLElement target_el, EA.Element mainTargetEl)
        {
            EA.Method targetMethod = (EA.Method)findTargetEl4SubTask(target_el.getElName(), sub_task.TargetType, mainTargetEl);
            if (TransJobData.CREATMODE_NEW.ToLower().Equals(trans_job.CreatMode))
            {
                if (targetMethod == null)
                {
                    this.createTargetMethod2Model4SubTask(target_el, sub_task.TargetType, mainTargetEl);
                }
            }
            else if (TransJobData.CREATMODE_UPDATE.ToLower().Equals(trans_job.CreatMode))
            {
                if (targetMethod == null)
                {
                    this.createTargetMethod2Model4SubTask(target_el, sub_task.TargetType, mainTargetEl);
                }
                else
                {
                    this.updateTargetEl2Model4SubTask(target_el, targetMethod, mainTargetEl);
                }
            }
            else
            {
                if (targetMethod != null)
                {
                    //删除当前元素
                    this.deleteTargetMethod2Model4SubTask(targetMethod, mainTargetEl);
                    //创建新的元素
                    this.createTargetMethod2Model4SubTask(target_el, sub_task.TargetType, mainTargetEl);
                }
                else
                {
                    this.createTargetMethod2Model4SubTask(target_el, sub_task.TargetType, mainTargetEl);
                }
            }
        }

        private void createTargetEl2Model4SubTask(IUMLElement target_el, String target_element_type, EA.Element mainTargetEl)
        {
            if (PackageEl.EA_TYPE.ToLower().Equals(target_element_type.ToLower()))
            {
                if (PackageEl.EA_TYPE.ToLower().Equals(mainTargetEl.Type.ToLower()))
                {
                    EA.Package cp = m_Repository.GetPackageByGuid(mainTargetEl.ElementGUID);
                    EA.Package targetEl = (EA.Package)cp.Packages.AddNew(target_el.getElName(), target_element_type);
                    targetEl.Update();
                    target_el.setEl(targetEl.Element);
                    targetEl.Update();
                }
                else
                {
                    EA.Package cp = m_Repository.GetPackageByID(mainTargetEl.PackageID);
                    EA.Package targetEl = (EA.Package)cp.Packages.AddNew(target_el.getElName(), target_element_type);
                    targetEl.Update();
                    target_el.setEl(targetEl.Element);
                    targetEl.Update();
                }
            }
            else
            {
                if (PackageEl.EA_TYPE.ToLower().Equals(mainTargetEl.Type.ToLower()))
                {
                    EA.Package cp = m_Repository.GetPackageByGuid(mainTargetEl.ElementGUID);
                    EA.Element targetEl = cp.Elements.AddNew(target_el.getElName(), target_element_type);
                    targetEl.Update();
                    target_el.setEl(targetEl);
                    targetEl.Update();
                    targetEl.Refresh();
                }
                else
                {
                    EA.Element targetEl = mainTargetEl.Elements.AddNew(target_el.getElName(), target_element_type);
                    targetEl.Update();
                    target_el.setEl(targetEl);
                    targetEl.Update();
                    targetEl.Refresh();
                }
            }
        }

        private void createTargetAttr2Model4SubTask(IUMLElement target_el, String target_element_type, EA.Element mainTargetEl)
        {
            if (AttributeEl.EA_TYPE.ToLower().Equals(target_element_type.ToLower()))
            {
                EA.Attribute targetAttr = mainTargetEl.Attributes.AddNew(target_el.getElName(), target_element_type);
                targetAttr.Update();
                target_el.setEl(targetAttr);
                targetAttr.Update();
            }
        }

        private void createTargetMethod2Model4SubTask(IUMLElement target_el, String target_element_type, EA.Element mainTargetEl)
        {
            if (OptionEl.EA_TYPE.ToLower().Equals(target_element_type.ToLower()))
            {
                EA.Method targetMethod = mainTargetEl.Methods.AddNew(target_el.getElName(), target_element_type);
                targetMethod.Update();
                target_el.setEl(targetMethod);
                targetMethod.Update();
            }
        }

        private void updateTargetEl2Model4SubTask(IUMLElement target_el, EA.Element targetEl, EA.Element mainTargetEl)
        {
            target_el.setEl(targetEl);
            targetEl.Update();
        }

        private void updateTargetEl2Model4SubTask(IUMLElement target_el, EA.Attribute targetAttribute, EA.Element mainTargetEl)
        {
            target_el.setEl(targetAttribute);
            targetAttribute.Update();
        }

        private void updateTargetEl2Model4SubTask(IUMLElement target_el, EA.Method targetMethod, EA.Element mainTargetEl)
        {
            target_el.setEl(targetMethod);
            targetMethod.Update();
        }

        private void deleteTargetEl2Model4SubTask(EA.Element targetEl, EA.Element mainTargetEl)
        {
            if (PackageEl.EA_TYPE.ToLower().Equals(targetEl.Type.ToLower()))
            {
                if (PackageEl.EA_TYPE.ToLower().Equals(mainTargetEl.Type.ToLower()))
                {
                    EA.Package cp = m_Repository.GetPackageByGuid(mainTargetEl.ElementGUID);
                    short p_index = 0;
                    foreach (EA.Package p in cp.Packages)
                    {
                        if (targetEl.Name.Equals(p.Name))
                        {
                            break;
                        }
                        p_index++;
                    }
                    cp.Packages.Delete(p_index);
                    cp.Packages.Refresh();
                }else{
                    EA.Package cp = m_Repository.GetPackageByID(mainTargetEl.PackageID);
                    short p_index = 0;
                    foreach (EA.Package p in cp.Packages)
                    {
                        if (targetEl.Name.Equals(p.Name))
                        {
                            break;
                        }
                        p_index++;
                    }
                    cp.Packages.Delete(p_index);
                    cp.Packages.Refresh();
                }
            }
            else
            {
                if (PackageEl.EA_TYPE.ToLower().Equals(mainTargetEl.Type.ToLower()))
                {
                    EA.Package cp = m_Repository.GetPackageByGuid(mainTargetEl.ElementGUID);
                    short p_index = 0;
                    foreach (EA.Element e in cp.Elements)
                    {
                        if (targetEl.Name.Equals(e.Name))
                        {
                            break;
                        }
                        p_index++;
                    }
                    mainTargetEl.Elements.Delete(p_index);
                    mainTargetEl.Elements.Refresh();
                }
                else {
                    short p_index = 0;
                    foreach (EA.Element e in mainTargetEl.Elements)
                    {
                        if (targetEl.Name.Equals(e.Name))
                        {
                            break;
                        }
                        p_index++;
                    }
                    mainTargetEl.Elements.Delete(p_index);
                    mainTargetEl.Elements.Refresh();
                }
            }
        }

        private void deleteTargetAttr2Model4SubTask(EA.Attribute targetAttr, EA.Element mainTargetEl)
        {
            short p_index = 0;
            foreach (EA.Attribute a in mainTargetEl.Attributes)
            {
                if (targetAttr.Name.Equals(a.Name))
                {
                    break;
                }
                p_index++;
            }
            mainTargetEl.Attributes.Delete(p_index);
            mainTargetEl.Attributes.Refresh();
        }

        private void deleteTargetMethod2Model4SubTask(EA.Method targetMethod, EA.Element mainTargetEl)
        {
            short p_index = 0;
            foreach (EA.Method m in mainTargetEl.Methods)
            {
                if (targetMethod.Name.Equals(m.Name))
                {
                    break;
                }
                p_index++;
            }
            mainTargetEl.Methods.Delete(p_index);
            mainTargetEl.Methods.Refresh();
        }

        private Object findTargetEl4SubTask(String target_element_name, String target_element_type, EA.Element mainTargetEl)
        {
            if (mainTargetEl != null)
            {
                if (PackageEl.EA_TYPE.ToLower().Equals(target_element_type.ToLower()))
                {
                    if (PackageEl.EA_TYPE.ToLower().Equals(mainTargetEl.Type.ToLower()))
                    {
                        EA.Package cp = m_Repository.GetPackageByGuid(mainTargetEl.ElementGUID);
                        foreach (EA.Package p in cp.Packages)
                        {
                            if (target_element_name.Equals(p.Name))
                            {
                                return p.Element;
                            }
                        }
                    }else{
                        EA.Package cp = m_Repository.GetPackageByID(mainTargetEl.PackageID);
                        foreach (EA.Package p in cp.Packages)
                        {
                            if (target_element_name.Equals(p.Name))
                            {
                                return p.Element;
                            }
                        }
                    }
                }
                else if (AttributeEl.EA_TYPE.ToLower().Equals(target_element_type.ToLower()))
                {
                    if (ClassEl.EA_TYPE.ToLower().Equals(mainTargetEl.Type.ToLower()))
                    {
                        foreach (EA.Attribute a in mainTargetEl.Attributes)
                        {
                            if (target_element_name.Equals(a.Name))
                            {
                                return a;
                            }
                        }
                    }
                }
                else if (OptionEl.EA_TYPE.ToLower().Equals(target_element_type.ToLower()))
                {
                    if (ClassEl.EA_TYPE.ToLower().Equals(mainTargetEl.Type.ToLower()))
                    {
                        foreach (EA.Method m in mainTargetEl.Methods)
                        {
                            if (target_element_name.Equals(m.Name))
                            {
                                return m;
                            }
                        }
                    }
                }
                else
                {
                    foreach (EA.Element e in mainTargetEl.Elements)
                    {
                        if (target_element_name.Equals(e.Name))
                        {
                            return e;
                        }
                    }
                }
            }

            return null;
        }

    }
}
