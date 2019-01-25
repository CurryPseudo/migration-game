using UnityEngine;
public class Vector2Util {
    public static Vector2 RotateAxisDir(float angle, float length) {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * length;
    }
}