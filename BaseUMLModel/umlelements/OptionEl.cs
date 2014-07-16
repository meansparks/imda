using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUMLModel.umlelements
{
    public class OptionEl : BaseElement
    {
        public const String EA_TYPE = "Method";

        public OptionEl()
        {

        }

        public OptionEl(EA.Method theMethod)
        {
            if (theMethod != null)
            {
                this.Name = theMethod.Name;
                this.Alias = theMethod.Style;
                this.Notes = theMethod.Notes;
                this.Scope = theMethod.Visibility;
                this.Stereotype = theMethod.Stereotype;
                //this.Parameter = theMethod.Parameters;
                this.Return_type = theMethod.ReturnType;

                this.Parameters = new List<ParameterEl>();
                if (theMethod.Parameters != null && theMethod.Parameters.Count > 0)
                {
                    foreach (EA.Parameter theParameter in theMethod.Parameters)
                    {
                        this.Parameters.Add(new ParameterEl(theParameter));
                    }
                }

            }
        }

        private List<ParameterEl> parameters;
        public List<ParameterEl> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        private string return_type;
        public string Return_type
        {
            get { return return_type; }
            set { return_type = value; }
        }

        public new EA.Method setEl(EA.Method theMethod)
        {
            base.setEl(theMethod);
            theMethod.ReturnType = this.Return_type;
            return theMethod;
        }

    }
}
