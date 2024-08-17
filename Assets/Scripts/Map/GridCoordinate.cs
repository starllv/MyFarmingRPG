using UnityEngine;

[System.Serializable]
public class GridCoordinate
{
    public int x;
    public int y;

    public GridCoordinate(int p1, int p2) {

        x = p1;
        y = p2;
    }
    // 自定义GridCoordinate到Vector2的显式转换
    public static explicit operator Vector2(GridCoordinate gridCoordinate) {

        return new Vector2((float)gridCoordinate.x, (float)gridCoordinate.y);
    }
    // 自定义GridCoordinate到Vector2Int的显式转换
    public static explicit operator Vector2Int(GridCoordinate gridCoordinate) {
        
        return new Vector2Int(gridCoordinate.x, gridCoordinate.y);
    }
    // 自定义GridCoordinate到Vector3的显式转换
    public static explicit operator Vector3(GridCoordinate gridCoordinate) {

        return new Vector3((float)gridCoordinate.x, (float)gridCoordinate.y, 0f);
    }
    // 自定义GridCoordinate到Vector3Int的显式转换
    public static explicit operator Vector3Int(GridCoordinate gridCoordinate) {

        return new Vector3Int(gridCoordinate.x, gridCoordinate.y, 0);
    }
}
