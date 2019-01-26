using UnityEngine;

public abstract class Interactive : MonoBehaviour {

    public abstract void Interaction();
}

public class TestInteractive : Interactive {
    public override void Interaction() {
        Debug.Log("get");
    }
}