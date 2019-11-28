﻿using System;
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
        SearchAMLComponentFile searchAMLComponentFile = new SearchAMLComponentFile();
        MWDevice device = new MWDevice();

        


        public DeviceDescription()
        {
            InitializeComponent();

           
        }

        public DeviceDescription(MWController mWController)
        {
            this.mWController = mWController;
            InitializeComponent();
            device.DictionaryForInterfaceClassesInElectricalInterfaces = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();
            device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();
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
            else
            {
                Environment.Exit(0);

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
           

        }

        private void GenerateAML_Click(object sender, EventArgs e)
        {
            if (generateAML.Text == "Save AML File")
            {
                try
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    
                    saveFileDialog.Filter = "AML Files(*.aml; *.amlx;*.xml;*.AML )|*.aml; *.amlx;*.xml;*.AML;";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //saveFileDialog.FileName = vendorNameTxtBx.Text;
                        device.filepath = Path.GetDirectoryName(saveFileDialog.FileName) ;
                    }

                }
                catch (Exception)
                {

                    throw;
                }
                
            }
            
            // Create a new Device
            //var device = new MWDevice();
            
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
            
            device.ipProtection = ipProtectionTxtBx.Text;

            if (!String.IsNullOrWhiteSpace(opTempMaxTxtBx.Text))
            {
                try { device.minTemperature = Convert.ToDouble(opTempMinTxtBx.Text); } catch (Exception) { device.minTemperature = Double.NaN; MessageBox.Show("Min Temperature is in an invalid format (Expected only numbers)! Ignoring!"); }
            }
            if (!String.IsNullOrWhiteSpace(opTempMaxTxtBx.Text))
            {
                try { device.maxTemperature = Convert.ToDouble(opTempMaxTxtBx.Text); } catch (Exception) { device.maxTemperature = Double.NaN; MessageBox.Show("Max Temperature is in an invalid format (Expected only numbers)! Ignoring!"); }
            }
           
           

            // storing user defined values of Attachebles data grid view in to list 

           
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
            device.DictionaryForInterfaceClassesInElectricalInterfaces = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();
            device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();
           
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
            

            // clear tree view
            dataHierarchyTreeView.Nodes.Clear();

            attachablesInfoDataGridView.Rows.Clear();
            electricalInterfacesCollectionDataGridView.Rows.Clear();
            elecInterAttDataGridView.Rows.Clear();
            genericInformationDataGridView.Rows.Clear();
            genericparametersAttrDataGridView.Rows.Clear();

            // reset switch case to case 1 again.
            //electricalInterfacesComboBox.Items.Clear();

        }



        private void IdentificationDataBtn_Click(object sender, EventArgs e)
        {
            
        }



        private void CommercialDataBtn_Click(object sender, EventArgs e)
        {
           
        }

        

        private void VendorNameRefSemanticBtn_Click(object sender, EventArgs e)
        {

            AMC.SemanticSystemOpener(vendorNameRefSemanticBtn.Text);
           

        }

        private void VendorLogoDisplayBtn_Click(object sender, EventArgs e)
        {

        }

        private void DeviceNameRefSemanticBtn_Click(object sender, EventArgs e)
        {
            AMC.SemanticSystemOpener(deviceNameRefSemanticBtn.Text);
            
        }

        private void ProductFamilyRefSemanticBtn_Click(object sender, EventArgs e)
        {
            AMC.SemanticSystemOpener(productFamilyRefSemanticBtn.Text);
           
        }

        private void VendorHomepageRefSemanticBtn_Click(object sender, EventArgs e)
        {
            AMC.SemanticSystemOpener(vendorHomepageRefSemanticBtn.Text);
           
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
                {

                    var index = attachablesInfoDataGridView.Rows.Add();
                    attachablesInfoDataGridView.Rows[index].Cells["ElementName"].Value = AMLfileLabel.Text;
                    attachablesInfoDataGridView.Rows[index].Cells["FilePath"].Value = selectedFileLocationTxtBx.Text;

                    selectedFileLocationTxtBx.Text = "";
                    AMLfileLabel.Text = "";
                    AMLURLLabel.Text = "";
                    panelSelectFile.Size = panelSelectFile.MinimumSize;
                   
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
                    var index = attachablesInfoDataGridView.Rows.Add();
                    attachablesInfoDataGridView.Rows[index].Cells["ElementName"].Value = AMLURLLabel.Text;
                    attachablesInfoDataGridView.Rows[index].Cells["FilePath"].Value = selectedFileURLTextBox.Text;
                    AMLURLLabel.Text = "";
                    selectedFileURLTextBox.Text = "";
                    panelSelectFile.Size = panelSelectFile.MinimumSize;
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

       


      

        

       
       
       

        // this below dictionary stores each attribute values from electrical Interaface Data grid View to the current Hierarchy Treeview by a key.
        Dictionary<string, List<ElectricalInterfaceParameters>> dictofElectricalInterfaceParametrs = new Dictionary<string, List<ElectricalInterfaceParameters>>();
        private void selectAMLFileBtn_Click(object sender, EventArgs e)
        {
            searchAMLLibraryFile.dictionaryofRoleClassattributes = new Dictionary<string, List<ClassOfListsFromReferencefile>>();
           
            searchAMLLibraryFile.DictionaryForInterfaceClassInstancesAttributes = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();
            searchAMLLibraryFile.DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();

            searchAMLLibraryFile.DictionaryForRoleClassInstanceAttributes = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();
            searchAMLLibraryFile.DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();

            searchAMLLibraryFile.DictioanryOfIDofInterfaceClassLibraryNodes = new Dictionary<string, string>();

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
                                searchAMLLibraryFile.CheckForAttributesOfReferencedClassName(classType);
                                searchAMLLibraryFile.SearchForReferencedClassName(document, referencedClassName, classType);
                            }
                            else
                            {
                                 roleNode = libNode.Nodes.Add(classType.ToString(), classType.ToString() , 1);
                            }
                            
                           

                            if (classType.ExternalInterface.Exists)
                            {
                                foreach (var externalinterface in classType.ExternalInterface)
                                {
                                    TreeNode externalinterfacenode;
                                   
                                    if (externalinterface.BaseClass != null)
                                    {
                                        referencedClassName = externalinterface.BaseClass.ToString();
                                        externalinterfacenode = roleNode.Nodes.Add(externalinterface.ToString(), externalinterface.ToString() + "{" + "Class:" + "  " + referencedClassName + "}", 2);

                                        searchAMLLibraryFile.CheckForAttributesOfReferencedClassNameofExternalIterface(classType, externalinterface);
                                        searchAMLLibraryFile.SearchForReferencedClassNameofExternalIterface(document, referencedClassName, classType, externalinterface);

                                    }
                                    else
                                    {
                                        externalinterfacenode = roleNode.Nodes.Add(externalinterface.ToString(), externalinterface.ToString(), 2);
                                    }

                                   

                                    searchAMLLibraryFile.PrintExternalInterfaceNodes(document,externalinterfacenode, externalinterface, classType);
                                }
                                
                            }
                            searchAMLLibraryFile.PrintNodesRecursiveInRoleClassLib(document, roleNode, classType, referencedClassName);
                         }

                     }
                   
                    foreach (var classLibType in document.CAEXFile.InterfaceClassLib)
                    {
                       // searchAMLLibraryFile.DictioanryOfIDofInterfaceClassLibraryNodes.Add(classLibType.Name.ToString(), classLibType.ID.ToString());
                        TreeNode libNode = treeViewInterfaceClassLib.Nodes.Add(classLibType.ToString(), classLibType.ToString(),0);
                        
                       

                        foreach (var classType in classLibType.InterfaceClass) 
                        {
                            TreeNode interfaceclassNode;
                            if (classType.ReferencedClassName != "")
                            {
                               // searchAMLLibraryFile.DictioanryOfIDofInterfaceClassLibraryNodes.Add(classType.Name.ToString(), classType.ID.ToString());

                                referencedClassName = classType.ReferencedClassName;
                                interfaceclassNode = libNode.Nodes.Add(classType.ToString(), classType.ToString() + "{" + "Class:" + "  " + referencedClassName + "}", 1);
                              
                                searchAMLLibraryFile.CheckForAttributesOfReferencedClassName(classType);
                                searchAMLLibraryFile.SearchForReferencedClassName(document, referencedClassName, classType);
                            }
                            else
                            {
                                //searchAMLLibraryFile.DictioanryOfIDofInterfaceClassLibraryNodes.Add(classType.Name.ToString(), classType.ID.ToString());

                                interfaceclassNode = libNode.Nodes.Add(classType.ToString(), classType.ToString(), 1);
                            }

                             

                            if (classType.ExternalInterface.Exists)
                            {
                                foreach (var externalinterface in classType.ExternalInterface)
                                {
                                    TreeNode externalinterfacenode;
                                   
                                    if (externalinterface.BaseClass != null)
                                    {
                                        //searchAMLLibraryFile.DictioanryOfIDofInterfaceClassLibraryNodes.Add(classType.Name.ToString()+ externalinterface.ToString(), externalinterface.ID.ToString());

                                        referencedClassName = externalinterface.BaseClass.ToString();
                                        externalinterfacenode = interfaceclassNode.Nodes.Add(externalinterface.ToString(), externalinterface.ToString() + "{" + "Class:" + "  " + referencedClassName + "}", 2);
                                        searchAMLLibraryFile.CheckForAttributesOfReferencedClassNameofExternalIterface(classType, externalinterface);
                                        searchAMLLibraryFile.SearchForReferencedClassNameofExternalIterface(document, referencedClassName, classType, externalinterface);
                                    }
                                    else
                                    {
                                        //searchAMLLibraryFile.DictioanryOfIDofInterfaceClassLibraryNodes.Add(classType.Name.ToString() + externalinterface.ToString(), externalinterface.ID.ToString());

                                        externalinterfacenode = interfaceclassNode.Nodes.Add(externalinterface.ToString(), externalinterface.ToString(), 2);
                                    }
                                     
                                       
                                    searchAMLLibraryFile.PrintExternalInterfaceNodes(document,externalinterfacenode, externalinterface, classType);
                                }
                            }
                            searchAMLLibraryFile.PrintNodesRecursiveInInterfaceClassLib(document,interfaceclassNode, classType, referencedClassName);
                        }
                        
                    }
                    
                }


                catch (Exception)
                {
                   
                    MessageBox.Show("Missing names of attributes or Same atrribute sequence is repeated in the given file","Missing Names", MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);
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
               
               
            }
            catch (Exception)
            {
                MessageBox.Show("A whole Interface Library cannot be added ","Select Parent Node to add Inetrface",MessageBoxButtons.OKCancel,MessageBoxIcon.Information);
            }
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

        

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            searchAMLComponentFile.DictionaryofElectricalConnectorType = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();
            searchAMLComponentFile.DictioanryofElectricalConnectorPinType = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();
            if (vendorNameTxtBx.Text != "")
            {
                if (MessageBox.Show("Save Current File", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    GenerateAML_Click(sender, e);
                    return;
                }
                if(MessageBox.Show("Save Current File", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) == DialogResult.No)
                {
                    clear();
                    return;
                }
                if (MessageBox.Show("Save Current File", "Save", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
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
                                    int i = 1;
                                    foreach (var electricalConnectorType in internalElements.ExternalInterface)
                                    {
                                        
                                        if (electricalConnectorType != null)
                                        {

                                            searchAMLComponentFile.CheckForAttributesOfExternalIterface(i,electricalConnectorType);

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

                                            electricalInterfacesCollectionDataGridView.Rows[num].Cells[1].Value = "(" + i + ")" + electricalConnectorType.Name.ToString()
                                                +"{" + "Class:" + "  " + electricalConnectorType.BaseClass+"}";
                                            electricalInterfacesCollectionDataGridView.Rows[num].Cells[4].Value = true;


                                            foreach (var electricalConnectorPins in electricalConnectorType.ExternalInterface)
                                            {
                                                if (electricalConnectorPins != null)
                                                {
                                                    searchAMLComponentFile.CheckForAttributesOfEclectricalConnectorPins(i, electricalConnectorPins, electricalConnectorType);
                                                }
                                            }
                                        }

                                        i++;
                                    }
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
            row = treeViewInterfaceClassLib.SelectedNode.Text;//or whatever value you need from the node
           
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
                electricalInterfacesCollectionDataGridView.Rows[num].Cells[3].Value = true;


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
                    electricalInterfacesCollectionDataGridView.CurrentRow.Selected = true;
                    string interfaceSerialNumber = electricalInterfacesCollectionDataGridView.Rows[rowIndex].Cells[0].Value.ToString();
                    string interfaceClass = electricalInterfacesCollectionDataGridView.Rows[rowIndex].Cells[1].Value.ToString();

                    try
                    {
                        if (device.DictionaryForInterfaceClassesInElectricalInterfaces.ContainsKey("(" + interfaceSerialNumber + ")" + interfaceClass))
                        {
                            device.DictionaryForInterfaceClassesInElectricalInterfaces.Remove("(" + interfaceSerialNumber + ")" + interfaceClass);
                            
                        }
                        

                    }
                    catch (Exception)
                    {

                        throw;
                    }

                    foreach (var pair in searchAMLLibraryFile.DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib)
                    {
                        if (pair.Key.Contains(interfaceClass))
                        {
                            try
                            {
                                if (device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces.ContainsKey("(" + interfaceSerialNumber + ")" + pair.Key.ToString()))
                                {
                                    device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces.Remove("(" + interfaceSerialNumber + ")" + pair.Key.ToString());
                                   
                                }
                               
                            }
                            catch (Exception)
                            {

                                throw;
                            }

                        }
                    }

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
            row = treeViewRoleClassLib.SelectedNode.Text;//or whatever value you need from the node
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
            treeViewElectricalInterfaces.Nodes.Clear();

            TreeNode parentNode;
            TreeNode childNodes;
            string childNodeName = "";
            var AutomationMLDataTables = new AutomationMLDataTables();
            electricalInterfacesCollectionDataGridView.CurrentRow.Selected = true;
            

            if (electricalInterfacesCollectionDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                string interfaceSerialNumber = electricalInterfacesCollectionDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();

                if (Convert.ToBoolean(electricalInterfacesCollectionDataGridView.CurrentRow.Cells[3].Value) == true)
                {
                    elecInterAttDataGridView.Rows.Clear();
                    string interfaceClass = electricalInterfacesCollectionDataGridView.CurrentRow.Cells[1].Value.ToString();
                    foreach (var pair in searchAMLLibraryFile.DictionaryForInterfaceClassInstancesAttributes)
                    {
                        if (pair.Key.ToString() == interfaceClass)
                        {
                            try
                            {
                                if (device.DictionaryForInterfaceClassesInElectricalInterfaces.ContainsKey("(" + interfaceSerialNumber + ")" + interfaceClass))
                                {
                                    device.DictionaryForInterfaceClassesInElectricalInterfaces.Remove("(" + interfaceSerialNumber + ")" + interfaceClass);
                                    device.DictionaryForInterfaceClassesInElectricalInterfaces.Add("(" + interfaceSerialNumber + ")" + interfaceClass, pair.Value);
                                }
                                else
                                {
                                    device.DictionaryForInterfaceClassesInElectricalInterfaces.Add("(" + interfaceSerialNumber + ")" + interfaceClass , pair.Value);
                                }
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                           
                        }

                    }


                    parentNode = treeViewElectricalInterfaces.Nodes.Add("(" + interfaceSerialNumber + ")" + interfaceClass,
                        "(" + interfaceSerialNumber + ")" + interfaceClass, 2);


                    foreach (var pair in searchAMLLibraryFile.DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib)
                    {
                        if (pair.Key.Contains(interfaceClass))
                        {
                            try
                            {
                                if (device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces.ContainsKey("(" + interfaceSerialNumber + ")" + pair.Key.ToString()))
                                {
                                    device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces.Remove("(" + interfaceSerialNumber + ")" + pair.Key.ToString());
                                    device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces.Add("(" + interfaceSerialNumber + ")" + pair.Key.ToString(),pair.Value);
                                }
                                else
                                {
                                    device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces.Add("(" + interfaceSerialNumber + ")" + pair.Key.ToString(),pair.Value);
                                }
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            

                            childNodes = parentNode.Nodes.Add( pair.Key.Replace(interfaceClass,"").ToString()
                                , pair.Key.Replace(interfaceClass, "").ToString() , 2);
                        }
                    }

                    electricalInterfacesCollectionDataGridView.CurrentRow.Cells[3].Value = true;
                }

                if (Convert.ToBoolean(electricalInterfacesCollectionDataGridView.CurrentRow.Cells[4].Value) == true)
                {
                    elecInterAttDataGridView.Rows.Clear();
                    string interfaceClass = electricalInterfacesCollectionDataGridView.CurrentRow.Cells[1].Value.ToString();
                    foreach (var pair in searchAMLComponentFile.DictionaryofElectricalConnectorType)
                    {
                        if (pair.Key.ToString() == interfaceClass)
                        {
                            try
                            {
                                if (device.DictionaryForInterfaceClassesInElectricalInterfaces.ContainsKey("(" + interfaceSerialNumber + ")" + interfaceClass))
                                {
                                    device.DictionaryForInterfaceClassesInElectricalInterfaces.Remove("(" + interfaceSerialNumber + ")" + interfaceClass);
                                    device.DictionaryForInterfaceClassesInElectricalInterfaces.Add("(" + interfaceSerialNumber + ")" + interfaceClass, pair.Value);
                                }
                                else
                                {
                                    device.DictionaryForInterfaceClassesInElectricalInterfaces.Add("(" + interfaceSerialNumber + ")" + interfaceClass, pair.Value);
                                }
                            }
                            catch (Exception)
                            {

                                throw;
                            }

                        }

                    }


                    parentNode = treeViewElectricalInterfaces.Nodes.Add("(" + interfaceSerialNumber + ")" + interfaceClass,
                        "(" + interfaceSerialNumber + ")" + interfaceClass, 2);


                    foreach (var pair in searchAMLComponentFile.DictioanryofElectricalConnectorPinType)
                    {
                        if (pair.Key.Contains(interfaceClass))
                        {
                            try
                            {
                                if (device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces.ContainsKey("(" + interfaceSerialNumber + ")" + pair.Key.ToString()))
                                {
                                    device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces.Remove("(" + interfaceSerialNumber + ")" + pair.Key.ToString());
                                    device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces.Add("(" + interfaceSerialNumber + ")" + pair.Key.ToString(), pair.Value);
                                }
                                else
                                {
                                    device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces.Add("(" + interfaceSerialNumber + ")" + pair.Key.ToString(), pair.Value);
                                }
                            }
                            catch (Exception)
                            {

                                throw;
                            }


                            childNodes = parentNode.Nodes.Add(pair.Key.Replace(interfaceClass, "").ToString()
                                , pair.Key.Replace(interfaceClass, "").ToString(), 2);
                        }
                    }

                   // electricalInterfacesCollectionDataGridView.CurrentRow.Cells[4].Value = true;
                }

                
                
            }

        }
        private void electricalInterfacesCollectionDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            electricalInterfacesCollectionDataGridView.CurrentRow.Selected = true;
            if (electricalInterfacesCollectionDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                AMC.WindowSizeChanger(electricalInterfacesTreeViewPanel);
            }
            
        }
        private void treeViewElectricalInterfaces_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string searchName = "";
            var AutomationMLDataTables = new AutomationMLDataTables();
            ClearHeaderTabPageValuesofElectricalInterfaces();

            TreeNode targetNode = treeViewElectricalInterfaces.SelectedNode;
           /* targetNode.SelectedImageIndex = targetNode.ImageIndex;*/

            elecInterAttDataGridView.Rows.Clear();

            try
            {
                if (e.Button == MouseButtons.Left)
                {
                    
                    if (targetNode.Parent != null)
                    {
                        
                        searchName = targetNode.Parent.Text + targetNode.Text;
                        electricalInterfacesHeaderlabel.Text = searchName;
                        //nameTxtBxElecAttri.Text = searchName;
                        foreach (var pair in device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces)
                        {
                            if (pair.Key.ToString() == searchName)
                            {
                                DataTable AMLDataTable = AutomationMLDataTables.AMLAttributeParameters();
                                AutomationMLDataTables.CreateDataTableWithColumns(AMLDataTable, elecInterAttDataGridView, pair);
                               
                            }

                        }
                        foreach (var pair in device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces)
                        {
                            if (pair.Key.ToString() == searchName)
                            {
                                foreach (var valueList in pair.Value)
                                {
                                    foreach (var item in valueList)
                                    {
                                        descriptionTxtBoxElecAttri.Text = item.Description;
                                        copyrightTxtBxElecAttri.Text = item.CopyRight;
                                        RefClassNameTxtBxElecAttri.Text = item.ReferencedClassName;
                                        RefBaseClassPathTxtBxElecAttri.Text = item.RefBaseClassPath;
                                        attributepathTxtBxElecAttri.Text = item.AttributePath;
                                        idTxtBxElecAttri.Text = item.ID;
                                    }
                                }
                            }

                        }

                    }
                    else
                    {
                        searchName = targetNode.Text;
                        electricalInterfacesHeaderlabel.Text = searchName;
                        //nameTxtBxElecAttri.Text = searchName;
                        foreach (var pair in device.DictionaryForInterfaceClassesInElectricalInterfaces)
                        {
                            if (pair.Key.ToString() == searchName)
                            {
                                DataTable AMLDataTable = AutomationMLDataTables.AMLAttributeParameters();
                                AutomationMLDataTables.CreateDataTableWithColumns(AMLDataTable, elecInterAttDataGridView, pair);
                               
                            }

                        }
                        foreach (var pair in device.DictionaryForInterfaceClassesInElectricalInterfaces)
                        {
                            if (pair.Key.ToString() == searchName)
                            {
                                
                                foreach (var valueList in pair.Value)
                                {
                                    foreach (var item in valueList)
                                    {
                                        descriptionTxtBoxElecAttri.Text = item.Description;
                                        copyrightTxtBxElecAttri.Text = item.CopyRight;
                                        RefClassNameTxtBxElecAttri.Text = item.ReferencedClassName;
                                        RefBaseClassPathTxtBxElecAttri.Text = item.RefBaseClassPath;
                                        attributepathTxtBxElecAttri.Text = item.AttributePath;
                                        idTxtBxElecAttri.Text = item.ID;
                                    }
                                }
                            }

                        }
                    }

                }
            }
            catch (Exception) {}
           
            

        }
        private void treeViewElectricalInterfaces_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                TreeNode targetNode = treeViewElectricalInterfaces.SelectedNode;

                targetNode.SelectedImageIndex = targetNode.ImageIndex;
            }
            catch (Exception){}
            
        }

        private void genericInformationDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {

           
            genericInformationtreeView.Nodes.Clear();

            TreeNode parentNode;
            TreeNode childNodes;
            string childNodeName = "";
            var AutomationMLDataTables = new AutomationMLDataTables();
            genericInformationDataGridView.CurrentRow.Selected = true;
            if (genericInformationDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                //if (Convert.ToBoolean(electricalInterfacesCollectionDataGridView.CurrentRow.Cells[3].Value) == false)
                {
                    genericparametersAttrDataGridView.Rows.Clear();
                    string roleClass = genericInformationDataGridView.CurrentRow.Cells[1].Value.ToString();

                   
                    parentNode = genericInformationtreeView.Nodes.Add(roleClass, roleClass, 2);
                    foreach (var pair in searchAMLLibraryFile.DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib)
                    {
                        if (pair.Key.Contains(roleClass))
                        {

                            childNodes = parentNode.Nodes.Add(pair.Key.Replace(parentNode.Name, "").ToString()
                                , pair.Key.Replace(parentNode.Name, "").ToString(), 2);
                        }
                    }

                    
                }

                

            }
        }

        
       
        private void toolStripButton6_Click(object sender, EventArgs e)
        {

        }

        private void genericInformationDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            genericInformationDataGridView.CurrentRow.Selected = true;
            if (genericInformationDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                AMC.WindowSizeChanger(genericInformationpanel);
            }
        }

        private void genericInformationtreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string searchName = "";
            var AutomationMLDataTables = new AutomationMLDataTables();

            TreeNode targetNode = genericInformationtreeView.SelectedNode;
            /* targetNode.SelectedImageIndex = targetNode.ImageIndex;*/

            genericparametersAttrDataGridView.Rows.Clear();

            try
            {
                if (e.Button == MouseButtons.Left)
                {

                    if (targetNode.Parent != null)
                    {
                        searchName = targetNode.Parent.Text + targetNode.Text;
                        foreach (var pair in searchAMLLibraryFile.DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib)
                        {
                            if (pair.Key.ToString() == searchName)
                            {
                                DataTable AMLDataTable = AutomationMLDataTables.AMLAttributeParameters();
                                AutomationMLDataTables.CreateDataTableWithColumns(AMLDataTable, genericparametersAttrDataGridView, pair);
                            }

                        }
                    }
                    else
                    {
                        searchName = targetNode.Text;
                        foreach (var pair in searchAMLLibraryFile.DictionaryForRoleClassInstanceAttributes)
                        {
                            if (pair.Key.ToString() == searchName)
                            {
                                DataTable AMLDataTable = AutomationMLDataTables.AMLAttributeParameters();
                                AutomationMLDataTables.CreateDataTableWithColumns(AMLDataTable, genericparametersAttrDataGridView, pair);
                            }

                        }
                    }

                }
            }
            catch (Exception) { }
        }

        private void genericInformationtreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                TreeNode targetNode = genericInformationtreeView.SelectedNode;

                targetNode.SelectedImageIndex = targetNode.ImageIndex;
            }
            catch (Exception) { }
        }
        public void ClearHeaderTabPageValuesofElectricalInterfaces()
        {
            descriptionTxtBoxElecAttri.Text = "";
            copyrightTxtBxElecAttri.Text = "";
            RefClassNameTxtBxElecAttri.Text = "";
            RefBaseClassPathTxtBxElecAttri.Text = "";
            attributepathTxtBxElecAttri.Text = "";
            idTxtBxElecAttri.Text = "";
            nameTxtBxElecAttri.Text = "";
        }

        private void elecInterAttDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            ClearHeaderTabPageValuesofElectricalInterfaces();
            elecInterAttDataGridView.CurrentRow.Selected = true;
            if (elecInterAttDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                string attributeName = elecInterAttDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                string values = elecInterAttDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                string defaults = elecInterAttDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                string Units = elecInterAttDataGridView.Rows[e.RowIndex].Cells[3].Value.ToString();
                string datatype = elecInterAttDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString();
                string semantics = elecInterAttDataGridView.Rows[e.RowIndex].Cells[5].Value.ToString();
               


                //if (Convert.ToBoolean(electricalInterfacesCollectionDataGridView.CurrentRow.Cells[3].Value) == false)
                {

                    string interfaceClass = electricalInterfacesHeaderlabel.Text;
                    foreach (var pair in device.DictionaryForInterfaceClassesInElectricalInterfaces)
                    {
                        if (pair.Key.ToString() == interfaceClass)
                        {
                            foreach (var valueList in pair.Value)
                            {
                                foreach (var item in valueList)
                                {
                                    if (item.Name.ToString() == attributeName)
                                    {
                                      

                                        descriptionTxtBoxElecAttri.Text = item.Description;
                                        copyrightTxtBxElecAttri.Text = item.CopyRight;
                                        RefClassNameTxtBxElecAttri.Text = item.ReferencedClassName;
                                        RefBaseClassPathTxtBxElecAttri.Text = item.RefBaseClassPath;
                                        attributepathTxtBxElecAttri.Text = item.AttributePath;
                                        idTxtBxElecAttri.Text = item.ID;
                                        nameTxtBxElecAttri.Text = item.Name;

                                        foreach (var pair2 in device.DictionaryForInterfaceClassesInElectricalInterfaces)
                                        {
                                            if (pair2.Key.ToString() == interfaceClass)
                                            {
                                                foreach (var valueList2 in pair2.Value)
                                                {
                                                    foreach (var item2 in valueList2)
                                                    {
                                                        if (item2.Name.ToString() == attributeName)
                                                        {
                                                            item2.Name = attributeName;
                                                            item2.Value = values;
                                                            item2.Default = defaults;
                                                            item2.Unit = Units;
                                                            item2.Semantic = semantics;
                                                            item2.Description = descriptionTxtBoxElecAttri.Text;
                                                            item2.CopyRight = copyrightTxtBxElecAttri.Text;
                                                            item2.ReferencedClassName = RefClassNameTxtBxElecAttri.Text;
                                                            item2.RefBaseClassPath = RefBaseClassPathTxtBxElecAttri.Text;
                                                            item2.AttributePath = attributepathTxtBxElecAttri.Text;
                                                            item2.ID = idTxtBxElecAttri.Text;
                                                            item2.Name = nameTxtBxElecAttri.Text;
                                                        }
                                                    }
                                                }
                                            }
                                            
                                        }
                                    }
                                }

                            }

                        }

                    }
                    foreach (var pair in device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces)
                    {
                        if (pair.Key.ToString() == interfaceClass)
                        {
                            foreach (var valueList in pair.Value)
                            {
                                foreach (var item in valueList)
                                {
                                    if (item.Name.ToString() == attributeName)
                                    {
                                       /* item.Value = values;
                                        item.Default = defaults;
                                        item.Unit = Units;
                                        item.Semantic = semantics;*/


                                        descriptionTxtBoxElecAttri.Text = item.Description;
                                        copyrightTxtBxElecAttri.Text = item.CopyRight;
                                        RefClassNameTxtBxElecAttri.Text = item.ReferencedClassName;
                                        RefBaseClassPathTxtBxElecAttri.Text = item.RefBaseClassPath;
                                        attributepathTxtBxElecAttri.Text = item.AttributePath;
                                        idTxtBxElecAttri.Text = item.ID;
                                        nameTxtBxElecAttri.Text = item.Name;

                                        foreach (var pair2 in device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces)
                                        {
                                            if (pair2.Key.ToString() == interfaceClass)
                                            {
                                                foreach (var valueList2 in pair2.Value)
                                                {
                                                    foreach (var item2 in valueList2)
                                                    {
                                                        if (item2.Name.ToString() == attributeName)
                                                        {
                                                            item2.Name = attributeName;
                                                            item2.Value = values;
                                                            item2.Default = defaults;
                                                            item2.Unit = Units;
                                                            item2.Semantic = semantics;
                                                            item2.Description = descriptionTxtBoxElecAttri.Text;
                                                            item2.CopyRight = copyrightTxtBxElecAttri.Text;
                                                            item2.ReferencedClassName = RefClassNameTxtBxElecAttri.Text;
                                                            item2.RefBaseClassPath = RefBaseClassPathTxtBxElecAttri.Text;
                                                            item2.AttributePath = attributepathTxtBxElecAttri.Text;
                                                            item2.ID = idTxtBxElecAttri.Text;
                                                            item2.Name = nameTxtBxElecAttri.Text;
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }

                            }

                        }

                    }
                }
            }
            elecInterAttDataGridView.CurrentRow.Selected = false;
        }

        private void saveeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
