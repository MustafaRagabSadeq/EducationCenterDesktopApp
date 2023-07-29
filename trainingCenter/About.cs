using trainingCenter.BL;
using MetroSet_UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Xml.Linq;

namespace trainingCenter
{
    public partial class About : MetroSetForm
    {
        public About()
        {
            InitializeComponent();
            MaximumSize = MinimumSize = Size;
        }
        
    }
}