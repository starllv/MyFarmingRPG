using System.Collections.Generic;

[System.Serializable]
public class SceneSave
{
    // 字符串是场景物体列表的名字
    public Dictionary<string, List<SceneItem>> listSceneItemDictionary;
}
