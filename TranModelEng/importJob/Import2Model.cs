using BaseUMLModel.umlelements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranModelEng.ConfigParser;
using TranModelEng.Properties;
using Util.util;

namespace TranModelEng.importJob
{
    class Import2Model
    {
        private EA.Repository m_Repository;

        private ImportJobData import_job;
        private EA.Package targetPackage;

        public Import2Model(EA.Repository m_Repository, ImportJobData import_job, EA.Package targetPackage)
        {
            this.m_Repository = m_Repository;
            this.import_job = import_job;
            this.targetPackage = targetPackage;
        }

        public void toImport(List<ClassEl> classes)
        {
            try
            {
                if (classes != null && classes.Count > 0)
                {   
                    /* 循环class逐个导入到目标包 */
                    foreach (ClassEl c in classes)
                    {
                        this.genTargetModel(c);
                    }
                }

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

        private void genTargetModel(ClassEl theClassEl)
        {
            EA.Element targetEl = findTargetEl(theClassEl.Name);
            if (ImportJobData.CREATMODE_NEW.ToLower().Equals(import_job.CreatMode))
            {
                if (targetEl == null)
                {
                    this.createTargetEl2Model(theClassEl);
                }
            }
            else if (ImportJobData.CREATMODE_UPDATE.ToLower().Equals(import_job.CreatMode))
            {
                if (targetEl == null)
                {
                    this.createTargetEl2Model(theClassEl);
                }
                else
                {
                    this.deleteTargetEl2Model(targetEl);
                    this.createTargetEl2Model(theClassEl);
                }
            }
            else
            {
                if (targetEl != null)
                {
                    //删除当前元素
                    this.deleteTargetEl2Model(targetEl);
                    //创建新的元素
                    this.createTargetEl2Model(theClassEl);
                }
                else
                {
                    this.createTargetEl2Model(theClassEl);
                }
            }
        }

        private void createTargetEl2Model(ClassEl target_el)
        {
            //创建类
            EA.Element targetEl = this.targetPackage.Elements.AddNew(target_el.Name, ClassEl.EA_TYPE);
            targetEl.Update();
            target_el.setEl(targetEl);
            targetEl.Update();
            targetEl.Refresh();

            //创建属性
            if (target_el.Attributes != null && target_el.Attributes.Count > 0)
            {
                foreach (AttributeEl a in target_el.Attributes)
                {
                    EA.Attribute targetAttribute = targetEl.Attributes.AddNew(a.Name, AttributeEl.EA_TYPE);
                    a.setEl(targetAttribute);
                    targetAttribute.Update();
                }
                targetEl.Attributes.Refresh();
            }

            //创建方法
            if (target_el.Options != null && target_el.Options.Count > 0)
            {
                foreach (OptionEl o in target_el.Options)
                {
                    EA.Method targetMethod = targetEl.Methods.AddNew(o.Name, OptionEl.EA_TYPE);
                    o.setEl(targetMethod);
                    targetMethod.Update();
                }
                targetEl.Methods.Refresh();
            }

        }

        private void deleteTargetEl2Model(EA.Element targetEl)
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

        private EA.Element findTargetEl(String target_element_name)
        {
            if (this.targetPackage != null)
            {
				this.targetPackage.Elements.Refresh();
                foreach (EA.Element e in this.targetPackage.Elements)
                {
                    if (target_element_name.Equals(e.Name) && ClassEl.EA_TYPE.ToLower().Equals(e.Type.ToLower()))
                    {
                        return e;
                    }
                }
            }
            return null;
        }


    }
}
