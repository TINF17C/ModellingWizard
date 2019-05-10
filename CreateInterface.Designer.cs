namespace Aml.Editor.Plugin
{
    partial class CreateInterface
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtInterfaceNumber = new System.Windows.Forms.TextBox();
            this.txtInterfaceDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.createInterfaceBtn = new System.Windows.Forms.Button();
            this.cmbConnectorType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPinCount = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.interfacePortMappingGrid = new System.Windows.Forms.DataGridView();
            this.pinnumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.attribute = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.backBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.interfacePortMappingGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(20, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Create a new Interface";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label2.Location = new System.Drawing.Point(21, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Interface Number:";
            // 
            // txtInterfaceNumber
            // 
            this.txtInterfaceNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInterfaceNumber.Location = new System.Drawing.Point(119, 59);
            this.txtInterfaceNumber.Name = "txtInterfaceNumber";
            this.txtInterfaceNumber.Size = new System.Drawing.Size(157, 20);
            this.txtInterfaceNumber.TabIndex = 1;
            // 
            // txtInterfaceDescription
            // 
            this.txtInterfaceDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtInterfaceDescription.Location = new System.Drawing.Point(119, 90);
            this.txtInterfaceDescription.Name = "txtInterfaceDescription";
            this.txtInterfaceDescription.Size = new System.Drawing.Size(157, 20);
            this.txtInterfaceDescription.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label3.Location = new System.Drawing.Point(7, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Interface description:";
            // 
            // createInterfaceBtn
            // 
            this.createInterfaceBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.createInterfaceBtn.Location = new System.Drawing.Point(436, 511);
            this.createInterfaceBtn.Name = "createInterfaceBtn";
            this.createInterfaceBtn.Size = new System.Drawing.Size(97, 23);
            this.createInterfaceBtn.TabIndex = 50;
            this.createInterfaceBtn.Text = "Create Interface";
            this.createInterfaceBtn.UseVisualStyleBackColor = true;
            this.createInterfaceBtn.Click += new System.EventHandler(this.createInterfaceBtn_Click);
            // 
            // cmbConnectorType
            // 
            this.cmbConnectorType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbConnectorType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbConnectorType.FormattingEnabled = true;
            this.cmbConnectorType.Location = new System.Drawing.Point(119, 123);
            this.cmbConnectorType.Name = "cmbConnectorType";
            this.cmbConnectorType.Size = new System.Drawing.Size(157, 21);
            this.cmbConnectorType.TabIndex = 3;
            this.cmbConnectorType.SelectionChangeCommitted += new System.EventHandler(this.cmbConnectorType_SelectionChangeCommitted);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label4.Location = new System.Drawing.Point(31, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 37;
            this.label4.Text = "Connector type:";
            // 
            // txtPinCount
            // 
            this.txtPinCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPinCount.Location = new System.Drawing.Point(314, 124);
            this.txtPinCount.Name = "txtPinCount";
            this.txtPinCount.Size = new System.Drawing.Size(34, 20);
            this.txtPinCount.TabIndex = 4;
            this.txtPinCount.TextChanged += new System.EventHandler(this.interfacePinCountInput_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(282, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 38;
            this.label5.Text = "with";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label6.Location = new System.Drawing.Point(354, 126);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 40;
            this.label6.Text = "Pins";
            // 
            // interfacePortMappingGrid
            // 
            this.interfacePortMappingGrid.AllowUserToAddRows = false;
            this.interfacePortMappingGrid.AllowUserToDeleteRows = false;
            this.interfacePortMappingGrid.AllowUserToResizeRows = false;
            this.interfacePortMappingGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.interfacePortMappingGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.interfacePortMappingGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.interfacePortMappingGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pinnumber,
            this.attribute});
            this.interfacePortMappingGrid.Location = new System.Drawing.Point(280, 162);
            this.interfacePortMappingGrid.Name = "interfacePortMappingGrid";
            this.interfacePortMappingGrid.RowHeadersVisible = false;
            this.interfacePortMappingGrid.Size = new System.Drawing.Size(253, 330);
            this.interfacePortMappingGrid.TabIndex = 5;
            // 
            // pinnumber
            // 
            this.pinnumber.FillWeight = 50F;
            this.pinnumber.HeaderText = "Pin";
            this.pinnumber.Name = "pinnumber";
            // 
            // attribute
            // 
            this.attribute.HeaderText = "Attribute";
            this.attribute.Name = "attribute";
            // 
            // backBtn
            // 
            this.backBtn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.backBtn.Location = new System.Drawing.Point(24, 511);
            this.backBtn.Name = "backBtn";
            this.backBtn.Size = new System.Drawing.Size(52, 23);
            this.backBtn.TabIndex = 51;
            this.backBtn.Text = "Back";
            this.backBtn.UseVisualStyleBackColor = true;
            this.backBtn.Click += new System.EventHandler(this.backBtn_Click);
            // 
            // CreateInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.backBtn);
            this.Controls.Add(this.interfacePortMappingGrid);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtPinCount);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbConnectorType);
            this.Controls.Add(this.createInterfaceBtn);
            this.Controls.Add(this.txtInterfaceDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtInterfaceNumber);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "CreateInterface";
            this.Size = new System.Drawing.Size(556, 554);
            ((System.ComponentModel.ISupportInitialize)(this.interfacePortMappingGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtInterfaceNumber;
        private System.Windows.Forms.TextBox txtInterfaceDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button createInterfaceBtn;
        private System.Windows.Forms.ComboBox cmbConnectorType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPinCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView interfacePortMappingGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn pinnumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn attribute;
        private System.Windows.Forms.Button backBtn;
    }
}
