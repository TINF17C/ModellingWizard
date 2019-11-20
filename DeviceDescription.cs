using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aml.Engine.AmlObjects;
using Aml.Engine.CAEX;
using System.IO;
using System.IO.Packaging;
using System.Xml;
using System.Collections;
using System.IO.Compression;
using Aml.Editor.Plugin.Contracts;



namespace Aml.Editor.Plugin
{
    public partial class DeviceDescription : UserControl
    {
        private MWController mWController;
        private MWData.MWFileType filetype;


        bool isEditing = false;
        AnimationClass AMC = new AnimationClass();
        SearchAMLLibraryFile searchAMLLibraryFile = new SearchAMLLibraryFile();

        public DeviceDescription()
        {
            InitializeComponent();
        }

        public DeviceDescription(MWController mWController)
        {
            this.mWController = mWController;
            InitializeComponent();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateAML_Click(sender, e);

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clear();
            DataHierarchyTreeView();

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (vendorNameTxtBx.Text != "")
            {
                if (MessageBox.Show("Save Current File", "Save", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    GenerateAML_Click(sender, e);
                    return;
                }
                else
                {
                    Environment.Exit(0);

                }
            }


        }

        private void ClearDeviceDataBtn_Click(object sender, EventArgs e)
        {
            vendorNameTxtBx.Text = "";
            productRangeTxtBx.Text = "";
            vendorIDTxtBx.Text = "";
            hardwareReleaseTxtBx.Text = "";
            softwareReleaseTxtBx.Text = "";
            productNumberTxtBx.Text = "";
            orderNumberTxtBx.Text = "";
            vendorHomepageTxtBx.Text = "";
            communicationTechnologyTxtBx.Text = "";
            opTempMaxTxtBx.Text = "";
            opTempMinTxtBx.Text = "";
            producTxtBx.Text = "";
            productGroupTxtBx.Text = "";
            deviceNameTxtBx.Text = "";
            deviceIDTxtBx.Text = "";
            productFamilyTxtBx.Text = "";

            vendorNameRefSemanticBtn.Text = "";
            deviceNameRefSemanticBtn.Text = "";
            vendorHomepageRefSemanticBtn.Text = "";
            productFamilyRefSemanticBtn.Text = "";

            vendorNameRefSemanticBtn.Visible = false;
            deviceNameRefSemanticBtn.Visible = false;
            vendorHomepageRefSemanticBtn.Visible = false;
            productFamilyRefSemanticBtn.Visible = false;
        }
       public void DataHierarchyTreeView()
        {
            if (dataHierarchyTreeView.Nodes.Count == 0)
            {
                // Tree view updates on the "dataHiereachyTreeView"
                //TreeNode node;
                TreeNode node1;
                TreeNode node2;
                TreeNode node3;
                TreeNode node4;
                TreeNode node5;
                //node = dataHierarchyTreeView.Nodes.Add("Device Data");

                node1 = dataHierarchyTreeView.Nodes.Add("Device Data");



                node2 = dataHierarchyTreeView.Nodes.Add("Field Attachables");
                node2.Nodes.Add("Add");


                node3 = dataHierarchyTreeView.Nodes.Add("Generic Data");


                node4 = dataHierarchyTreeView.Nodes.Add("Interfaces");
                node4.Nodes.Add("Electrical Interface");
                node4.Nodes.Add("Sensor interface");
                node4.Nodes.Add("Mechanical interface");



            }
        }


        private void AddSemanticSystemBtn_Click(object sender, EventArgs e)
        {
            // call "Datatables" class 
            Datatables dataTables = new Datatables();

            if (semanticSystemTextBox.Text == "IEC-CDD" && dataHierarchyTreeView.Nodes.Count == 0)
            {
                //Initializing "Pararmeter of data table" and assigning a name.
                DataTable datatableheaderIRDIID = dataTables.Parametersdatatable();
                DataTable datatableheaderIRDIProductDetails = dataTables.Parametersdatatable();

                // Calling "IRDI Dictionary class" and retreving required Dictionary from it in this case the Dictionary is "IRDIIdentificationdata"
                //Later values in the dictionary are assigned to respective "Data Grid Views"
                DictionaryIRDI DIRDI = new DictionaryIRDI();
                Dictionary<int, Parameters> IRDIID = DIRDI.IRDIIdentificationdata();

                dataTables.CreateDataTableWith3Columns(IRDIID, datatableheaderIRDIID, identificationDataGridView);



                // Dispaly buttons

                AMC.DispalySemanticBtn(vendorNameRefSemanticBtn, identificationDataGridView, "Manufacturer Name");
                AMC.DispalySemanticBtn(deviceNameRefSemanticBtn, identificationDataGridView, "Product Name");
                AMC.DispalySemanticBtn(vendorHomepageRefSemanticBtn, identificationDataGridView, "Product Online Information URL");
                AMC.DispalySemanticBtn(productFamilyRefSemanticBtn, identificationDataGridView, "Product Family");

            }
            





        }

        // This Save button method takes multiple electrical interfaces intances and store them in a list 

        List<List<ElectricalParameters>> electricalInterfaceslistoflists = new List<List<ElectricalParameters>>();

        public void saveElectricalInterfaceBtn_Click(object sender, EventArgs e)
        {

            if (electricalInterfacesComboBox.Items.Count == Convert.ToInt32(electricalInterfacenumbTxtBox.Text))
            {
                MessageBox.Show("Specified Interfaces Limit has been reached, Change Number of Interfaces to add more Interfaces", " Interface Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                if (electricalInterfacesComboBox.Items.Count < Convert.ToInt32(electricalInterfacenumbTxtBox.Text))
                {
                    electricalInterfacesComboBox.Items.Add(connectorCombBox.Text);
                }
                ElectricalParameters eachElectricalParameter = new ElectricalParameters();
                eachElectricalParameter.Connector = Convert.ToString(connectorCombBox.Text);
                eachElectricalParameter.ConnectorCode = Convert.ToString(connectorCodeCombBox.Text);
                eachElectricalParameter.ConnectorType = Convert.ToString(connectorTypeCombBox.Text);
                eachElectricalParameter.Pins = Convert.ToString(numbOfPinsTxtBox.Text);

                List<ElectricalParametersInElectricalDataDataGridView> listofElectricalParametersFromElectricalDataDataGridView = new List<ElectricalParametersInElectricalDataDataGridView>();
                if (electricalDataDataGridView != null)
                {

                    int i = 0;
                    int j = electricalDataDataGridView.Rows.Count;
                    if (i <= 0)
                    {
                        while (i < j)
                        {
                            ElectricalParametersInElectricalDataDataGridView parametersFromElectricalDataDataGrid = new ElectricalParametersInElectricalDataDataGridView();
                            try
                            {
                                parametersFromElectricalDataDataGrid.ReferenceID = Convert.ToString(electricalDataDataGridView.Rows[i].Cells[0].Value);
                                parametersFromElectricalDataDataGrid.Attributes = Convert.ToString(electricalDataDataGridView.Rows[i].Cells[1].Value);
                                parametersFromElectricalDataDataGrid.Values = Convert.ToString(electricalDataDataGridView.Rows[i].Cells[2].Value);
                                parametersFromElectricalDataDataGrid.Units = Convert.ToString(electricalDataDataGridView.Rows[i].Cells[3].Value);
                            }
                            catch (Exception ex) { MessageBox.Show(ex.Message); }


                            listofElectricalParametersFromElectricalDataDataGridView.Add(parametersFromElectricalDataDataGrid);
                            i++;

                        }
                    }
                }
                eachElectricalParameter.listofElectricalDataDataGridViewParameters = listofElectricalParametersFromElectricalDataDataGridView;
                List<PinParametersInPinInfoDataGridView> listofPinFrompinInfoDataGridView = new List<PinParametersInPinInfoDataGridView>();
                if (pinInfoDataGridView != null)
                {

                    int i = 0;
                    int j = pinInfoDataGridView.Rows.Count;
                    if (i <= 0)
                    {
                        while (i < j)
                        {
                            PinParametersInPinInfoDataGridView parametersFrompinInfoDataGridView = new PinParametersInPinInfoDataGridView();
                            try
                            {
                                if (pinInfoDataGridView.Rows[i].Cells[0].Value != null)
                                {
                                    parametersFrompinInfoDataGridView.PinNumber = Convert.ToString(pinInfoDataGridView.Rows[i].Cells[0].Value);
                                    parametersFrompinInfoDataGridView.ReferenceID = Convert.ToString(pinInfoDataGridView.Rows[i].Cells[1].Value);
                                    parametersFrompinInfoDataGridView.Attributes = Convert.ToString(pinInfoDataGridView.Rows[i].Cells[2].Value);
                                    parametersFrompinInfoDataGridView.Values = Convert.ToString(pinInfoDataGridView.Rows[i].Cells[3].Value);
                                    parametersFrompinInfoDataGridView.Units = Convert.ToString(pinInfoDataGridView.Rows[i].Cells[4].Value);
                                }

                            }
                            catch (Exception ex) { MessageBox.Show(ex.Message); }


                            listofPinFrompinInfoDataGridView.Add(parametersFrompinInfoDataGridView);
                            i++;

                        }
                    }
                }
                eachElectricalParameter.listOfPinInfoDataGridViewParameters = listofPinFrompinInfoDataGridView;
                List<ElectricalParameters> listOfElectrialParameters = new List<ElectricalParameters>();

                listOfElectrialParameters.Add(eachElectricalParameter);


                if (electricalInterfacesComboBox.Items.Count <= Convert.ToInt32(electricalInterfacenumbTxtBox.Text) && electricalInterfacesComboBox.Items.Count != 15)
                {

                    switch (electricalInterfacesComboBox.Items.Count)
                    {
                        case 1:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 1 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information); ;
                            break;
                        case 2:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 2 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 3:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 3 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 4:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 4 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 5:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 5 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 6:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 6 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 7:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 7 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 8:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 8 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 9:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 9 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 10:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 10 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 11:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 11 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 12:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 12 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 13:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 13 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 14:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 14 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 15:
                            electricalInterfaceslistoflists.Add(listOfElectrialParameters);
                            MessageBox.Show("Electrical Interface number 15 is Created", "Electrical Interfaces of a Component", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        default:
                            break;
                    }



                }

                /*else
                {
                    MessageBox.Show("Specified Interfaces Limit has been reached, Change Number of Interfaces to add more Interfaces", " Interface Limit Reached",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }*/

            }




        }

        private void GenerateAML_Click(object sender, EventArgs e)
        {
            // Create a new Device
            var device = new MWDevice();
            
            // Read all the input fields and write them to the device data
            if (communicationTechnologyTxtBx.SelectedItem != null)
            {
                device.deviceType = communicationTechnologyTxtBx.SelectedItem.ToString();
            }
            else
            {
                device.deviceType = "";
            }

            // Check if there was an input in this field, if so: try to convert it to integer
            if (!String.IsNullOrWhiteSpace(vendorIDTxtBx.Text))
            {
                try { device.vendorID = Convert.ToInt32(vendorIDTxtBx.Text); } catch (Exception) { MessageBox.Show("Warning: Vendor ID must be number.\n please correct input"); }
            }
            // Check if there was an input in this field, if so: try to convert it to integer
            if (!String.IsNullOrWhiteSpace(productRangeTxtBx.Text))
            {
                try { device.deviceID = Convert.ToInt32(deviceIDTxtBx.Text); } catch (Exception) { MessageBox.Show("Device ID is in an invalid format (Expected only numbers)! Ignoring!"); }
            }
            device.vendorName = vendorNameTxtBx.Text;
            device.vendorHomepage = vendorHomepageTxtBx.Text;
            device.deviceName = deviceNameTxtBx.Text;
            device.productRange = productRangeTxtBx.Text;
            device.productNumber = productNumberTxtBx.Text;
            device.orderNumber = orderNumberTxtBx.Text;
            device.productText = producTxtBx.Text;
            device.harwareRelease = hardwareReleaseTxtBx.Text;
            device.softwareRelease = softwareReleaseTxtBx.Text;
            device.productFamily = productFamilyTxtBx.Text;
            device.productGroup = productGroupTxtBx.Text;
            device.semanticsystem = semanticSystemTextBox.Text;
            device.ipProtection = ipProtectionTxtBx.Text;

            if (!String.IsNullOrWhiteSpace(opTempMaxTxtBx.Text))
            {
                try { device.minTemperature = Convert.ToDouble(opTempMinTxtBx.Text); } catch (Exception) { device.minTemperature = Double.NaN; MessageBox.Show("Min Temperature is in an invalid format (Expected only numbers)! Ignoring!"); }
            }
            if (!String.IsNullOrWhiteSpace(opTempMaxTxtBx.Text))
            {
                try { device.maxTemperature = Convert.ToDouble(opTempMaxTxtBx.Text); } catch (Exception) { device.maxTemperature = Double.NaN; MessageBox.Show("Max Temperature is in an invalid format (Expected only numbers)! Ignoring!"); }
            }
           
            device.ElectricalInterfaceInstances = electricalInterfaceslistoflists;

            // storing user defined values of Attachebles data grid view in to list 

            device.dataGridAttachablesParametrsList = new List<AttachablesDataGridViewParameters>();
            if (attachablesInfoDataGridView != null)
            {
                int i = 0;
                int j = attachablesInfoDataGridView.Rows.Count - 1;
                if (i <= 0)
                {
                    while (i < j)
                    {

                        AttachablesDataGridViewParameters parametersFromAttachablesDataGrid = new AttachablesDataGridViewParameters();

                        try
                        {
                            parametersFromAttachablesDataGrid.ElementName = Convert.ToString(attachablesInfoDataGridView.Rows[i].Cells[0].Value);
                            parametersFromAttachablesDataGrid.FilePath = Convert.ToString(attachablesInfoDataGridView.Rows[i].Cells[1].Value);
                            parametersFromAttachablesDataGrid.AddToFile = Convert.ToString(attachablesInfoDataGridView.Rows[i].Cells[2].Value);
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning); }

                        device.dataGridAttachablesParametrsList.Add(parametersFromAttachablesDataGrid);
                        i++;

                    }
                }
            }
            if (generateAML.Text == "Update AML File")
            {
               
                generateAML.Text = "Save AML FiLe";
            }
           

            // Pass the device to the controller
            string result = mWController.CreateDeviceOnClick(device, isEditing);


            clear();
            // Display the result
            if (result != null)
            {
                // Display error Dialog
                MessageBox.Show(result);
            }
           
            // Assigning values and parameters in "Identification data grid" to properties given in class "DatatableParametersCarrier" in MWDevice


        }
        public void clear()
        {
            vendorNameTxtBx.Text = "";
            deviceNameTxtBx.Text = "";
            deviceIDTxtBx.Text = "";
            productRangeTxtBx.Text = "";
            vendorIDTxtBx.Text = "";
            hardwareReleaseTxtBx.Text = "";
            softwareReleaseTxtBx.Text = "";
            productNumberTxtBx.Text = "";
            orderNumberTxtBx.Text = "";
            vendorHomepageTxtBx.Text = "";
            communicationTechnologyTxtBx.Text = "";
            ipProtectionTxtBx.Text = "";
            opTempMaxTxtBx.Text = "";
            opTempMinTxtBx.Text = "";
            producTxtBx.Text = "";
            productGroupTxtBx.Text = "";
            deviceNameTxtBx.Text = "";
            
            productFamilyTxtBx.Text = "";



            // All "Data Grid Views" gets cleared
            dataGridViewManufacturerDetails.Rows.Clear();
            dataGridViewProductDetails.Rows.Clear();
            dataGridViewProductOrderDetails.Rows.Clear();
            dataGridViewProductPriceDetails.Rows.Clear();
            identificationDataGridView.Rows.Clear();
            semanticSystemTextBox.Text = "";

            // clear tree view
            dataHierarchyTreeView.Nodes.Clear();

            attachablesInfoDataGridView.Rows.Clear();
            electricalInterfacesCollectionDataGridView.Rows.Clear();
            elecInterAttDataGridView.Rows.Clear();
            genericInformationDataGridView.Rows.Clear();
            gwnericparametersAttrDataGridView.Rows.Clear();

            // reset switch case to case 1 again.
            //electricalInterfacesComboBox.Items.Clear();

        }



        private void IdentificationDataBtn_Click(object sender, EventArgs e)
        {
            
        }



        private void CommercialDataBtn_Click(object sender, EventArgs e)
        {
           
        }

        private void ClearSemanticSystemBtn_Click(object sender, EventArgs e)
        {
            dataGridViewManufacturerDetails.Rows.Clear();
            dataGridViewProductDetails.Rows.Clear();
            dataGridViewProductOrderDetails.Rows.Clear();
            dataGridViewProductPriceDetails.Rows.Clear();
            identificationDataGridView.Rows.Clear();
            semanticSystemTextBox.Text = "";
            dataHierarchyTreeView.Nodes.Clear();

            // hide ref semantic buttons 

        }

        private void VendorNameRefSemanticBtn_Click(object sender, EventArgs e)
        {
            if (semanticSystemTextBox.Text == "IEC-CDD")
            {
                AMC.SemanticSystemOpener(vendorNameRefSemanticBtn.Text);
            }

        }

        private void VendorLogoDisplayBtn_Click(object sender, EventArgs e)
        {

        }

        private void DeviceNameRefSemanticBtn_Click(object sender, EventArgs e)
        {
            if (semanticSystemTextBox.Text == "IEC-CDD")
            {
                AMC.SemanticSystemOpener(deviceNameRefSemanticBtn.Text);
            }
        }

        private void ProductFamilyRefSemanticBtn_Click(object sender, EventArgs e)
        {
            if (semanticSystemTextBox.Text == "IEC-CDD")
            {
                AMC.SemanticSystemOpener(productFamilyRefSemanticBtn.Text);
            }
        }

        private void VendorHomepageRefSemanticBtn_Click(object sender, EventArgs e)
        {
            if (semanticSystemTextBox.Text == "IEC-CDD")
            {
                AMC.SemanticSystemOpener(vendorHomepageRefSemanticBtn.Text);
            }
        }

        private void dataHierarchyTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {

            if (dataHierarchyTreeView.SelectedNode.Text == "Device Data")
            {
                dataTabControl.SelectTab("DeviceDataTabPage");
            }
            if (dataHierarchyTreeView.SelectedNode.Text == "Field Attachables")
            {
                dataTabControl.SelectTab("DocsTabPage");

            }
            if (dataHierarchyTreeView.SelectedNode.Text == "Add")
            {
                dataTabControl.SelectTab("DocsTabPage");
                AMC.WindowSizeChanger(addPicturesandDocsPanel);
            }

            if (dataHierarchyTreeView.SelectedNode.Text == "Interfaces")
            {
                dataTabControl.SelectTab("Interface");
                
            }
            if (dataHierarchyTreeView.SelectedNode.Text == "Electrical Interfaces")
            {
                dataTabControl.SelectTab("Interface");
                AMC.WindowSizeChanger(electricalInterfacesPanel);
            }
            if (dataHierarchyTreeView.SelectedNode.Text == "Generic Data")
            {
                dataTabControl.SelectTab("genericData");
               
            }
            dataHierarchyTreeView.SelectedNode = null;
        }



        private void electricalInterfaceBtn_Click_1(object sender, EventArgs e)
        {
            AMC.WindowSizeChanger(electricalInterfacePanel, electricalInterfaceBtn);
        }

        private void addElectricalInterfacesBtn_Click(object sender, EventArgs e)
        {


        }

        private void addElectricalDataBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (connectorCombBox.SelectedItem != null && connectorTypeCombBox.SelectedItem != null && electricalDataDataGridView.Rows.Count <= 1)
                {
                    Datatables datatables = new Datatables();

                    DataTable datatableheadersIRDIED = datatables.Parametersdatatable();

                    DictionaryIRDI DIRDI = new DictionaryIRDI();

                    Dictionary<int, Parameters> IRDIED = DIRDI.IRDIElectricalData();

                    datatables.CreateDataTableWith4Columns(IRDIED, datatableheadersIRDIED, electricalDataDataGridView);


                }
                else
                {
                    MessageBox.Show(" 'Select Connector from Connector Combo Box' or 'Select Connector Type from Connector Type combo box' ", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception)
            {


            }



        }
        private void clearElectricalDataBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (electricalDataDataGridView.CurrentCell != null)
                {
                    int rowIndex = electricalDataDataGridView.CurrentCell.RowIndex;
                    electricalDataDataGridView.Rows.RemoveAt(rowIndex);
                }

            }
            catch (Exception) { }
        }

