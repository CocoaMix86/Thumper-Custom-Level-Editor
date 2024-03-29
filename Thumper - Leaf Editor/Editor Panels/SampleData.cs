﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thumper_Custom_Level_Editor
{
    public class SampleData
    {
        public string obj_name { get; set; }
        public string path { get; set; }
        public decimal volume { get; set; }
        public decimal pitch { get; set; }
        public decimal pan { get; set; }
        public decimal offset { get; set; }
        public string channel_group { get; set; }

        public override string ToString()
        {
            return obj_name;
        }
    }
}
