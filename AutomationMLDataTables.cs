using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using System.Drawing;

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
            AMLAttributeParameters.Columns.Add("Semantic", typeof(ComboBox));
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
                    List<string> listofRefsemantics = new List<string>();
                    DataRow row = DataRowName.NewRow();

                    /*row["AttributeName"] = item.Name;
                    row["Value"] = item.Value;
                    row["Default"] = item.Default;
                    row["Unit"] = item.Unit;
                    row["DataType"] = null;*/
                    int num = DataGridViewName.Rows.Add();


                    if (item.Name == "Manufacturer" || item.Name == "Model")
                    {
                        DataGridViewName.Rows[num].Cells[0].Value = item.Name;
                        DataGridViewName.Rows[num].Cells[0].Style.ForeColor = Color.Red;
                    }
                    else
                    {
                        DataGridViewName.Rows[num].Cells[0].Value = item.Name;
                    }
                    
                    DataGridViewName.Rows[num].Cells[1].Value = item.Value;
                    DataGridViewName.Rows[num].Cells[2].Value = item.Default;
                    DataGridViewName.Rows[num].Cells[3].Value = item.Unit;
                    DataGridViewName.Rows[num].Cells[4].Value = item.DataType;
                    try
                    {
                        foreach (var value in item.RefSemanticList.Elements)
                        {
                            listofRefsemantics.Add(value.FirstAttribute.Value.ToString());
                        }
                    }
                    catch (Exception)
                    {

                    }

                    DataGridViewComboBoxCell dgvcbc = (DataGridViewComboBoxCell)DataGridViewName.Rows[num].Cells[5];
                    dgvcbc.Items.Clear();

                    foreach (var items in listofRefsemantics)
                    {
                        dgvcbc.Items.Add(items);
                    }

                   
                    
                 
                    DataRowName.Rows.Add(row);

                  

                    break;
                }
               
                
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
                row["DataType"] = item.DataType;
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
