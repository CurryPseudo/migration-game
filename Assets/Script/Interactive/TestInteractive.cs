using UnityEngine;
public class TestInteractive : Interactive {
    public GameObject RecipeMenu;
    public override void Interaction(PlayerController controller) {
        Debug.Log("get");
        RecipeMenu.SetActive(true);
    }
}