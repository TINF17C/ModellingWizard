using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace Aml.Editor.Plugin
{
    class Datatables
    {
       /* public Dictionary<int,Parameters> DictName { get; set; }
        public DataTable DataRowName { get; set; }
        public DataGridView DataGridViewName { get; set; }*/
        /*public Datatables()
        {

        }
        public Datatables(string dictName, string dataRowName)
            : this()
        {
            this.DictName = dictName;
            this.DictName = dataRowName;
        }*/
        public DataTable ThreeParametersdatatable( )
        {
            DataTable threeParametersdatatable = new DataTable();
            threeParametersdatatable.Columns.Add("ReferenceID");
            threeParametersdatatable.Columns.Add("Attribute");
            threeParametersdatatable.Columns.Add("Value");

            return threeParametersdatatable;

        }
        // method to iterate 
        public void CreateParameter(Dictionary<int,Parameters> dictName, DataTable dataRowName, DataGridView dataGridViewName)
        {
            Dictionary<int, Parameters> DictName = dictName;
            DataTable DataRowName = dataRowName;
            DataGridView DataGridViewName = dataGridViewName;
            foreach (KeyValuePair<int, Parameters> eClassKeyValuePair in DictName)
            {
                Parameters par = eClassKeyValuePair.Value;

                DataRow row = DataRowName.NewRow();

                row["ReferenceID"] = par.RefSemanticPrefix;
                row["Attribute"] = par.Parameter;
                row["Value"] = "";
                DataRowName.Rows.Add(row);


            }
            // For each loop creating the rows in the data table 
            foreach (DataRow IDT in DataRowName.Rows)
            {
                int num = DataGridViewName.Rows.Add();
                DataGridViewName.Rows[num].Cells[0].Value = IDT["ReferenceID"].ToString();
                DataGridViewName.Rows[num].Cells[1].Value = IDT["Attribute"].ToString();
                DataGridViewName.Rows[num].Cells[2].Value = IDT["Value"].ToString();
            }
        }
        
        

        
    }
}
