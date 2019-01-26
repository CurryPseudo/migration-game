using UnityEngine;

[RequireComponent(typeof(MapGridPosition))]
[ExecuteInEditMode]
public class MapGridOrder : MonoBehaviour {
    public new SpriteRenderer renderer;
    private void Update() {
        var grid = GetComponent<MapGridPosition>();
        if(renderer != null) {
            renderer.sortingOrder = -(grid.MapPositionInt.x + grid.MapPositionInt.y) * 2;
        }
    }
}