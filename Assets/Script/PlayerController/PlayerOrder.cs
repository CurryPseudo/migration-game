using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[ExecuteInEditMode]
public class PlayerOrder : MonoBehaviour {
    public new SpriteRenderer renderer;
    private void Update() {
        if(renderer != null) {
            var pos = GetComponent<PlayerController>().player.PositionInMap;
            renderer.sortingOrder = -2 * Mathf.FloorToInt(pos.x + pos.y) - 1;
        }
    }
}