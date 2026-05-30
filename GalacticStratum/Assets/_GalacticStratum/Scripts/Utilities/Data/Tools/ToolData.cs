using UnityEngine;

[CreateAssetMenu(fileName = "ToolData", menuName = "Scriptable Objects/Tools/ToolData")]
public class ToolData : ScriptableObject
{
    public enum ToolType
    {
        All,
        Locator,
        Miner,
        Storage
    }

    [Header("Tool Information")]
    [SerializeField] private ToolType toolType;
    [SerializeField] private string toolName;
    [SerializeField] private string toolDescription;
    [SerializeField] private Sprite toolIcon;

    [Header("Tool Properties")]
    [SerializeField] private int toolPrice;
    [SerializeField] private _ToolObject tool;

    public ToolType Type => toolType;
    public string Name => toolName;
    public string Description => toolDescription;
    public Sprite Icon => toolIcon;
    public int Price => toolPrice;
    public _ToolObject Tool => tool;
}
