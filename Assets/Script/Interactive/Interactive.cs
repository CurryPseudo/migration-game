using UnityEngine;
using Sirenix.OdinInspector;

public abstract class Interactive : SerializedMonoBehaviour {

    public abstract void Interaction(PlayerController player);
}

