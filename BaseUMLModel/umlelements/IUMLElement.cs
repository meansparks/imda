using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUMLModel.umlelements
{
    public interface IUMLElement
    {
        EA.Element setEl(EA.Element theElement);
        EA.Attribute setEl(EA.Attribute theAttribute);
        EA.Method setEl(EA.Method theMethod);
        EA.Parameter setEl(EA.Parameter theParameter);

        String getElName();
    }
}
