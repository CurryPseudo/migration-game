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
	public PlayerUnit playerUnit_1;
	public PlayerUnit playerUnit_2;

	public bool CanMake(string name) {
		int i = 0;
		foreach (var item in recipes) {
			if (item.name.Equals(name))
				break;
			i++;
		}
		for (int j = 0; j <= 2; j++) {
			if (recipes[i].Level >= recipes[i].cost.Length || wealthController.wealth[j].count < recipes[i].cost[recipes[i].Level][j]) {
				return false;
			}
		}
		return true;
	}

	public void Manufacturing(string name) {
		int i = 0;
		foreach (var item in recipes) {
			if (item.name.Equals(name))
				break;
			i++;
		}
		bool canMake = CanMake(name);
		if (canMake) {
			for (int j = 0; j <= 2; j++) {
				wealthController.wealth[j].count -= (int)recipes[i].cost[recipes[i].Level][j];
			}
			recipes[i].Level++;
		}
		playerUnit_1.TechCheck(name);
		playerUnit_2.TechCheck(name);
	}

	public int GetTechLevel(string name) {
		int i = 0;
		foreach (var item in recipes) {
			if (item.name.Equals(name))
				break;
			i++;
		}
		return recipes[i].Level;
	}
}