        private void addPinsBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (numbOfPinsTxtBox.Text != null && connectorCodeCombBox.SelectedItem != null && pinInfoDataGridView.Rows.Count <= 1)
                {

                    int countofpins = 0;
                    string enteredvalue = numbOfPinsTxtBox.Text;
                    int convertedtonumber = Convert.ToInt32(enteredvalue);
                    for (int i = 0; i < convertedtonumber; i++)
                    {
                        pinInfoDataGridView.Rows.Add();
                        pinInfoDataGridView.Rows[countofpins + i].Cells[0].Value = (1 + i).ToString();
                    }
                    numbOfPinsTxtBox.Text = null;
                }
                else
                {
                    MessageBox.Show("Missing Number of Pins or Connector Code", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            catch (Exception)
            {

            }
        }

        private void clearPinsBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (pinInfoDataGridView.CurrentCell != null)
                {
                    int rowIndex = pinInfoDataGridView.CurrentCell.RowIndex;
                    pinInfoDataGridView.Rows.RemoveAt(rowIndex);
                }

            }
            catch (Exception) { }
        }

        private void addbtn_Click(object sender, EventArgs e)
        {
            AMC.WindowSizeChanger(addPicturesandDocsPanel, addBtn);
        }

        private void addRole_Click(object sender, EventArgs e)
        {

            if (automationMLRoleCmbBx.Text != null && attachablesInfoDataGridView.Rows.Count > 0)
            {

                string searchValue = automationMLRoleCmbBx.Text;
                string mid = "_";

                int result = 1;
                string end = Convert.ToString(result);
                string final = searchValue + mid + end;

                List<string> listofstrings = new List<string>();
                List<int> listofintegers = new List<int>();

                int i;
                int result3;
                string ultrafinal = String.Empty;
                attachablesInfoDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                foreach (DataGridViewRow row in attachablesInfoDataGridView.Rows)
                {
                    try
                    {
                        if (row.Cells[0].Value == null)
                        {
                            AMLfileLabel.Text = automationMLRoleCmbBx.Text;
                            AMLURLLabel.Text = automationMLRoleCmbBx.Text;
                        }
                    }
                    catch (Exception) {}

                }
                foreach (DataGridViewRow eachrow in attachablesInfoDataGridView.Rows)
                {
                    try
                    {
                        if (eachrow.Cells[0].Value.Equals(searchValue))
                        {

                            foreach (DataGridViewRow eachrow3 in attachablesInfoDataGridView.Rows)
                            {
                                try
                                {
                                    if (eachrow3.Cells[0].Value != null && eachrow3.Cells[0].Value.ToString().Contains(searchValue))
                                    {
                                        string eachstringindataGridView = eachrow3.Cells[0].Value.ToString();
                                        listofstrings.Add(eachstringindataGridView);
                                    }

                                }
                                catch (Exception){ throw;}
                            }
                            foreach (string eachstring in listofstrings)
                            {
                                bool success = int.TryParse(new string(eachstring.SkipWhile(x => !char.IsDigit(x)).TakeWhile(x => char.IsDigit(x)).ToArray()), out i);
                                if (success == false)
                                {
                                    i = 0;
                                }
                                listofintegers.Add(i);
                            }

                            result3 = listofintegers.Max();

                            ultrafinal = searchValue + mid + Convert.ToString(++result3);

                            AMLfileLabel.Text = ultrafinal;
                            AMLURLLabel.Text = ultrafinal;

                        }

                    }
                    catch (Exception){}
                }
            }
            if (automationMLRoleCmbBx.SelectedItem == null || automationMLRoleCmbBx.SelectedItem != null)
            {
                automationMLRoleCmbBx.DroppedDown = true;
                panelSelectFile.Size = panelSelectFile.MaximumSize;
            }
            

        }

       

        private void selectFileBtn_Click(object sender, EventArgs e)
        {
            if (AMLfileLabel.Text != "")
            {

                string filename = AMC.OpenFileDialog(selectedFileLocationTxtBx);
                if (selectedFileLocationTxtBx.Text != "")
                {/*
                    if (MessageBox.Show("Add selected file to AMLX-Package", "Caution", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                    {*/
                        var index = attachablesInfoDataGridView.Rows.Add();
                        attachablesInfoDataGridView.Rows[index].Cells["ElementName"].Value = AMLfileLabel.Text;
                        attachablesInfoDataGridView.Rows[index].Cells["FilePath"].Value = selectedFileLocationTxtBx.Text;
                        
                        selectedFileLocationTxtBx.Text = "";
                        AMLfileLabel.Text = "";
                        AMLURLLabel.Text = "";
                        panelSelectFile.Size = panelSelectFile.MinimumSize;
                    //}

                }

            }

            else
            {
                MessageBox.Show("Select AutomationML Role type from the combo box and Click Add button.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void selectURLBtn_Click(object sender, EventArgs e)
        {
          
            if (AMLURLLabel.Text != "")
            {
                if (selectedFileURLTextBox.Text != "" )
                {
                   /* if (MessageBox.Show("Add selected file to AML-File", "Caution", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
                    {*/
                        var index = attachablesInfoDataGridView.Rows.Add();
                        attachablesInfoDataGridView.Rows[index].Cells["ElementName"].Value = AMLURLLabel.Text;
                        attachablesInfoDataGridView.Rows[index].Cells["FilePath"].Value = selectedFileURLTextBox.Text;
                        AMLURLLabel.Text = "";
                        selectedFileURLTextBox.Text = "";
                        panelSelectFile.Size = panelSelectFile.MinimumSize;
                    //}
                }
               
            }
            else
            {
                MessageBox.Show("Select AutomationML Role type from the combo box and Click Add button.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void clearSelectedRowBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (attachablesInfoDataGridView.CurrentCell != null)
                {
                    int rowIndex = attachablesInfoDataGridView.CurrentCell.RowIndex;
                    attachablesInfoDataGridView.Rows.RemoveAt(rowIndex);
                }

            }
            catch (Exception) { }

        }

       

        private void clearAllElectricalDataBtn_Click(object sender, EventArgs e)
        {
            electricalDataDataGridView.Rows.Clear();
        }

        private void clearAllPinsInfoBtn_Click(object sender, EventArgs e)
        {
            pinInfoDataGridView.Rows.Clear();
            numbOfPinsTxtBox.Text = "";
            connectorCodeCombBox.SelectedItem = "";
        }

        private void deleteElectricalInterfaceBtn_Click(object sender, EventArgs e)
        {
            try
            {
                electricalInterfaceslistoflists.RemoveAt(electricalInterfacesComboBox.Items.IndexOf(electricalInterfacesComboBox.SelectedItem));
            }
            catch (Exception)
            {


            }

            electricalInterfacesComboBox.Items.Remove(electricalInterfacesComboBox.SelectedItem);
            MessageBox.Show("Electrical Interface is removed", "Deleted Interface", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        Dictionary<string, List<ClassOfListsFromReferencefile>> dictionaryofInterfaceClassattributes = new Dictionary<string, List<ClassOfListsFromReferencefile>>();
        Dictionary<string, List<ClassOfListsFromReferencefile>> dictionaryofRoleClassattributes = new Dictionary<string, List<ClassOfListsFromReferencefile>>();
        Dictionary<string, List<ClassOfListsFromReferencefile>> dictionaryofExternalInterfaceattributes = new Dictionary<string, List<ClassOfListsFromReferencefile>>();

        // this below dictionary stores each attribute values from electrical Interaface Data grid View to the current Hierarchy Treeview by a key.
        Dictionary<string, List<ElectricalInterfaceParameters>> dictofElectricalInterfaceParametrs = new Dictionary<string, List<ElectricalInterfaceParameters>>();
        private void selectAMLFileBtn_Click(object sender, EventArgs e)
        {

            searchAMLLibraryFile.dictionaryofInterfaceClassattributes = new Dictionary<string, List<ClassOfListsFromReferencefile>>();

            searchAMLLibraryFile.dictionaryofRoleClassattributes = new Dictionary<string, List<ClassOfListsFromReferencefile>>();
            searchAMLLibraryFile.dictionaryofExternalInterfaceattributes = new Dictionary<string, List<ClassOfListsFromReferencefile>>();
            searchAMLLibraryFile.dictionaryForInterfaceClassInstancesAttributes = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();


            treeViewRoleClassLib.Nodes.Clear();
            treeViewInterfaceClassLib.Nodes.Clear();

            CAEXDocument document = null;
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "AML Files(*.aml; *.amlx;*.xml;*.AML )|*.aml; *.amlx;*.xml;*.AML;";

            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string file = open.FileName;
                    FileInfo fileInfo = new FileInfo(file);
                    string objectName = fileInfo.Name;
                    string filetype = null;
                    if (( filetype = Convert.ToString(Path.GetExtension(open.FileName))) == ".amlx")
                    {
                        // Load the amlx container from the given filepath
                        AutomationMLContainer amlx = new AutomationMLContainer(file);

                        // Get the root path -> main .aml file
                        IEnumerable<PackagePart> rootParts = amlx.GetPartsByRelationShipType(AutomationMLContainer.RelationshipType.Root);

                        // We expect the aml to only have one root part
                        if (rootParts.First() != null)
                        {
                            
                            PackagePart part = rootParts.First();

                            // load the aml file as an CAEX document
                            document = CAEXDocument.LoadFromStream(part.GetStream());


                            // Iterate over all SystemUnitClassLibs and SystemUnitClasses and scan if it matches our format
                            // since we expect only one device per aml(x) file, return after on is found
                        }
                    }
                    if ((filetype = Convert.ToString(Path.GetExtension(open.FileName))) == ".aml" || (filetype = Convert.ToString(Path.GetExtension(open.FileName))) == ".xml")
                    {
                        document = CAEXDocument.LoadFromFile(file);
                    }


                    string referencedClassName = "";
                    foreach (var classLibType in document.CAEXFile.RoleClassLib)
                     {

                        TreeNode libNode = treeViewRoleClassLib.Nodes.Add(classLibType.ToString(), classLibType.ToString(),0) ;
                        
                        
                         foreach (var classType in classLibType.RoleClass)
                         {
                            TreeNode roleNode;

                            if (classType.ReferencedClassName != "")
                            {
                                 referencedClassName = classType.ReferencedClassName;
                                 roleNode = libNode.Nodes.Add(classType.ToString(), classType.ToString() + "{"+"Class:" + "  " + referencedClassName + "}", 1);
                            }
                            else
                            {
                                 roleNode = libNode.Nodes.Add(classType.ToString(), classType.ToString() , 1);
                            }
                            
                            searchAMLLibraryFile.CheckForAttributes(classType);

                            if (classType.ExternalInterface.Exists)
                            {
                                foreach (var externalinterface in classType.ExternalInterface)
                                {
                                    TreeNode externalinterfacenode;
                                   
                                    if (externalinterface.BaseClass != null)
                                    {
                                        referencedClassName = externalinterface.BaseClass.ToString();
                                        externalinterfacenode = roleNode.Nodes.Add(externalinterface.ToString(), externalinterface.ToString() + "{" + "Class:" + "  " + referencedClassName + "}", 2);
                                    }
                                    else
                                    {
                                        externalinterfacenode = roleNode.Nodes.Add(externalinterface.ToString(), externalinterface.ToString(), 2);
                                    }

                                    searchAMLLibraryFile.CheckForAttributes(externalinterface);

                                    searchAMLLibraryFile.PrintExternalInterfaceNodes(externalinterfacenode, externalinterface);
                                }
                                
                            }
                            searchAMLLibraryFile.PrintNodesRecursiveInRoleClassLib(roleNode, classType);
                         }

                     }
                   
                    foreach (var classLibType in document.CAEXFile.InterfaceClassLib)
                    {
                        TreeNode libNode = treeViewInterfaceClassLib.Nodes.Add(classLibType.ToString(), classLibType.ToString(),0);
                        
                       

                        foreach (var classType in classLibType.InterfaceClass) 
                        {
                            TreeNode interfaceclassNode;
                            if (classType.ReferencedClassName != "")
                            {
                                 referencedClassName = classType.ReferencedClassName;
                                interfaceclassNode = libNode.Nodes.Add(classType.ToString(), classType.ToString() + "{" + "Class:" + "  " + referencedClassName + "}", 1);
                                /*searchAMLLibraryFile.CheckForAttributesOfReferencedClassName(classType);*/
                                searchAMLLibraryFile.SearchForReferencedClassName(document, referencedClassName, classType);
                            }
                            else
                            {
                                interfaceclassNode = libNode.Nodes.Add(classType.ToString(), classType.ToString(), 1);
                            }

                            searchAMLLibraryFile.CheckForAttributes(classType);

                            if (classType.ExternalInterface.Exists)
                            {
                                foreach (var externalinterface in classType.ExternalInterface)
                                {
                                    TreeNode externalinterfacenode;
                                   
                                    if (externalinterface.BaseClass != null)
                                    {
                                        referencedClassName = externalinterface.BaseClass.ToString();
                                        externalinterfacenode = interfaceclassNode.Nodes.Add(externalinterface.ToString(), externalinterface.ToString() + "{" + "Class:" + "  " + referencedClassName + "}", 2);
                                    }
                                    else
                                    {
                                        externalinterfacenode = interfaceclassNode.Nodes.Add(externalinterface.ToString(), externalinterface.ToString(), 2);
                                    }
                                     
                                    searchAMLLibraryFile.CheckForAttributes(externalinterface);
                                    searchAMLLibraryFile.PrintExternalInterfaceNodes(externalinterfacenode, externalinterface);
                                }
                            }
                            searchAMLLibraryFile.PrintNodesRecursiveInInterfaceClassLib(document,interfaceclassNode, classType, referencedClassName);
                        }
                        
                    }
                    /*foreach (var classLibType in document.CAEXFile.InterfaceClassLib)
                    {
                        foreach (var classType in classLibType.InterfaceClass)
                        {
                            if (classType.ReferencedClassName != "")
                            {
                               
                            }
                        }

                    }*/
                }


                catch (Exception)
                {
                    throw; 
                   // MessageBox.Show("Missing names of attributes or Same atrribute sequence is repeated in the given file","Missing Names", MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);
                }

            }
        }
     
        /// <summary>
        /// Drag and drop events of "AutomationML Interface Treeview" and "AutomationML Interface treeview" in Interfaces
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
       
      

        private void treeViewRoleClassLib_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                TreeNode targetNode = treeViewRoleClassLib.SelectedNode;
                targetNode.SelectedImageIndex = targetNode.ImageIndex;
            }
            catch (Exception){}
        }

        private void treeViewInterfaceClassLib_MouseDown(object sender, MouseEventArgs e)
        {
              
            //this.treeViewInterfaceClassLib.MouseDown += new MouseEventHandler(this.tree_MouseDown);
              
        }

        private void treeViewInterfaceClassLib_DragOver(object sender, DragEventArgs e)
        {
            // this.treeViewInterfaceClassLib.DragOver += new DragEventHandler(this.tree_DragOver);

            // Retrieve the client coordinates of the mouse position.
            Point targetPoint = treeViewInterfaceClassLib.PointToClient(new Point(e.X, e.Y));

            // Select the node at the mouse position.
            treeViewInterfaceClassLib.SelectedNode = treeViewInterfaceClassLib.GetNodeAt(targetPoint);
        }

        private void treeViewInterfaceClassLib_DragDrop(object sender, DragEventArgs e)
        {
           
        }


        private void buttonElectricalInterface_Click(object sender, EventArgs e)
        {
            AMC.WindowSizeChanger(panelElectricalInterface,buttonElectricalInterface) ;
        }

        private void DeleteInterfaceBtn_Click(object sender, EventArgs e)
        {
            // this button is on the AML imported Interface Hierarchy tree view.
            dictofElectricalInterfaceParametrs.Clear();
            TreeNode sourceNode = treeViewInterfaceClassLib.SelectedNode;
            if (treeViewImportedInterfaceHierarchy.SelectedNode.Text != "AML" )
            {
                try
                {
                   if (treeViewImportedInterfaceHierarchy.SelectedNode.Text == sourceNode.Text)
                   {
                    treeViewImportedInterfaceHierarchy.Nodes.Remove(treeViewImportedInterfaceHierarchy.SelectedNode);
                    treeViewImportedInterfaceHierarchy.ImageList = imageList2;
                    TreeNode newNode = new TreeNode("AML");
                    treeViewImportedInterfaceHierarchy.Nodes.Add(newNode);

                   }
                    if (treeViewImportedInterfaceHierarchy.SelectedNode.Text != sourceNode.Text)
                    {
                        MessageBox.Show("Library Hierarchy cannot be deleted. Select Parent node to delete whole Hierarchy and import new hierachy", "Child nodes cannot be deleted", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    }
                }
                    catch (Exception){}
                //treeViewImportedInterfaceHierarchy.Nodes.Remove(treeViewImportedInterfaceHierarchy.SelectedNode);

            }
            
        }

        private void treeViewImportedInterfaceHierarchy_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void treeViewImportedInterfaceHierarchy_MouseDown(object sender, MouseEventArgs e)
        {
           // this.treeViewImportedInterfaceHierarchy.MouseDown += new MouseEventHandler(this.tree_MouseDown);

        }

        private void treeViewImportedInterfaceHierarchy_DragOver(object sender, DragEventArgs e)
        {
           // this.treeViewImportedInterfaceHierarchy.DragOver += new DragEventHandler(this.tree_DragOver);
        }

        private void treeViewImportedInterfaceHierarchy_DragDrop(object sender, DragEventArgs e)
        {
            //this.treeViewImportedInterfaceHierarchy.DragDrop += new DragEventHandler(this.tree_DragDrop);
        }

       


        private void treeViewInterfaceClassLib_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                treeViewInterfaceClassLib.SelectedNode = e.Node;
                e.Node.ContextMenuStrip = contextMenuStripforInterfaceClassLib;
            }
        }

        private void asInterfaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            try
            {
                TreeNode sourceNode = treeViewInterfaceClassLib.SelectedNode;

                int num = electricalInterfacesCollectionDataGridView.Rows.Add();
                List<string> listofSerialNumbers = new List<string>();
                List<int> listofFinalSerialNumber = new List<int>();
                string number = "";
                int finalNumber = 0;
                int ultimatenumber = 0;
                if (electricalInterfacesCollectionDataGridView.Rows.Count > 2)
                {
                    foreach (DataGridViewRow row in electricalInterfacesCollectionDataGridView.Rows)
                    {
                        if (row.Cells[0].Value == null)
                        {
                            number = "0";
                            listofSerialNumbers.Add(number);
                        }
                        if (row.Cells[0].Value != null)
                        {
                            number = row.Cells[0].Value.ToString();
                            listofSerialNumbers.Add(number);
                        }
                    }
                    foreach (string str in listofSerialNumbers)
                    {
                        finalNumber = Convert.ToInt32(str);
                        listofFinalSerialNumber.Add(finalNumber);
                    }
                    ultimatenumber = listofFinalSerialNumber.Max();
                    electricalInterfacesCollectionDataGridView.Rows[num].Cells[0].Value = ++ultimatenumber;
                }
                else 
                {
                    electricalInterfacesCollectionDataGridView.Rows[num].Cells[0].Value = 1;
                }
               
                electricalInterfacesCollectionDataGridView.Rows[num].Cells[1].Value = sourceNode.Text;
               
                /* TreeNode targetNode = treeViewImportedInterfaceHierarchy.SelectedNode;

                 TreeNode targetNode2 = treeViewCurrentHierarchy.SelectedNode;

                 foreach (TreeNode n in sourceNode.Nodes)
                 {
                     targetNode.Nodes.Add((TreeNode)n.Clone());

                     targetNode2.Nodes.Add((TreeNode)n.Clone());
                 }
                 targetNode.Text = sourceNode.Text;
                 targetNode.ImageIndex = sourceNode.ImageIndex;
                 targetNode.SelectedImageIndex = targetNode.ImageIndex;

                 targetNode2.Text = sourceNode.Text;
                 targetNode2.ImageIndex = sourceNode.ImageIndex;
                 targetNode2.SelectedImageIndex = targetNode2.ImageIndex;*/
            }
            catch (Exception)
            {
                MessageBox.Show("A whole Interface Library cannot be added ","Select Parent Node to add Inetrface",MessageBoxButtons.OKCancel,MessageBoxIcon.Information);
            }
        }

        private void treeViewImportedInterfaceHierarchy_MouseClick_1(object sender, MouseEventArgs e)
        {
           
        }
        
        private void treeViewImportedInterfaceHierarchy_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var AutomationMLDataTables = new AutomationMLDataTables();
          
            try
            {
                //treeViewAttributeHierarchy.Nodes.Clear();
                dataGridViewElectricalAttributes.Rows.Clear();
               
                TreeNode targetNode = treeViewImportedInterfaceHierarchy.SelectedNode;
               
                targetNode.SelectedImageIndex = targetNode.ImageIndex;
                if (e.Button == MouseButtons.Left)
                {
                    if (targetNode.ImageIndex == 1)
                    {
                        interfaceClassTextBox.Text = targetNode.Text;
                        externalInterfaceTxtBox.Text = null;
                        foreach (KeyValuePair<string, List<ClassOfListsFromReferencefile>> pair in dictionaryofInterfaceClassattributes)
                        {
                            if (pair.Key.Contains(targetNode.Text))
                            {
                                //treeViewAttributeHierarchy.Nodes.Add(pair.Key.ToString());
                                
                                DataTable AMLDataTable = AutomationMLDataTables.AMLAttributeParameters();
                                AutomationMLDataTables.CreateDataTableWithColumns(AMLDataTable, dataGridViewElectricalAttributes,pair) ;
                            }

                        }
                    }
                    if (targetNode.ImageIndex == 2)
                    {
                       
                        externalInterfaceTxtBox.Text = targetNode.Text;
                        if (targetNode.Parent.ImageIndex == 2)
                        {
                            SearchForRightParentNode(targetNode, interfaceClassTextBox);
                        }
                        else
                        {
                            interfaceClassTextBox.Text = targetNode.Parent.Text;
                        }
                        foreach (KeyValuePair<string, List<ClassOfListsFromReferencefile>> pair in dictionaryofExternalInterfaceattributes)
                        {
                            if (pair.Key.Contains(targetNode.Parent.Text +targetNode.Text))
                            {
                                //treeViewAttributeHierarchy.Nodes.Add(pair.Key.ToString());

                                DataTable AMLDataTable = AutomationMLDataTables.AMLAttributeParameters();
                                AutomationMLDataTables.CreateDataTableWithColumns(AMLDataTable, dataGridViewElectricalAttributes, pair);
                                
                            }

                        }
                    }


                }
            }
            catch (Exception)
            {

                
            }
            
        }
        public void SearchForRightParentNode(TreeNode treeNode, ToolStripTextBox textBox)
        {
            TreeNode targetNode = treeNode.Parent;
            if (targetNode.ImageIndex == 2)
            {
                SearchForRightParentNode(targetNode,textBox);
            }
            if (targetNode.ImageIndex == 1)
            {
                textBox.Text = targetNode.Text;
            }
        }
       
        private void deleteAttributeHierarchyBtn_Click(object sender, EventArgs e)
        {
            // this button is on the current hierarchy tree view.
            dictofElectricalInterfaceParametrs.Clear();
            TreeNode sourceNode = treeViewInterfaceClassLib.SelectedNode;
            if (treeViewCurrentHierarchy.SelectedNode.Text != "AML")
            {
                try
                {
                    if (treeViewCurrentHierarchy.SelectedNode.Text == sourceNode.Text)
                    {
                        treeViewCurrentHierarchy.Nodes.Remove(treeViewCurrentHierarchy.SelectedNode);
                        treeViewCurrentHierarchy.ImageList = imageList2;
                        TreeNode newNode = new TreeNode("AML");
                        treeViewCurrentHierarchy.Nodes.Add(newNode);

                    }
                    if (treeViewCurrentHierarchy.SelectedNode.Text != sourceNode.Text)
                    {
                        MessageBox.Show("Library Hierarchy cannot be deleted. Select Parent node to delete whole Hierarchy and import new hierachy","Child nodes cannot be deleted", MessageBoxButtons.OKCancel,MessageBoxIcon.Information);
                    }
                }
                catch (Exception) { }
               

            }
        }

        private void treeViewImportedInterfaceHierarchy_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                TreeNode targetNode = treeViewImportedInterfaceHierarchy.SelectedNode;
                targetNode.SelectedImageIndex = targetNode.ImageIndex;
               
            }
            catch (Exception){}
            
        }

        private void treeViewInterfaceClassLib_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                TreeNode targetNode = treeViewInterfaceClassLib.SelectedNode;
                targetNode.SelectedImageIndex = targetNode.ImageIndex;
            }
            catch (Exception) {}
        }

        private void treeViewCurrentHierarchy_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                TreeNode targetNode = treeViewCurrentHierarchy.SelectedNode;
                targetNode.SelectedImageIndex = targetNode.ImageIndex;
            }
            catch (Exception) { }
        }
      
        private void saveDataFromElectricalInterfaceDataGridView_Click(object sender, EventArgs e)
        {
            List<ElectricalInterfaceParameters> listofElectricalInterfaceParameters = new List<ElectricalInterfaceParameters>();
            if (dataGridViewElectricalAttributes != null)
            {

                int i = 0;
                int j = dataGridViewElectricalAttributes.Rows.Count;
                if (i <= 0)
                {
                    while (i < j)
                    {
                        ElectricalInterfaceParameters parametersFromElectricalInterfaceDataGridView = new ElectricalInterfaceParameters();
                        try
                        {
                            parametersFromElectricalInterfaceDataGridView.AttributeName = Convert.ToString(dataGridViewElectricalAttributes.Rows[i].Cells[0].Value);
                            parametersFromElectricalInterfaceDataGridView.Values = Convert.ToString(dataGridViewElectricalAttributes.Rows[i].Cells[1].Value);
                            parametersFromElectricalInterfaceDataGridView.Default = Convert.ToString(dataGridViewElectricalAttributes.Rows[i].Cells[2].Value);
                            parametersFromElectricalInterfaceDataGridView.Units = Convert.ToString(dataGridViewElectricalAttributes.Rows[i].Cells[3].Value);
                            parametersFromElectricalInterfaceDataGridView.Default = Convert.ToString(dataGridViewElectricalAttributes.Rows[i].Cells[4].Value);
                            parametersFromElectricalInterfaceDataGridView.Semantic = Convert.ToString(dataGridViewElectricalAttributes.Rows[i].Cells[5].Value);
                            parametersFromElectricalInterfaceDataGridView.Reference = Convert.ToString(dataGridViewElectricalAttributes.Rows[i].Cells[6].Value);
                           
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }


                        listofElectricalInterfaceParameters.Add(parametersFromElectricalInterfaceDataGridView);
                        i++;

                    }
                }

               
            }
            if (dictofElectricalInterfaceParametrs.ContainsKey(Convert.ToString(interfaceClassTextBox.Text.ToString() + externalInterfaceTxtBox.Text.ToString())))
            {
                dictofElectricalInterfaceParametrs.Remove(Convert.ToString(interfaceClassTextBox.Text.ToString() + externalInterfaceTxtBox.Text.ToString()));
            }
            dictofElectricalInterfaceParametrs.Add(interfaceClassTextBox.Text.ToString()+ externalInterfaceTxtBox.Text.ToString(), listofElectricalInterfaceParameters);
            dataGridViewElectricalAttributes.Rows.Clear();
        }

        private void treeViewCurrentHierarchy_MouseClick(object sender, MouseEventArgs e)
        {
            /*try
            {
                TreeNode targetNode = treeViewCurrentHierarchy.SelectedNode;
                targetNode.SelectedImageIndex = targetNode.ImageIndex;
            }
            catch (Exception) { }*/
            var AutomationMLDataTables = new AutomationMLDataTables();

            try
            {
                //treeViewAttributeHierarchy.Nodes.Clear();
                dataGridViewElectricalAttributes.Rows.Clear();

                TreeNode targetNode = treeViewCurrentHierarchy.SelectedNode;

                targetNode.SelectedImageIndex = targetNode.ImageIndex;
                if (e.Button == MouseButtons.Left)
                {
                    if (targetNode.ImageIndex == 1)
                    {
                        interfaceClassTextBox.Text = targetNode.Text;
                        externalInterfaceTxtBox.Text = null;
                        foreach (KeyValuePair<string, List<ElectricalInterfaceParameters>> pair in dictofElectricalInterfaceParametrs)
                        {
                            if (dictofElectricalInterfaceParametrs.ContainsKey(targetNode.Text))
                            {
                                //treeViewAttributeHierarchy.Nodes.Add(pair.Key.ToString());

                                
                                DataTable AMLDataTable = AutomationMLDataTables.AMLAttributeParameters();
                                AutomationMLDataTables.CreateDataTableWithColumns(AMLDataTable, dataGridViewElectricalAttributes, pair);
                               
                            }

                        }
                    }
                    if (targetNode.ImageIndex == 2)
                    {

                        externalInterfaceTxtBox.Text = targetNode.Text;
                        if (targetNode.Parent.ImageIndex == 2)
                        {
                            SearchForRightParentNode(targetNode, interfaceClassTextBox);
                        }
                        else
                        {
                            interfaceClassTextBox.Text = targetNode.Parent.Text;
                        }
                        foreach (KeyValuePair<string, List<ElectricalInterfaceParameters>> pair in dictofElectricalInterfaceParametrs)
                        {
                            if (dictofElectricalInterfaceParametrs.ContainsKey(targetNode.Parent.Text + targetNode.Text))
                            {
                                //treeViewAttributeHierarchy.Nodes.Add(pair.Key.ToString());
                                
                                DataTable AMLDataTable = AutomationMLDataTables.AMLAttributeParameters();
                               AutomationMLDataTables.CreateDataTableWithColumns(AMLDataTable, dataGridViewElectricalAttributes, pair);

                            }

                        }
                    }


                }
            }
            catch (Exception)
            {


            }
        }

       

        private void treeViewCurrentHierarchy_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                TreeNode targetNode = treeViewCurrentHierarchy.SelectedNode;
                targetNode.SelectedImageIndex = targetNode.ImageIndex;

            }
            catch (Exception) { }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (vendorNameTxtBx.Text != "")
            {
                if (MessageBox.Show("Save Current File", "Save", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    GenerateAML_Click(sender, e);
                    return;
                }
                else
                {
                    return;
                }
            }
           
            CAEXDocument document = null;
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "AML Files(*.aml; *.amlx;*.xml;*.AML )|*.aml; *.amlx;*.xml;*.AML;";
            clear();
            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string file = open.FileName;
                    FileInfo fileInfo = new FileInfo(file);
                    string objectName = fileInfo.Name;
                    

                    DataHierarchyTreeView();

                    DirectoryInfo directory = new DirectoryInfo(Path.GetDirectoryName(file));
                    
                   
                    // Load the amlx container from the given filepath
                    AutomationMLContainer amlx = new AutomationMLContainer(file);
                         amlx.ExtractAllFiles(Path.GetDirectoryName(file));
                        // Get the root path -> main .aml file
                        IEnumerable<PackagePart> rootParts = amlx.GetPartsByRelationShipType(AutomationMLContainer.RelationshipType.Root);
                        
                        // We expect the aml to only have one root part
                        if (rootParts.First() != null)
                        {
                            PackagePart part = rootParts.First();

                            // load the aml file as an CAEX document
                             document = CAEXDocument.LoadFromStream(part.GetStream());
                           
                            
                            // Iterate over all SystemUnitClassLibs and SystemUnitClasses and scan if it matches our format
                            // since we expect only one device per aml(x) file, return after on is found
                        }
                        
                    
                    generateAML.Text = "Update AML File";
                    foreach (var classLibType in document.CAEXFile.SystemUnitClassLib)
                    {
                        foreach (var classType in classLibType.SystemUnitClass)
                        {
                            foreach (var internalElements in classType.InternalElement)
                            {
                                if (internalElements.Name.Equals("DeviceIdentification"))
                                {
                                    foreach (var attribute in internalElements.Attribute)
                                    {
                                        switch (attribute.Name)
                                        {
                                            case "CommunicationTechonolgy":
                                                communicationTechnologyTxtBx.SelectedItem = attribute.Value;
                                                break;
                                            case "VendorId":
                                                try
                                                {
                                                    vendorIDTxtBx.Text = Convert.ToString(attribute.Value);
                                                }catch (Exception){/*// let the value be null*/}
                                                break;
                                            case "VendorName":
                                               vendorNameTxtBx.Text = attribute.Value;
                                                break;
                                            case "DeviceId":
                                                try{deviceIDTxtBx.Text = Convert.ToString(attribute.Value);}
                                                catch (Exception)
                                                {
                                                    // let the value be null
                                                }
                                                break;
                                            case "DeviceName":
                                                deviceNameTxtBx.Text = attribute.Value;
                                                break;
                                            case "ProductRange":
                                                productRangeTxtBx.Text = attribute.Value;
                                                break;
                                            case "ProductNumber":
                                                productNumberTxtBx.Text = attribute.Value;
                                                break;
                                            case "OrderNumber":
                                                orderNumberTxtBx.Text = attribute.Value;
                                                break;
                                            case "ProductText":
                                                producTxtBx.Text = attribute.Value;
                                                break;
                                            case "IPProtection":
                                                ipProtectionTxtBx.Text = attribute.Value;
                                                break;
                                            case "OperatingTemperatureMin":
                                                try
                                                {
                                                    opTempMinTxtBx.Text = Convert.ToString(attribute.Value);
                                                }
                                                catch (Exception)
                                                {
                                                    opTempMinTxtBx.Text = "";
                                                }
                                                break;
                                            case "OperatingTemperatureMax":
                                                try
                                                {
                                                   opTempMaxTxtBx.Text = Convert.ToString(attribute.Value);
                                                }
                                                catch (Exception)
                                                {
                                                    opTempMaxTxtBx.Text = "";
                                                }
                                                break;
                                            case "VendorHomepage":
                                                vendorHomepageTxtBx.Text = attribute.Value;
                                                break;
                                            case "HardwareRelease":
                                                hardwareReleaseTxtBx.Text = attribute.Value;
                                                break;
                                            case "SoftwareRelease":
                                                softwareReleaseTxtBx.Text = attribute.Value;
                                                break;
                                            case "ProductGroup":
                                                productGroupTxtBx.Text = attribute.Value;
                                                break;
                                            case "ProductFamily":
                                                productFamilyTxtBx.Text = attribute.Value;
                                                break;
                                        }
                                    }
                                }
                                if (internalElements.Name != "ElectricalInterfaces" && internalElements.Name != "DeviceIdentification")
                                {
                                   

                                    int num = attachablesInfoDataGridView.Rows.Add();
                                    attachablesInfoDataGridView.Rows[num].Cells[0].Value = internalElements.Name;
                                    foreach (var externalInterface in internalElements.ExternalInterface)
                                    {
                                       
                                        foreach (var attribute in externalInterface.Attribute)
                                        {

                                            foreach (FileInfo fileInfo1 in directory.GetFiles())
                                            {
                                                string name = attribute.Value.ToString();
                                                if (name.Contains("%20"))
                                                {
                                                        //name.Replace("%20", " ");
                                                    name = Uri.UnescapeDataString(name);
                                                }
                                                if (name.Contains("%28") || name.Contains("%29"))
                                                {
                                                    name = Uri.UnescapeDataString(name);
                                                }
                                                if ( name.Contains(fileInfo1.ToString()))
                                                {
                                                    attachablesInfoDataGridView.Rows[num].Cells[1].Value = fileInfo1.FullName;
                                                    attachablesInfoDataGridView.Rows[num].Cells[2].Value = true;
                                                }
                                               
                                            }
                                            //attachablesInfoDataGridView.Rows[num].Cells[1].Value = attribute.Value;
                                        }
                                        
                                    }
                                }
                                if (internalElements.Name == "ElectricalInterfaces")
                                {

                                }
                            }
                        }
                        
                    }
                    amlx.Dispose();
                    
                    
                }
                catch { }
                
            }
        }

        private void fileButton_MouseHover(object sender, EventArgs e)
        {
            fileButton.ShowDropDown();
        }

        private void fileButton_ButtonClick(object sender, EventArgs e)
        {
            fileButton.ShowDropDown();
        }

        private void helpButton_ButtonClick(object sender, EventArgs e)
        {
            helpButton.ShowDropDown();
        }

        private void helpButton_MouseHover(object sender, EventArgs e)
        {
            helpButton.ShowDropDown();
        }
        OpenFileDialog openFileDialog = new OpenFileDialog();
        private void importIODDFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filetype = MWData.MWFileType.IODD;
            openFileDialog.Filter = "IODD Files (*.xml)|*.xml|All Files (*.*)|*.*";
            openFileDialog.ShowDialog();
        }

        private void importGSDFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filetype = MWData.MWFileType.GSD;
            openFileDialog.Filter = "GSDML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            openFileDialog.ShowDialog();
        }

       

        private void automationMLRoleCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (automationMLRoleCmbBx.Text != null && attachablesInfoDataGridView.Rows.Count > 0)
            {

                string searchValue = automationMLRoleCmbBx.Text;
                string mid = "_";

                int result = 1;
                string end = Convert.ToString(result);
                string final = searchValue + mid + end;

                List<string> listofstrings = new List<string>();
                List<int> listofintegers = new List<int>();

                int i;
                int result3;
                string ultrafinal = String.Empty;
                attachablesInfoDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                foreach (DataGridViewRow row in attachablesInfoDataGridView.Rows)
                {
                    try
                    {
                        if (row.Cells[0].Value == null)
                        {
                            AMLfileLabel.Text = automationMLRoleCmbBx.Text;
                            AMLURLLabel.Text = automationMLRoleCmbBx.Text;
                        }
                    }
                    catch (Exception) { }

                }
                foreach (DataGridViewRow eachrow in attachablesInfoDataGridView.Rows)
                {
                    try
                    {
                        if (eachrow.Cells[0].Value.Equals(searchValue))
                        {

                            foreach (DataGridViewRow eachrow3 in attachablesInfoDataGridView.Rows)
                            {
                                try
                                {
                                    if (eachrow3.Cells[0].Value != null && eachrow3.Cells[0].Value.ToString().Contains(searchValue))
                                    {
                                        string eachstringindataGridView = eachrow3.Cells[0].Value.ToString();
                                        listofstrings.Add(eachstringindataGridView);
                                    }

                                }
                                catch (Exception) { throw; }
                            }
                            foreach (string eachstring in listofstrings)
                            {
                                bool success = int.TryParse(new string(eachstring.SkipWhile(x => !char.IsDigit(x)).TakeWhile(x => char.IsDigit(x)).ToArray()), out i);
                                if (success == false)
                                {
                                    i = 0;
                                }
                                listofintegers.Add(i);
                            }

                            result3 = listofintegers.Max();

                            ultrafinal = searchValue + mid + Convert.ToString(++result3);

                            AMLfileLabel.Text = ultrafinal;
                            AMLURLLabel.Text = ultrafinal;

                        }

                    }
                    catch (Exception) { }
                }
            }
        }

        private void electricalInterfacesButton_Click(object sender, EventArgs e)
        {
            AMC.WindowSizeChanger(electricalInterfacesPanel, electricalInterfacesButton);

        }
       

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            dictionaryofInterfaceClassattributes = new Dictionary<string, List<ClassOfListsFromReferencefile>>();
            dictionaryofRoleClassattributes = new Dictionary<string, List<ClassOfListsFromReferencefile>>();
            dictionaryofExternalInterfaceattributes = new Dictionary<string, List<ClassOfListsFromReferencefile>>();

           
            treeViewRoleClassLib.Nodes.Clear();
            treeViewInterfaceClassLib.Nodes.Clear();

            CAEXDocument document = null;
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "AML Files(*.aml; *.amlx;*.xml;*.AML )|*.aml; *.amlx;*.xml;*.AML;";

            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string file = open.FileName;
                    FileInfo fileInfo = new FileInfo(file);
                    string objectName = fileInfo.Name;
                    string filetype = null;
                    if ((filetype = Convert.ToString(Path.GetExtension(open.FileName))) == ".amlx")
                    {
                        // Load the amlx container from the given filepath
                        AutomationMLContainer amlx = new AutomationMLContainer(file);

                        // Get the root path -> main .aml file
                        IEnumerable<PackagePart> rootParts = amlx.GetPartsByRelationShipType(AutomationMLContainer.RelationshipType.Root);

                        // We expect the aml to only have one root part
                        if (rootParts.First() != null)
                        {
                            PackagePart part = rootParts.First();

                            // load the aml file as an CAEX document
                            document = CAEXDocument.LoadFromStream(part.GetStream());


                            // Iterate over all SystemUnitClassLibs and SystemUnitClasses and scan if it matches our format
                            // since we expect only one device per aml(x) file, return after on is found
                        }
                    }
                    if ((filetype = Convert.ToString(Path.GetExtension(open.FileName))) == ".aml" || (filetype = Convert.ToString(Path.GetExtension(open.FileName))) == ".xml")
                    {
                        document = CAEXDocument.LoadFromFile(file);
                    }



                    foreach (var classLibType in document.CAEXFile.RoleClassLib)
                    {

                        TreeNode libNode = treeViewRoleClassLib.Nodes.Add(classLibType.ToString(), classLibType.ToString(), 0);
                        foreach (var classType in classLibType.RoleClass)
                        {
                            TreeNode roleNode = libNode.Nodes.Add(classType.ToString(), classType.ToString(), 1);
                            searchAMLLibraryFile.CheckForAttributes(classType);
                            if (classType.ExternalInterface.Exists)
                            {
                                foreach (var externalinterface in classType.ExternalInterface)
                                {
                                    searchAMLLibraryFile.CheckForAttributes(externalinterface);
                                    TreeNode externalinterfacenode = roleNode.Nodes.Add(externalinterface.ToString(), externalinterface.ToString(), 2);

                                    searchAMLLibraryFile.PrintExternalInterfaceNodes(externalinterfacenode, externalinterface);
                                }

                            }
                            searchAMLLibraryFile.PrintNodesRecursiveInRoleClassLib(roleNode, classType);
                        }

                    }

                    foreach (var classLibType in document.CAEXFile.InterfaceClassLib)
                    {
                        TreeNode libNode = treeViewInterfaceClassLib.Nodes.Add(classLibType.ToString(), classLibType.ToString(), 0);
                        foreach (var classType in classLibType.InterfaceClass)
                        {
                            searchAMLLibraryFile.CheckForAttributes(classType);
                            TreeNode interfaceclassNode = libNode.Nodes.Add(classType.ToString(), classType.ToString(), 1);


                            if (classType.ExternalInterface.Exists)
                            {
                                foreach (var externalinterface in classType.ExternalInterface)
                                {
                                    TreeNode externalinterfacenode = interfaceclassNode.Nodes.Add(externalinterface.ToString(), externalinterface.ToString(), 2);
                                    searchAMLLibraryFile.CheckForAttributes(externalinterface);
                                    searchAMLLibraryFile.PrintExternalInterfaceNodes(externalinterfacenode, externalinterface);
                                }
                            }
                           // searchAMLLibraryFile.PrintNodesRecursiveInInterfaceClassLib(interfaceclassNode, classType);
                        }
                    }
                }


                catch (Exception)
                {

                    MessageBox.Show("Missing names of attributes or Same atrribute sequence is repeated in the given file", "Missing Names", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }

            }
        }







        //these declerations need to happen inside of the class but outside of any methods in the class

        private object _row;

        public object row //this is a property decleration
        {
             get { return this._row; }
            private set { this._row = value; }
        }

        public bool dragging = false; //this is your global boolean

        private void treeViewInterfaceClassLib_ItemDrag(object sender, ItemDragEventArgs e)
        {
            dragging = true;
            row = new object();
            treeViewInterfaceClassLib.SelectedNode = (TreeNode)e.Item;//dragging doesn't automatically change the selected index
            row = treeViewInterfaceClassLib.SelectedNode.Name;//or whatever value you need from the node

        }


        private void electricalInterfacesCollectionDataGridView_MouseUp(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                
                int num = electricalInterfacesCollectionDataGridView.Rows.Add();
                List<string> listofSerialNumbers = new List<string>();
                List<int> listofFinalSerialNumber = new List<int>();
                string number = "";
                int finalNumber = 0;
                int ultimatenumber = 0;
                if (electricalInterfacesCollectionDataGridView.Rows.Count > 2)
                {
                    foreach (DataGridViewRow row in electricalInterfacesCollectionDataGridView.Rows)
                    {
                        if (row.Cells[0].Value == null)
                        {
                            number = "0";
                            listofSerialNumbers.Add(number);
                        }
                        if (row.Cells[0].Value != null)
                        {
                            number = row.Cells[0].Value.ToString();
                            listofSerialNumbers.Add(number);
                        }
                    }
                    foreach (string str in listofSerialNumbers)
                    {
                        finalNumber = Convert.ToInt32(str);
                        listofFinalSerialNumber.Add(finalNumber);
                    }
                    ultimatenumber = listofFinalSerialNumber.Max();
                    electricalInterfacesCollectionDataGridView.Rows[num].Cells[0].Value = ++ultimatenumber;
                }
                else
                {
                    electricalInterfacesCollectionDataGridView.Rows[num].Cells[0].Value = 1;
                }

                electricalInterfacesCollectionDataGridView.Rows[num].Cells[1].Value = row;
               
                dragging = false;

                //set your cursor back to the deafault
            }
        }
      
        private void deleterowsInelectricalInterfacesDataGridView_Click(object sender, EventArgs e)
        {

            try
            {
                if (electricalInterfacesCollectionDataGridView.CurrentCell != null)
                {
                    int rowIndex = electricalInterfacesCollectionDataGridView.CurrentCell.RowIndex;
                    electricalInterfacesCollectionDataGridView.Rows.RemoveAt(rowIndex);
                }

            }
            catch (Exception) { }
        }

        private void automationMLRoleCmbBx_Click(object sender, EventArgs e)
        {
            if (automationMLRoleCmbBx.SelectedItem != null)
            {
                panelSelectFile.Size = panelSelectFile.MaximumSize;
            }
        }

        private void treeViewRoleClassLib_ItemDrag(object sender, ItemDragEventArgs e)
        {
            dragging = true;
            row = new object();
            treeViewRoleClassLib.SelectedNode = (TreeNode)e.Item;//dragging doesn't automatically change the selected index
            row = treeViewRoleClassLib.SelectedNode.Name;//or whatever value you need from the node
        }

        private void genericInformationDataGridView_MouseUp(object sender, MouseEventArgs e)
        {
            if (dragging)
            {

                int num = genericInformationDataGridView.Rows.Add();
                List<string> listofSerialNumbers = new List<string>();
                List<int> listofFinalSerialNumber = new List<int>();
                string number = "";
                int finalNumber = 0;
                int ultimatenumber = 0;
                if (genericInformationDataGridView.Rows.Count > 2)
                {
                    foreach (DataGridViewRow row in genericInformationDataGridView.Rows)
                    {
                        if (row.Cells[0].Value == null)
                        {
                            number = "0";
                            listofSerialNumbers.Add(number);
                        }
                        if (row.Cells[0].Value != null)
                        {
                            number = row.Cells[0].Value.ToString();
                            listofSerialNumbers.Add(number);
                        }
                    }
                    foreach (string str in listofSerialNumbers)
                    {
                        finalNumber = Convert.ToInt32(str);
                        listofFinalSerialNumber.Add(finalNumber);
                    }
                    ultimatenumber = listofFinalSerialNumber.Max();
                    genericInformationDataGridView.Rows[num].Cells[0].Value = ++ultimatenumber;
                }
                else
                {
                    genericInformationDataGridView.Rows[num].Cells[0].Value = 1;
                }

                genericInformationDataGridView.Rows[num].Cells[1].Value = row;

                dragging = false;

                //set your cursor back to the deafault
            }
        }

        private void deleteRoleClassButton_Click(object sender, EventArgs e)
        {

            try
            {
                if (genericInformationDataGridView.CurrentCell != null)
                {
                    int rowIndex = genericInformationDataGridView.CurrentCell.RowIndex;
                    genericInformationDataGridView.Rows.RemoveAt(rowIndex);
                }

            }
            catch (Exception) { }
        }

        private void electricalInterfacesCollectionDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            var AutomationMLDataTables = new AutomationMLDataTables();
            electricalInterfacesCollectionDataGridView.CurrentRow.Selected = true;
            if (electricalInterfacesCollectionDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                if (Convert.ToBoolean(electricalInterfacesCollectionDataGridView.CurrentRow.Cells[3].Value) == false)
                {
                    elecInterAttDataGridView.Rows.Clear();
                    string interfaceClass = electricalInterfacesCollectionDataGridView.CurrentRow.Cells[1].Value.ToString();
                    foreach (var pair in searchAMLLibraryFile.dictionaryofInterfaceClassattributes)
                    {
                        if (pair.Key.Contains(interfaceClass))
                        {
                            DataTable AMLDataTable = AutomationMLDataTables.AMLAttributeParameters();
                            AutomationMLDataTables.CreateDataTableWithColumns(AMLDataTable, elecInterAttDataGridView, pair);
                        }
                    }
                    electricalInterfacesCollectionDataGridView.CurrentRow.Cells[3].Value = true;
                }
                if (Convert.ToBoolean(electricalInterfacesCollectionDataGridView.CurrentRow.Cells[3].Value) == true)
                {

                }
                
            }
        }

        private void genericInformationDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            var AutomationMLDataTables = new AutomationMLDataTables();
            if (genericInformationDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                gwnericparametersAttrDataGridView.Rows.Clear();
                genericInformationDataGridView.CurrentRow.Selected = true;
                string roleClass = genericInformationDataGridView.CurrentRow.Cells[1].Value.ToString();
                foreach (var pair in searchAMLLibraryFile.dictionaryofRoleClassattributes)
                {
                    if (pair.Key.Contains(roleClass))
                    {
                        DataTable AMLDataTable = AutomationMLDataTables.AMLAttributeParameters();
                        AutomationMLDataTables.CreateDataTableWithColumns(AMLDataTable, gwnericparametersAttrDataGridView, pair);
                    }
                }
            }
        }

        private void saveFromelecInterAttrButton_Click(object sender, EventArgs e)
        {
            List<ElectricalInterfaceParameters> listofElectricalInterfaceParameters = new List<ElectricalInterfaceParameters>();
            if (elecInterAttDataGridView != null)
            {

                int i = 0;
                int j = elecInterAttDataGridView.Rows.Count;
                if (i <= 0)
                {
                    while (i < j)
                    {
                        ElectricalInterfaceParameters parametersFromElectricalInterfaceDataGridView = new ElectricalInterfaceParameters();
                        try
                        {
                            parametersFromElectricalInterfaceDataGridView.AttributeName = Convert.ToString(elecInterAttDataGridView.Rows[i].Cells[0].Value);
                            parametersFromElectricalInterfaceDataGridView.Values = Convert.ToString(elecInterAttDataGridView.Rows[i].Cells[1].Value);
                            parametersFromElectricalInterfaceDataGridView.Default = Convert.ToString(elecInterAttDataGridView.Rows[i].Cells[2].Value);
                            parametersFromElectricalInterfaceDataGridView.Units = Convert.ToString(elecInterAttDataGridView.Rows[i].Cells[3].Value);
                            parametersFromElectricalInterfaceDataGridView.Default = Convert.ToString(elecInterAttDataGridView.Rows[i].Cells[4].Value);
                            parametersFromElectricalInterfaceDataGridView.Semantic = Convert.ToString(elecInterAttDataGridView.Rows[i].Cells[5].Value);
                            parametersFromElectricalInterfaceDataGridView.Reference = Convert.ToString(elecInterAttDataGridView.Rows[i].Cells[6].Value);

                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }


                        listofElectricalInterfaceParameters.Add(parametersFromElectricalInterfaceDataGridView);
                        i++;

                    }
                }


            }
            if (dictofElectricalInterfaceParametrs.ContainsKey(Convert.ToString(interfaceClassTextBox.Text.ToString() + externalInterfaceTxtBox.Text.ToString())))
            {
                dictofElectricalInterfaceParametrs.Remove(Convert.ToString(interfaceClassTextBox.Text.ToString() + externalInterfaceTxtBox.Text.ToString()));
            }
            dictofElectricalInterfaceParametrs.Add(interfaceClassTextBox.Text.ToString() + externalInterfaceTxtBox.Text.ToString(), listofElectricalInterfaceParameters);
            elecInterAttDataGridView.Rows.Clear();
        }

        
    }
}
