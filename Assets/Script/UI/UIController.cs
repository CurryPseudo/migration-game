using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public Slider energySlider_1;
	public Slider energySlider_2;

	public Text text_wood;
	public Text text_Stone;
	public Text text_Iron;

	public PlayerUnit player_1;
	public PlayerUnit player_2;
	public WealthController wealthController;

	private void Update() {
		SliderUpdate();
	}
	
	public void SliderUpdate() {
		SliderController();
	}
	public void SliderController() {
		energySlider_1.value = player_1.Energy;
		energySlider_2.value = player_2.Energy;
	}

	public void TxtUpdate() {
		text_wood.text = wealthController.wealth[0].count.ToString();
		text_Stone.text = wealthController.wealth[1].count.ToString();
		text_Iron.text = wealthController.wealth[2].count.ToString();
	}
}
