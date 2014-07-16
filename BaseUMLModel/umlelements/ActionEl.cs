using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUMLModel.umlelements
{
    public class ActionEl : BaseElement
    {
        public const String EA_TYPE = "Action";

        public ActionEl()
        {

        }
        public ActionEl(EA.Element theAction)
        {
            if (theAction != null)
            {

                this.Name = theAction.Name;
                this.Alias = theAction.Alias;
                this.Notes = theAction.Notes;
                this.Scope = theAction.Visibility;
                this.Stereotype = theAction.Stereotype;

            }
        }

        public new EA.Element setEl(EA.Element theElement)
        {
            if (ActionEl.EA_TYPE.Equals(theElement.Type))
            {
                base.setEl(theElement);
            }
            return theElement;
        }
    }
}
