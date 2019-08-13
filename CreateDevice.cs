using Aml.Editor.Plugin.Properties;
using System;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.Drawing;
using System.Collections.Generic;


namespace Aml.Editor.Plugin
{
    public partial class CreateDevice
        : UserControl
    {
       
        // Controller class to pass the data to
        MWController mWController;
        AnimationClass AMC = new AnimationClass();

        // flag if we are creating a new device or editing one
        bool isEditing = false;

        // holders for the selected paths. These will be set by the FileOKs of the openFileDialogs
        string vendorPicturePath = "";
        string devicePicturePath = "";
        string deviceIconPath = "";

        #region Public Constructors
        /// <summary>
        /// Create and initialize the GUI
        /// </summary>
        /// <param name="mWController">The <see cref="MWController"/>MWController Object to which the data will be passed to</param>
        public CreateDevice(MWController mWController)
        {
            this.mWController = mWController;
            InitializeComponent();

        }

        #endregion Public Constructors

        /// <summary>
        /// OnClick Methode of the Create Device Button.
        /// This creates a new <see cref="MWDevice"/>Device which will be passed to the <see cref="mWController"/>
        /// </summary>
        /// <param name="sender">Event Argument, not used</param>
        /// <param name="e">Event Argument, not used</param>
        private void createDeviceBtn_Click(object sender, System.EventArgs e)
        {

            // Create a new Device
            var device = new MWDevice();

            // Read all the input fields and write them to the device data
            if (deviceTypeListBox.SelectedItem != null)
            {
                device.deviceType = deviceTypeListBox.SelectedItem.ToString();
            }
            else
            {
                device.deviceType = "";
            }

            // Check if there was an input in this field, if so: try to convert it to integer
            if (!String.IsNullOrWhiteSpace(txtVendorId.Text))
            {
                try { device.vendorID = Convert.ToInt32(txtVendorId.Text); } catch (Exception) { MessageBox.Show("Warning: Vendor ID must be number.\n please correct input"); }
            }
            // Check if there was an input in this field, if so: try to convert it to integer
            if (!String.IsNullOrWhiteSpace(txtDeviceId.Text))
            {
                try { device.deviceID = Convert.ToInt32(txtDeviceId.Text); } catch (Exception) { MessageBox.Show("Device ID is in an invalid format (Expected only numbers)! Ignoring!"); }
            }
            device.vendorName = txtVendorName.Text;
            device.vendorHomepage = txtVendorHomepage.Text;
            device.deviceName = txtDeviceName.Text;
            device.deviceFamily = txtDeviceFamily.Text;
            device.productName = txtProductName.Text;
            device.orderNumber = txtOrderNumber.Text;
            device.productText = productTxtBox.Text;
            device.harwareRelease = hwRelTxt.Text;
            device.softwareRelease = swRelTxt.Text;


            device.ipProtection = txtIpProduction.Text;
            if (!String.IsNullOrWhiteSpace(txtMaxTemp.Text))
            {
                try { device.minTemperature = Convert.ToDouble(txtMinTemp.Text); } catch (Exception) { device.minTemperature = Double.NaN; MessageBox.Show("Min Temperature is in an invalid format (Expected only numbers)! Ignoring!"); }
            }
            if (!String.IsNullOrWhiteSpace(txtMaxTemp.Text))
            {
                try { device.maxTemperature = Convert.ToDouble(txtMaxTemp.Text); } catch (Exception) { device.maxTemperature = Double.NaN; MessageBox.Show("Max Temperature is in an invalid format (Expected only numbers)! Ignoring!"); }
            }

            device.deviceIcon = deviceIconPath;
            device.devicePicture = devicePicturePath;
            device.vendorLogo = vendorPicturePath;

            // Pass the device to the controller
            string result = mWController.CreateDeviceOnClick(device, isEditing);
            clear();
            // Display the result
            if (result != null)
            {
                // Display error Dialog
                MessageBox.Show(result);
            }
        }

        /// <summary>
        /// OnClick of the Back Button
        /// Call the <see cref="mWController"/> to display the Start GUI
        /// If <see cref="isEditing"/> was set, clear all input fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backBtn_Click(object sender, EventArgs e)
        {
            mWController.ChangeGui(MWController.MWGUIType.Start);
            if (isEditing)
            {
                clear();
            }
        }

        /// <summary>
        /// Clears all input fields and reset the GUI
        /// </summary>
        internal void clear()
        {
            deviceTypeListBox.SelectedItem = -1;
            txtVendorId.Text = "";
            txtVendorName.Text = "";
            txtVendorHomepage.Text = "";
            txtDeviceId.Text = "";
            txtDeviceName.Text = "";

            txtDeviceFamily.Text = "";
            txtProductName.Text = "";
            txtOrderNumber.Text = "";
            productTxtBox.Clear();
            txtIpProduction.Text = "";
            hwRelTxt.Text = "";
            swRelTxt.Text = "";
            txtMinTemp.Text = "";
            txtMaxTemp.Text = "";

            createDeviceBtn.Text = "Create Device";
            txtDeviceName.Enabled = true;
            txtVendorName.Enabled = true;
            isEditing = false;
        }

        /// <summary>
        /// Fill the input fields with data from the device
        /// </summary>
        /// <param name="device">The device with the data</param>
        internal void prefill(MWDevice device)
        {
            deviceTypeListBox.SelectedItem = device.deviceType;
            txtVendorId.Text = device.vendorID.ToString();
            txtVendorName.Text = device.vendorName;
            txtVendorHomepage.Text = device.vendorHomepage;
            txtDeviceId.Text = device.deviceID.ToString();
            txtDeviceName.Text = device.deviceName;

            txtDeviceFamily.Text = device.deviceFamily;
            txtProductName.Text = device.productName;
            txtOrderNumber.Text = device.orderNumber;
            productTxtBox.Text = device.productText;
            txtIpProduction.Text = device.ipProtection;
            hwRelTxt.Text = device.harwareRelease;
            swRelTxt.Text = device.softwareRelease;

            if (!Double.IsNaN(device.minTemperature))
            {
                txtMinTemp.Text = device.minTemperature.ToString();
            }
            else
            {
                txtMinTemp.Text = "";
            }

            if (!Double.IsNaN(device.maxTemperature))
            {
                txtMaxTemp.Text = device.maxTemperature.ToString();
            }
            else
            {
                txtMaxTemp.Text = "";
            }

            createDeviceBtn.Text = "Update Device";
            txtDeviceName.Enabled = false;
            txtVendorName.Enabled = false;

            vendorPicturePath = device.vendorLogo;
            deviceIconPath = device.deviceIcon;
            devicePicturePath = device.devicePicture;

            isEditing = true;
        }

        /// <summary>
        /// Open the VendorLogo File Dialog
        /// </summary>
        /// <param name="sender">Event arg, not used</param>
        /// <param name="e">Event arg, not used</param>
        private void openVendorLogoBtn_Click(object sender, EventArgs e)
        {
            openVendorLogoDialog.ShowDialog();
        }

        /// <summary>
        /// Open DeviceIcon File Dialog
        /// </summary>
        /// <param name="sender">Event arg, not used</param>
        /// <param name="e">Event arg, not used</param>
        private void openDeviceIconBtn_Click(object sender, EventArgs e)
        {
            openDeviceIconDialog.ShowDialog();
        }

        /// <summary>
        /// Open the openDevicePicture File Dialog
        /// </summary>
        /// <param name="sender">Event arg, not used</param>
        /// <param name="e">Event arg, not used</param>
        private void openDevicePicture_Click(object sender, EventArgs e)
        {
            openDevicePictureDialog.ShowDialog();
        }

        /// <summary>
        /// update the <see cref="vendorPicturePath"/> with the selected file
        /// </summary>
        /// <param name="sender">Event arg, not used</param>
        /// <param name="e">Event arg, not used</param>
        private void openVendorLogoDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            vendorPicturePath = openVendorLogoDialog.FileName;
        }

