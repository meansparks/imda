using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUMLModel.umlelements
{
    public class ActivityEl : BaseElement
    {
        public const String EA_TYPE = "Activity";

        public ActivityEl()
        {

        }
        public ActivityEl(EA.Element theActivity)
        {
            if (theActivity != null)
            {

                this.Name = theActivity.Name;
                this.Alias = theActivity.Alias;
                this.Notes = theActivity.Notes;
                this.Scope = theActivity.Visibility;
                this.Stereotype = theActivity.Stereotype;

            }
        }

        public new EA.Element setEl(EA.Element theElement)
        {
            if (ActivityEl.EA_TYPE.Equals(theElement.Type))
            {
                base.setEl(theElement);
            }
            return theElement;
        }

    }
}
