using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour {
	public float Energy = 100;
	public float Cost;
	public float addSpeed = 0.5f;
	public float addPushingSpeed = 0.5f;
	public float addCollectTime = 0.5f;
	public int collectNum;
	public int MaxCollectNum = 1;
	public RecipeController recipeController;

	public bool isPushingHouse = false;

	private void Update() {
		EnergyCost();
	}

	public bool EnergyCost() {
		if (isPushingHouse && Energy >= 0) {
			Energy -= Cost * Time.deltaTime;
			return true;
		}
		else {
			return false;
		}
	}
	public void ControlMaxEnergy() {
		if (Energy > 100) {
			Energy = 100;
		}
	}

	public void TechCheck(string techName) {
		if (techName.Equals("Shoes")) {
			GetComponent<PlayerController>().player.Speed += addSpeed;
		}
		if (techName.Equals("Wheels")) {
			GetComponent<PlayerController>().player.PushingSpeed += addPushingSpeed;
		}
		if (techName.Equals("Axe")) {
			GetComponent<PlayerController>().player.CollectTime -= addCollectTime;
		}
		if (techName.Equals("Iron Gloves")) {
			Cost *= 0.5f;
		}
		if (techName.Equals("Package")) {
			MaxCollectNum += 1;
		}
	}
}
