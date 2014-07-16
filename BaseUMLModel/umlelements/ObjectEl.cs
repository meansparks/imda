using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUMLModel.umlelements
{
    public class ObjectEl : BaseElement
    {
        public const String EA_TYPE = "Object";

        public ObjectEl()
        {

        }
        public ObjectEl(EA.Element theObject)
        {
            if (theObject != null)
            {

                this.Name = theObject.Name;
                this.Alias = theObject.Alias;
                this.Notes = theObject.Notes;
                this.Scope = theObject.Visibility;
                this.Stereotype = theObject.Stereotype;

            }
        }

        public new EA.Element setEl(EA.Element theElement)
        {
            if (ObjectEl.EA_TYPE.Equals(theElement.Type))
            {
                base.setEl(theElement);
            }
            return theElement;
        }

    }
}
