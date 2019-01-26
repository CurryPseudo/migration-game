using UnityEngine;

public abstract class MigrationInput : MonoBehaviour {

	public abstract Vector2 GetInputAxis();
	public abstract bool GetInputInteraction();

}
