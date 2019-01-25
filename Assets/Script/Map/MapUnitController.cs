using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class MapUnitController : SerializedMonoBehaviour {
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
    private void OnDrawGizmos() {
    }
    public void Start() {
    }
    private void OnDisable() {
        if(mapController != null) {
            if(mapUnit != null) {
                mapController.map.DeleteMapUnit(mapUnit);
            }
        }
    }
    public void LateUpdate() {
        if(mapController != null) {
            if(mapUnit == null) {
                mapUnit = new SingleMapUnit(mapController.map.WorldToMapPointClampedRounded(Position));
                mapController.map.InsertMapUnit(mapUnit);
            }
            else {
                mapController.map.UpdateMapUnit(mapUnit, mapController.map.WorldToMapPointClampedRounded(Position));
            }
            MapPosReadOnly = mapUnit.GetOriginPoint();
            Position = mapController.map.MapToWorldPoint(MapPosReadOnly);
        }
    }
}