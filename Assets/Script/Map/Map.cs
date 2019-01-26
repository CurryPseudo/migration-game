using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
public interface IMap {
    IMapUnit GetMapUnit(int x, int y);
    void Init();
}

public class Map : IMap {
    private Dictionary<IMapUnit, List<Vector2Int>> unitPositionsMap = new Dictionary<IMapUnit, List<Vector2Int>>();
    public List<List<IMapUnit>> data = new List<List<IMapUnit>>();
    [ReadOnly]
    public Vector2Int lastSize = Vector2Int.zero;
    public Vector2Int size;
    public float angle;
    public float length;
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
    [Button("Resize")]
    public void Init() {
        unitPositionsMap = new Dictionary<IMapUnit, List<Vector2Int>>();
    }
    public void Resize() {
        List<IMapUnit> units = new List<IMapUnit>();
        foreach(var unit in GetAllUnits()) {
            bool ifAdd = true;
            foreach(var pos in unit.GetPositionsInt()) {
                if(!(pos.x >= 0 && pos.x < size.x && pos.y >= 0 && pos.y < size.y)) {
                    unit.GetController().gameObject.SetActive(false);
                    ifAdd = false;
                    break;
                }
            }
            if(ifAdd) {
                units.Add(unit);
            }
        }
        data.Clear(); 
        for(int i = 0; i < size.x; i++) {
            var unitRow = new List<IMapUnit>();
            for(int j = 0; j < size.y; j++) {
                unitRow.Add(null);
            }
            data.Add(unitRow);
        }
        foreach(var unit in units) {
            InsertMapUnit(unit);
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
        if(x < 0 || x >= size.x || y < 0 || y >= size.y) {
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