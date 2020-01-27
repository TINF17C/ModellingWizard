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
        /// <summary>
        /// These are private fields of this class.
        /// </summary>
        private MWController mWController;
        private MWData.MWFileType filetype;


        bool isEditing = false;

        /// <summary>
        /// Instance of Animation Class is created.
        /// </summary>
        AnimationClass AMC = new AnimationClass();

        /// <summary>
        /// Instance of SearchforAMLLibraryFile is created.
        /// This class search for "Interface Class Libraries" and "Role Class Libraries" in AML file loaded by user into plugin.
        /// </summary>
        SearchAMLLibraryFile searchAMLLibraryFile = new SearchAMLLibraryFile();

        /// <summary>
        /// Instance of "SearchAMLComponentFile" is created
        /// This class search for "System Unit Class Libraries"  in AML Component  file loaded by user into plugin. 
        /// </summary>
        SearchAMLComponentFile searchAMLComponentFile = new SearchAMLComponentFile();

        /// <summary>
        /// Instance of MWDevice Class
        /// </summary>
        MWDevice device = new MWDevice();

        

        /// <summary>
        /// Constructor with no arguments that intilizes Device Description GUI
        /// </summary>
        public DeviceDescription()
        {
            InitializeComponent();

           
        }

        /// <summary>
        /// This is a constructor of this class with MWControlle rargument. 
        /// </summary>
        /// <param name="mWController"></param>
        public DeviceDescription(MWController mWController)
        {
            this.mWController = mWController;
            InitializeComponent();

            // After intialization of this GUI, plugin all this function to load Standard Libraries.  
            loadStandardLibrary();

            // This Function look for "AutomationComponent" Role and assign it to "Generic Data Tab" as a compulsary role along with attributes.
            checkForAutomtionComponent();

            // These are the dictionaries created in MWDevice Class to store attributes inside them.
            //These dictionaries are initiated as new dictionaries in here.
            device.DictionaryForInterfaceClassesInElectricalInterfaces = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();
            device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();

            device.DictionaryForRoleClassofComponent = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();
            device.DictionaryForExternalInterfacesUnderRoleClassofComponent = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();

        }

        /// <summary>
        /// This function loads "Interface Class Libraries" and"Role Class Libraries" from already defined libaraies in plugin or, 
        /// libraries from the AML file those user want ot load from local machine.
        /// </summary>
        public void loadStandardLibrary()
        {
            CAEXDocument doc = null;

            // These library already come along with plugin. This library is loaded into GUI automaticcally by plugin.
            doc = CAEXDocument.LoadFromBinary(Properties.Resources.AutomationComponentLibrary_v1_0_0_Full);

            //Following newly initiated dictionaries store "Interface Classes and its attributes" and "Role Classes and its attributes" of loaded file
            //in the respective libraries.

            //(Note:- This libaray is not used at all)
            searchAMLLibraryFile.dictionaryofRoleClassattributes = new Dictionary<string, List<ClassOfListsFromReferencefile>>();


            // These are the main libraraies used.
            searchAMLLibraryFile.DictionaryForInterfaceClassInstancesAttributes = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();
            searchAMLLibraryFile.DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();

            searchAMLLibraryFile.DictionaryForRoleClassInstanceAttributes = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();
            searchAMLLibraryFile.DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();


        //(´Note:- This library is not used ata all.)
            searchAMLLibraryFile.DictioanryOfIDofInterfaceClassLibraryNodes = new Dictionary<string, string>();

            // These are the tree hierarchies in the GUI, which has to be cleared all during intiation of plugin.
            treeViewRoleClassLib.Nodes.Clear();
            treeViewInterfaceClassLib.Nodes.Clear();

            {
                try
                {
                  
                    // This is a string variable that store the name of the "referenced name" of each "Interface Class in ICL of loaded file"
                    // and/or "Referenced name" of each "Role Class in RCL of loaded file" 
                    string referencedClassName = "";

                    // This foreach loop look into every individual "Role Class libaray" in RCL in the loaded file. 
                    foreach (var classLibType in doc.CAEXFile.RoleClassLib)
                    {
                        // This Populate Role Class Tree Node in GUI
                        TreeNode libNode = treeViewRoleClassLib.Nodes.Add(classLibType.ToString(), classLibType.ToString(), 0);

                        // This foreach loop looks inside the individual "Role Class" 
                        foreach (var classType in classLibType.RoleClass)
                        {

                            TreeNode roleNode;

                            // This If loop check for the "refernced name" of each role class.
                            if (classType.ReferencedClassName != "")
                            {
                                //Store "referenced name" in the String that declared above "referencedClassName"
                                referencedClassName = classType.ReferencedClassName;
                                // Print the role  node
                                roleNode = libNode.Nodes.Add(classType.ToString(), classType.ToString() + "{" + "Class:" + "  " + referencedClassName + "}", 1);

                                // Search for the "refernced name" (This referenced name will be as an "Role Class" in the RCL).....
                                //.....in the whole RCL to find the attribute behind it and also its further "referenced name"
                                searchAMLLibraryFile.SearchForReferencedClassName(doc, referencedClassName, classType);
                                //This method is responsible to check attributes of referenced Class
                                searchAMLLibraryFile.CheckForAttributesOfReferencedClassName(classType);

                            }
                            // If there is no "Referenced Class name" then just print the name in GUI.
                            else
                            {
                                roleNode = libNode.Nodes.Add(classType.ToString(), classType.ToString(), 1);
                            }


                            // This If loop check for the "ExternalInterface" under each role class.
                            if (classType.ExternalInterface.Exists)
                            {
                                // This foreach loop look for number of "ExternalInterfaces" under "Role Class"
                                foreach (var externalinterface in classType.ExternalInterface)
                                {
                                    TreeNode externalinterfacenode;

                                    // This If loop check for the "refernced name" of each externalinterface.
                                    if (externalinterface.BaseClass != null)
                                    {
                                        referencedClassName = externalinterface.BaseClass.ToString();
                                        externalinterfacenode = roleNode.Nodes.Add(externalinterface.ToString(), externalinterface.ToString() + "{" + "Class:" + "  " + referencedClassName + "}", 2);
                                        externalinterfacenode.ForeColor = SystemColors.GrayText;

                                        //This method is responsible to check for "Referenced Class Name" of "External Interfaces" under the "Role Class"
                                        searchAMLLibraryFile.SearchForReferencedClassNameofExternalIterface(doc, referencedClassName, classType, externalinterface);

                                        // This Function is responsible to search attributes under the "Referenced Classs Name" i.e. in this part "RoleFamilyType"
                                        searchAMLLibraryFile.CheckForAttributesOfReferencedClassNameofExternalIterface(classType, externalinterface);


                                    }
                                    //Else directly print the node.
                                    else
                                    {
                                        externalinterfacenode = roleNode.Nodes.Add(externalinterface.ToString(), externalinterface.ToString(), 2);
                                        // searchAMLLibraryFile.CheckForAttributesOfReferencedClassNameofExternalIterface(classType, externalinterface);
                                    }


                                    //This method is called to print "External Interfaces" in both "Role class Library and Interface Class Library" in the plugin.
                                    searchAMLLibraryFile.PrintExternalInterfaceNodes(doc, externalinterfacenode, externalinterface, classType);
                                }

                            }
                            //This method takes arguments "TreeNode" and "RoleFamilyType" to print tree nodes in "Role Class Library TreeView " in Plugin.
                            searchAMLLibraryFile.PrintNodesRecursiveInRoleClassLib(doc, roleNode, classType, referencedClassName);
                        }

                    }

                    foreach (var classLibType in doc.CAEXFile.InterfaceClassLib)
                    {
                        // Print a "Interface Class lib" treenode in GUI 
                        TreeNode libNode = treeViewInterfaceClassLib.Nodes.Add(classLibType.ToString(), classLibType.ToString(), 0);


                        // for each "interface classlib" print chlid nodes of "Interface Classes"
                        foreach (var classType in classLibType.InterfaceClass)
                        {

                            TreeNode interfaceclassNode;
                            //If "refernced Class Name" is not null
                            if (classType.ReferencedClassName != "")
                            {
                                // Print Child node...

                                referencedClassName = classType.ReferencedClassName;
                                interfaceclassNode = libNode.Nodes.Add(classType.ToString(), classType.ToString() + "{" + "Class:" + "  " + referencedClassName + "}", 1);

                                //This method search for "Referenced Class Name" "Interface Class"
                                searchAMLLibraryFile.SearchForReferencedClassName(doc, referencedClassName, classType);
                                //
                                searchAMLLibraryFile.CheckForAttributesOfReferencedClassName(classType);

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
                                        externalinterfacenode.ForeColor = SystemColors.GrayText;

                                        searchAMLLibraryFile.SearchForReferencedClassNameofExternalIterface(doc, referencedClassName, classType, externalinterface);
                                        searchAMLLibraryFile.CheckForAttributesOfReferencedClassNameofExternalIterface(classType, externalinterface);
                                    }
                                    else
                                    {
                                        //searchAMLLibraryFile.DictioanryOfIDofInterfaceClassLibraryNodes.Add(classType.Name.ToString() + externalinterface.ToString(), externalinterface.ID.ToString());

                                        externalinterfacenode = interfaceclassNode.Nodes.Add(externalinterface.ToString(), externalinterface.ToString(), 2);
                                    }


                                    searchAMLLibraryFile.PrintExternalInterfaceNodes(doc, externalinterfacenode, externalinterface, classType);
                                }
                            }
                            searchAMLLibraryFile.PrintNodesRecursiveInInterfaceClassLib(doc, interfaceclassNode, classType, referencedClassName);
                        }

                    }

                }


                catch (Exception)
                {

                    MessageBox.Show("Missing names of attributes or Same atrribute sequence is repeated in the given file", "Missing Names", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }

            }

        }

        public void checkForAutomtionComponent()
        {
            foreach (TreeNode node in treeViewRoleClassLib.Nodes)
            {
                if (node.Nodes != null)
                {
                    foreach (TreeNode childNode in node.Nodes)
                    {
                        if (childNode.Name == "AutomationComponent")
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

                            genericInformationDataGridView.Rows[num].Cells[1].Value = childNode.Text.ToString();
                            genericInformationDataGridView.Rows[num].Cells[3].Value = true;
/*
                            int rowindex = genericInformationDataGridView.Rows[num].Cells[1].RowIndex;
                            int columnindex = genericInformationDataGridView.Rows[num].Cells[1].ColumnIndex;

                            genericInformationDataGridView_CellClick(new object(), new DataGridViewCellEventArgs(columnindex, rowindex));*/
                        }
                    }
                }
               
            }
        }
        
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (vendorNameTextBox.Text == "" && deviceNameTextBox.Text=="")
            {
                MessageBox.Show("Enter Vendor Name and Device Name before saving an Autoamtion Component","Missing Fields",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            if (vendorNameTextBox.Text != null && deviceNameTextBox.Text != null)
            {
                device.vendorName = vendorNameTextBox.Text;

                device.deviceName = deviceNameTextBox.Text;


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


                if (fileNameLabel.Text == "")
                {

                    try
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();

                       // saveFileDialog.Filter = "AML Files( *.amlx )| *.amlx;";
                        saveFileDialog.FileName = vendorNameTextBox.Text + "-" + deviceNameTextBox.Text + "-V.1.0-" + DateTime.Now.Date.ToShortDateString();

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {

                            device.filepath = Path.GetDirectoryName(saveFileDialog.FileName);
                            device.environment = Path.GetDirectoryName(saveFileDialog.FileName);
                            filePathLabel.Text = Path.GetDirectoryName(saveFileDialog.FileName);
                            device.fileName = saveFileDialog.FileName;


                            fileNameLabel.Text = "";
                            // storing user defined values of Attachebles data grid view in to list 

                            // Pass the device to the controller
                            string result1 = mWController.CreateDeviceOnClick(device, isEditing);

                            
                            
                            //clear();

                            // Display the result
                            if (result1 != null)
                            {
                                // Display error Dialog
                                MessageBox.Show(result1, "Automation Component Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                       

                    }
                    catch (Exception)
                    {

                        throw;
                    }

                }
                if (fileNameLabel.Text != "")
                {
                    //device.filepath = filePathLabel.Text;
                   // device.environment = Path.GetDirectoryName(saveFileDialog.FileName);
                    device.fileName = vendorNameTextBox.Text + "-" + deviceNameTextBox.Text + "-V.1.0-" + DateTime.Now.Date.ToShortDateString();

                    fileNameLabel.Text = "";
                    // storing user defined values of Attachebles data grid view in to list 

                    // Pass the device to the controller
                    string result = mWController.CreateDeviceOnClick(device, isEditing);


                    //clear();

                    // Display the result
                    if (result != null)
                    {
                        // Display error Dialog
                        MessageBox.Show(result, "Automation Component Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
              
               

                device.DictionaryForInterfaceClassesInElectricalInterfaces = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();
                device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();


                device.DictionaryForRoleClassofComponent = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();
                device.DictionaryForExternalInterfacesUnderRoleClassofComponent = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();

                // Assigning values and parameters in "Identification data grid" to properties given in class "DatatableParametersCarrier" in MWDevice


            }


        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clear();
           

            loadStandardLibrary();
            checkForAutomtionComponent();

            foreach (DataGridViewRow row in genericInformationDataGridView.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    if (row.Cells[0].Value.ToString() == "1" && row.Cells[1].Value.ToString() == "AutomationComponent{Class:  AutomationMLBaseRole}")
                    {
                        string SRCSerialNumber = row.Cells[0].Value.ToString();
                        string SRC = row.Cells[1].Value.ToString();
                        foreach (var pair in searchAMLLibraryFile.DictionaryForRoleClassInstanceAttributes)
                        {
                            if (pair.Key.ToString() == SRC)
                            {
                                try
                                {
                                    if (device.DictionaryForRoleClassofComponent.ContainsKey("(" + SRCSerialNumber + ")" + SRC))
                                    {
                                        device.DictionaryForRoleClassofComponent.Remove("(" + SRCSerialNumber + ")" + SRC);
                                        device.DictionaryForRoleClassofComponent.Add("(" + SRCSerialNumber + ")" + SRC, pair.Value);
                                    }
                                    else
                                    {
                                        device.DictionaryForRoleClassofComponent.Add("(" + SRCSerialNumber + ")" + SRC, pair.Value);
                                    }
                                }
                                catch (Exception)
                                {

                                    throw;
                                }

                            }

                        }

                    }
                }
               
            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                Environment.Exit(0);

            }
        }

        private void ClearDeviceDataBtn_Click(object sender, EventArgs e)
        {
            vendorNameTextBox.Text = "";
           
            deviceNameTextBox.Text = "";
           
        }

        
        public void clear()
        {
            vendorNameTextBox.Text = "";
            deviceNameTextBox.Text = "";
            genericInformationDataGridView.Rows.Clear();
            genericInformationtreeView.Nodes.Clear();
            genericparametersAttrDataGridView.Rows.Clear();
            attachablesInfoDataGridView.Rows.Clear();
            electricalInterfacesCollectionDataGridView.Rows.Clear();
            elecInterAttDataGridView.Rows.Clear();
            treeViewElectricalInterfaces.Nodes.Clear();

            device.DictionaryForInterfaceClassesInElectricalInterfaces = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();
            device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();


            device.DictionaryForRoleClassofComponent = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();
            device.DictionaryForExternalInterfacesUnderRoleClassofComponent = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();



        }



        private void IdentificationDataBtn_Click(object sender, EventArgs e)
        {
            
        }

        private void CommercialDataBtn_Click(object sender, EventArgs e)
        {
           
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

            searchAMLComponentFile.DictionaryofRolesforAutomationComponenet = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();


            device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();
            device.DictionaryForExternalInterfacesUnderRoleClassofComponent = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();
            device.DictionaryForInterfaceClassesInElectricalInterfaces = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();
            device.DictionaryForRoleClassofComponent = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();
            device.DictofElectricalInterfaceParameters = new Dictionary<string, List<ElectricalInterfaceParameters>>();
           
            CAEXDocument document = null;
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "AML Files(*.aml; *.amlx;*.xml;*.AML )|*.aml; *.amlx;*.xml;*.AML;";
            clear();
            if (open.ShowDialog() == DialogResult.OK)
                device.filepath = Path.GetDirectoryName(open.FileName);
            {
                try
                {
                    string file = open.FileName;
                    FileInfo fileInfo = new FileInfo(file);
                    string objectName = fileInfo.Name;
                    
                    
                   

                   // DirectoryInfo directory = new DirectoryInfo(Path.GetDirectoryName(file));

                    string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                    Directory.CreateDirectory(tempDirectory);

                    DirectoryInfo directory = new DirectoryInfo(tempDirectory);
                    // Load the amlx container from the given filepath

                   
                    AutomationMLContainer amlx = new AutomationMLContainer(file);
                   

                    amlx.ExtractAllFiles(tempDirectory);

                   

                    //amlx.ExtractAllFiles(Path.GetDirectoryName(file));
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


                    fileNameLabel.Text = fileInfo.Name;
                    foreach (var classLibType in document.CAEXFile.SystemUnitClassLib)
                    {
                        foreach (var classType in classLibType.SystemUnitClass)
                        {
                            if (classType.SupportedRoleClass.Exists)
                            {
                                int i = 1;
                                foreach (var SRC in classType.SupportedRoleClass)
                                {
                                    if (classType.Attribute.Exists)
                                    {
                                      
                                       
                                        foreach (var attribute in classType.Attribute)
                                        {
                                            searchForComponentNames(attribute);
                                            if (attribute.Name == "Manufacturer")
                                            {
                                                if (attribute.Value != null)
                                                {
                                                    vendorNameTextBox.Text = attribute.Value;
                                                }
                                                else
                                                {
                                                    vendorNameTextBox.Text = "No Vendor Name Set";
                                                }
                                              
                                            }
                                            if (attribute.Name == "Model")
                                            {
                                                if (attribute.Value != null)
                                                {
                                                    deviceNameTextBox.Text = attribute.Value;
                                                }
                                                else
                                                {
                                                    deviceNameTextBox.Text = "No Device Name Set";
                                                }
                                               
                                            }
                                            
                                        }
                                    }
                                   
                                   
                                    searchAMLComponentFile.CheckForAttributesOfComponent(i, SRC, classType); 

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

                                    genericInformationDataGridView.Rows[num].Cells[1].Value = "(" + i + ")" + SRC.RoleReference.ToString()
                                       /* + "{" + "Class:" + "  " + electricalConnectorType.BaseClass + "}"*/;
                                    genericInformationDataGridView.Rows[num].Cells[4].Value = true;

                                    /*int rowindex = genericInformationDataGridView.Rows[num].Cells[1].RowIndex;
                                    int columnindex = genericInformationDataGridView.Rows[num].Cells[1].ColumnIndex;

                                    genericInformationDataGridView_CellClick(new object(), new DataGridViewCellEventArgs(columnindex, rowindex));*/

                                    genericInformationtreeView.Nodes.Clear();

                                    TreeNode parentNode;
                                    TreeNode childNodes;

                                    var AutomationMLDataTables = new AutomationMLDataTables();
                                    genericInformationDataGridView.CurrentRow.Selected = true;


                                    if (genericInformationDataGridView.Rows[num].Cells[0].Value != null)
                                    {
                                        string SRCSerialNumber = genericInformationDataGridView.Rows[num].Cells[0].Value.ToString();

                                       

                                        if (Convert.ToBoolean(genericInformationDataGridView.Rows[num].Cells[4].Value) == true)
                                        {
                                            genericparametersAttrDataGridView.Rows.Clear();
                                            string SRCName = genericInformationDataGridView.Rows[num].Cells[1].Value.ToString();
                                            foreach (var pair in searchAMLComponentFile.DictionaryofRolesforAutomationComponenet)
                                            {
                                                if (pair.Key.ToString() == SRCName)
                                                {
                                                    try
                                                    {
                                                        if (device.DictionaryForRoleClassofComponent.ContainsKey("(" + SRCSerialNumber + ")" + SRCName))
                                                        {
                                                            device.DictionaryForRoleClassofComponent.Remove("(" + SRCSerialNumber + ")" + SRCName);
                                                            device.DictionaryForRoleClassofComponent.Add("(" + SRCSerialNumber + ")" + SRCName, pair.Value);
                                                        }
                                                        else
                                                        {
                                                            device.DictionaryForRoleClassofComponent.Add("(" + SRCSerialNumber + ")" + SRCName, pair.Value);
                                                        }
                                                    }
                                                    catch (Exception)
                                                    {

                                                        throw;
                                                    }

                                                }

                                            }


                                            parentNode = genericInformationtreeView.Nodes.Add("(" + SRCSerialNumber + ")" + SRCName,
                                                "(" + SRCSerialNumber + ")" + SRCName, 2);




                                        }



                                    }
                                    vendorNameTextBox_Leave(new object(), new EventArgs());
                                    deviceNameTextBox_Leave(new object(), new EventArgs());



                                    i++;
                                }
                            }

                            foreach (var internalElements in classType.InternalElement)
                            {
                                /*if (internalElements.Name.Equals("DeviceIdentification"))
                                {
                                    foreach (var attribute in internalElements.Attribute)
                                    {
                                        switch (attribute.Name)
                                        {
                                          
                                            case "VendorName":
                                               vendorNameTextBox.Text = attribute.Value;
                                                break;
                                           
                                            case "DeviceName":
                                                deviceNameTextBox.Text = attribute.Value;
                                                break;
                                          
                                        }
                                    }
                                }*/
                                if (internalElements.Name != "ElectricalInterfaces" && internalElements.Name != "DeviceIdentification")
                                {
                                   

                                    int num = attachablesInfoDataGridView.Rows.Add();
                                    attachablesInfoDataGridView.Rows[num].Cells[0].Value = internalElements.Name;
                                    foreach (var externalInterface in internalElements.ExternalInterface)
                                    {
                                       
                                        foreach (var attribute in externalInterface.Attribute)
                                        {
                                            if (attribute.Value.Contains("https://") || attribute.Value.Contains("http://") || attribute.Value.Contains("www") || attribute.Value.Contains("WWW"))
                                            {
                                                attachablesInfoDataGridView.Rows[num].Cells[1].Value = attribute.Value;
                                                attachablesInfoDataGridView.Rows[num].Cells[2].Value = true;
                                            }

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
                                    foreach (var subinternalElements in internalElements.InternalElement)
                                    {
                                        foreach (var electricalConnectorType in subinternalElements.ExternalInterface)
                                        {
                                           
                                            if (electricalConnectorType != null)
                                            {

                                                searchAMLComponentFile.CheckForAttributesOfExternalIterface(i, electricalConnectorType);

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
                                                    + "{" + "Class:" + "  " + electricalConnectorType.BaseClass + "}";
                                                electricalInterfacesCollectionDataGridView.Rows[num].Cells[4].Value = true;

                                               
                                              /* int rowindex = electricalInterfacesCollectionDataGridView.Rows[num].Cells[1].RowIndex;
                                                int columnindex = electricalInterfacesCollectionDataGridView.Rows[num].Cells[1].ColumnIndex;*/

                                                


                                                foreach (var electricalConnectorPins in electricalConnectorType.ExternalInterface)
                                                {
                                                    if (electricalConnectorPins != null)
                                                    {
                                                        searchAMLComponentFile.CheckForAttributesOfEclectricalConnectorPins(i, electricalConnectorPins, electricalConnectorType);
                                                    }
                                                }


                                                treeViewElectricalInterfaces.Nodes.Clear();

                                                TreeNode parentNode;
                                                TreeNode childNodes;

                                                var AutomationMLDataTables = new AutomationMLDataTables();
                                                electricalInterfacesCollectionDataGridView.CurrentRow.Selected = true;


                                                if (electricalInterfacesCollectionDataGridView.Rows[num].Cells[0].Value != null)
                                                {
                                                    string interfaceSerialNumber = electricalInterfacesCollectionDataGridView.Rows[num].Cells[0].Value.ToString();


                                                    if (Convert.ToBoolean(electricalInterfacesCollectionDataGridView.Rows[num].Cells[4].Value) == true)
                                                    {
                                                        elecInterAttDataGridView.Rows.Clear();
                                                        string interfaceClass = electricalInterfacesCollectionDataGridView.Rows[num].Cells[1].Value.ToString();
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
                                                /* electricalInterfacesCollectionDataGridView_CellClick(new object(), new DataGridViewCellEventArgs(columnindex, rowindex));*/
                                            }

                                        }
                                        

                                        i++;
                                    }
                                }
                            }
                        }
                        
                    }
                   
                    amlx.Dispose();

                    amlx.Close();
                   

                }
                catch { }
                try
                {
                    if (open.FileName != null)
                    {
                        DirectoryInfo newdirectory = new DirectoryInfo(Path.GetDirectoryName(open.FileName));
                        foreach (FileInfo fileInfos in newdirectory.GetFiles())
                        {
                            if (fileInfos.Extension != ".amlx")
                            {
                                fileInfos.Delete();
                            }


                        }
                    }
                }
                catch (Exception)
                {
                }
               
            }
        }

        public void searchForComponentNames(AttributeType classType)
        {
            if (classType.Attribute.Exists)
            {
                
                foreach (var attribute in classType.Attribute)
                {
                    searchForComponentNames(attribute);
                    if (attribute.Name == "Manufacturer")
                    {
                        if (attribute.Value != null)
                        {
                            vendorNameTextBox.Text = attribute.Value;
                        }
                        else
                        {
                            vendorNameTextBox.Text = "No Vendor Name Set";
                        }
                    }
                    if (attribute.Name == "Model")
                    {
                        if (attribute.Value != null)
                        {
                            deviceNameTextBox.Text = attribute.Value;
                        }
                        else
                        {
                            deviceNameTextBox.Text = "No Device Name Set";
                        }
                    }
                }
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
            if (treeViewInterfaceClassLib.SelectedNode == null)
            {
                dragging = false;
            }
            else
            {
                if (treeViewInterfaceClassLib.SelectedNode.ImageIndex == 2)
                {
                    return;
                }
                else
                {
                    dragging = true;
                    row = new object();


                    treeViewInterfaceClassLib.SelectedNode = (TreeNode)e.Item;//dragging doesn't automatically change the selected index
                    row = treeViewInterfaceClassLib.SelectedNode.Text;//or whatever value you need from the node
                }
            }
            
           
           
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

                electricalInterfacesCollectionDataGridView.Rows[num].Selected = false;

                /*int rowindex = electricalInterfacesCollectionDataGridView.Rows[num].Cells[1].RowIndex;
                 int columnindex = electricalInterfacesCollectionDataGridView.Rows[num].Cells[1].ColumnIndex;


                electricalInterfacesCollectionDataGridView_CellClick(new object(), new DataGridViewCellEventArgs(columnindex, rowindex));*/

                treeViewElectricalInterfaces.Nodes.Clear();

                TreeNode parentNode;
                TreeNode childNodes;

                var AutomationMLDataTables = new AutomationMLDataTables();
                electricalInterfacesCollectionDataGridView.CurrentRow.Selected = true;


                if (electricalInterfacesCollectionDataGridView.Rows[num].Cells[0].Value != null)
                {
                    string interfaceSerialNumber = electricalInterfacesCollectionDataGridView.Rows[num].Cells[0].Value.ToString();

                    if (Convert.ToBoolean(electricalInterfacesCollectionDataGridView.Rows[num].Cells[3].Value) == true)
                    {
                        elecInterAttDataGridView.Rows.Clear();
                        string interfaceClass = electricalInterfacesCollectionDataGridView.Rows[num].Cells[1].Value.ToString();
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


                        foreach (var pair in searchAMLLibraryFile.DictionaryForExternalInterfacesInstanceAttributesofInterfaceClassLib)
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

                        electricalInterfacesCollectionDataGridView.CurrentRow.Cells[3].Value = true;
                    }

                    if (Convert.ToBoolean(electricalInterfacesCollectionDataGridView.Rows[num].Cells[4].Value) == true)
                    {
                        elecInterAttDataGridView.Rows.Clear();
                        string interfaceClass = electricalInterfacesCollectionDataGridView.Rows[num].Cells[1].Value.ToString();
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
            if (treeViewRoleClassLib.SelectedNode == null)
            {
                dragging = false;
            }
            else
            {
                if (treeViewRoleClassLib.SelectedNode.ImageIndex == 2)
                {
                    return;
                }
                else
                {
                    dragging = true;
                    row = new object();
                    treeViewRoleClassLib.SelectedNode = (TreeNode)e.Item;//dragging doesn't automatically change the selected index
                    row = treeViewRoleClassLib.SelectedNode.Text;//or whatever value you need from the node
                }
            }
           
           
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
                genericInformationDataGridView.Rows[num].Cells[3].Value = true;

                /*int rowindex = genericInformationDataGridView.Rows[num].Cells[1].RowIndex;
                int columnindex = genericInformationDataGridView.Rows[num].Cells[1].ColumnIndex;

                genericInformationDataGridView_CellClick(new object(), new DataGridViewCellEventArgs(columnindex, rowindex));*/

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
                    genericInformationDataGridView.CurrentRow.Selected = true;
                    string interfaceSerialNumber = genericInformationDataGridView.Rows[rowIndex].Cells[0].Value.ToString();
                    string interfaceClass = genericInformationDataGridView.Rows[rowIndex].Cells[1].Value.ToString();

                    try
                    {
                        if (device.DictionaryForRoleClassofComponent.ContainsKey("(" + interfaceSerialNumber + ")" + interfaceClass))
                        {
                            device.DictionaryForRoleClassofComponent.Remove("(" + interfaceSerialNumber + ")" + interfaceClass);

                        }


                    }
                    catch (Exception)
                    {

                        throw;
                    }

                    foreach (var pair in searchAMLLibraryFile.DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib)
                    {
                        if (pair.Key.Contains(interfaceClass))
                        {
                            try
                            {
                                if (device.DictionaryForExternalInterfacesUnderRoleClassofComponent.ContainsKey("(" + interfaceSerialNumber + ")" + pair.Key.ToString()))
                                {
                                    device.DictionaryForExternalInterfacesUnderRoleClassofComponent.Remove("(" + interfaceSerialNumber + ")" + pair.Key.ToString());

                                }

                            }
                            catch (Exception)
                            {

                                throw;
                            }

                        }
                    }

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
           // if (electricalInterfacesCollectionDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
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


            /*genericInformationtreeView.Nodes.Clear();

            TreeNode parentNode;
            TreeNode childNodes;
            
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

                

            }*/
            genericInformationtreeView.Nodes.Clear();

            TreeNode parentNode;
            TreeNode childNodes;

            var AutomationMLDataTables = new AutomationMLDataTables();
            genericInformationDataGridView.CurrentRow.Selected = true;


            if (genericInformationDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                string SRCSerialNumber = genericInformationDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();

                if (Convert.ToBoolean(genericInformationDataGridView.CurrentRow.Cells[3].Value) == true)
                {
                    genericparametersAttrDataGridView.Rows.Clear();

                    string SRC = genericInformationDataGridView.CurrentRow.Cells[1].Value.ToString();

                    foreach (var pair in searchAMLLibraryFile.DictionaryForRoleClassInstanceAttributes)
                    {
                        if (pair.Key.ToString() == SRC)
                        {
                            try
                            {
                                if (device.DictionaryForRoleClassofComponent.ContainsKey("(" + SRCSerialNumber + ")" + SRC))
                                {
                                    device.DictionaryForRoleClassofComponent.Remove("(" + SRCSerialNumber + ")" + SRC);
                                    device.DictionaryForRoleClassofComponent.Add("(" + SRCSerialNumber + ")" + SRC, pair.Value);
                                }
                                else
                                {
                                    device.DictionaryForRoleClassofComponent.Add("(" + SRCSerialNumber + ")" + SRC, pair.Value);
                                }
                            }
                            catch (Exception)
                            {

                                throw;
                            }

                        }

                    }


                    parentNode = genericInformationtreeView.Nodes.Add("(" + SRCSerialNumber + ")" + SRC,
                        "(" + SRCSerialNumber + ")" + SRC, 2);


                    foreach (var pair in searchAMLLibraryFile.DictionaryForExternalInterfacesInstancesAttributesOfRoleClassLib)
                    {
                        if (pair.Key.Contains(SRC))
                        {
                            try
                            {
                                if (device.DictionaryForExternalInterfacesUnderRoleClassofComponent.ContainsKey("(" + SRCSerialNumber + ")" + pair.Key.ToString()))
                                {
                                    device.DictionaryForExternalInterfacesUnderRoleClassofComponent.Remove("(" + SRCSerialNumber + ")" + pair.Key.ToString());
                                    device.DictionaryForExternalInterfacesUnderRoleClassofComponent.Add("(" + SRCSerialNumber + ")" + pair.Key.ToString(), pair.Value);
                                }
                                else
                                {
                                    device.DictionaryForExternalInterfacesUnderRoleClassofComponent.Add("(" + SRCSerialNumber + ")" + pair.Key.ToString(), pair.Value);
                                }
                            }
                            catch (Exception)
                            {

                                throw;
                            }


                            childNodes = parentNode.Nodes.Add(pair.Key.Replace(SRC, "").ToString()
                                , pair.Key.Replace(SRC, "").ToString(), 2);
                        }
                    }

                    genericInformationDataGridView.CurrentRow.Cells[3].Value = true;
                }

                if (Convert.ToBoolean(genericInformationDataGridView.CurrentRow.Cells[4].Value) == true)
                {
                    genericparametersAttrDataGridView.Rows.Clear();
                    string SRC = genericInformationDataGridView.CurrentRow.Cells[1].Value.ToString();
                    foreach (var pair in searchAMLComponentFile.DictionaryofRolesforAutomationComponenet)
                    {
                        if (pair.Key.ToString() == SRC)
                        {
                            try
                            {
                                if (device.DictionaryForRoleClassofComponent.ContainsKey("(" + SRCSerialNumber + ")" + SRC))
                                {
                                    device.DictionaryForRoleClassofComponent.Remove("(" + SRCSerialNumber + ")" + SRC);
                                    device.DictionaryForRoleClassofComponent.Add("(" + SRCSerialNumber + ")" + SRC, pair.Value);
                                }
                                else
                                {
                                    device.DictionaryForRoleClassofComponent.Add("(" + SRCSerialNumber + ")" + SRC, pair.Value);
                                }
                            }
                            catch (Exception)
                            {

                                throw;
                            }

                        }

                    }


                    parentNode = genericInformationtreeView.Nodes.Add("(" + SRCSerialNumber + ")" + SRC,
                        "(" + SRCSerialNumber + ")" + SRC, 2);



                   
                }



            }
            vendorNameTextBox_Leave(new object(), new EventArgs());
            deviceNameTextBox_Leave(new object(), new EventArgs());


        }



        

        private void genericInformationDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            genericInformationDataGridView.CurrentRow.Selected = true;
            //if (genericInformationDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
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
            ClearHeaderTabPageValuesofgenericData();
            genericparametersAttrDataGridView.Rows.Clear();

            try
            {
                if (e.Button == MouseButtons.Left)
                {

                    if (targetNode.Parent != null)
                    {
                        searchName = targetNode.Parent.Text + targetNode.Text;
                        genericDataHeaderLabel.Text = searchName;
                        foreach (var pair in device.DictionaryForExternalInterfacesUnderRoleClassofComponent)
                        {
                            if (pair.Key.ToString() == searchName)
                            {
                                DataTable AMLDataTable = AutomationMLDataTables.AMLAttributeParameters();
                                AutomationMLDataTables.CreateDataTableWithColumns(AMLDataTable, genericparametersAttrDataGridView, pair);
                            }

                        }
                        foreach (var pair in device.DictionaryForExternalInterfacesUnderRoleClassofComponent)
                        {
                            if (pair.Key.ToString() == searchName)
                            {
                                foreach (var valueList in pair.Value)
                                {
                                    foreach (var item in valueList)
                                    {
                                        genericDataDescriptionTxtBx.Text = item.Description;
                                        genericDataCopyrightTxtBx.Text = item.CopyRight;
                                        genericDataRefClassNameTxtBx.Text = item.ReferencedClassName;
                                        genericDataRefBaseClassPathTxtBx.Text = item.RefBaseClassPath;
                                        genericDataAttributePathTxtBx.Text = item.AttributePath;
                                        genericDataIDTxtBx.Text = item.ID;
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                       
                        searchName = targetNode.Text;
                        genericDataHeaderLabel.Text = searchName;
                        foreach (var pair in device.DictionaryForRoleClassofComponent)
                        {
                            if (pair.Key.ToString() == searchName)
                            {
                                DataTable AMLDataTable = AutomationMLDataTables.AMLAttributeParameters();
                                AutomationMLDataTables.CreateDataTableWithColumns(AMLDataTable, genericparametersAttrDataGridView, pair);
                                
                            }

                        }
                        foreach (var pair in device.DictionaryForRoleClassofComponent)
                        {
                            if (pair.Key.ToString() == searchName)
                            {

                                foreach (var valueList in pair.Value)
                                {
                                    foreach (var item in valueList)
                                    {
                                        genericDataDescriptionTxtBx.Text = item.Description;
                                        genericDataCopyrightTxtBx.Text = item.CopyRight;
                                        genericDataRefClassNameTxtBx.Text = item.ReferencedClassName;
                                        genericDataRefBaseClassPathTxtBx.Text = item.RefBaseClassPath;
                                        genericDataAttributePathTxtBx.Text = item.AttributePath;
                                        genericDataIDTxtBx.Text = item.ID;
                                    }
                                }
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
        public void ClearHeaderTabPageValuesofgenericData()
        {
            genericDataDescriptionTxtBx.Text = "";
            genericDataCopyrightTxtBx.Text = "";
            genericDataRefClassNameTxtBx.Text = "";
            genericDataRefBaseClassPathTxtBx.Text = "";
            genericDataAttributePathTxtBx.Text = "";
            genericDataIDTxtBx.Text = "";
            genericDataNameTxtBx.Text = "";
        }

        private void elecInterAttDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void saveeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (vendorNameTextBox.Text == "" && deviceNameTextBox.Text == "")
            {
                MessageBox.Show("Enter Vendor Name and Device Name before saving an Autoamtion Component", "Missing Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            device.vendorName = vendorNameTextBox.Text;

            device.deviceName = deviceNameTextBox.Text;


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


            // if (generateAML.Text == "Save AML File")
            
                try
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();

                   
                    saveFileDialog.FileName = vendorNameTextBox.Text + "-" + deviceNameTextBox.Text + "-V.1.0-" + DateTime.Now.Date.ToShortDateString();
                    
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                       
                        device.filepath = Path.GetDirectoryName(saveFileDialog.FileName);
                        device.environment = Path.GetDirectoryName(saveFileDialog.FileName);
                        device.fileName = saveFileDialog.FileName;
                    }
                   /* if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                    {
                        return;
                    }
*/
                }
                catch (Exception)
                {

                    throw;
                }

            

            fileNameLabel.Text = "";
            // storing user defined values of Attachebles data grid view in to list 



            // Pass the device to the controller
            string result = mWController.CreateDeviceOnClick(device, isEditing);


            //clear();

            // Display the result
            if (result != null)
            {
                // Display error Dialog
                MessageBox.Show(result, "Automation Component Saved",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }

            device.DictionaryForInterfaceClassesInElectricalInterfaces = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();
            device.DictionaryForExternalInterfacesUnderInterfaceClassInElectricalInterfaces = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();


            device.DictionaryForRoleClassofComponent = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();
            device.DictionaryForExternalInterfacesUnderRoleClassofComponent = new Dictionary<string, List<List<ClassOfListsFromReferencefile>>>();

            // Assigning values and parameters in "Identification data grid" to properties given in class "DatatableParametersCarrier" in MWDevice

        }

        private void treeViewElectricalInterfaces_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode targetNode = treeViewRoleClassLib.SelectedNode;
                targetNode.SelectedImageIndex = targetNode.ImageIndex;
            }
            catch (Exception) { }
        }

        private void treeViewElectricalInterfaces_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void dataTabControl_Click(object sender, EventArgs e)
        {
            
        }

        private void dataTabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                //Size, Name, Style change...
                Font Font;
                
                //For forground color
                Brush foreBrush;

                //Aktueller Focus
                if (e.Index == this.dataTabControl.SelectedIndex)
                {
                    //This line of code will help you to change the appearance like size,name,style.
                    Font = new Font(e.Font, FontStyle.Bold | FontStyle.Bold);
                    Font = new Font(e.Font, FontStyle.Bold);

                    //backBrush = new System.Drawing.SolidBrush(Color.Black);
                    foreBrush = Brushes.Black;
                }
                else
                {
                    Font = e.Font;
                    //backBrush = new SolidBrush(e.BackColor);
                    foreBrush = new SolidBrush(e.ForeColor);
                }

                //To set the alignment of the caption.
                string sTabName = this.dataTabControl.TabPages[e.Index].Text;
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;

                //Thsi will help you to fill the interior portion of
                //selected tabpage.
                e.Graphics.FillRectangle(new SolidBrush(Color.LightBlue), e.Bounds);
                Rectangle rect = e.Bounds;
                rect = new Rectangle(rect.X, rect.Y + 3, rect.Width, rect.Height - 3);
                e.Graphics.DrawString(sTabName, Font, foreBrush, rect, sf);

                sf.Dispose();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message.ToString(), "Error Occured", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void loadLibraryFile_Click(object sender, EventArgs e)
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


                    string referencedClassName = "";
                    foreach (var classLibType in document.CAEXFile.RoleClassLib)
                    {

                        TreeNode libNode = treeViewRoleClassLib.Nodes.Add(classLibType.ToString(), classLibType.ToString(), 0);


                        foreach (var classType in classLibType.RoleClass)
                        {
                            TreeNode roleNode;

                            if (classType.ReferencedClassName != "")
                            {
                                referencedClassName = classType.ReferencedClassName;
                                roleNode = libNode.Nodes.Add(classType.ToString(), classType.ToString() + "{" + "Class:" + "  " + referencedClassName + "}", 1);
                                
                                searchAMLLibraryFile.SearchForReferencedClassName(document, referencedClassName, classType);
                                searchAMLLibraryFile.CheckForAttributesOfReferencedClassName(classType);

                            }
                            else
                            {
                                roleNode = libNode.Nodes.Add(classType.ToString(), classType.ToString(), 1);
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
                                        externalinterfacenode.ForeColor = SystemColors.GrayText;
                                        searchAMLLibraryFile.SearchForReferencedClassNameofExternalIterface(document, referencedClassName, classType, externalinterface);
                                        searchAMLLibraryFile.CheckForAttributesOfReferencedClassNameofExternalIterface(classType, externalinterface);
                                      

                                    }
                                    else
                                    {
                                        externalinterfacenode = roleNode.Nodes.Add(externalinterface.ToString(), externalinterface.ToString(), 2);
                                       // searchAMLLibraryFile.CheckForAttributesOfReferencedClassNameofExternalIterface(classType, externalinterface);
                                    }



                                    searchAMLLibraryFile.PrintExternalInterfaceNodes(document, externalinterfacenode, externalinterface, classType);
                                }

                            }
                            searchAMLLibraryFile.PrintNodesRecursiveInRoleClassLib(document, roleNode, classType, referencedClassName);
                        }

                    }
                    

                    foreach (var classLibType in document.CAEXFile.InterfaceClassLib)
                    {
                        // searchAMLLibraryFile.DictioanryOfIDofInterfaceClassLibraryNodes.Add(classLibType.Name.ToString(), classLibType.ID.ToString());
                        TreeNode libNode = treeViewInterfaceClassLib.Nodes.Add(classLibType.ToString(), classLibType.ToString(), 0);



                        foreach (var classType in classLibType.InterfaceClass)
                        {
                            TreeNode interfaceclassNode;
                            if (classType.ReferencedClassName != "")
                            {
                                // searchAMLLibraryFile.DictioanryOfIDofInterfaceClassLibraryNodes.Add(classType.Name.ToString(), classType.ID.ToString());

                                referencedClassName = classType.ReferencedClassName;
                                interfaceclassNode = libNode.Nodes.Add(classType.ToString(), classType.ToString() + "{" + "Class:" + "  " + referencedClassName + "}", 1);
                                searchAMLLibraryFile.SearchForReferencedClassName(document, referencedClassName, classType);
                                searchAMLLibraryFile.CheckForAttributesOfReferencedClassName(classType);
                                
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
                                        externalinterfacenode.ForeColor = SystemColors.GrayText;

                                        searchAMLLibraryFile.SearchForReferencedClassNameofExternalIterface(document, referencedClassName, classType, externalinterface);
                                        searchAMLLibraryFile.CheckForAttributesOfReferencedClassNameofExternalIterface(classType, externalinterface);
                                    }
                                    else
                                    {
                                        //searchAMLLibraryFile.DictioanryOfIDofInterfaceClassLibraryNodes.Add(classType.Name.ToString() + externalinterface.ToString(), externalinterface.ID.ToString());

                                        externalinterfacenode = interfaceclassNode.Nodes.Add(externalinterface.ToString(), externalinterface.ToString(), 2);
                                    }


                                    searchAMLLibraryFile.PrintExternalInterfaceNodes(document, externalinterfacenode, externalinterface, classType);
                                }
                            }
                            searchAMLLibraryFile.PrintNodesRecursiveInInterfaceClassLib(document, interfaceclassNode, classType, referencedClassName);
                        }

                    }

                }


                catch (Exception)
                {
                    
                    MessageBox.Show("Missing names of attributes or Same atrribute sequence is repeated in the given file","Missing Names", MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);
                }

            }
        }

        private void genericparametersAttrDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }

        private void treeViewInterfaceClassLib_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
          
        }

        private void librariesSplitButton_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            if (op.ShowDialog() == DialogResult.OK)
            {

            }
        }

        private void librariesSplitButton_MouseHover(object sender, EventArgs e)
        {
            librariesSplitButton.ShowDropDown();
        }



        public void selectLibrary(byte[] file)
        {
            CAEXDocument doc = null;
            doc = CAEXDocument.LoadFromBinary(file);
            try
            {

                string referencedClassName = "";
                foreach (var classLibType in doc.CAEXFile.RoleClassLib)
                {
                    bool decisiontoPrint = true ;
                    foreach (TreeNode node in treeViewRoleClassLib.Nodes)
                    {
                        if (node.Name == classLibType.Name.ToString())
                        {
                            decisiontoPrint = false;
                            break;
                        }


                    }

                    if (decisiontoPrint == true)
                    {
                        TreeNode libNode = treeViewRoleClassLib.Nodes.Add(classLibType.ToString(), classLibType.ToString(), 0);


                        foreach (var classType in classLibType.RoleClass)
                        {
                            TreeNode roleNode;

                            if (classType.ReferencedClassName != "")
                            {
                                referencedClassName = classType.ReferencedClassName;
                                roleNode = libNode.Nodes.Add(classType.ToString(), classType.ToString() + "{" + "Class:" + "  " + referencedClassName + "}", 1);

                                searchAMLLibraryFile.SearchForReferencedClassName(doc, referencedClassName, classType);
                                searchAMLLibraryFile.CheckForAttributesOfReferencedClassName(classType);

                            }
                            else
                            {
                                roleNode = libNode.Nodes.Add(classType.ToString(), classType.ToString(), 1);
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
                                        externalinterfacenode.ForeColor = SystemColors.GrayText;
                                        searchAMLLibraryFile.SearchForReferencedClassNameofExternalIterface(doc, referencedClassName, classType, externalinterface);
                                        searchAMLLibraryFile.CheckForAttributesOfReferencedClassNameofExternalIterface(classType, externalinterface);


                                    }
                                    else
                                    {
                                        externalinterfacenode = roleNode.Nodes.Add(externalinterface.ToString(), externalinterface.ToString(), 2);
                                        externalinterfacenode.ForeColor = SystemColors.GrayText;
                                        // searchAMLLibraryFile.CheckForAttributesOfReferencedClassNameofExternalIterface(classType, externalinterface);
                                    }



                                    searchAMLLibraryFile.PrintExternalInterfaceNodes(doc, externalinterfacenode, externalinterface, classType);
                                }

                            }
                            searchAMLLibraryFile.PrintNodesRecursiveInRoleClassLib(doc, roleNode, classType, referencedClassName);
                        }
                    }

                   

                }




                foreach (var classLibType in doc.CAEXFile.InterfaceClassLib)
                {

                    bool decisiontoPrint = true;
                    foreach (TreeNode node in treeViewInterfaceClassLib.Nodes)
                    {
                        if (node.Name == classLibType.Name.ToString())
                        {
                            decisiontoPrint = false;
                            break;
                        }


                    }
                    if (decisiontoPrint == true)
                    {
                        // searchAMLLibraryFile.DictioanryOfIDofInterfaceClassLibraryNodes.Add(classLibType.Name.ToString(), classLibType.ID.ToString());
                        TreeNode libNode = treeViewInterfaceClassLib.Nodes.Add(classLibType.ToString(), classLibType.ToString(), 0);



                        foreach (var classType in classLibType.InterfaceClass)
                        {
                            TreeNode interfaceclassNode;
                            if (classType.ReferencedClassName != "")
                            {
                                // searchAMLLibraryFile.DictioanryOfIDofInterfaceClassLibraryNodes.Add(classType.Name.ToString(), classType.ID.ToString());

                                referencedClassName = classType.ReferencedClassName;
                                interfaceclassNode = libNode.Nodes.Add(classType.ToString(), classType.ToString() + "{" + "Class:" + "  " + referencedClassName + "}", 1);
                                searchAMLLibraryFile.SearchForReferencedClassName(doc, referencedClassName, classType);
                                searchAMLLibraryFile.CheckForAttributesOfReferencedClassName(classType);

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
                                        externalinterfacenode.ForeColor = SystemColors.GrayText;

                                        searchAMLLibraryFile.SearchForReferencedClassNameofExternalIterface(doc, referencedClassName, classType, externalinterface);
                                        searchAMLLibraryFile.CheckForAttributesOfReferencedClassNameofExternalIterface(classType, externalinterface);
                                    }
                                    else
                                    {
                                        //searchAMLLibraryFile.DictioanryOfIDofInterfaceClassLibraryNodes.Add(classType.Name.ToString() + externalinterface.ToString(), externalinterface.ID.ToString());

                                        externalinterfacenode = interfaceclassNode.Nodes.Add(externalinterface.ToString(), externalinterface.ToString(), 2);
                                        externalinterfacenode.ForeColor = SystemColors.GrayText;
                                    }


                                    searchAMLLibraryFile.PrintExternalInterfaceNodes(doc, externalinterfacenode, externalinterface, classType);
                                }
                            }
                            searchAMLLibraryFile.PrintNodesRecursiveInInterfaceClassLib(doc, interfaceclassNode, classType, referencedClassName);
                        }
                    }

                }

            }


            catch (Exception)
            {

                MessageBox.Show("Missing names of attributes or Same atrribute sequence is repeated in the given file", "Missing Names", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }




        }

       

        private void automationComponentLibraryv100ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectLibrary(Properties.Resources.AutomationComponentLibrary_v1_0_0);
        }

        private void automationComponentLibraryv100CAEX3BETAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectLibrary(Properties.Resources.AutomationComponentLibrary_v1_0_0_CAEX3_BETA);
        }

        private void automationComponentLibraryv100FullToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectLibrary(Properties.Resources.AutomationComponentLibrary_v1_0_0_Full);
        }

        private void automationComponentLibraryv100FullCAEX3BETAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectLibrary(Properties.Resources.AutomationComponentLibrary_v1_0_0_Full_CAEX3_BETA);
        }

        private void electricConnectorLibraryv100ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectLibrary(Properties.Resources.ElectricConnectorLibrary_v1_0_0);
        }

        private void industrialSensorLibraryv100ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectLibrary(Properties.Resources.IndustrialSensorLibrary_v1_0_0);
        }

        private void vendorNameTextBox_Leave(object sender, EventArgs e)
        {
            

            foreach (var pair in device.DictionaryForRoleClassofComponent)
            {
                if (pair.Key != null && pair.Key.ToString() == "(" + 1+ ")"+ "AutomationComponent{Class:  AutomationMLBaseRole}")
                {
                    foreach (var valueList in pair.Value)
                    {
                        foreach (var item in valueList)
                        {
                            if (item.Name == "Manufacturer")
                            {
                                item.Value = vendorNameTextBox.Text;
                            }
                        }
                    }

                }
                if (pair.Key != null && pair.Key.Contains("(" + 1+ ")"))
                {
                    foreach (var valueList in pair.Value)
                    {
                        foreach (var item in valueList)
                        {
                            if (item.Name == "Manufacturer")
                            {
                                item.Value = vendorNameTextBox.Text;
                            }
                        }
                    }
                }

            }
        }

        private void deviceNameTextBox_Leave(object sender, EventArgs e)
        {
            foreach (var pair in device.DictionaryForRoleClassofComponent)
            {
                if (pair.Key != null && pair.Key.ToString() == "(" + 1 + ")" + "AutomationComponent{Class:  AutomationMLBaseRole}")
                {
                    foreach (var valueList in pair.Value)
                    {
                        foreach (var item in valueList)
                        {
                            if (item.Name == "Model")
                            {
                                item.Value = deviceNameTextBox.Text;
                            }
                        }
                    }

                }
                if (pair.Key != null && pair.Key.Contains("(" + 1 + ")"))
                {
                    foreach (var valueList in pair.Value)
                    {
                        foreach (var item in valueList)
                        {
                            if (item.Name == "Model")
                            {
                                item.Value = deviceNameTextBox.Text;
                            }
                        }
                    }
                }
            }
        }

        private void genericparametersAttrDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            ClearHeaderTabPageValuesofgenericData();
            genericparametersAttrDataGridView.CurrentRow.Selected = true;

            string attributeName = "";
            string values = "";
            string defaults = "";
            string Units = "";
            string datatype = "";

            if (genericparametersAttrDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                try
                {
                    if (genericparametersAttrDataGridView.Rows[e.RowIndex].Cells[0].Value != null)
                    {
                        attributeName = genericparametersAttrDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                    }
                }
                catch (Exception)
                {}

                try
                {
                    if (genericparametersAttrDataGridView.Rows[e.RowIndex].Cells[1].Value != null)
                    {
                         values = genericparametersAttrDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                    }
                }
                catch (Exception)
                { }

                try
                {
                    if (genericparametersAttrDataGridView.Rows[e.RowIndex].Cells[2].Value != null)
                    {
                        defaults = genericparametersAttrDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                    }
                }
                catch (Exception)
                { }

                try
                {
                    if (genericparametersAttrDataGridView.Rows[e.RowIndex].Cells[3].Value != null)
                    {
                         Units = genericparametersAttrDataGridView.Rows[e.RowIndex].Cells[3].Value.ToString();
                    }
                }
                catch (Exception)
                { }
                try
                {
                    if (genericparametersAttrDataGridView.Rows[e.RowIndex].Cells[4].Value != null)
                    {
                        datatype = genericparametersAttrDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString();
                    }
                }
                catch (Exception)
                { }


               

                List<string> lists = new List<string>();
                DataGridViewComboBoxCell dgvcbc = (DataGridViewComboBoxCell)genericparametersAttrDataGridView.Rows[e.RowIndex].Cells[5];
                
                foreach (var refsemantic in dgvcbc.Items)
                {
                    try
                    {
                        if (refsemantic != null)
                        {
                            lists.Add( refsemantic.ToString());
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    
                }
               /* string semantics = genericparametersAttrDataGridView.Rows[e.RowIndex].Cells[5].Value.ToString();*/



                //if (Convert.ToBoolean(electricalInterfacesCollectionDataGridView.CurrentRow.Cells[3].Value) == false)
                {

                    string interfaceClass = genericDataHeaderLabel.Text;
                    foreach (var pair in device.DictionaryForRoleClassofComponent)
                    {
                        if (pair.Key.ToString() == interfaceClass)
                        {
                            foreach (var valueList in pair.Value)
                            {
                                foreach (var item in valueList)
                                {
                                    if (item.Name.ToString() == attributeName)
                                    {
                                        
                                        
                                        genericDataDescriptionTxtBx.Text = item.Description;
                                        genericDataCopyrightTxtBx.Text = item.CopyRight;
                                        genericDataRefClassNameTxtBx.Text = item.ReferencedClassName;
                                        genericDataRefBaseClassPathTxtBx.Text = item.RefBaseClassPath;
                                        genericDataAttributePathTxtBx.Text = item.AttributePath;
                                        genericDataIDTxtBx.Text = item.ID;
                                        genericDataNameTxtBx.Text = item.Name;

                                        foreach (var pair2 in device.DictionaryForRoleClassofComponent)
                                        {
                                            if (pair2.Key.ToString() == interfaceClass)
                                            {
                                                foreach (var valueList2 in pair2.Value)
                                                {
                                                    foreach (var item2 in valueList2)
                                                    {
                                                        if (item2.Name.ToString() == attributeName)
                                                        {
                                                            item2.RefSemanticList.Remove();
                                                            item2.Name = attributeName;
                                                            item2.Value = values;
                                                            item2.Default = defaults;
                                                            item2.Unit = Units;
                                                           
                                                            foreach (var str in lists)
                                                            {
                                                                var refsems = item2.RefSemanticList.Append();
                                                                refsems.CorrespondingAttributePath = str;
                                                            }

                                                            item2.Description = genericDataDescriptionTxtBx.Text;
                                                            item2.CopyRight = genericDataCopyrightTxtBx.Text;
                                                            item2.ReferencedClassName = genericDataRefClassNameTxtBx.Text;
                                                            item2.RefBaseClassPath = genericDataRefBaseClassPathTxtBx.Text;
                                                            item2.AttributePath = genericDataAttributePathTxtBx.Text;
                                                            item2.ID = genericDataIDTxtBx.Text;
                                                            item2.Name = genericDataNameTxtBx.Text;
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
                    foreach (var pair in device.DictionaryForExternalInterfacesUnderRoleClassofComponent)
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

                                        genericDataDescriptionTxtBx.Text = item.Description;
                                        genericDataCopyrightTxtBx.Text = item.CopyRight;
                                        genericDataRefClassNameTxtBx.Text = item.ReferencedClassName;
                                        genericDataRefBaseClassPathTxtBx.Text = item.RefBaseClassPath;
                                        genericDataAttributePathTxtBx.Text = item.AttributePath;
                                        genericDataIDTxtBx.Text = item.ID;
                                        genericDataNameTxtBx.Text = item.Name;

                                        foreach (var pair2 in device.DictionaryForExternalInterfacesUnderRoleClassofComponent)
                                        {
                                            if (pair2.Key.ToString() == interfaceClass)
                                            {
                                                foreach (var valueList2 in pair2.Value)
                                                {
                                                    foreach (var item2 in valueList2)
                                                    {
                                                        if (item2.Name.ToString() == attributeName)
                                                        {
                                                            item2.RefSemanticList.Remove();
                                                            item2.Name = attributeName;
                                                            item2.Value = values;
                                                            item2.Default = defaults;
                                                            item2.Unit = Units;
                                                            
                                                            foreach (var str in lists)
                                                            {
                                                                var refsems = item2.RefSemanticList.Append();
                                                                refsems.CorrespondingAttributePath = str;
                                                            }
                                                            item2.Description = genericDataDescriptionTxtBx.Text;
                                                            item2.CopyRight = genericDataCopyrightTxtBx.Text;
                                                            item2.ReferencedClassName = genericDataRefClassNameTxtBx.Text;
                                                            item2.RefBaseClassPath = genericDataRefBaseClassPathTxtBx.Text;
                                                            item2.AttributePath = genericDataAttributePathTxtBx.Text;
                                                            item2.ID = genericDataIDTxtBx.Text;
                                                            item2.Name = genericDataNameTxtBx.Text;
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
            genericparametersAttrDataGridView.CurrentRow.Selected = false;
        }

        private void elecInterAttDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            ClearHeaderTabPageValuesofElectricalInterfaces();
            elecInterAttDataGridView.CurrentRow.Selected = true;

            string attributeName = "";
            string values = "";
            string defaults = "";
            string Units = "";
            string datatype = "";


            if (elecInterAttDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                try
                {
                    if (elecInterAttDataGridView.Rows[e.RowIndex].Cells[0].Value != null)
                    {
                        attributeName = elecInterAttDataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
                    }
                }
                catch (Exception)
                { }

                try
                {
                    if (elecInterAttDataGridView.Rows[e.RowIndex].Cells[1].Value != null)
                    {
                        values = elecInterAttDataGridView.Rows[e.RowIndex].Cells[1].Value.ToString();
                    }
                }
                catch (Exception)
                { }

                try
                {
                    if (elecInterAttDataGridView.Rows[e.RowIndex].Cells[2].Value != null)
                    {
                        defaults = elecInterAttDataGridView.Rows[e.RowIndex].Cells[2].Value.ToString();
                    }
                }
                catch (Exception)
                { }

                try
                {
                    if (elecInterAttDataGridView.Rows[e.RowIndex].Cells[3].Value != null)
                    {
                        Units = elecInterAttDataGridView.Rows[e.RowIndex].Cells[3].Value.ToString();
                    }
                }
                catch (Exception)
                { }
                try
                {
                    if (elecInterAttDataGridView.Rows[e.RowIndex].Cells[4].Value != null)
                    {
                        datatype = elecInterAttDataGridView.Rows[e.RowIndex].Cells[4].Value.ToString();
                    }
                }
                catch (Exception)
                { }


                List<string> lists = new List<string>();
                DataGridViewComboBoxCell dgvcbc = (DataGridViewComboBoxCell)elecInterAttDataGridView.Rows[e.RowIndex].Cells[5];

                foreach (var refsemantic in dgvcbc.Items)
                {
                    try
                    {
                        if (refsemantic != null)
                        {
                            lists.Add(refsemantic.ToString());
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                }


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
                                                            item2.RefSemanticList.Remove();
                                                            item2.Name = attributeName;
                                                            item2.Value = values;
                                                            item2.Default = defaults;
                                                            item2.Unit = Units;

                                                           
                                                            foreach (var str in lists)
                                                            {
                                                                var refsems = item2.RefSemanticList.Append();
                                                                refsems.CorrespondingAttributePath = str;
                                                            }

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
                                                            item2.RefSemanticList.Remove();
                                                            item2.Name = attributeName;
                                                            item2.Value = values;
                                                            item2.Default = defaults;
                                                            item2.Unit = Units;
                                                            
                                                            foreach (var str in lists)
                                                            {
                                                                var refsems = item2.RefSemanticList.Append();
                                                                refsems.CorrespondingAttributePath = str;
                                                            }
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

        private void manualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string search = "https://balluff-my.sharepoint.com/personal/raj_pulaparthi_balluff_de/_layouts/15/onedrive.aspx?id=%2Fpersonal%2Fraj%5Fpulaparthi%5Fballuff%5Fde%2FDocuments%2FDocumentation%20Modelling%20Wizard%2Epdf&parent=%2Fpersonal%2Fraj%5Fpulaparthi%5Fballuff%5Fde%2FDocuments";
            System.Diagnostics.Process.Start(search);
        }
    }
    
}
