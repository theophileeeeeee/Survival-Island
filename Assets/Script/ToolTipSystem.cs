using UnityEditor;
using UnityEngine;

public class ToolTipSystem : MonoBehaviour
{
    public static ToolTipSystem instance;
    [SerializeField] private ToolTip ToolTip;
    private void Awake()
    {
        instance = this;
    }
    public void Show(string content, string header = "")
    {
        ToolTip.SetText(content, header);
        ToolTip.gameObject.SetActive(true);
    }
    public void Hide()
    {
        ToolTip.gameObject.SetActive(false);
    }
}
