using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using System.Text;
using System.IO;
using NUnit.Framework.Internal;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
public partial class DlgMgr : MonoBehaviour
{
    public TMP_Text _chr;
    public TMP_Text _txt;
    public TMP_Text _cap;
    public Button _next_Btn;//点击对话面板继续
    public Canvas _dlgui;
    public GameObject _btn_Tplt;//选项按钮模板
    //静态化供全局调用
    public static TMP_Text chr;
    public static TMP_Text txt;
    public static TMP_Text cap;
    public static Button next_Btn;
    public static Canvas dlgui;
    public static GameObject BtnTplt;

    public static bool DialogMode//禁用视角和移动
    {
        get { return _dlg_mode; }
        set
        {
            _dlg_mode = value;
            Player.MoveEnable = !value;
            View.ViewEnable = !value;
        }
    }
    static bool _dlg_mode = false;
    private static bool isInitialized = false;

    void Awake()
    {
        chr = _chr;
        txt = _txt;
        cap = _cap;
        next_Btn = _next_Btn;
        dlgui = _dlgui;
        BtnTplt = _btn_Tplt;
        isInitialized = true;
    }
    void Start()
    {
        dlgui.enabled = false;
    }
    public static void StartDlg(TextAsset storyFile)
    {
        if (!isInitialized)
        {
            Debug.LogError("DlgMgr is not initialized. Please ensure there is an active GameObject in your scene with the DlgMgr script attached, and all its public fields (like UI elements) are assigned in the inspector.");
            return;
        }

        if (storyFile == null)
        {
            Debug.LogError("No story file provided to StartDlg!");
            return;
        }
        JsonSrcMgr.LoadStory(storyFile);

        dlgui.enabled = true;
        // 重置对话状态
        Dialog.ResetDialog();
        if (JsonSrcMgr.JsonSource == null || JsonSrcMgr.JsonSource.Count == 0)
        {
            Debug.LogError("Failed to load story or story is empty.");
            return;
        }
        // JsonSource is an array, so we need to access the first element first.
        Dialog.SceneInit(JsonSrcMgr.JsonSource[0]["scene"].ToString());
        Dialog.DisplayOne(Dialog.CurrentObj);
        next_Btn.onClick.RemoveAllListeners(); // 防止重复绑定
        next_Btn.onClick.AddListener(() => Click_Next());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!Dialog.DialogEnabled || !DialogMode || Dialog.IsWaitForChoice())
                return;
            if (Dialog.IsTypingTxt && !Dialog.AllowSkip)
                return;
            Dialog.DisplayOne(Dialog.CurrentObj);
        }
    }

    //对话脚本结构中 之所以不用键名存储实际文本 是因为键名无法被修改
    private static void Click_Next()
    {
        if (!Dialog.DialogEnabled || Dialog.IsWaitForChoice())
            return;
        if (Dialog.IsTypingTxt && !Dialog.AllowSkip)
            return;
        Dialog.DisplayOne(Dialog.CurrentObj);
    }
    class DialogGroup
    {
        public JArray Array;
        public int NextIndex;
        public DialogGroup(JArray _array)
        {
            Array = _array;
            NextIndex = 0;//0才是有效的第一个
        }
    }
    class Dialog
    {
        //对于vs调试显示的json数据 外层都被加了一组{} 实际上是不存在的
        public static int Choice = 0;//注意 从1开始！！！
        public static int CurrentGroupObjIndex = 0;//目前遍历到的组内对话对象
        public static int scene_index = 0;
        static Stack<DialogGroup> DialogArray = new();
        public static JObject CurrentObj;//目前遍历到的对话对象
        static bool waitForChoice = false;
        public static bool EndDialog = false;//下一次点击直接关闭对话
        static string NextDialog = null;//指定next所指向的下一个对话场景 为null表示不跳转
        static List<ChoiceBtn> branch_btns = new();
        public static JToken DialogScene;
        public static bool DialogEnabled = true;

        // 新增：打字效果相关
        public static bool AllowSkip = true;//是否允许跳过对话
        public static bool IsTypingTxt = false;//是否正在打字
        public static float TypingSpeed = 0.04f;//打字间隔秒数
        static CancellationTokenSource cancel; // 用于取消当前的打印任务

        public static bool IsWaitForChoice()
        {
            return waitForChoice;
        }

        public static JArray CrtArray
        {
            get { return DialogArray.Count > 0 ? DialogArray.Peek().Array : null; }
            set { if (DialogArray.Count > 0) DialogArray.Peek().Array = value; }
        }
        public static int CrtIndex
        {
            get { return DialogArray.Count > 0 ? DialogArray.Peek().NextIndex : 0; }
            set { if (DialogArray.Count > 0) DialogArray.Peek().NextIndex = value; }
        }
        public static void SceneInit(string _scene)
        {
            DlgMgr.DialogMode = true;
            //??=如果为null才赋值 防止重复赋值
            DialogScene = JsonSrcMgr.GetSceneObj(_scene);//根（场景）键值对的值为数组  Token代表任意数据节点 Prop代表键值对 Object代表{xxx}

            if (DialogScene == null)
            {
                Debug.LogError($"Scene '{_scene}' not found in JsonSource!");
                return;
            }

            CurrentGroupObjIndex = 0;
            NextDialog = null;
            waitForChoice = false;
            DlgMgr.cap.text = DialogScene["cap"]?.ToString() ?? "";
            DialogArray.Clear();

            if (DialogScene["dia"] == null)
            {
                Debug.LogError($"Scene '{_scene}' does not contain 'dia' array!");
                return;
            }

            DialogArray.Push(new DialogGroup((JArray)DialogScene["dia"]));

            // 确保数组有内容
            if (CrtArray == null || CrtArray.Count == 0)
            {
                Debug.LogError($"Dialog array for scene '{_scene}' is empty!");
                return;
            }

            CurrentObj = (JObject)CrtArray[0];
        }

        private static void ChoiceBtn_Click(ChoiceBtn choiceBtn, JObject optionOwner)//选项点击 也相当于点击了一次继续
        {
            // 立即禁用所有按钮防止重复点击
            foreach (var btn in branch_btns)
            {
                btn.GetComponent<Button>().interactable = false;
            }

            Choice = choiceBtn.choice;

            // 使用创建按钮时捕获的、正确的对话对象，而不是不稳定的全局CurrentObj
            if (optionOwner == null || !optionOwner.ContainsKey("opt"))
            {
                Debug.LogError("Invalid choice button click - The dialogue object that created this option was not valid or had no options!");
                return;
            }

            JArray options = (JArray)optionOwner["opt"];
            if (Choice < 1 || Choice > options.Count)
            {
                Debug.LogError($"Invalid choice index: {Choice}. Valid range: 1-{options.Count}");
                return;
            }

            JObject selectedOption = (JObject)options[Choice - 1];
            if (!selectedOption.ContainsKey("dia"))
            {
                Debug.LogError("Selected option does not contain 'dia' key!");
                return;
            }

            JArray diaArray = (JArray)selectedOption["dia"];
            DialogArray.Push(new DialogGroup(diaArray));//根据选项定位新的对话组

            // 确保栈不为空再获取当前对象
            if (DialogArray.Count > 0 && CrtArray != null && CrtArray.Count > 0)
            {
                CurrentObj = (JObject)CrtArray[0];//进入选项内部对话
            }
            else
            {
                Debug.LogError("Failed to get valid dialog array after choice!");
                return;
            }

            foreach (var i in branch_btns)
            {
                //销毁按钮
                GameObject.Destroy(i.gameObject);
            }
            branch_btns.Clear();
            DisplayOne(CurrentObj);
        }

        // 新增：异步打字效果
        public static async Task TypingTxtAsync(string txt, TMP_Text label)
        {
            label.text = "";
            IsTypingTxt = true;

            // 如果存在正在进行的打印任务，取消它
            cancel?.Cancel();
            var currentCancelTokenSource = new CancellationTokenSource();
            cancel = currentCancelTokenSource;
            var token = currentCancelTokenSource.Token;

            StringBuilder sb = new StringBuilder();
            bool cancelled = false;
            for (int i = 0; i < txt.Length; i++)
            {
                if (token.IsCancellationRequested)
                {
                    cancelled = true;
                    break;
                }

                sb.Append(txt[i]);
                label.text = sb.ToString();
                try
                {
                    await Task.Delay((int)(TypingSpeed * 1000), token);
                }
                catch (TaskCanceledException)
                {
                    cancelled = true;
                    break;
                }
            }

            if (!cancelled)
            {
                label.text = txt; // 确保显示完整文本
            }
            IsTypingTxt = false;
        }

        public static void DisplayOne(JObject crt_obj)//传入一个对话对象
        {
            #region 处理跳转和初始化
            if (EndDialog)
            {
                End();
                EndDialog = false;
                return;
            }
            if (NextDialog != null)
            {
                SceneInit(NextDialog);
                DisplayOne(CurrentObj);
                return;
            }
            // 添加栈空检查
            if (DialogArray.Count == 0)
            {
                Debug.LogError("DialogArray is empty! Please call SceneInit first.");
                return;
            }
            waitForChoice = false;
            DialogEnabled = true;
            #endregion

            foreach (JProperty key in crt_obj.Properties())//解析一个dia下所有参数
            {
                switch (key.Name)
                {
                    case "chr":
                        DlgMgr.chr.text = Map.ChrMap[(int)key.Value];
                        break;
                    case "txt":
                        if (cancel != null && cancel.Token.CanBeCanceled)
                            cancel.Cancel(); // 取消之前的打印任务

                        cancel = new CancellationTokenSource(); // 创建新的取消令牌
                        _ = TypingTxtAsync(key.Value.ToString(), DlgMgr.txt); // 不等待异步任务,让它独立执行
                        break;
                    case "act":
                        foreach (JProperty acts in key.Value)
                        {
                            try
                            {
                                string fun = acts.Name.ToString();
                                string[] args = acts.Value.ToString().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                if (args.Count() == 0)
                                    Map.ActMap[fun]();
                                else
                                    Map.ActArgMap[fun](args);
                            }
                            catch (Exception ex)
                            {
                                Debug.LogError($"[{acts.Name}]未绑定到函数: {ex.Message}");
                            }
                        }
                        break;
                    case "opt":
                        DialogEnabled = false;
                        waitForChoice = true;
                        // 在创建新按钮前，确保旧按钮被清理
                        foreach (var oldBtn in branch_btns)
                        {
                            GameObject.Destroy(oldBtn.gameObject);
                        }
                        branch_btns.Clear();

                        int i = 1;
                        JObject optionOwnerObject = crt_obj; // 捕获当前对话对象作为选项的"所有者"
                        foreach (JObject option in key.Value)
                        {
                            // 直接创建按钮
                            GameObject btnObject = GameObject.Instantiate(DlgMgr.BtnTplt.gameObject);
                            btnObject.transform.SetParent(DlgMgr.dlgui.transform, false);
                            ChoiceBtn btn = btnObject.GetComponent<ChoiceBtn>();
                            branch_btns.Add(btn);
                            btn.text = option["optn"].ToString();
                            btn.choice = i;

                            RectTransform rectTransform = btn.GetComponent<RectTransform>();
                            float buttonHeight = rectTransform.rect.height;
                            float buttonWidth = rectTransform.rect.width;
                            rectTransform.anchoredPosition = new Vector2(DlgMgr.dlgui.GetComponent<RectTransform>().rect.width - buttonWidth, DlgMgr.dlgui.GetComponent<RectTransform>().rect.height / 3 + buttonHeight / 2 + buttonHeight * 1.5f * (i - 1));

                            Button button = btn.GetComponent<Button>();
                            // 将捕获的"所有者"对象传入监听器，确保点击时能找到正确的选项数据
                            button.onClick.AddListener(() => ChoiceBtn_Click(btn, optionOwnerObject)); // 绑定点击事件
                            i++;
                        }
                        break;
                    case "next":
                        NextDialog = key.Value.ToString();
                        break;
                }
            }

            //解析任务结束 已经显示在屏幕上 开始定位下一次解析位置 所有current皆为下次待解析对象
            // 添加栈空检查
            if (DialogArray.Count == 0)
            {
                Debug.LogError("DialogArray became empty during processing!");
                return;
            }

            if (CrtIndex < CrtArray.Count)
                CrtIndex++;//最高的优先级 优先解析下一个对话
            if (waitForChoice)
                return;//若处于等待选项时 直接返回继续等待
            if (NextDialog != null)
            {
                SceneInit(NextDialog);
                return;
            }

            // 再次检查栈状态
            if (DialogArray.Count == 0)
            {
                Debug.LogError("DialogArray is empty after NextDialog processing!");
                return;
            }

            if (CrtArray != null && CrtArray.Count - CrtIndex == 0)//本层已全部解析完毕 退出本层
            {
                while (DialogArray.Count > 0 && CrtArray != null && CrtArray.Count - CrtIndex == 0)
                {
                    DialogArray.Pop();
                    if (DialogArray.Count == 0)//场景所有对话结束
                    {
                        if (scene_index >= JsonSrcMgr.JsonSource.Count - 1)
                        {
                            EndDialog = true;
                            return;
                        }
                        SceneInit(JsonSrcMgr.JsonSource[++scene_index]["scene"].ToString());
                        //下次点击 在事件开头直接退出
                        return;
                    }
                }
                // 确保栈不为空且有有效数据
                if (DialogArray.Count > 0 && CrtArray != null && CrtIndex < CrtArray.Count)
                {
                    CurrentObj = (JObject)CrtArray[CrtIndex];//切换到外层
                }
            }
            else if (DialogArray.Count > 0 && CrtArray != null && CrtArray.Count - CrtIndex > 0)
            {
                CurrentObj = (JObject)CrtArray[CrtIndex];
            }
        }

        public static void End()
        {
            DlgMgr.dlgui.enabled = false;
            DlgMgr.DialogMode = false;
        }

        public static void ResetDialog()
        {
            // 重置所有对话状态
            Choice = 0;
            CurrentGroupObjIndex = 0;
            scene_index = 0;
            DialogArray.Clear();
            CurrentObj = null;
            waitForChoice = false;
            EndDialog = false;
            NextDialog = null;

            // 清理选项按钮
            foreach (var btn in branch_btns)
            {
                GameObject.Destroy(btn.gameObject);
            }
            branch_btns.Clear();

            // 取消正在进行的打字效果
            cancel?.Cancel();
            cancel = null;
            IsTypingTxt = false;

            // 重置对话启用状态
            DialogEnabled = true;
        }

    }

    public class JsonSrcMgr
    {
        public static JArray JsonSource { get; private set; }

        public static void LoadStory(TextAsset storyFile)
        {
            if (storyFile != null)
            {
                try
                {
                    JsonSource = JArray.Parse(storyFile.text);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error parsing story file: {e.Message}");
                    JsonSource = null;
                }
            }
            else
            {
                Debug.LogError("Story file is not assigned!");
                JsonSource = null;
            }
        }
        public static Stack<JArray> History = new Stack<JArray>();
        public static JObject GetSceneObj(string scene)
        {
            if (JsonSource == null)
            {
                Debug.LogError("JsonSource is not loaded! Call LoadStory first.");
                return null;
            }
            foreach (var obj in JsonSource)
            {
                if (obj["scene"].ToString() == scene)
                    return obj as JObject;
            }
            return null;
        }
    }
}