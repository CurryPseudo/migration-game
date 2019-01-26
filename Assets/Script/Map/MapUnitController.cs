using UnityEngine;
using Sirenix.OdinInspector;

public class MapUnitController : SerializedMonoBehaviour {
    public MapController mapController;
    public MapUnit mapUnit;
    private bool ifInited = false;

    private void OnDrawGizmos() {
    }
    public void Start() {
    }
    private void OnDisable() {
        if(mapController != null) {
            mapController.map.DeleteMapUnit(mapUnit);
        }
    }
    public void Update() {
        
        if(mapController != null) {
            mapUnit.controller = this;
            if(ifInited) {
                mapController.map.UpdateMapUnit(mapUnit);
            }
            else {
                mapController.map.InsertMapUnit(mapUnit);
                ifInited = true;
            }
        }
    }
}