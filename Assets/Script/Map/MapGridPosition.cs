using UnityEngine;

[ExecuteInEditMode]
public class MapGridPosition : MonoBehaviour {
    public bool rounded = true;
    public bool clamped = true;
    public MapController mapController;
    [SerializeField]
    private Vector2 mapPosition;
    public Vector2 MapPosition {
        get {
            return mapPosition;
        }
        set {
            mapPosition = value;
            Position = mapController.map.MapToWorldPoint(mapPosition);
        }
    }
    public Vector2Int MapPositionInt {
        get {
            return Vector2Util.RoundToInt(mapPosition);
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
            mapPosition = mapController.map.WorldToMapPoint(Position);
            if(clamped) {
                mapPosition = mapController.map.ClampPosition(mapPosition);
            }
            if(rounded) {
                mapPosition = Vector2Util.RoundToInt(mapPosition);
            }
            Position = mapController.map.MapToWorldPoint(mapPosition);
        }
    }
}