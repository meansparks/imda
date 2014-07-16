using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUMLModel.umlelements
{
    public class ParameterEl : BaseElement
    {
        public const String EA_TYPE = "Parameter";

        public ParameterEl()
        {

        }

        public ParameterEl(EA.Parameter theParameter)
        {
            if (theParameter != null)
            {
                this.Name = theParameter.Name;
                this.Alias = theParameter.Style;
                this.Notes = theParameter.Notes;
                this.Stereotype = theParameter.Stereotype;
                this.param_type = theParameter.Type;
            }
        }

        private string param_type;
        public string Param_type
        {
            get { return param_type; }
            set { param_type = value; }
        }

        public new EA.Parameter setEl(EA.Parameter theParameter)
        {
            if (ParameterEl.EA_TYPE.Equals(theParameter.Type))
            {
                base.setEl(theParameter);
                theParameter.Type = this.param_type;
            }
            return theParameter;
        }
    }
}
