using UnityEngine;

[ExecuteAlways]
public class GenerateGUID : MonoBehaviour
{
    [SerializeField] private string _gUID = "";

    public string GUID { get => _gUID; set => _gUID = value; }

    private void Awake() {
        // 仅在编辑模式运行
        if (!Application.IsPlaying(gameObject)) {
            // 确保游戏物体有一个GUID
            if (_gUID == "") {

                _gUID = System.Guid.NewGuid().ToString();
            }
        }
    }
}
