using UnityEngine;
using Sirenix.OdinInspector;

public class MapUnitController : SerializedMonoBehaviour {
    public MapController mapController;
    public MapUnit mapUnit;
    private bool ifInited = false;
    private bool firstInited = true;

    private void OnDrawGizmos() {
    }
    public void Start() {
    }
    private void OnEnable() {
        if(firstInited) {
            mapUnit.controller = this;
            gameObject.SetActive(false);
            firstInited = false;
            if(mapController.map.unitControllers == null) {
                mapController.map.unitControllers = new System.Collections.Generic.List<MapUnitController>();
            }
            mapController.map.unitControllers.Add(this);
            return;
        }
        Init();
    }
    private void OnDisable() {
        if(ifInited) {
            mapController.map.DeleteMapUnit(mapUnit);
            ifInited = false;
        }
    }
    public void Init() {
        if(!ifInited) {
            ifInited = true;
            mapController.map.InsertMapUnit(mapUnit);
        }
    }
    public void Update() {
        mapController.map.UpdateMapUnit(mapUnit);
    }
}