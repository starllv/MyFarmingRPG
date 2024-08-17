using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "so_GridProperties", menuName = "Scriptable Objects/Grid Properties")]
public class SO_GridProperties : ScriptableObject
{
    public SceneName sceneName;      // 场景名称
    public int gridWidth;           // 网格宽度
    public int gridHeight;         // 网格高度
    public int originX;            // 网格原点X坐标
    public int originY;           // 网格原点Y坐标

    [SerializeField] public List<GridProperty> gridPropertyList;       // 网格属性列表
    
}
