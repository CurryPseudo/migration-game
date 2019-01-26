using System.Collections.Generic;
using UnityEngine;
public interface IMapUnit {
    Vector2Int GetOriginPoint();
    IEnumerable<Vector2Int> GetSizeUnitOffset();
    IEnumerable<Vector2Int> GetPositions();
    void SetPosition(Vector2Int originPoint);
    MapUnitController GetController();
}
public class OutsideMapUnit : IMapUnit
{
    public MapUnitController GetController()
    {
        return null;
    }

    public IEnumerable<Vector2Int> GetLastPositions()
    {
        throw new System.NotImplementedException();
    }

    public Vector2Int GetOriginPoint()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<Vector2Int> GetPositions()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<Vector2Int> GetSizeUnitOffset()
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

    public void SetPosition(Vector2Int originPoint)
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

    public virtual IEnumerable<Vector2Int> GetPositions() {
        foreach(var pos in GetSizeUnitOffset()) {
            yield return GetOriginPoint() + pos;
        }
    }


    public Vector2Int GetOriginPoint()
    {
        return ActualOriginPosition;
    }
    public abstract void SetPosition(Vector2Int originPoint);

    public abstract IEnumerable<Vector2Int> GetSizeUnitOffset();



    public abstract Vector2Int ActualOriginPosition {
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

    public override Vector2Int ActualOriginPosition
    {
        get
        {
            return Grid.MapPositionInt;
        }
    }

    public override IEnumerable<Vector2Int> GetSizeUnitOffset()
    {
        yield return Vector2Int.zero;
    }

    public override void SetPosition(Vector2Int originPoint)
    {
        Grid.MapPositionInt = originPoint;
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


    public override Vector2Int ActualOriginPosition
    {
        get
        {
            return MinGrid().MapPositionInt;
        }
    }

    private int GridValue(MapGridPosition grid) {
        return grid.MapPositionInt.x + grid.MapPositionInt.y;
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

    public override IEnumerable<Vector2Int> GetSizeUnitOffset()
    {
        foreach(var grid in GridPositions) {
            yield return grid.MapPositionInt - GetOriginPoint();
        }
    }

    public override void SetPosition(Vector2Int originPoint)
    {
        Vector2Int delta = originPoint - GetOriginPoint();
        foreach(var grid in GridPositions) {
            grid.MapPositionInt = grid.MapPositionInt + delta;
        }
    }
}