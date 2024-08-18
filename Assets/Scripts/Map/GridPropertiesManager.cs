using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GenerateGUID))]
public class GridPropertiesManager : SingletonMonoBehaviour<GridPropertiesManager>, ISaveable
{
    public Grid grid;
    private Dictionary<string, GridPropertyDetails> gridPropertyDictionary;           // 保存当前激活场景的网格属性的字典
    [SerializeField] private SO_GridProperties[] sO_GridPropertiesArray = null;

    private string _iSaveableUniqueID;
    public string ISaveableUniqueID { get { return _iSaveableUniqueID; } set { _iSaveableUniqueID = value; } }

    private GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }

    protected override void Awake() {

        base.Awake();

        ISaveableUniqueID = GetComponent<GenerateGUID>().GUID;
        GameObjectSave = new GameObjectSave();
    }

    private void OnEnable() {
        // 注册为要保存的对象
        ISaveableRegister();
        // 订阅场景加载完成事件
        EventHandler.AfterSceneLoadEvent += AfterSceneLoaded;
    }

    private void OnDisable() {

        ISaveableDeregister();

        EventHandler.AfterSceneLoadEvent -= AfterSceneLoaded;
    }

    private void Start() {
        // 初始化网格属性
        InitialiseGridProperties();
    }

    private void InitialiseGridProperties() {
        // 遍历各个场景的网格属性列表
        foreach (SO_GridProperties so_GridProperties in sO_GridPropertiesArray) {

            Dictionary<string, GridPropertyDetails> gridPropertyDictionary = new Dictionary<string, GridPropertyDetails>();
            // 遍历各个网格属性
            foreach (GridProperty gridProperty in so_GridProperties.gridPropertyList) {

                GridPropertyDetails gridPropertyDetails;
                // 从字典中获取坐标处的网格属性
                gridPropertyDetails = GetGridPropertyDetails(gridProperty.gridCoordinate.x, gridProperty.gridCoordinate.y, gridPropertyDictionary);
                // 若网格属性不存在则创建一个新的对象
                if (gridPropertyDetails == null) {

                    gridPropertyDetails = new GridPropertyDetails();
                }
                // 根据网格属性设置网格属性细节的各个属性
                switch (gridProperty.gridBoolProperty) {

                    case GridBoolProperty.diggable:
                        gridPropertyDetails.isDiggable = gridProperty.gridBoolValue;
                        break;
                    case GridBoolProperty.canDropItem:
                        gridPropertyDetails.canDropItem = gridProperty.gridBoolValue;
                        break;
                    case GridBoolProperty.canPlaceFurniture:
                        gridPropertyDetails.canPlaceFurniture = gridProperty.gridBoolValue;
                        break;
                    case GridBoolProperty.isPath:
                        gridPropertyDetails.isPath = gridProperty.gridBoolValue;
                        break;
                    case GridBoolProperty.isNPCObstacle:
                        gridPropertyDetails.isNPCObstacle = gridProperty.gridBoolValue;
                        break;

                    default:
                        break;
                }
                // 将网格属性细节保存到网格属性字典中
                SetGridPropertyDetails(gridProperty.gridCoordinate.x, gridProperty.gridCoordinate.y, gridPropertyDetails, gridPropertyDictionary);
            }
            // 新建场景保存类
            SceneSave sceneSave = new SceneSave();
            // 场景保存类中有网格属性细节字典，保存每个场景的网格属性字典
            sceneSave.gridPropertyDetailsDictionary = gridPropertyDictionary;
            // 若当前遍历的是开始场景的网格属性，则将属性字典保存下来，因为这是游戏开始时加载的第一个场景
            if (so_GridProperties.sceneName.ToString() == SceneControllerManager.Instance.startingSceneName.ToString()) {

                this.gridPropertyDictionary = gridPropertyDictionary;
            }
            // 将已设置好的各场景保存类添加到游戏物体保存类中，用于场景信息的保存
            GameObjectSave.sceneData.Add(so_GridProperties.sceneName.ToString(), sceneSave);
        }
    }

    private void AfterSceneLoaded() {

        grid = GameObject.FindObjectOfType<Grid>();
    }

    public GridPropertyDetails GetGridPropertyDetails(int gridX, int gridY, Dictionary<string, GridPropertyDetails> gridPropertyDictionary) {

        string key = "x" + gridX + "y" + gridY;

        GridPropertyDetails gridPropertyDetails;

        if (!gridPropertyDictionary.TryGetValue(key, out gridPropertyDetails)) {

            return null;
        }
        else {

            return gridPropertyDetails;
        }
    }

    public GridPropertyDetails GetGridPropertyDetails(int gridX, int gridY) {

        return GetGridPropertyDetails(gridX, gridY, gridPropertyDictionary);
    }

    public void ISaveableRegister() {

        SaveLoadManager.Instance.iSaveableObjectList.Add(this);
    }

    public void ISaveableDeregister() {

        SaveLoadManager.Instance.iSaveableObjectList.Remove(this);
    }

    public void ISaveableStoreScene(string sceneName) {

        GameObjectSave.sceneData.Remove(sceneName);

        SceneSave sceneSave = new SceneSave();

        sceneSave.gridPropertyDetailsDictionary = gridPropertyDictionary;

        GameObjectSave.sceneData.Add(sceneName, sceneSave);
    }

    public void ISaveableRestoreScene(string sceneName) {

        if (GameObjectSave.sceneData.TryGetValue(sceneName, out SceneSave sceneSave)) {

            if (sceneSave.gridPropertyDetailsDictionary != null) {

                gridPropertyDictionary = sceneSave.gridPropertyDetailsDictionary;
            }
        }
    }

    public void SetGridPropertyDetails(int gridX, int gridY, GridPropertyDetails gridPropertyDetails) {

        SetGridPropertyDetails(gridX, gridY, gridPropertyDetails, gridPropertyDictionary);
    }

    public void SetGridPropertyDetails(int gridX, int gridY, GridPropertyDetails gridPropertyDetails, Dictionary<string, GridPropertyDetails> gridPropertyDictionary) {
        // 生成字典的键
        string key = "x" + gridX + "y" + gridY;

        gridPropertyDetails.gridX = gridX;
        gridPropertyDetails.gridY = gridY;
        // 键值对保存到字典
        gridPropertyDictionary[key] = gridPropertyDetails;
    }
}
