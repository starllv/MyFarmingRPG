using UnityEngine;
using Cinemachine;

public class SwitchConfineBoundingShape : MonoBehaviour
{
    private void OnEnable() {

        EventHandler.AfterSceneLoadEvent += SwitchBoundingShape;
    }

    private void OnDisable() {

        EventHandler.AfterSceneLoadEvent -= SwitchBoundingShape;
    }

    // 切换cinemachine用来定义屏幕边界的碰撞器
    private void SwitchBoundingShape()
    {
        PolygonCollider2D polygonCollider2D = GameObject.FindGameObjectWithTag(Tags.BoundsConfiner).GetComponent<PolygonCollider2D>();

        CinemachineConfiner cinemachineConfiner = GetComponent<CinemachineConfiner>();

        cinemachineConfiner.m_BoundingShape2D = polygonCollider2D;

        cinemachineConfiner.InvalidatePathCache();
    }
}
