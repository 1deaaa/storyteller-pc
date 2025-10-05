// StartDlg.cs (快速修复版)
using UnityEngine;

public class StartDlg : MonoBehaviour
{
    [Tooltip("需要旋转的文本UI的Transform组件")]
    public Transform textTransform; // 把 Text (TMP) 物体拖到这里

    [Tooltip("设置UI朝向的目标相机，如果为空，则默认为主相机")]
    public Camera view;

    [Tooltip("UI可见并朝向相机的最大距离")]
    public float maxVisibleDistance = 100f;

    private bool playerInRange = false;

    [Tooltip("对话文本文件")]
    public TextAsset StoryFile;

    void Start()
    {
        if (view == null) view = Camera.main;
        if (textTransform != null) textTransform.gameObject.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (StoryFile == null)
            {
                Debug.LogError("StartDlg: StoryFile 未分配！");
                return;
            }
            DlgMgr.StartDlg(StoryFile);
        }
    }

    // ... OnTriggerEnter 和 OnTriggerExit 不变 ...
    void OnTriggerEnter(Collider other) { if (other.CompareTag("Player")) playerInRange = true; }
    void OnTriggerExit(Collider other) { if (other.CompareTag("Player")) playerInRange = false; }


    void LateUpdate()
    {
        if (view == null || textTransform == null) return;

        float distance = Vector3.Distance(transform.position, view.transform.position);
        bool shouldBeVisible = (distance <= maxVisibleDistance);

        if (textTransform.gameObject.activeSelf != shouldBeVisible)
        {
            textTransform.gameObject.SetActive(shouldBeVisible);
        }

        if (shouldBeVisible)
        {
            // 【核心修改】只旋转指定的textTransform，而不是这个脚本所在的transform
            textTransform.rotation = view.transform.rotation;
        }
    }
}
