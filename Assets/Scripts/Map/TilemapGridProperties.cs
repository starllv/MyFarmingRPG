using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
public class TilemapGridProperties : MonoBehaviour
{
    private Tilemap tilemap;            // Tilemap组件
    [SerializeField] private SO_GridProperties gridProperties = null;        // GridProperties资源
    [SerializeField] private GridBoolProperty gridBoolProperty = GridBoolProperty.diggable;      // GridBoolProperty网格各属性

    private void OnEnable() {
        // 只有在编辑器模式下才执行
        if (!Application.IsPlaying(gameObject)){
            
            tilemap = GetComponent<Tilemap>();
            if (gridProperties != null) {
                // 清空grid属性列表
                gridProperties.gridPropertyList.Clear();
            }
        }
    }

    private void OnDisable() {
        // 只有在编辑器模式下才执行
        if (!Application.IsPlaying(gameObject)) {

            UpdateGridProperties();

            if (gridProperties != null) {
                // 标记目标物体已改变。当资源已改变并需要保存到磁盘，Unity内部使用dirty标识来查找。
                // 如果修改一个prefab的MonoBehaviour或ScriptableObject变量，必须告诉Unity该值已经改变。
                EditorUtility.SetDirty(gridProperties);
            }
        }
    }

    private void UpdateGridProperties() {
        // 将 Tilemap 的原点和大小压缩到存在 Tiles 的边界。
        tilemap.CompressBounds();

        if (!Application.IsPlaying(gameObject)) {

            if (gridProperties != null) {

                Vector3Int startCell = tilemap.cellBounds.min;
                Vector3Int endCell = tilemap.cellBounds.max;

                for (int x = startCell.x; x <= endCell.x; x++) {

                    for (int y = startCell.y; y <= endCell.y; y++) {

                        TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0)); 

                        if (tile != null) {

                            gridProperties.gridPropertyList.Add(new GridProperty(new GridCoordinate(x, y), gridBoolProperty, true));
                        }
                    }
                }
            }
        }  
    }

    private void Update() {

        if (!Application.IsPlaying(gameObject)) {

            Debug.Log("DISABLE PROPERTY TILEMAPS");
        }
    }
}
