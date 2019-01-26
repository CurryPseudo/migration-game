using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipesMenuButton : MonoBehaviour {
	public RecipeController recipeController;
	public new string name;
	[Header("Color")]
	public Color ActiveColor;
	public Color NegativeColor;

	private void Update() {
		if (recipeController.CanMake(name)) {
			GetComponent<Image>().color = ActiveColor;
		}
		else {
			GetComponent<Image>().color = NegativeColor;
		}
	}
	
}
