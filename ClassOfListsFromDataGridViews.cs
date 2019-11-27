using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aml.Editor.Plugin
{
    class ClassOfListsFromDataGridViews
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Default { get; set; }
        public string Unit { get; set; }
        public string Semantic { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public string CopyRight { get; set; }
        public string AttributePath { get; set; }
        public string RefBaseClassPath { get; set; }
        public string ID { get; set; }
        public string ReferencedClassName { get; set; }
        //public CAEXSequence<RefSemanticType> RefSemanticList { get; set; }


       

        public ClassOfListsFromDataGridViews()
        {
            // RefSemanticList = new List<CAEXSequence<RefSemanticType>>();
        }

        public ClassOfListsFromDataGridViews(string name, string value,
            string _default, string unit,
            string reference, string description,
            string copyRight, string semantic, string
            attributePath, string refBaseClassPath,
            string id, string referencedClassName /*CAEXSequence<RefSemanticType> refSemanticList*/)
            : this()
        {
            this.Name = name;
            this.Value = value;
            this.Default = _default;
            this.Unit = unit;
            this.Reference = reference;
            this.Description = description;
            this.CopyRight = copyRight;
            this.Semantic = semantic;
            this.AttributePath = attributePath;
            this.RefBaseClassPath = refBaseClassPath;
            this.ID = id;
            this.ReferencedClassName = referencedClassName;
            //this.RefSemanticList = refSemanticList;

        }

        public override string ToString()
        {
            return "ClassOfListsFromReferencefile(" + Name + "=" + Value + "=" + Default + "=" + Unit + "=" + Reference + "="
                + Description + "=" + CopyRight + "=" + Semantic + "=" + AttributePath + "=" + RefBaseClassPath + "=" + ID
                + "=" + ReferencedClassName + ")";
        }

    }
}
