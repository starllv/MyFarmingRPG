[System.Serializable]
public class GridProperty
{
    public GridCoordinate gridCoordinate;        // 网格的位置
    public GridBoolProperty gridBoolProperty;    // 网格的布尔属性，枚举类型，定义在Enums
    public bool gridBoolValue = false;          // 网格的布尔值

    public GridProperty(GridCoordinate gridCoordinate, GridBoolProperty gridBoolProperty, bool gridBoolValue) {

        this.gridCoordinate = gridCoordinate;
        this.gridBoolProperty = gridBoolProperty;
        this.gridBoolValue = gridBoolValue;
    }
}
