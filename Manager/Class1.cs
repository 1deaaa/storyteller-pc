using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using DialogSystem;

namespace DialogSystem
{
    public enum NodeType
    {
        OptionRoot,
        ActionRoot,
        Option,
        Action,
        Dialog,
        Scene,
        Next,
    }
    public static class ThemeColor
    {
        public static Color Option = Color.LimeGreen;
        public static Color Action = Color.Orange;
        public static Color Dialog = Color.BlueViolet;
        public static Color Branch = Color.SkyBlue;
    }
    public class RichNode : TreeNode
    {
        public int id = 0;
        public string chr = null;
        public string txt = null;
        public string next = null;
        public JObject opt = null;
        public JObject act = null;
        public string scene = null;
        NodeType _type;
        public NodeType NodeType
        {
            get { return _type; }
            set
            {

            }
        }
    }
    public class Map
    {
        public delegate void ActionHandler();
        public static Dictionary<string, ActionHandler> ActMap = new Dictionary<string, Map.ActionHandler>
        {
          #region 行为映射表
                //绑定所有指令和函数
                    {"动作",Method.Animate
    }
    #endregion 行为映射表
        };
        public static Dictionary<int, string> ChrMap= new Dictionary<int, string>()
        {
                    #region 角色映射表
                    {0,"我" },
                    {1,"心声" },
                    { 2,"可可酱"}
                    #endregion 角色映射表
        };
    }
    public static class Method
    {
        public static void Error(object e)
        {
            MessageBox.Show(e.ToString(), "o(TヘTo)", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void Inf(object e)
        {
            MessageBox.Show(e.ToString(), "o(=•ェ•=)m", MessageBoxButtons.OK,MessageBoxIcon.Information );
        }
        public static void Music(string bgm)
        {
            
        }
        public static void Animate()
        {
            
        }
        public static void RecordBranch(string brc)
        {
            
        }
    }
}
