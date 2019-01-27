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
	public Sprite[] sprites;

	private void Update() {
		if (recipeController.CanMake(name)) {
			GetComponent<Image>().color = ActiveColor;
		}
		else {
			GetComponent<Image>().color = NegativeColor;
		}
	}

	public void ChangeIcon() {
		int LV = recipeController.GetTechLevel(name);
		GetComponent<Image>().sprite = sprites[LV];
	}
	
}
