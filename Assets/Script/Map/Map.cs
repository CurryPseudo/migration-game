using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
public interface IMap {
    IMapUnit GetMapUnit(int x, int y);
    void _Init();
}

public class Map : IMap {
    private Dictionary<IMapUnit, List<Vector2Int>> unitPositionsMap = new Dictionary<IMapUnit, List<Vector2Int>>();
    public int screenWidth;
    [System.NonSerialized]
    public List<MapUnitController> unitControllers;
    private List<MapGround>[,] grounds;
    private List<MapUnit>[] showingUnit;
    public List<List<IMapUnit>> data = new List<List<IMapUnit>>();
    [ReadOnly]
    public Vector2Int lastSize = Vector2Int.zero;
    public Vector2Int size;
    public float angle;
    public float length;
    public int currentScreenX = 0;
    public Vector2 UAxis {
        get {
            return Vector2Util.RotateAxisDir(90 - angle / 2, length);
        }
    }
    public Vector2 VAxis {
        get {
            return Vector2Util.RotateAxisDir(angle / 2 + 90, length);
        }
    }
    public Vector2 originPoint;
    public IEnumerable<IMapUnit> GetAllUnits() {
        foreach(var unitRow in data) {
            foreach(var unit in unitRow) {
                if(unit != null) {
                    yield return unit;
                }
            }
        }
    }
    public void _Init() {
        grounds = new List<MapGround>[size.x, size.y];
        showingUnit = new List<MapUnit>[size.x - screenWidth + 1];
        foreach(var ground in Object.FindObjectsOfType<MapGround>()) {
            var pos = ground.GetComponent<MapGridPosition>();
            if(pos != null) {
                var list = grounds[pos.MapPositionInt.x, pos.MapPositionInt.y];
                if(list == null) {
                    list = new List<MapGround>();
                    grounds[pos.MapPositionInt.x, pos.MapPositionInt.y] = list;
                }
                list.Add(ground);
                ground.gameObject.SetActive(InScreen(pos.MapPositionInt));
            }
        }
        foreach(var mapUnit in unitControllers) {
            mapUnit.gameObject.SetActive(AllInScreen(mapUnit.mapUnit.GetPositionsInt()));
            int x = mapUnit.mapUnit.GetOriginPointInt().x - screenWidth;
            if(x >= 0) {
                var list = showingUnit[x];
                if(list == null) {
                    list = new List<MapUnit>();
                    showingUnit[x] = list;
                }
                list.Add(mapUnit.mapUnit);
            }
        }
    }
    public void FallAllCurrentGround() {
        for(int i = 0; i < size.y; i++) {
            foreach(var ground in grounds[currentScreenX, i]) {
                ground.ChangeState(ground.Fall());
            }
        }
    }
    public float GetNextMoveOnTime() {
        return 60f + 1f / 8f * currentScreenX * currentScreenX - 5 * currentScreenX;
    }
    public IEnumerator WaitToMoveOn() {
        for(int i = 0; i < size.y; i++) {
            foreach(var ground in grounds[currentScreenX, i]) {
                ground.ChangeState(ground.Alert());
            }
        }
        {
            float timeCount = 0;
            while(timeCount < GetNextMoveOnTime()) {
                yield return null;
                for(int i = 0; i < size.y; i++) {
                    foreach(var ground in grounds[currentScreenX, i]) {
                        ground.alertCycle = AlertCycle(GetNextMoveOnTime() - timeCount);
                    }
                }
                timeCount += Time.deltaTime;
            }
        }
        yield break;
    }
    public float AlertCycle(float timeLeft) {
        if(timeLeft > 30f) {
            return 2f;
        }
        else if(timeLeft > 15f) {
            return 1.5f;
        }
        else if(timeLeft > 10f) {
            return 1f;
        }
        else if (timeLeft > 5f) {
            return 0.5f;
        }
        else {
            return 0.25f;
        }
    }
    public void MoveOnActive() {
        for(int i = 0; i < size.y; i++) {
            {
                foreach(var ground in grounds[currentScreenX, i]) {
                    ground.gameObject.SetActive(false);
                }
                var unit = GetMapUnit(currentScreenX, i);
                if(unit != null) {
                    var controller = unit.GetController();
                    var house = controller.GetComponent<House>();
                    if(house != null) {
                        house.PrepareToDestroy();
                    }
                    else {
                        controller.gameObject.SetActive(false);
                    }
                }
            }
        }
        currentScreenX++;
        for(int i = 0; i < size.y; i++) {
            foreach(var ground in grounds[currentScreenX - 1 + screenWidth, i]) {
                ground.gameObject.SetActive(true);
            }
            {
                var list = showingUnit[currentScreenX - 1];
                if(list != null) {
                    foreach(var unit in list) {
                        unit.GetController().gameObject.SetActive(true);
                    }
                }
            }
        }
    }
    public bool AllInScreen(IEnumerable<Vector2Int> positions) {
        foreach(var pos in positions) {
            if(!InScreen(pos)) return false;
        }
        return true;
    }
    public bool InScreen(Vector2Int pos) {
        return pos.x >= currentScreenX && pos.x < currentScreenX + screenWidth && pos.y >= 0 && pos.y < size.y;
    }
    public Vector2 ScreenClamp(Vector2 pos) {
        pos.x = Mathf.Clamp(pos.x, currentScreenX - 0.5f, currentScreenX + screenWidth - 0.5f);
        pos.y = Mathf.Clamp(pos.y, -0.5f, size.y - 0.5f);
        return pos;
    }
    [Button("Resize")]
    public void Resize() {
        data.Clear(); 
        for(int i = 0; i < size.x; i++) {
            var unitRow = new List<IMapUnit>();
            for(int j = 0; j < size.y; j++) {
                unitRow.Add(null);
            }
            data.Add(unitRow);
        }
        lastSize = size;
    }
    public Vector2Int WorldToMapPointClampedRounded(Vector2 worldPoint) {
        Vector2 result = WorldToMapPointClamped(worldPoint);
        return Vector2Util.RoundToInt(result);
    }
    public Vector2 WorldToMapPointClamped(Vector2 worldPoint) {
        //Debug.Log("world: " + worldPoint.ToString());
        Vector2 result = ClampPosition(WorldToMapPoint(worldPoint));
        //Debug.Log("to map: " + result.ToString());
        return result;
    }
    public Vector2 WorldToMapPoint(Vector2 worldPoint) {
        Vector2 offset = worldPoint - originPoint;
        Vector2 u = UAxis;
        Vector2 v = VAxis;
        float a = u.y * v.x - u.x * v.y;
        float fv = (u.y * offset.x - u.x * offset.y) / a;
        float fu = (v.x * offset.y - v.y * offset.x) / a;
        return new Vector2(fu, fv);
    }
    public Vector2 MapToWorldPoint(Vector2 mapPoint) {
        //Debug.Log("map: " + mapPoint);
        Vector2 result = mapPoint.x * UAxis + mapPoint.y * VAxis + originPoint;
        //Debug.Log("to world: " + result);
        return result;
    }
    public Vector2 MapToWorldDirection(Vector2 direction) {
        return MapToWorldPoint(direction) - MapToWorldPoint(Vector2.zero);
    }
    public bool AreaEmpty(IEnumerable<Vector2Int> positions) {
        foreach(Vector2Int position in positions) {
            if(GetMapUnit(position) != null) return false;
        }
        return true;
    }
    public void InsertMapUnit(IMapUnit unit) {
        Debug.Assert(AreaEmpty(unit.GetPositionsInt()));
        if(unitPositionsMap == null) unitPositionsMap = new Dictionary<IMapUnit, List<Vector2Int>>();
        unitPositionsMap.Add(unit, new List<Vector2Int>());
        foreach(var pos in unit.GetPositionsInt()) {
            SetMapUnit(pos, unit);
            unitPositionsMap[unit].Add(pos);
        }
    }
    public Vector2 ClampPosition(Vector2 position) {
        position.x = Mathf.Clamp(position.x, 0, size.x - 1);
        position.y = Mathf.Clamp(position.y, 0, size.y - 1);
        return position;
    }
    public void UpdateMapUnit(IMapUnit unit) {
        DeleteMapUnit(unit);
        Debug.Assert(AreaEmpty(unit.GetPositionsInt()));
        InsertMapUnit(unit);
    }
    public void DeleteMapUnit(IMapUnit unit) {
        foreach(var pos in unitPositionsMap[unit]) {
            SetMapUnit(pos, null);
        }
        unitPositionsMap.Remove(unit);
    }
    public IMapUnit GetMapUnit(Vector2Int position)
    {
        return GetMapUnit(position.x, position.y);
    }
    public IMapUnit GetMapUnit(int x, int y)
    {
        if(!InScreen(new Vector2Int(x, y))) {
            return new OutsideMapUnit();
        }
        return data[x][y];
    }
    public void SetMapUnit(int x, int y, IMapUnit mapUnit) {
        data[x][y] = mapUnit;
    }
    public void SetMapUnit(Vector2Int position, IMapUnit mapUnit) {
        SetMapUnit(position.x, position.y, mapUnit);
    }
}