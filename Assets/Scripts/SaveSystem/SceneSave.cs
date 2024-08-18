using System.Collections.Generic;

[System.Serializable]
public class SceneSave
{
    public List<SceneItem> listSceneItem;               // 场景物体列表
    public Dictionary<string, GridPropertyDetails> gridPropertyDetailsDictionary;   // 网格属性细节字典，用于保存所有网格属性
}
