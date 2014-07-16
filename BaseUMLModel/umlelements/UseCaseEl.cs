using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUMLModel.umlelements
{
    public class UseCaseEl : BaseElement
    {
        public const String EA_TYPE = "UseCase";

        public UseCaseEl()
        {

        }
        public UseCaseEl(EA.Element theUseCase)
        {
            if (theUseCase != null)
            {

                this.Name = theUseCase.Name;
                this.Alias = theUseCase.Alias;
                this.Notes = theUseCase.Notes;
                this.Scope = theUseCase.Visibility;
                this.Stereotype = theUseCase.Stereotype;

            }
        }

        public new EA.Element setEl(EA.Element theElement)
        {
            if (UseCaseEl.EA_TYPE.Equals(theElement.Type))
            {
                base.setEl(theElement);
            }
            return theElement;
        }

    }
}
