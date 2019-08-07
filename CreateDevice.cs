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

        private void Button6_Click(object sender, EventArgs e)
        {
            if (panel5.Size == panel5.MaximumSize)
            {
                panel5.Size = panel5.MinimumSize;
                button6.Image = Resources.icons8_expand_arrow_24;
            }
            else
            {
                panel5.Size = panel5.MaximumSize;
                button6.Image = Resources.icons8_collapse_arrow_24;
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            if (panel7.Size == panel7.MaximumSize)
            {
                panel7.Size = panel7.MinimumSize;
                button7.Image = Resources.icons8_expand_arrow_24;
            }
            else
            {
                panel7.Size = panel7.MaximumSize;
                button7.Image = Resources.icons8_collapse_arrow_24;
            }

        }

        private void Button8_Click(object sender, EventArgs e)
        {
            if (panel8.Size == panel8.MaximumSize)
            {
                panel8.Size = panel8.MinimumSize;
                button8.Image = Resources.icons8_expand_arrow_24;
            }
            else
            {
                panel8.Size = panel8.MaximumSize;
                button8.Image = Resources.icons8_collapse_arrow_24;
            }
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
            if (panel10.Size == panel10.MaximumSize)
            {
                panel10.Size = panel10.MinimumSize;
                button1.Image = Resources.icons8_expand_arrow_24;
            }
            else
            {
                panel10.Size = panel10.MaximumSize;
                button1.Image = Resources.icons8_collapse_arrow_24;
            }
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            if (panel14.Size == panel14.MaximumSize)
            {
                panel14.Size = panel14.MinimumSize;
                button10.Image = Resources.icons8_expand_arrow_24;
            }
            else
            {
                panel14.Size = panel14.MaximumSize;
                button10.Image = Resources.icons8_collapse_arrow_24;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (panel24.Size == panel24.MaximumSize)
            {
                panel24.Size = panel24.MinimumSize;
                button2.Image = Resources.icons8_expand_arrow_24;
            }
            else
            {
                panel24.Size = panel24.MaximumSize;
                button2.Image = Resources.icons8_collapse_arrow_24;
            }
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
            if (panel27.Size == panel27.MaximumSize)
            {
                panel27.Size = panel27.MinimumSize;
                button3.Image = Resources.icons8_expand_arrow_24;
            }
            else
            {
                panel27.Size = panel27.MaximumSize;
                button3.Image = Resources.icons8_collapse_arrow_24;
            }
        }

        private void Button32_Click(object sender, EventArgs e)
        {
            if (panel28.Size == panel28.MaximumSize)
            {
                panel28.Size = panel28.MinimumSize;
                button32.Image = Resources.icons8_expand_arrow_24;
            }
            else
            {
                panel28.Size = panel28.MaximumSize;
                button32.Image = Resources.icons8_collapse_arrow_24;
            }
        }

        private void Button33_Click(object sender, EventArgs e)
        {
            if (panel29.Size == panel29.MaximumSize)
            {
                panel29.Size = panel29.MinimumSize;
                button33.Image = Resources.icons8_expand_arrow_24;
            }
            else
            {
                panel29.Size = panel29.MaximumSize;
                button33.Image = Resources.icons8_collapse_arrow_24;
            }
        }

        
       
      

        private void AddSemanticSystems_Click(object sender, EventArgs e)
        {

            if(semanticSystemCmbx.Text == "eClass")
            {

               /* Datatables productDetails = new Datatables();
                DataTable createProductDetailsdatatable = productDetails.ThreeParametersdatatable();


                CommercialDataDictionary productDetailsDictionary = new CommercialDataDictionary();
                Dictionary<int, Parameters> productDetailsDictionaryParameters = productDetailsDictionary.ProductDetails();

                productDetails.CreateParameter(productDetailsDictionaryParameters, createProductDetailsdatatable, dataGridViewPD);

                semanticSystemdrpdwn.DropDownItems.Add(semanticSystemCmbx.Text);*/
                    
                    Datatables a = new Datatables();
                    DataTable b = a.ThreeParametersdatatable();
                

                    DictionaryeClass n = new DictionaryeClass();
                    Dictionary<int, Parameters> IDP = n.eClassIdentificationdataParameters();

                    a.CreateParameter(IDP,b,dataGridViewIDT);

                    
                   /* DataTable ProductDetailsdatatable = new DataTable();
                    ProductDetailsdatatable.Columns.Add("ReferenceID");
                    ProductDetailsdatatable.Columns.Add("Attribute");
                    ProductDetailsdatatable.Columns.Add("Value");
                   CommercialDataDictionary p = new CommercialDataDictionary();
                    Dictionary<int, Parameters> PDP = p.ProductDetails();

                    // Foreach loop for looping every parameters in the Dictionary
                    foreach (KeyValuePair<int, Parameters> eClassKeyValuePair in PDP)
                    {
                        Parameters par = eClassKeyValuePair.Value;

                        DataRow row = ProductDetailsdatatable.NewRow();

                        row["ReferenceID"] = par.RefSemanticPrefix;
                        row["Attribute"] = par.Parameter;
                        row["Value"] = "";
                        ProductDetailsdatatable.Rows.Add(row);

                    }
                    // For each loop creating the rows in the data table 
                    foreach (DataRow IDT in ProductDetailsdatatable.Rows)
                    {
                        int num = dataGridViewPD.Rows.Add();
                        dataGridViewPD.Rows[num].Cells[0].Value = IDT["ReferenceID"].ToString();
                        dataGridViewPD.Rows[num].Cells[1].Value = IDT["Attribute"].ToString();
                        dataGridViewPD.Rows[num].Cells[2].Value = IDT["Value"].ToString();
                    }*/

                               

            }
            if (semanticSystemCmbx.Text == "IRDI")
            {
                semanticSystemdrpdwn.DropDownItems.Add(semanticSystemCmbx.Text);
                Datatables a = new Datatables();
                DataTable b = a.ThreeParametersdatatable();

                DictionaryIRDI n = new DictionaryIRDI();
                Dictionary<int, Parameters> IDP = n.IRDIIdentificationdata();

                // Foreach loop for looping every parameters in the Dictionary
                foreach (KeyValuePair<int, Parameters> eClassKeyValuePair in IDP)
                {
                    Parameters par = eClassKeyValuePair.Value;

                    DataRow row = b.NewRow();

                    row["ReferenceID"] = par.RefSemanticPrefix;
                    row["Attribute"] = par.Parameter;
                    row["Value"] = "";
                    b.Rows.Add(row);



                }
                // For each loop creating the rows in the data table 
                foreach (DataRow IDT in b.Rows)
                {
                    int num = dataGridViewIDT.Rows.Add();
                    dataGridViewIDT.Rows[num].Cells[0].Value = IDT["ReferenceID"].ToString();
                    dataGridViewIDT.Rows[num].Cells[1].Value = IDT["Attribute"].ToString();
                    dataGridViewIDT.Rows[num].Cells[2].Value = IDT["Value"].ToString();

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
    }
   



}


