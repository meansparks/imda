using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUMLModel.umlelements
{
    public abstract class BaseElement : IUMLElement
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string stereotype;
        public string Stereotype
        {
            get { return stereotype; }
            set { stereotype = value; }
        }

        private string scope;
        public string Scope
        {
            get { return scope; }
            set { scope = value; }
        }

        private string alias;
        public string Alias
        {
            get { return alias; }
            set { alias = value; }
        }

        private string notes;
        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        public EA.Element setEl(EA.Element theElement)
        {

            theElement.Name = this.Name;
            theElement.Alias = this.Alias;
            theElement.Notes = this.Notes;
            theElement.Visibility = this.Scope;
            theElement.Stereotype = this.Stereotype;

            return theElement;
        }
        public EA.Attribute setEl(EA.Attribute theAttribute)
        {
            theAttribute.Name = this.Name;
            theAttribute.Notes = this.Notes;
            theAttribute.Visibility = this.Scope;
            theAttribute.Stereotype = this.Stereotype;

            return theAttribute;
        }
        public EA.Method setEl(EA.Method theMethod)
        {
            theMethod.Name = this.Name;
            theMethod.Notes = this.Notes;
            theMethod.Visibility = this.Scope;
            theMethod.Stereotype = this.Stereotype;

            return theMethod;
        }
        public EA.Parameter setEl(EA.Parameter theParameter) {
            theParameter.Name = this.Name;
            theParameter.Alias = this.Alias;
            theParameter.Notes = this.Notes;
            theParameter.Stereotype = this.Stereotype;

            return theParameter;
        }

        public String getElName()
        {
            return this.Name;
        }
    }
}
