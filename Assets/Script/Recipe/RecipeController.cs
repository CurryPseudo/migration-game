using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeController : MonoBehaviour {
	[System.Serializable]
	public struct Recipe {
		public string name;
		public Vector3[] cost;
		public int Level;
	}
	public Recipe[] recipes;

	public WealthController wealthController;

	public void Manufacturing(string name) {
		int i = 0;
		bool canMake = false;
		foreach (var item in recipes) {
			if (item.name.Equals(name))
				break;
			i++;
		}
		for (int j = 0; j < 2; j++) {
			if (wealthController.wealth[j].count < recipes[i].cost[recipes[i].Level][j] || recipes[i].Level >= recipes[i].cost.Length) {
				return;
			}
			else {
				canMake = true;
			}
		}
		if (canMake) {
			for (int j = 0; j < 2; j++) {
				wealthController.wealth[j].count -= (int)recipes[i].cost[recipes[i].Level][j];
				recipes[i].Level++;
			}
		}
	}
}
