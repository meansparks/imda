using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUMLModel.umlelements
{
    public class AttributeEl : BaseElement
    {
        public const String EA_TYPE = "Attribute";

        public AttributeEl()
        {

        }

        public AttributeEl(EA.Attribute theAttribute)
        {
            if (theAttribute != null)
            {
                this.Name = theAttribute.Name;
                this.Alias = theAttribute.Style;
                this.Notes = theAttribute.Notes;
                this.Scope = theAttribute.Visibility;
                this.Stereotype = theAttribute.Stereotype;
                this.Initial = theAttribute.Default;
                this.Type = theAttribute.Type;
            }
        }

        private string type;
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        private string initial;
        public string Initial
        {
            get { return initial; }
            set { initial = value; }
        }

        public new EA.Attribute setEl(EA.Attribute theAttribute)
        {
            base.setEl(theAttribute);
            theAttribute.Default = this.Initial;
            theAttribute.Type = this.Type;
            return theAttribute;
        }
    }
}
