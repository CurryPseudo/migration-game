using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour {
	public float Energy = 100;
	public float Cost;
	public float addSpeed = 0.5f;
	public float addPushingSpeed = 0.5f;
	public float CollectSpeed;
	public int MaxCollectNum;
	public RecipeController recipeController;
	public Player player;

	public bool isPushingHouse = false;


	private void Start() {
		player = GetComponent<Player>();
	}


	public void EnergyCost() {
		if (isPushingHouse) {
			Energy -= Cost;
		}
	}
	public void TechCheck(string techName) {
		if (techName.Equals("Shoes")) {
			player.Speed += addSpeed;
		}
		if (techName.Equals("Wheels")) {
			player.PushingSpeed += addPushingSpeed;
		}
		if (techName.Equals("Axe")) {
			CollectSpeed = recipeController.GetTechLevel("Axe");
		}
		if (techName.Equals("Iron Gloves")) {
			Cost *= 0.75f;
		}
		if (techName.Equals("Package")) {
			MaxCollectNum += 1;
		}
	}
}
