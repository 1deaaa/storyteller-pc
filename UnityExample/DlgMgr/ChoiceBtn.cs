using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class ChoiceBtn : MonoBehaviour//用于扩展按钮选项
{
    public string text
    {
        get => buttonText != null ? buttonText.text : null;
        set
        {
            if (buttonText != null)
            {
                buttonText.text = value;
            }
            else
            {
                Debug.LogWarning("Button text is not assigned!");
            }
        }
    }

    public Vector2 size
    {
        get => buttonRect != null ? buttonRect.sizeDelta : Vector2.zero;
        set
        {
            if (buttonRect != null)
            {
                buttonRect.sizeDelta = value;
            }
            else
            {
                Debug.LogWarning("Button RectTransform is not assigned!");
            }
        }
    }

    public Vector3 position
    {
        get => buttonRect != null ? buttonRect.localPosition : Vector3.zero;
        set
        {
            if (buttonRect != null)
            {
                buttonRect.localPosition = value;
            }
            else
            {
                Debug.LogWarning("Button RectTransform is not assigned!");
            }
        }
    }

    public int choice; // 自动属性，用于保存选择值

    private TMP_Text buttonText;
    private RectTransform buttonRect;

    private void Awake()
    {
        buttonRect = GetComponent<RectTransform>();
        buttonText = GetComponentInChildren<TMP_Text>();
    }
}
