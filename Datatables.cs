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
        public DataTable Parametersdatatable()
        {
            DataTable Parametersdatatable = new DataTable();
            Parametersdatatable.Columns.Add("ReferenceID");
            Parametersdatatable.Columns.Add("Attribute");
            Parametersdatatable.Columns.Add("Min");
            Parametersdatatable.Columns.Add("Nom");
            Parametersdatatable.Columns.Add("Max");
            Parametersdatatable.Columns.Add("Value");
            Parametersdatatable.Columns.Add("Unit");
            Parametersdatatable.Columns.Add("Pin");
           


            return Parametersdatatable;

        }
        // method to iterate 
        public void CreateDataTableWith3Columns(Dictionary<int, Parameters> dictName, DataTable dataRowName, DataGridView dataGridViewName)
        {
            Dictionary<int, Parameters> DictName = dictName;
            DataTable DataRowName = dataRowName;
            DataGridView DataGridViewName = dataGridViewName;
            foreach (KeyValuePair<int, Parameters> everyKeyValuePair in DictName)
            {
                Parameters par = everyKeyValuePair.Value;

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
        public void CreateDataTableWith4Columns(Dictionary<int, Parameters> dictName, DataTable dataRowName, DataGridView dataGridViewName)
        {
            Dictionary<int, Parameters> DictName = dictName;
            DataTable DataRowName = dataRowName;
            DataGridView DataGridViewName = dataGridViewName;
            foreach (KeyValuePair<int, Parameters> everyKeyValuePair in DictName)
            {
                Parameters par = everyKeyValuePair.Value;

                DataRow row = DataRowName.NewRow();

                row["ReferenceID"] = par.RefSemanticPrefix;
                row["Attribute"] = par.Parameter;
                row["Value"] = par.Value;
                row["Unit"] = par.Unit;
                DataRowName.Rows.Add(row);


            }
            // For each loop creating the rows in the data table 
            foreach (DataRow IDT in DataRowName.Rows)
            {
                int num = DataGridViewName.Rows.Add();
                DataGridViewName.Rows[num].Cells[0].Value = IDT["ReferenceID"].ToString();
                DataGridViewName.Rows[num].Cells[1].Value = IDT["Attribute"].ToString();
                DataGridViewName.Rows[num].Cells[2].Value = IDT["Value"].ToString();
                DataGridViewName.Rows[num].Cells[3].Value = IDT["Unit"].ToString();
            }



        }
        public void CreateDataTableWith5Columns(Dictionary<int, Parameters> dictName, DataTable dataRowName, DataGridView dataGridViewName)
        {
            Dictionary<int, Parameters> DictName = dictName;
            DataTable DataRowName = dataRowName;
            DataGridView DataGridViewName = dataGridViewName;
            foreach (KeyValuePair<int, Parameters> everyKeyValuePair in DictName)
            {
                Parameters par = everyKeyValuePair.Value;

                DataRow row = DataRowName.NewRow();

                row["P"] = par.RefSemanticPrefix;
                row["ReferenceID"] = par.RefSemanticPrefix;
                row["Attribute"] = par.Parameter;
                row["Value"] = "";
                row["Unit"] = par.Unit;
                DataRowName.Rows.Add(row);

                break;


            }
            // For each loop creating the rows in the data table 
            foreach (DataRow IDT in DataRowName.Rows)
            {
                int num = DataGridViewName.Rows.Add();
                DataGridViewName.Rows[num].Cells[0].Value = IDT["ReferenceID"].ToString();
                DataGridViewName.Rows[num].Cells[1].Value = IDT["Attribute"].ToString();
                DataGridViewName.Rows[num].Cells[2].Value = IDT["Value"].ToString();
                DataGridViewName.Rows[num].Cells[3].Value = IDT["Unit"].ToString();
                
                break;
            }



        }
        public void CreateDataTableWith6Columns(Dictionary<int, Parameters> dictName, DataTable dataRowName, DataGridView dataGridViewName)
        {
            Dictionary<int, Parameters> DictName = dictName;
            DataTable DataRowName = dataRowName;
            DataGridView DataGridViewName = dataGridViewName;
            foreach (KeyValuePair<int, Parameters> everyKeyValuePair in DictName)
            {
                Parameters par = everyKeyValuePair.Value;

                DataRow row = DataRowName.NewRow();


                row["ReferenceID"] = par.RefSemanticPrefix;
                row["Attribute"] = par.Parameter;
                row["Min"] = "";
                row["Nom"] = "";
                row["Max"] = "";
                row["Unit"] = par.Unit;
                DataRowName.Rows.Add(row);

                


            }
            // For each loop creating the rows in the data table 
            foreach (DataRow IDT in DataRowName.Rows)
            {
                int num = DataGridViewName.Rows.Add();
                DataGridViewName.Rows[num].Cells[0].Value = IDT["ReferenceID"].ToString();
                DataGridViewName.Rows[num].Cells[1].Value = IDT["Attribute"].ToString();
                DataGridViewName.Rows[num].Cells[2].Value = IDT["Min"].ToString();
                DataGridViewName.Rows[num].Cells[3].Value = IDT["Nom"].ToString();
                DataGridViewName.Rows[num].Cells[4].Value = IDT["Max"].ToString();
                DataGridViewName.Rows[num].Cells[5].Value = IDT["Unit"].ToString();
                
            }
        }
    }
}