        /// <summary>
        /// update the <see cref="deviceIconPath"/> with the selected file
        /// </summary>
        /// <param name="sender">Event arg, not used</param>
        /// <param name="e">Event arg, not used</param>
        private void openDeviceIconDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            deviceIconPath = openDeviceIconDialog.FileName;
        }

        /// <summary>
        /// update the <see cref="devicePicturePath"/> with the selected file
        /// </summary>
        /// <param name="sender">Event arg, not used</param>
        /// <param name="e">Event arg, not used</param>
        private void openDevicePictureDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            devicePicturePath = openDevicePictureDialog.FileName;
        }

        private void CreateDevice_Load(object sender, EventArgs e)
        {

        }

        private void MachineVisionAndOpticalDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripButton1_Click(object sender, EventArgs e)
        {
            mWController.ChangeGui(MWController.MWGUIType.Start);
        }

        private void MenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

       

        private void SensorInterfaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void MechanicalDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void ToolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            tabControl2.SelectTab(tabPage1);
        }

        

        private void ElectricalDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl2.SelectTab(tabPage4);
        }

        private void VScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
           
        }

        private void MaterialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl2.SelectTab(tabPage3);
        }
   
        private void BackBtn_Click_1(object sender, EventArgs e)
        {

        }

        private void CreateDeviceBtn_Click_1(object sender, EventArgs e)
        {

        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            menuStrip1.Select();
        }

        private void TxtDeviceName_TextChanged(object sender, EventArgs e)
        {

        }

        private void ToolStrip4_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void ToolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        private void Label19_Click(object sender, EventArgs e)
        {

        }

        private void Label23_Click(object sender, EventArgs e)
        {

        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        

        private void TabPage5_Click(object sender, EventArgs e)
        {

        }

        private void Panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        

        private void SplitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void TextBox19_TextChanged(object sender, EventArgs e)
        {

        }

        private void TableLayoutPanel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
           AMC.WindowSizeChanger(panel10,button1);
          
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            AMC.WindowSizeChanger(panel14,button10);
            
        }

       

        private void TextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {
           

        }

        

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            saveFileDialog1.OpenFile();
        }

        private void Panel28_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Button3_Click_1(object sender, EventArgs e)
        {
            AMC.WindowSizeChanger(panel27,button3);
        }

        private void Button32_Click(object sender, EventArgs e)
        {
            AMC.WindowSizeChanger(panel28, button32);
           
        }

        private void Button33_Click(object sender, EventArgs e)
        {
            AMC.WindowSizeChanger(panel29, button33);
           
        }

        
       
      

        private void AddSemanticSystems_Click(object sender, EventArgs e)
        {

            if (semanticSystemCmbx.SelectedItem != null)
            {
                // Call "Datatables Class"  from the "Dictionary File/Class" and store in "datatables"
                Datatables datatables = new Datatables();
                // Call "ThreeParametersdatatable" method from "Dictionary Class" and store in "datatablesheader" Seperately for PD,POD,PPD,MD
                DataTable datatableheadersPD = datatables.Parametersdatatable();
                DataTable datatableheadersPPD = datatables.Parametersdatatable();
                DataTable datatableheadersPOD = datatables.Parametersdatatable();
                DataTable datatableheadersMD = datatables.Parametersdatatable();

                // Call ""Commercialdatadictionary Class" From "Dictioanry File/Class" and store in "CDD"
                CommercialDataDictionary CDD = new CommercialDataDictionary();
                // Call "Product Details" method from "CommercialDataDictionary" and store in "PD"
                Dictionary<int, Parameters> PD = CDD.ProductDetails();

                // Call "Product Price Details" method from "CommercialDataDictionary" and store in "PPD"
                Dictionary<int, Parameters> PPD = CDD.ProductPriceDetails();


                // Call "Product Order Details" method from "CommercialDataDictionary" and store in "POD"
                Dictionary<int, Parameters> POD = CDD.ProductOrderDetails();


                // Call "Manufacturer Details" method from "CommercialDataDictionary" and store in "MD"
                Dictionary<int, Parameters> MD = CDD.ManufacturerDetails();

                //
                datatables.CreateDataTableWith3Columns(PD, datatableheadersPD, dataGridViewPD);

                datatables.CreateDataTableWith3Columns(PPD, datatableheadersPPD, dataGridViewPPD);
                datatables.CreateDataTableWith3Columns(POD, datatableheadersPOD, dataGridViewPOD);
                datatables.CreateDataTableWith3Columns(MD, datatableheadersMD, dataGridViewMD);
                treeViewCH.Nodes.Add("Generic Data");



                if (semanticSystemCmbx.Text == "eClass")
                {
                    treeViewCH.Nodes.Add("Interfaces");
                    

                    DataTable datatableheadereClassID = datatables.Parametersdatatable();

                    DictionaryeClass DEC = new DictionaryeClass();
                    Dictionary<int, Parameters> eClassID = DEC.eClassIdentificationdataParameters();

                    datatables.CreateDataTableWith3Columns(eClassID, datatableheadereClassID, dataGridViewIDT);
                }
                if (semanticSystemCmbx.SelectedText  == "IRDI")
                {
                    DataTable datatableheaderIRDIID = datatables.Parametersdatatable();

                    DictionaryIRDI DIRDI = new DictionaryIRDI();
                    Dictionary<int, Parameters> IRDIID = DIRDI.IRDIIdentificationdata();

                    datatables.CreateDataTableWith3Columns(IRDIID, datatableheaderIRDIID,dataGridViewIDT);
                }

            }


           
           
            
        }

        private void ToolStripButton3_Click(object sender, EventArgs e)
        {
            semanticSystemCmbx.Text = "";
            classificationSystemTextBox.Text = "";
            versionTextBox.Text = "";
            dataGridViewIDT.Rows.Clear();
            dataGridViewPD.Rows.Clear();
            dataGridViewPOD.Rows.Clear();
            dataGridViewPPD.Rows.Clear();
            dataGridViewMD.Rows.Clear();
            dataGridViewElectricalConnection.Rows.Clear();

        }

        private void Panel32_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ToolStripButton6_Click(object sender, EventArgs e)
        {
            dataGridViewIDT.Rows.Clear(); 
        }

        private void SemanticSystemCmbx_Click(object sender, EventArgs e)
        {

        }

        private void TreeViewCH_AfterSelect(object sender, TreeViewEventArgs e)
        {
                   
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            AMC.WindowSizeChanger(panel4, button2);
        }

        private void ToolStripButton3_Click_1(object sender, EventArgs e)
        {
            
        }

        private void AddConnector_Click(object sender, EventArgs e)
        {
                Datatables datatables = new Datatables();

                DataTable datatableheadersIRDIED = datatables.Parametersdatatable();

                DictionaryIRDI DIRDI = new DictionaryIRDI();

                Dictionary<int, Parameters> IRDIED = DIRDI.IRDIElectricalData();

                datatables.CreateDataTableWith4Columns(IRDIED, datatableheadersIRDIED, dataGridViewIRDIElectricalData);

            
                        
        }

        private void ToolStripButton3_Click_2(object sender, EventArgs e)
        {


                
            if (pinNumberTxtBx.Text != null)
            {
              
                int c = 0;
                string a = pinNumberTxtBx.Text;
                int b = Convert.ToInt32(a);
                for (int i = 0; i < b; i++)
                {
                    dataGridViewPinInfo.Rows.Add();
                    dataGridViewPinInfo.Rows[c + i].Cells[0].Value = (1 + i).ToString();
                }
            }

           
            if(pinNumberTxtBx.Text == null)
            {
                MessageBox.Show("Enter valid Number", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void ToolStripButton3_Click_3(object sender, EventArgs e)
        {
            dataGridViewPinInfo.Rows.Clear();
        }

        private void ToolStripButton15_Click(object sender, EventArgs e)
        {
            dataGridViewIRDIElectricalData.Rows.Clear();
        }
        
        
        private void ToolStripButton16_Click(object sender, EventArgs e)
        {
            Datatables datatables = new Datatables();
            // IRDIEIDI = IRDI Electrical Interfacce Dimensional Information
            DataTable datatableheadersIRDIEIDI = datatables.Parametersdatatable();

            DictionaryIRDI DIRDI = new DictionaryIRDI();

            if (connectorStyleCmbx.Text == "Fixed Connectors")
            {
               
                if (mountingCmbx.Text == "Square flange mounting")
                {
                    
                    Dictionary<int, Parameters> IRDIEIDI = DIRDI.IRDIMountingSquareFlangeData();

                    datatables.CreateDataTableWith4Columns(IRDIEIDI, datatableheadersIRDIEIDI, dataGridViewIRDIMountingData);
                }
                if (mountingCmbx.Text == "Single hole mounting")
                {
                    Dictionary<int, Parameters> IRDIEIDI = DIRDI.IRDIMountingSingleHoleData();

                    datatables.CreateDataTableWith4Columns(IRDIEIDI, datatableheadersIRDIEIDI, dataGridViewIRDIMountingData);
                }
            }
            if (connectorStyleCmbx.Text == "Free Connectors")
            {
                connectorVersion.Enabled = true;
                connectorVersionCmbx.Enabled = true;

                if(mountingCmbx.Text == "Locking nut")
                {
                    if (connectorVersionCmbx.Text == "Right Angled")
                    {

                    }
                    if (connectorVersionCmbx.Text == "Straight")
                    {

                    }

                }
                

            }
        }

        private void ToolStripButton17_Click(object sender, EventArgs e)
        {
            dataGridViewIRDIMountingData.Rows.Clear();
        }

        private void ToolStripButton18_Click(object sender, EventArgs e)
        {
            Datatables datatables = new Datatables();
            //IRDIEIDI2 = IRDI Electrical Interafce Dimensional Information second data table
            DataTable datatableheadersIRDIEIDI2 = datatables.Parametersdatatable();

            DictionaryIRDI DIRDI = new DictionaryIRDI();

            if (connectorTypeCmbx.Text == "Male")
            {
                Dictionary<int, Parameters> IRDIEIDI2 = DIRDI.IRDIMaleConnectorDimensionData();

                datatables.CreateDataTableWith6Columns(IRDIEIDI2, datatableheadersIRDIEIDI2, dataGridViewIRDIConnectorDimensions);

            }
            if (connectorTypeCmbx.Text == "Female")
            {
                Dictionary<int, Parameters> IRDIEIDI2 = DIRDI.IRDIFemaleConnectorDimensionData();

                datatables.CreateDataTableWith6Columns(IRDIEIDI2, datatableheadersIRDIEIDI2, dataGridViewIRDIConnectorDimensions);

            }

        }

        private void ToolStripButton19_Click(object sender, EventArgs e)
        {
            dataGridViewIRDIConnectorDimensions.Rows.Clear();
        }
    }
   



}


