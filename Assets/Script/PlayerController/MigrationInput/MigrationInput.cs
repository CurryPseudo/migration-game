using UnityEngine;
using UnityEngine.EventSystems;

public abstract class MigrationInput : MonoBehaviour {

	public abstract Vector2 GetInputAxis();
	public abstract bool GetInputInteraction();
	public abstract bool OpenTechTree();
	public abstract bool CloseTechTree();
	

}
