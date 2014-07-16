using BaseUMLModel.umlelements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranModelEng.ConfigParser;
using TranModelEng.Properties;
using Util.util;

namespace TranModelEng.importJob
{
    class ImportJob
    {
        private EA.Repository m_Repository;
        private ImportJobData import_job;
        private ConfigData config_data;

        private List<String> file_paths = new List<String>();

        public ImportJob(EA.Repository m_Repository, ImportJobData import_job, ConfigData config_data)
        {
            this.m_Repository = m_Repository;
            this.import_job = import_job;
            this.config_data = config_data;
        }

        public bool runJob()
        {
            try
            {
                if (import_job != null && ((import_job.ModelFiles != null && import_job.ModelFiles.Count() > 0) || (import_job.ModelDirs != null && import_job.ModelDirs.Count() > 0)))
                {
                    //导入文件列表
                    foreach (String file_path in import_job.ModelFiles)
                    {
                        this.taskExecution(file_path);
                    }


                    //导入文件夹列表
                    foreach (ModelDirData m_dir in import_job.ModelDirs)
                    {
                        this.setFilePathList(m_dir);
                        if (this.file_paths != null && this.file_paths.Count > 0)
                        {
                            foreach (String file_path in this.file_paths)
                            {
                                this.taskExecution(file_path);
                            }
                        }
                    }


                    Tools.writerOutput(m_Repository, import_job.Id + IMDAResources.job_complete);
                    return true;
                }

                Tools.writerOutput(m_Repository, import_job.Id + IMDAResources.job_failed);
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

        private void setFilePathList(ModelDirData m_dir)
        {
            DirectoryInfo TheFolder = new DirectoryInfo(m_dir.DirPath);
            //遍历文件
            foreach (FileInfo f in TheFolder.GetFiles("*." + m_dir.ExtName))
            {
                this.file_paths.Add(f.FullName);
            }
            //遍历文件夹
            if (m_dir.IsSubPackage)
            {
                foreach (DirectoryInfo folder in TheFolder.GetDirectories("*"))
                {
                    ModelDirData curr_dir = new ModelDirData();
                    curr_dir.DirPath = folder.FullName;
                    curr_dir.ExtName = m_dir.ExtName;
                    curr_dir.IsSubPackage = m_dir.IsSubPackage;

                    this.setFilePathList(curr_dir);
                }
            }
        }

        private void taskExecution(String file_path)
        {
            try
            {
                Tools.writerOutput(m_Repository, this.import_job.Id + IMDAResources.task_begin);
                List<ClassEl> classes = ImportFileParse.parseClassEl(file_path);
                EA.Package targetPackage;
                /* 获得导入的目标包模型 */
                if ("".Equals(import_job.TargetPackagePath))
                {
                    targetPackage = m_Repository.GetTreeSelectedPackage();
                }
                else
                {
                    targetPackage = parseTargetPackagePath(import_job.TargetPackagePath);
                }

                Import2Model import2model = new Import2Model(m_Repository, import_job, targetPackage);
                import2model.toImport(classes);
                Tools.writerOutput(m_Repository, this.import_job.Id + IMDAResources.task_complete);
            }
            catch (IMDAException)
            {
                Tools.writerOutput(m_Repository, this.import_job.Id + IMDAResources.task_failed);
            }
            catch (Exception)
            {
                Tools.writerOutput(m_Repository, this.import_job.Id + IMDAResources.task_failed);
            }
        }

        private EA.Package parseTargetPackagePath(String targetPackagePath)
        {
            EA.Package target_path = null;
            try
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
