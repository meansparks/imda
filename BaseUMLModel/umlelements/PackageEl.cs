using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUMLModel.umlelements
{
    public class PackageEl : BaseElement
    {
        public const String EA_TYPE = "Package";

        public PackageEl()
        {

        }
        public PackageEl(EA.Element thePackage)
        {
            if (thePackage != null)
            {

                this.Name = thePackage.Name;
                this.Alias = thePackage.Alias;
                this.Notes = thePackage.Notes;
                this.Scope = thePackage.Visibility;
                this.Stereotype = thePackage.Stereotype;

            }
        }

        public new EA.Element setEl(EA.Element theElement)
        {
            if (PackageEl.EA_TYPE.Equals(theElement.Type))
            {
                base.setEl(theElement);
            }
            return theElement;
        }

    }
}
