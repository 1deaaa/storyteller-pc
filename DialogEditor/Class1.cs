using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Newtonsoft.Json.Linq;

namespace DialogSystem
{
    enum NodeType
    {
        OptionRoot,
        ActionRoot,
        Option,
        Action,
        Dialog,
    }
    static class ThemeColor
    {
        public static Color Option=Color.LimeGreen;
        public static Color Action = Color.Orange;
        public static Color Dialog = Color.BlueViolet;
        public static Color Branch=Color.SkyBlue;
    }
    class RichNode:TreeNode
    {
        public int id = 0;
        public string chr=null;
        public string txt=null;
        public string next=null;
        public JObject opt=null;
        public JObject act=null;

        NodeType _type;
        public NodeType nodeType 
        { get { return _type; }
            set
            {

            }
        }
    }
}
