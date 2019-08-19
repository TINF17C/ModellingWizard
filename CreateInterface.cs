using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Aml.Editor.Plugin
{
    /// <summary>
    /// This is a window forms UI control, containing a tree view. The Tree view is updated, ever when an InternalElement is selected
    /// in the editor which has an Instance Class relation to a SystemUnitClass. The Tree view is populated with the ExternalInterface
    /// objects and InternalElement objects of the referenced SystemUnitClass.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.UserControl" />
    public partial class CreateInterface : UserControl
    {
        // The Controller to pass the data to
        private MWController mWController;

        // holds the last pincount for the portMappingGrid
        private int pinCount = 0;

        // Flag if the GUI is used to edit a interface
        private bool isEditing;

        #region Public Constructors

        /// <summary>
        /// Init the GUI and initialize the available connector types
        /// </summary>
        /// <param name="mWController">The <see cref="MWController"/> to pass data to</param>
        public CreateInterface(MWController mWController)
        {
            this.mWController = mWController;
            InitializeComponent();

            foreach (var item in MWConnectorTypes.ConnectorMap.Keys)
            {
                this.cmbConnectorType.Items.Add(item);
            }

        }

        #endregion Public Constructors

        /// <summary>
        /// Create a new Device and call <see cref="MWController.CreateInterfaceOnClick(MWInterface)"/> of the <see cref="mWController"/>
        /// </summary>
        /// <param name="sender">Event arg, not used</param>
        /// <param name="e">Event arg, not used</param>
        private void createInterfaceBtn_Click(object sender, System.EventArgs e)
        {
            // Creating a new object (interface) and giving it the values of the textfields/input
            MWInterface interfaceObject = new MWInterface();

            interfaceObject.pinList = new List<MWPin>();

            try { interfaceObject.numberOfInterface = Convert.ToInt32(txtInterfaceNumber.Text); } catch (Exception ) { MessageBox.Show("Vendor ID is in an invalid format (Expected only numbers)!"); return; }
            interfaceObject.interfaceDescription = txtInterfaceDescription.Text;
            interfaceObject.connectorType = cmbConnectorType.Text;
            if (!String.IsNullOrWhiteSpace(txtPinCount.Text))
                try { interfaceObject.amountPins = Convert.ToInt32(txtPinCount.Text); } catch (Exception ) { MessageBox.Show("Pin Count is in an invalid format (Expected only numbers)! Ignoring!"); }

            // getting the values of the DataGridView and inserting it into the pinList of the object (Interface)
            int i = (interfaceObject.amountPins - 1);
            while (i >= 0)
            {
                var pinItem = new MWPin();
                try
                {
                    pinItem.pinNumber = Convert.ToInt32(interfacePortMappingGrid.Rows[i].Cells[0].Value);
                    pinItem.attribute = Convert.ToString(interfacePortMappingGrid.Rows[i].Cells[1].Value);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }

                interfaceObject.pinList.Add(pinItem);
                i += -1;
            }


            string result = mWController.CreateInterfaceOnClick(interfaceObject, isEditing);
            clear();
            if (result != null)
            {
                // Display error Dialog
                MessageBox.Show(result);
            }
        }

        /// <summary>
        /// Update the Pin DataGrid with the amount of cells specified in this field
        /// </summary>
        /// <param name="sender">Event arg, not used</param>
        /// <param name="e">Event arg, not used</param>
        private void interfacePinCountInput_TextChanged(object sender, System.EventArgs e)
        {
            try
            {
                int count = int.Parse(txtPinCount.Text);
                if (pinCount < count)
                {
                    // There are less rows in the grid then now specified -> Add new Rows
                    for (int i = 0; i < count - pinCount; i++)
                    {
                        interfacePortMappingGrid.Rows.Add();
                        interfacePortMappingGrid.Rows[pinCount + i].Cells[0].Value = (pinCount + i).ToString();
                    }
                }
                else if (pinCount > count)
                {
                    // There are more rows in the grid then now specified -> Remove the last rows
                    while (pinCount > count)
                    {
                        interfacePortMappingGrid.Rows.RemoveAt(pinCount - 1);
                        pinCount = interfacePortMappingGrid.Rows.Count;
                    }

                }

                pinCount = interfacePortMappingGrid.Rows.Count;
            }
            catch (FormatException)
            {
                // Invalid input
                // Do nothing; Let them enter a correct input
                return;
            }
        }

        /// <summary>
        /// Fill the fields with the data of the MWInterface/>
        /// </summary>
        /// <param name="mWObject">The <see cref="MWInterface"/> with the data</param>
        internal void prefill(MWInterface mWObject)
        {
            txtInterfaceNumber.Text = mWObject.numberOfInterface.ToString();
            txtInterfaceDescription.Text = mWObject.interfaceDescription;
            cmbConnectorType.SelectedItem = mWObject.connectorType;

            interfacePortMappingGrid.Rows.Clear();
            foreach(MWPin pin in mWObject.pinList)
            {
                int i = interfacePortMappingGrid.Rows.Add();
                interfacePortMappingGrid.Rows[i].Cells[0].Value = i;
                interfacePortMappingGrid.Rows[i].Cells[1].Value = pin.attribute;
            }
            pinCount = mWObject.pinList.Count;
            txtPinCount.Text = mWObject.amountPins.ToString();

            cmbConnectorType_SelectionChangeCommitted(null, null);
            pinCount = mWObject.amountPins;
            createInterfaceBtn.Text = "Update Interface";
            txtInterfaceNumber.Enabled = false;

            this.isEditing = true;
        }

        /// <summary>
        /// Clear and reset all fields
        /// </summary>
        internal void clear()
        {
            txtInterfaceNumber.Text = "";
            txtInterfaceDescription.Text = "";
            txtPinCount.Text = "";
            cmbConnectorType.SelectedIndex = -1;
            interfacePortMappingGrid.Rows.Clear();

            createInterfaceBtn.Text = "Create Interface";
            txtInterfaceNumber.Enabled = true;
            isEditing = false;
        }

        /// <summary>
        /// Call the Controller to display Start GUI
        /// </summary>
        /// <param name="sender">Event arg, not used</param>
        /// <param name="e">Event arg, not used</param>
        private void backBtn_Click(object sender, EventArgs e)
        {
            mWController.ChangeGui(MWController.MWGUIType.Start);
            clear();
        }

        /// <summary>
        /// Disable / Enable the pin count based on the selected connector type
        /// </summary>
        /// <param name="sender">Event arg, not used</param>
        /// <param name="e">Event arg, not used</param>
        private void cmbConnectorType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int pinCount = MWConnectorTypes.ConnectorMap[cmbConnectorType.SelectedItem.ToString()];
            switch (pinCount)
            {
                case -1:
                    // variable pin count
                    txtPinCount.Enabled = true;
                    txtPinCount.Text = "";
                    break;
                case 0:
                    // disabled pin count
                    txtPinCount.Enabled = false;
                    txtPinCount.Text = "";
                    break;
                default:
                    // fixed pin count
                    txtPinCount.Enabled = false;
                    txtPinCount.Text = pinCount.ToString();
                    break;
            }

        }
    }
}