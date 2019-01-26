using System.Collections.Generic;
using UnityEngine;
public interface IMapUnit {
    Vector2Int GetOriginPointInt();
    Vector2 GetOriginPoint();
    IEnumerable<Vector2Int> GetSizeUnitOffsetInt();
    IEnumerable<Vector2Int> GetPositionsInt();
    void SetPositionInt(Vector2Int originPoint);
    void SetPosition(Vector2 originPoint);
    MapUnitController GetController();
}
public class OutsideMapUnit : IMapUnit
{
    public Vector2 GetOriginPoint() {
        throw new System.NotImplementedException();
    }
    public void SetPosition(Vector2 originPoint) {
        throw new System.NotImplementedException();
    }
    public MapUnitController GetController()
    {
        return null;
    }

    public IEnumerable<Vector2Int> GetLastPositions()
    {
        throw new System.NotImplementedException();
    }

    public Vector2Int GetOriginPointInt()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<Vector2Int> GetPositionsInt()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<Vector2Int> GetSizeUnitOffsetInt()
    {
        throw new System.NotImplementedException();
    }

    public void Init(Vector2Int originPoint)
    {
        throw new System.NotImplementedException();
    }

    public void Init()
    {
        throw new System.NotImplementedException();
    }

    public bool IsDirty()
    {
        throw new System.NotImplementedException();
    }

    public void Revert()
    {
        throw new System.NotImplementedException();
    }

    public void SetPositionInt(Vector2Int originPoint)
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }
}
public abstract class MapUnit : IMapUnit
{
    [System.NonSerialized]
    public MapUnitController controller;
    public MapUnitController GetController()
    {
        return controller;
    }

    public virtual IEnumerable<Vector2Int> GetPositionsInt() {
        foreach(var pos in GetSizeUnitOffsetInt()) {
            yield return GetOriginPointInt() + pos;
        }
    }


    public Vector2Int GetOriginPointInt()
    {
        return Vector2Util.RoundToInt(ActualOriginPosition);
    }
    public Vector2 GetOriginPoint() {
        return ActualOriginPosition;
    }
    public abstract void SetPosition(Vector2 originPoint);
    public void SetPositionInt(Vector2Int originPoint) {
        SetPosition(Vector2Util.RoundToInt(originPoint));
    }

    public abstract IEnumerable<Vector2Int> GetSizeUnitOffsetInt();



    public abstract Vector2 ActualOriginPosition {
        get ;
    }
}
public class SingleMapUnit : MapUnit
{
    public MapGridPosition Grid {
        get {
            return controller.GetComponent<MapGridPosition>();
        }
    }

    public override Vector2 ActualOriginPosition
    {
        get
        {
            return Grid.MapPositionInt;
        }
    }

    public override IEnumerable<Vector2Int> GetSizeUnitOffsetInt()
    {
        yield return Vector2Int.zero;
    }

    public override void SetPosition(Vector2 originPoint)
    {
        Grid.MapPosition = originPoint;
    }
}
public class MultipleMapUnit : MapUnit
{
    public IEnumerable<MapGridPosition> GridPositions {
        get {
            foreach(var grid in controller.GetComponentsInChildren<MapGridPosition>()) {
                yield return grid;
            }
        }
    }


    public override Vector2 ActualOriginPosition
    {
        get
        {
            return MinGrid().MapPosition;
        }
    }

    private float GridValue(MapGridPosition grid) {
        return grid.MapPosition.x + grid.MapPosition.y;
    }
    public MapGridPosition MinGrid() {
        MapGridPosition min = null;
        foreach(var grid in GridPositions) {
            if(min == null || GridValue(grid) <= GridValue(min)) {
                min = grid;
            }
        }
        return min;
    }

    public override IEnumerable<Vector2Int> GetSizeUnitOffsetInt()
    {
        foreach(var grid in GridPositions) {
            yield return grid.MapPositionInt - GetOriginPointInt();
        }
    }

    public override void SetPosition(Vector2 originPoint)
    {
        Vector2 delta = originPoint - GetOriginPoint();
        foreach(var grid in GridPositions) {
            grid.MapPosition = grid.MapPosition + delta;
        }
    }
}