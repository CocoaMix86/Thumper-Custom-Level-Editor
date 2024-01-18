using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thumper_Custom_Level_Editor
{
    public class CellFunction
    {
        public string function { get; set; }
        public int rowindex { get; set; }
        public int columnindex { get; set; }

        public decimal Evaluate()
        {
            return 0;
        }
    }
}
