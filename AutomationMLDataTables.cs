using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;

namespace Aml.Editor.Plugin
{
    class AutomationMLDataTables 
    {
        public DataTable AMLAttributeParameters()
        {
            DataTable AMLAttributeParameters = new DataTable();
            AMLAttributeParameters.Columns.Add("AttributeName");
            AMLAttributeParameters.Columns.Add("Value");
            AMLAttributeParameters.Columns.Add("Default");
            AMLAttributeParameters.Columns.Add("Unit");
            AMLAttributeParameters.Columns.Add("DataType");
            AMLAttributeParameters.Columns.Add("Semantic");
            /*AMLAttributeParameters.Columns.Add("Reference");
            AMLAttributeParameters.Columns.Add("Description");*/

            return AMLAttributeParameters;
        }

        public void CreateDataTableWithColumns( DataTable dataRowName, DataGridView dataGridViewName,
            KeyValuePair<string, List<List<ClassOfListsFromReferencefile>>> pair)
        {
            KeyValuePair<string, List<List<ClassOfListsFromReferencefile>>> Pair = pair;
            
            DataTable DataRowName = dataRowName;
            DataGridView DataGridViewName = dataGridViewName;
            
            foreach (var valueList in Pair.Value)
            {
                foreach (var item in valueList)
                {
                    DataRow row = DataRowName.NewRow();

                    row["AttributeName"] = item.Name;
                    row["Value"] = item.Value;
                    row["Default"] = item.Default;
                    row["Unit"] = item.Unit;
                    row["DataType"] = null;
                    try
                    {
                        foreach (var value in item.RefSemanticList.Elements)
                        {
                            row["Semantic"] = value.FirstAttribute.Value.ToString();
                        }
                    }
                    catch (Exception)
                    {

                    }
                    
                   
                   /* row["Reference"] = item.Reference;
                    row["Description"] = item.Description;*/
                    DataRowName.Rows.Add(row);

                    break;
                }

            }
            // For each loop creating the rows in the data table 
            foreach (DataRow IDT in DataRowName.Rows)
            {
                
                int num = DataGridViewName.Rows.Add();
                DataGridViewName.Rows[num].Cells[0].Value = IDT["AttributeName"].ToString();
                DataGridViewName.Rows[num].Cells[1].Value = IDT["Value"].ToString();
                DataGridViewName.Rows[num].Cells[2].Value = IDT["Default"].ToString();
                DataGridViewName.Rows[num].Cells[3].Value = IDT["Unit"].ToString();
                DataGridViewName.Rows[num].Cells[4].Value = IDT["DataType"].ToString();
                DataGridViewName.Rows[num].Cells[5].Value = IDT["Semantic"].ToString();
               // DataGridViewName.Rows[num].Cells[6].Value = IDT["Reference"].ToString();
               // DataGridViewName.Rows[num].Cells[7].Value = IDT["Description"].ToString();

            }
        }
        public void CheckForSameNameTextOfInternalAttributes(DataTable dataRowName, DataGridView dataGridViewName, KeyValuePair<string, List<ClassOfListsFromReferencefile>> pair)
        {
            KeyValuePair<string, List<ClassOfListsFromReferencefile>> Pair = pair;
            DataTable DataRowName = dataRowName;
            DataGridView DataGridViewName = dataGridViewName;
            foreach (DataGridViewRow eachrow in DataGridViewName.Rows)
            {
                try
                {
                   /* if (eachrow.Cells[0].Value.Equals(item.Name))
                    {

                    }*/
                }
                catch (Exception) { }
            }
        }
        public void CreateDataTableWithColumns(DataTable dataRowName, DataGridView dataGridViewName, KeyValuePair<string, List<ElectricalInterfaceParameters>> pair)
        {
            KeyValuePair<string, List<ElectricalInterfaceParameters>> Pair = pair;
            DataTable DataRowName = new DataTable();
            DataRowName = dataRowName;
            DataGridView DataGridViewName = dataGridViewName;

            foreach (var item in Pair.Value)
            {
                
                DataRow row = DataRowName.NewRow();
               

                row["AttributeName"] = item.AttributeName;
                row["Value"] = item.Values;
                row["Default"] = item.Default;
                row["Unit"] = item.Units;
                row["DataType"] = null;
                row["Semantic"] = item.Semantic;
                row["Reference"] = item.Reference;
                row["Description"] = item.Description;
                DataRowName.Rows.Add(row);


               

               /* break;*/
               


            }
            foreach (DataRow IDT in DataRowName.Rows)
            {

                int num = DataGridViewName.Rows.Add();
                DataGridViewName.Rows[num].Cells[0].Value = IDT["AttributeName"].ToString();
                DataGridViewName.Rows[num].Cells[1].Value = IDT["Value"].ToString();
                DataGridViewName.Rows[num].Cells[2].Value = IDT["Default"].ToString();
                DataGridViewName.Rows[num].Cells[3].Value = IDT["Unit"].ToString();
                DataGridViewName.Rows[num].Cells[4].Value = IDT["DataType"].ToString();
                DataGridViewName.Rows[num].Cells[5].Value = IDT["Semantic"].ToString();
                DataGridViewName.Rows[num].Cells[6].Value = IDT["Reference"].ToString();
                // DataGridViewName.Rows[num].Cells[7].Value = IDT["Description"].ToString();

            }
            // For each loop creating the rows in the data table 

        }

    }
}
