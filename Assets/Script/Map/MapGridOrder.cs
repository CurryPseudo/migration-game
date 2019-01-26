using UnityEngine;

[RequireComponent(typeof(MapGridPosition), typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class MapGridOrder : MonoBehaviour {
    private void Update() {
        var renderer = GetComponent<SpriteRenderer>();
        var grid = GetComponent<MapGridPosition>();
        renderer.sortingOrder = -(grid.MapPosition.x + grid.MapPosition.y) * 2;
    }
}