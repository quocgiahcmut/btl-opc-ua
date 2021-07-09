using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientAppGiaBuild.Src
{
    public partial class Led7Seg : UserControl
    {
        public Led7Seg()
        {
            InitializeComponent();
        }

        public void SetData(int a, Color color)
        {
            Color gray = Color.Gray;
            switch (a)
            {
                case 0:
                    lbla.BackColor = color;
                    lblb.BackColor = color;
                    lblc.BackColor = color;
                    lbld.BackColor = color;
                    lble.BackColor = color;
                    lblf.BackColor = color;
                    lblg.BackColor = gray;
                    break;
                case 1:
                    lbla.BackColor = gray;
                    lblb.BackColor = color;
                    lblc.BackColor = color;
                    lbld.BackColor = gray;
                    lble.BackColor = gray;
                    lblf.BackColor = gray;
                    lblg.BackColor = gray;
                    break;
                case 2:
                    lbla.BackColor = color;
                    lblb.BackColor = color;
                    lblc.BackColor = gray;
                    lbld.BackColor = color;
                    lble.BackColor = color;
                    lblf.BackColor = gray;
                    lblg.BackColor = color;
                    break;
                case 3:
                    lbla.BackColor = color;
                    lblb.BackColor = color;
                    lblc.BackColor = color;
                    lbld.BackColor = color;
                    lble.BackColor = gray;
                    lblf.BackColor = gray;
                    lblg.BackColor = color;
                    break;
                case 4:
                    lbla.BackColor = gray;
                    lblb.BackColor = color;
                    lblc.BackColor = color;
                    lbld.BackColor = gray;
                    lble.BackColor = gray;
                    lblf.BackColor = color;
                    lblg.BackColor = color;
                    break;
                case 5:
                    lbla.BackColor = color;
                    lblb.BackColor = gray;
                    lblc.BackColor = color;
                    lbld.BackColor = color;
                    lble.BackColor = gray;
                    lblf.BackColor = color;
                    lblg.BackColor = color;
                    break;
                case 6:
                    lbla.BackColor = color;
                    lblb.BackColor = gray;
                    lblc.BackColor = color;
                    lbld.BackColor = color;
                    lble.BackColor = color;
                    lblf.BackColor = color;
                    lblg.BackColor = color;
                    break;
                case 7:
                    lbla.BackColor = color;
                    lblb.BackColor = color;
                    lblc.BackColor = color;
                    lbld.BackColor = gray;
                    lble.BackColor = gray;
                    lblf.BackColor = gray;
                    lblg.BackColor = gray;
                    break;
                case 8:
                    lbla.BackColor = color;
                    lblb.BackColor = color;
                    lblc.BackColor = color;
                    lbld.BackColor = color;
                    lble.BackColor = color;
                    lblf.BackColor = color;
                    lblg.BackColor = color;
                    break;
                case 9:
                    lbla.BackColor = color;
                    lblb.BackColor = color;
                    lblc.BackColor = color;
                    lbld.BackColor = color;
                    lble.BackColor = gray;
                    lblf.BackColor = color;
                    lblg.BackColor = color;
                    break;
            }
        }
    }
}
