using CodeGenEng.ConfigParser;
using CodeGenEng.Properties;
using CodeGenEng.velocity;
using CodeGenEng.velocity.engines;
using BaseUMLModel.umlelements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.util;

namespace CodeGenEng
{
    class VelocityGenCode
    {
        private EA.Element theClass;

        private GenJobData theGenJob;
        private TemplateData theTemplate;

        private String model_name;
        private ConfigData config_data;

        public VelocityGenCode(EA.Element theClass, GenJobData theGenJob, TemplateData theTemplate, String model_name, ConfigData config_data)
        {
            this.theClass = theClass;

            this.theGenJob = theGenJob;
            this.theTemplate = theTemplate;

            this.model_name = model_name;
            this.config_data = config_data;
        }

        public String getContent()
        {
            try
            {
                String templateDir = theGenJob.TRoot;
                INVelocityEngine fileEngine = NVelocityEngineFactory.CreateNVelocityFileEngine(templateDir, true);

                IDictionary context = new Hashtable();

                context.Add("theClass", new ClassEl(theClass));
                context.Add("theGenJob", theGenJob);
                context.Add("theTemplate", theTemplate);
                context.Add("model_name", model_name);
                context.Add("config_data", config_data);

                return fileEngine.Process(context, theTemplate.Template_name);

            }
            catch (Exception e)
            {
                throw new IMDAException(theTemplate.Template_name + IMDAResources.template_parse_failed, e);
            }
        }
    }
}
