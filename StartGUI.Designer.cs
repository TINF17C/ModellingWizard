namespace Aml.Editor.Plugin
{
    partial class StartGUI
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.createDeviceBtn = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.createInterfaceBtn = new System.Windows.Forms.Button();
            this.importIODDFileBtn = new System.Windows.Forms.Button();
            this.devicesComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.importGSDFileBtn = new System.Windows.Forms.Button();
            this.editFileBtn = new System.Windows.Forms.Button();
            this.openEditDialog = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(30, 38);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(240, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Modelling Wizard for Devices";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // createDeviceBtn
            // 
            this.createDeviceBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.createDeviceBtn.Location = new System.Drawing.Point(73, 111);
            this.createDeviceBtn.Name = "createDeviceBtn";
            this.createDeviceBtn.Size = new System.Drawing.Size(151, 23);
            this.createDeviceBtn.TabIndex = 1;
            this.createDeviceBtn.Text = "Create a new Device";
            this.createDeviceBtn.UseVisualStyleBackColor = true;
            this.createDeviceBtn.Click += new System.EventHandler(this.createDeviceBtn_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Multiselect = true;
            this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
            // 
            // createInterfaceBtn
            // 
            this.createInterfaceBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.createInterfaceBtn.Location = new System.Drawing.Point(73, 152);
            this.createInterfaceBtn.Name = "createInterfaceBtn";
            this.createInterfaceBtn.Size = new System.Drawing.Size(151, 23);
            this.createInterfaceBtn.TabIndex = 2;
            this.createInterfaceBtn.Text = "Create a new Interface";
            this.createInterfaceBtn.UseVisualStyleBackColor = true;
            this.createInterfaceBtn.Click += new System.EventHandler(this.createInterfaceBtn_Click);
            // 
            // importIODDFileBtn
            // 
            this.importIODDFileBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.importIODDFileBtn.Location = new System.Drawing.Point(73, 240);
            this.importIODDFileBtn.Name = "importIODDFileBtn";
            this.importIODDFileBtn.Size = new System.Drawing.Size(151, 23);
            this.importIODDFileBtn.TabIndex = 3;
            this.importIODDFileBtn.Text = "Import IODD-File";
            this.importIODDFileBtn.UseVisualStyleBackColor = true;
            this.importIODDFileBtn.Click += new System.EventHandler(this.importIODDFileBtn_Click);
            // 
            // devicesComboBox
            // 
            this.devicesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.devicesComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.devicesComboBox.FormattingEnabled = true;
            this.devicesComboBox.Items.AddRange(new object[] {
            "<No created Device>"});
            this.devicesComboBox.Location = new System.Drawing.Point(73, 372);
            this.devicesComboBox.Name = "devicesComboBox";
            this.devicesComboBox.Size = new System.Drawing.Size(151, 21);
            this.devicesComboBox.TabIndex = 5;
            this.devicesComboBox.SelectionChangeCommitted += new System.EventHandler(this.devicesComboBox_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(70, 353);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Modify an existing AML Object:";
            // 
            // importGSDFileBtn
            // 
            this.importGSDFileBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.importGSDFileBtn.Location = new System.Drawing.Point(73, 269);
            this.importGSDFileBtn.Name = "importGSDFileBtn";
            this.importGSDFileBtn.Size = new System.Drawing.Size(151, 23);
            this.importGSDFileBtn.TabIndex = 4;
            this.importGSDFileBtn.Text = "Import GSD-File";
            this.importGSDFileBtn.UseVisualStyleBackColor = true;
            this.importGSDFileBtn.Click += new System.EventHandler(this.importGSDFileBtn_Click);
            // 
            // editFileBtn
            // 
            this.editFileBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.editFileBtn.Location = new System.Drawing.Point(72, 416);
            this.editFileBtn.Name = "editFileBtn";
            this.editFileBtn.Size = new System.Drawing.Size(151, 23);
            this.editFileBtn.TabIndex = 7;
            this.editFileBtn.Text = "Choose file to edit";
            this.editFileBtn.UseVisualStyleBackColor = true;
            this.editFileBtn.Click += new System.EventHandler(this.editFileBtn_Click);
            // 
            // openEditDialog
            // 
            this.openEditDialog.DefaultExt = "amlx";
            this.openEditDialog.Filter = "AMLX File (*.amlx)|*.amlx";
            this.openEditDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openEditDialog_FileOk);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.createDeviceBtn);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.editFileBtn);
            this.panel1.Controls.Add(this.createInterfaceBtn);
            this.panel1.Controls.Add(this.importGSDFileBtn);
            this.panel1.Controls.Add(this.importIODDFileBtn);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.devicesComboBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 554);
            this.panel1.TabIndex = 8;
            // 
            // StartGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "StartGUI";
            this.Size = new System.Drawing.Size(304, 554);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button createDeviceBtn;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button createInterfaceBtn;
        private System.Windows.Forms.Button importIODDFileBtn;
        private System.Windows.Forms.ComboBox devicesComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button importGSDFileBtn;
        private System.Windows.Forms.Button editFileBtn;
        private System.Windows.Forms.OpenFileDialog openEditDialog;
        private System.Windows.Forms.Panel panel1;
    }
}
