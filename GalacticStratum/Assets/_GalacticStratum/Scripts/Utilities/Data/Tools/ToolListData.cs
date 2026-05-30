using UnityEngine;

[CreateAssetMenu(fileName = "ToolListData", menuName = "Scriptable Objects/Tools/Tool List")]
public class ToolListData : ScriptableObject
{
    [SerializeField] private ToolData[] tools;

    public ToolData[] Tools => tools;
}
