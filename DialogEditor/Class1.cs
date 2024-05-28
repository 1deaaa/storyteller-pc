using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace DialogSystem
{
    enum NodeType
    {
        Character,
        Text,
        Option,
        Action,
        Next
    }
    class RichNode:TreeNode
    {
        NodeType _type;
        public NodeType Type
        {
            get { return _type; }
            set { _type = value; 
                switch (value)
                {
                    case NodeType.Character:
                        BackColor=Color.CornflowerBlue;
                    break;
                    case NodeType.Text:
                        BackColor = Color.MediumSeaGreen;
                        break;
                    case NodeType.Option:
                        BackColor = Color.Salmon;
                        break;
                    case NodeType.Action:
                        BackColor = Color.Orange;
                        break;
                    case NodeType.Next:
                        BackColor = Color.DarkViolet;
                        break;
                }
            } }
    }
}
