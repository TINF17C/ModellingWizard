using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aml.Editor.Plugin.Properties;
using System.IO;
using System.Drawing;
using System.Web;
using System.Net;

namespace Aml.Editor.Plugin
{
    class AnimationClass
    {
        public Panel PanelNumber { get; set; }
        public Button ButtonNumber { get; set; }
        public WebBrowser Webbrowser { get; set; }
        public TextBox TextboxName { get; set; }
        public PictureBox PictureboxNumber { get; set; }
        public Button DisplayBtn { get; set; }
        public DataGridView dataGridView { get; set; }
        public string words { get; set; }
       // public TreeNode Node { get; set; }


        public AnimationClass()
        {

        }
        // Method for window size maximum and minimum
        public  void WindowSizeChanger(Panel panelNumber,Button buttonNumber)
        {
            PanelNumber = panelNumber;
            ButtonNumber = buttonNumber;
            if (panelNumber.Size == panelNumber.MaximumSize)
            {
                panelNumber.Size = panelNumber.MinimumSize;
                buttonNumber.Image = Resources.icons8_expand_arrow_24;
            }
            else
            {
                panelNumber.Size = panelNumber.MaximumSize;
                buttonNumber.Image = Resources.icons8_collapse_arrow_24;
            }
        }
        public void WindowSizeChanger(Panel panelNumber)
        {
            PanelNumber = panelNumber;
           // Node = node;
            if (panelNumber.Size == panelNumber.MaximumSize)
            {
                panelNumber.Size = panelNumber.MinimumSize;
                //Node.Image = Resources.icons8_expand_arrow_24;
            }
            else
            {
                panelNumber.Size = panelNumber.MaximumSize;
                //Node.Image = Resources.icons8_collapse_arrow_24;
            }
        }
        public string OpenFileDialog(TextBox textboxName)
        {
            TextboxName = textboxName;
            
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Document Files(*.pdf; *.doc;*.jpg; *.jpeg; *.gif; *.bmp; *.png;)|*.pdf; *.doc;*.jpg; *.jpeg; *.gif; *.bmp; *.png;";
            if (open.ShowDialog() == DialogResult.OK)
            {
                textboxName.Text = open.FileName;
                
            }
            string nameOfFile = Path.GetFileName(open.FileName);
            return nameOfFile;
        }
        public void OpenFileDialog(TextBox textboxName, Button pdfDisplayBtn)
        {
            TextboxName = textboxName;
            DisplayBtn = pdfDisplayBtn;
            WebBrowser webbrowser = new WebBrowser();
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Files(*.pdf; *.doc;)|*.pdf; *.doc;";
            if (open.ShowDialog() == DialogResult.OK)
            {
                textboxName.Text = open.FileName;
                pdfDisplayBtn.Visible = true;
                pdfDisplayBtn.Text = Path.GetFileName(open.FileName);
                
            }
        }
       // method for opening IEC-CDD urls 
       public void ManualOpener(string btnText)
        {
            
            string mainUrl = "https://cdd.iec.ch/CDD/IEC62683/iec62683.nsf/PropertiesAllVersions/0112-2---62683%23";
            string lastUrl = "?OpenDocument";
            string midUrl = btnText.Substring(15);
            string finalUrl = mainUrl +midUrl+ lastUrl;
           
            System.Diagnostics.Process.Start(finalUrl);
        }


        // Open Dialog Box related method that takes parmeters of textbox name and the picture box number.
        public void OpenFileDialog(TextBox textboxName,PictureBox pictureBoxNumber)
        {
            TextboxName = textboxName;
            PictureboxNumber = pictureBoxNumber;
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png;)|*.jpg; *.jpeg; *.gif; *.bmp; *.png;";
            if (open.ShowDialog() == DialogResult.OK)
            {
                textboxName.Text = open.FileName;
                pictureBoxNumber.Image = new Bitmap(open.FileName);
            }
        }
        public void OpenFileDialog(TextBox textboxName, PictureBox pictureBoxNumber, Button displayButton)
        {
            TextboxName = textboxName;
            PictureboxNumber = pictureBoxNumber;
            DisplayBtn = displayButton;
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png;)|*.jpg; *.jpeg; *.gif; *.bmp; *.png;";
            if (open.ShowDialog() == DialogResult.OK)
            {
                textboxName.Text = open.FileName;
                pictureBoxNumber.Image = new Bitmap(open.FileName);
                displayButton.Visible = true;
                displayButton.Text = Path.GetFileName(open.FileName);
            }
        }
        // this method dispaly all hidden buttons with the Refsemantic Id in them.
        public void DispalySemanticBtn(Button refSemanticBtn, DataGridView dataGrids,string word)
        {
            words = word;
            DisplayBtn = refSemanticBtn;
            dataGridView = dataGrids;

            DisplayBtn.Visible = true;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            try
            {
                foreach (DataGridViewRow attribute in dataGridView.Rows)
                {
                    if (attribute.Cells[1].Value.ToString().Equals(words))
                    {
                        DisplayBtn.Text = attribute.Cells[0].Value.ToString();
                        break;
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

        }
        //  this method is responsible to load all the datagrid parameters from the user in to the MWDevice class in to respective lists 
        
    }
    
}
