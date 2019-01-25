using UnityEngine;
[ExecuteInEditMode]
public class MapController : MonoBehaviour {
    public Map map;
    public void Update() {
        map.Update();
    }

}