using UnityEngine;

[ExecuteInEditMode]
public class MapUnitController : MonoBehaviour {
    public MapController mapController;
    public MapUnit mapUnit;
    public Vector2Int MapPosReadOnly;
    public Vector2 Position {
        get {
            return transform.position;
        }
        set {
            transform.position = new Vector3(value.x, value.y, transform.position.z);
        }
    }
    public void Start() {
    }
    public void Update() {
        if(mapController != null) {
            if(mapUnit == null) {
                mapUnit = new SingleMapUnit(mapController.map.WorldToMapPoint(Position));
                mapController.map.InsertMapUnit(mapUnit);
            }
            else {
                mapController.map.UpdateMapUnit(mapUnit, mapController.map.WorldToMapPoint(Position));
            }
            MapPosReadOnly = mapUnit.GetOriginPoint();
            Position = mapController.map.MapToWorldPoint(MapPosReadOnly);
        }
    }
}