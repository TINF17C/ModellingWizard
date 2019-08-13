using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aml.Editor.Plugin.Properties;

namespace Aml.Editor.Plugin
{
    class AnimationClass
    {
        public Panel PanelNumber { get; set; }
        public Button ButtonNumber { get; set; }

        public AnimationClass()
        {

        }
        // Method for window size maximum nd minimum
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

    }
    
}
