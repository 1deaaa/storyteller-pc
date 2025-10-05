using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Resources;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class Map
{
    public static Dictionary<string, Action> ActMap = new Dictionary<string, Action>
    {
        #region 行为映射表
        // 绑定所有全局指令和函数 剩余的可以动态添加
        { "exit", FuncMap.Exit }
        #endregion 行为映射表
    };
    public static Dictionary<string, Action<string[]>> ActArgMap = new Dictionary<string, Action<string[]>>
    {
        #region 行为映射表
        { "bgm", FuncMap.Music},
        { "trans", FuncMap.Trans} // 新增：同步WinForm版本的Trans方法
        #endregion 行为映射表
    };


    public static Dictionary<int, string> ChrMap = new Dictionary<int, string>
    {
        #region 角色映射表
        { 0, " " },
        { 1, "我" },
        { 2, "初音" },
        { 3, "心声" }
        #endregion 角色映射表
    };
}
public static class FuncMap
{
    public static string GetRandomString(int length)
    {
        System.Random random = new System.Random();
        StringBuilder stringBuilder = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            char asciiChar = (char)(random.Next(32, 127)); // 32 到 126 是可打印的 ASCII 范围
            stringBuilder.Append(asciiChar);
        }
        return stringBuilder.ToString();
    }
    // public static void Error(object e)
    // {
    //     MessageBox.Show(e.ToString(), "o(TヘTo)", MessageBoxButtons.OK, MessageBoxIcon.Error);
    // }
    // public static void Inf(object e)
    // {
    //     MessageBox.Show(e.ToString(), "o(=•ェ•=)m", MessageBoxButtons.OK, MessageBoxIcon.Information);
    // }
    // public static bool Warn(object e)
    // {
    //     if (MessageBox.Show(e.ToString(), "⚠️", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
    //         return true;
    //     return false;
    // }
    public static void Music(string[] bgm)
    {
        // 音乐播放逻辑
        Debug.Log($"播放音乐: {string.Join(" ", bgm)}");
    }
    
    public static void Trans(string[] xy)
    {
        Method.Inf("正在播放" + xy[0] + "，" + xy[1]);
    }
    
    public static void Exit()
    {
        Application.Quit();
    }
    public static void RecordBranch(string brc)
    {

    }
}
class Method
{
    // 同步WinForm版本的错误和信息显示方法
    public static void Error(object e)
    {
        Debug.LogError(e.ToString());
    }
    
    public static void Inf(object e)
    {
        Debug.Log(e.ToString());
    }
    
    public static bool Warn(object e)
    {
        Debug.LogWarning(e.ToString());
        return true; // Unity中简化处理，总是返回true
    }

    private static IEnumerator TypeText(TMP_Text txt,string text,float deltaTime=0.05f)
    {
        txt.text = "";
        foreach (char c in text)
        {
            txt.text += c;
            yield return new WaitForSeconds(deltaTime); // 控制每个字符显示的速度
        }
    }
}
