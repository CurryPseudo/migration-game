using System.Collections.Generic;
using UnityEngine;
public interface IMapUnit {
    Vector2Int GetOriginPoint();
    IEnumerable<Vector2Int> GetSizeUnitOffset();
    IEnumerable<Vector2Int> GetPositions();
    void SetPosition(Vector2Int originPoint);
}
public abstract class MapUnit : IMapUnit
{
    public abstract Vector2Int GetOriginPoint();
    public IEnumerable<Vector2Int> GetPositions() {
        foreach(var pos in GetSizeUnitOffset()) {
            yield return GetOriginPoint() + pos;
        }
    }
    public abstract IEnumerable<Vector2Int> GetSizeUnitOffset();
    public abstract void SetPosition(Vector2Int originPoint);
}
public class SingleMapUnit : MapUnit
{
    public Vector2Int originPoint;
    public SingleMapUnit(Vector2Int originPoint) {
        this.originPoint = originPoint;
    }
    public override Vector2Int GetOriginPoint()
    {
        return originPoint;
    }

    public override IEnumerable<Vector2Int> GetSizeUnitOffset()
    {
        yield return Vector2Int.zero;
    }

    public override void SetPosition(Vector2Int originPoint)
    {
        this.originPoint = originPoint;
    }
}