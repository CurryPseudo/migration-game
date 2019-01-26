using UnityEngine;

[ExecuteInEditMode]
public class MapGridPosition : MonoBehaviour {
    public MapController mapController;

    [SerializeField]
    private Vector2Int mapPosition;
    public Vector2Int MapPosition {
        get {
            return mapPosition;
        }
        set {
            value = Vector2Util.RoundToInt(mapController.map.ClampPosition(value));
            mapPosition = value;
            Position = mapController.map.MapToWorldPoint(mapPosition);
        }
    }
    public Vector2 Position {
        get {
            return transform.position;
        }
        set {
            transform.position = new Vector3(value.x, value.y, transform.position.z);
        }
    }
    public void Update() {
        if(mapController != null) {
            mapPosition = mapController.map.WorldToMapPointClampedRounded(Position);
            Position = mapController.map.MapToWorldPoint(mapPosition);
        }
    }
}