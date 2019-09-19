using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Aml.Editor.Plugin
{
    public partial class PdfViewer : Form
    {
        public string PdfURL { get; set; }
        public PdfViewer()
        {
            InitializeComponent();
        }

        private void AxAcroPDF1_Enter(object sender, EventArgs e)
        {

        }
        public void Viewpdf(string pdfURL)
        {
            PdfURL = pdfURL;
           axAcroPDF1.src = pdfURL;

        }

        private void AxAcroPDF1_Enter_1(object sender, EventArgs e)
        {

        }
    }
}
