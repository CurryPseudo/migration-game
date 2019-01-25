using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
[ExecuteInEditMode]
public class MapController : SerializedMonoBehaviour {
    [OdinSerialize]
    public Map map;
    private void OnDrawGizmos() {
        for(int i = 0; i < map.size.x + 1; i++) {
            float x = i;
            Vector2 start = new Vector2(x - 0.5f, -0.5f);
            Vector2 end = new Vector2(x - 0.5f, map.size.y - 0.5f);
            Gizmos.DrawLine(map.MapToWorldPoint(start), map.MapToWorldPoint(end));
        }
        for(int i = 0; i < map.size.y + 1; i++) {
            float y = i;
            Vector2 start = new Vector2(-0.5f,y - 0.5f);
            Vector2 end = new Vector2(map.size.x - 0.5f, y - 0.5f);
            Gizmos.DrawLine(map.MapToWorldPoint(start), map.MapToWorldPoint(end));
        }
    }
    public void Update() {
        map.Update();
    }

}