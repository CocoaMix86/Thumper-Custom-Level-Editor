using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Thumper_Custom_Level_Editor
{
    /// <summary>
    /// https://stackoverflow.com/questions/9260303/how-to-change-menu-hover-color
    /// </summary>
    class MyRenderer : ToolStripProfessionalRenderer
    {
        public MyRenderer() : base(new MyColors()) { }
    }

    class MyColors : ProfessionalColorTable
    {
        public override Color MenuItemSelected
        {
            get { return Color.BlueViolet; }
        }
        public override Color MenuItemSelectedGradientBegin
        {
            get { return Color.DarkRed; }
        }
        public override Color MenuItemSelectedGradientEnd
        {
            get { return Color.Red; }
        }
    }
}
