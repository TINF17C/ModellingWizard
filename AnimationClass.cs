using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aml.Editor.Plugin.Properties;
using System.IO;
using System.Drawing;

namespace Aml.Editor.Plugin
{
    class AnimationClass
    {
        public Panel PanelNumber { get; set; }
        public Button ButtonNumber { get; set; }

        public TextBox TextboxName { get; set; }
        public PictureBox PictureboxNumber { get; set; }
        public Button PdfDisplayBtn { get; set; }

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
        public void OpenFileDialog(TextBox textboxName, Button pdfDisplayBtn)
        {
            TextboxName = textboxName;
            PdfDisplayBtn = pdfDisplayBtn;
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Document Files(*.pdf; *.doc;)|*.pdf; *.doc;";
            if (open.ShowDialog() == DialogResult.OK)
            {
                textboxName.Text = open.FileName;
                pdfDisplayBtn.Visible = true;
                pdfDisplayBtn.Text = Path.GetFileName(open.FileName);
                pdfDisplayBtn.Click += PdfButton_Click;
            }
        }
        public void PdfButton_Click(object sender, EventArgs e)
        {
            PdfViewer pdfViewer = new PdfViewer();
            pdfViewer.Show();

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

    }
    
}
