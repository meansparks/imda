using CodeGenEng.ConfigParser;
using CodeGenEng.Properties;
using CodeGenEng.velocity;
using CodeGenEng.velocity.engines;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.util;

namespace CodeGenEng
{
    class TemplateJob
    {
        private EA.Repository m_Repository;
        private GenJobData genjob;
        private ConfigData config_data;

        public TemplateJob(EA.Repository m_Repository, GenJobData genjob, ConfigData config_data)
        {
            this.m_Repository = m_Repository;
            this.genjob = genjob;
            this.config_data = config_data;
        }

        public bool runJob()
        {
            try
            {
                if (genjob != null && genjob.Templates != null && genjob.Templates.Count() > 0)
                {
                    foreach (TemplateData theTemplate in genjob.Templates)
                    {
                        ModelProcess mp = new ModelProcess(m_Repository, genjob, theTemplate, config_data);
                        mp.toProcess();
                    }
                    Tools.writerOutput(m_Repository, genjob.Id + IMDAResources.job_complete);
                    return true;
                }

                Tools.writerOutput(m_Repository, genjob.Id + IMDAResources.template_failed);
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

    }
}
