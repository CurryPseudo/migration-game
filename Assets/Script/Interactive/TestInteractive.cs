using UnityEngine;
using UnityEngine.EventSystems;

public class TestInteractive : Interactive {

    public WealthController wealthController;

    public override void Interaction(PlayerController controller) {
        Debug.Log("get");
    }
}
