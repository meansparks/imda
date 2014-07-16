using BaseUMLModel.umlelements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.util;

namespace BaseUMLModel
{
    public class ElFactory
    {
        private String umlelement_base_namespace = "BaseUMLModel.umlelements";

        public IUMLElement createElement(EA.Repository m_Repository, String elmentType)
        {
            try {
                if (OptionEl.EA_TYPE.ToLower().Equals(elmentType.ToLower()))
                {
                    elmentType = "Option";
                }
                String ElementClass = elmentType + "El";
                Type t = Type.GetType(this.umlelement_base_namespace + "." + ElementClass);
                return (IUMLElement)System.Activator.CreateInstance(t);
            }
            catch (Exception e)
            {
                Tools.writerOutput(m_Repository, elmentType + "El"  + e.Message);
                return null;
            }
        }

        public IUMLElement createElement(EA.Repository m_Repository, String elmentType, EA.Element theElement)
        {
            try
            {
                String ElementClass = elmentType + "El";
                return createElementByClassType(ElementClass, theElement);
            }
            catch (Exception e)
            {
                Tools.writerOutput(m_Repository, elmentType + "El" + e.Message);
                return null;
            }
        }

        public IUMLElement createElement(EA.Repository m_Repository, String elmentType, EA.Attribute theAttribute)
        {
            try
            {
                String ElementClass = elmentType + "El";
                return createElementByClassType(ElementClass, theAttribute);
            }
            catch (Exception e)
            {
                Tools.writerOutput(m_Repository, elmentType + "El" + e.Message);
                return null;
            }
        }

        public IUMLElement createElement(EA.Repository m_Repository, String elmentType, EA.Method theMethod)
        {
            try
            {
                String ElementClass = elmentType + "El";
                return createElementByClassType(ElementClass, theMethod);
            }
            catch (Exception e)
            {
                Tools.writerOutput(m_Repository, elmentType + "El" + e.Message);
                return null;
            }
        }

        private IUMLElement createElementByClassType(String ElementClass, EA.Element theElement)
        {
            Type t = Type.GetType(this.umlelement_base_namespace + "." + ElementClass);
            if (theElement != null && t.GetField("EA_TYPE").GetValue(null).ToString().ToLower().Equals(theElement.Type.ToLower()))
            {
                object[] parameters = { theElement };
                return (IUMLElement)System.Activator.CreateInstance(t, parameters);
            }
            return (IUMLElement)System.Activator.CreateInstance(t);
        }

        private IUMLElement createElementByClassType(String ElementClass, EA.Attribute theAttribute)
        {
            Type t = Type.GetType(this.umlelement_base_namespace + "." + ElementClass);
            if (theAttribute != null)
            {
                object[] parameters = { theAttribute };
                return (IUMLElement)System.Activator.CreateInstance(t, parameters);
            }
            return (IUMLElement)System.Activator.CreateInstance(t);
        }

        private IUMLElement createElementByClassType(String ElementClass, EA.Method theMethod)
        {
            Type t = Type.GetType(this.umlelement_base_namespace + "." + ElementClass);
            if (theMethod != null)
            {
                object[] parameters = { theMethod };
                return (IUMLElement)System.Activator.CreateInstance(t, parameters);
            }
            return (IUMLElement)System.Activator.CreateInstance(t);
        }





    }
}
