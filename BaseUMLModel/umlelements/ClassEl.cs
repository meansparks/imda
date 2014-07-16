using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseUMLModel.umlelements
{
    public class ClassEl : BaseElement
    {
        public const String EA_TYPE = "Class";

        public ClassEl()
        {

        }
        public ClassEl(EA.Element theClass)
        {
            if (theClass != null) {

                this.Name = theClass.Name;
                this.Alias = theClass.Alias;
                this.Notes = theClass.Notes;
                this.Persistence = theClass.Persistence;
                this.Scope = theClass.Visibility;
                this.Stereotype = theClass.Stereotype;

                this.Attributes = new List<AttributeEl>();
                if (theClass.Attributes != null && theClass.Attributes.Count > 0)
                {
                    foreach (EA.Attribute theAttribute in theClass.Attributes)
                    {
                        this.Attributes.Add(new AttributeEl(theAttribute));
                    }
                }

                this.Options = new List<OptionEl>();
                if (theClass.Methods != null && theClass.Methods.Count > 0)
                {
                    foreach (EA.Method theMethod in theClass.Methods)
                    {
                        this.Options.Add(new OptionEl(theMethod));
                    }
                }
            }
        }

        private string persistence;
        public string Persistence
        {
            get { return persistence; }
            set { persistence = value; }
        }


        private List<AttributeEl> attributes;
        public List<AttributeEl> Attributes
        {
            get
            {
                return attributes;
            }

            set
            {
                attributes = value;
            }
        }

        private List<OptionEl> options;
        public List<OptionEl> Options
        {
            get
            {
                return options;
            }

            set
            {
                options = value;
            }
        }

        public new EA.Element setEl(EA.Element theElement){
            if (ClassEl.EA_TYPE.Equals(theElement.Type))
            {
                base.setEl(theElement);

                theElement.Persistence = this.Persistence;
            }
            return theElement;
        }
    }
}
