using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleInput : MigrationInput {

	private float lastInput_A = 0;
	private float lastInput_B = 0;

	public GameObject PlayerKeyboard;


	public override Vector2 GetInputAxis() {
		Vector2 moveDir = new Vector2(0, 0);
		if (Input.GetAxisRaw("HorizontalHandle") > 0.3) {
			moveDir = new Vector2(1, -1).normalized;
		}
		if (Input.GetAxisRaw("HorizontalHandle") < -0.3) {
			moveDir = new Vector2(-1, 1).normalized;
		}
		if (Input.GetAxisRaw("VerticalHandle") > 0.3f) {
			moveDir = new Vector2(1, 1).normalized;
		}
		if (Input.GetAxisRaw("VerticalHandle") < -0.3f) {
			moveDir = new Vector2(-1, -1).normalized;
		}
		if (Input.GetAxisRaw("HorizontalHandle") > 0 && Input.GetAxisRaw("VerticalHandle") > 0) {
			moveDir = new Vector2(1, 0).normalized;
		}
		if (Input.GetAxisRaw("HorizontalHandle") > 0 && Input.GetAxisRaw("VerticalHandle") < -0) {
			moveDir = new Vector2(0, -1).normalized;
		}
		if (Input.GetAxisRaw("HorizontalHandle") < -0 && Input.GetAxisRaw("VerticalHandle") > 0) {
			moveDir = new Vector2(0, 1).normalized;
		}
		if (Input.GetAxisRaw("HorizontalHandle") < -0 && Input.GetAxisRaw("VerticalHandle") < -0) {
			moveDir = new Vector2(-1, 0).normalized;
		}
		if (Input.GetAxisRaw("HorizontalHandle") == 0 && Input.GetAxisRaw("VerticalHandle") == 0) {
			moveDir = new Vector2(0, 0);
		}
		return moveDir;
	}

	public override bool GetInputInteraction() {
		bool GetButtonDown;
		if (Input.GetAxis( "Button_A" ) > 0.2 && lastInput_A == 0)
			GetButtonDown = true;
		else
			GetButtonDown = false;
		lastInput_A = Input.GetAxis("Button_A");
		return GetButtonDown;
	}
	public override bool OpenTechTree() {
		bool GetButtonDown;
		if (Input.GetAxis( "Button_B" ) > 0.2 && lastInput_B == 0 && PlayerKeyboard.GetComponent<PlayerController>().player.notInTechTree)
			GetButtonDown = true;
		else
			GetButtonDown = false;
		lastInput_B = Input.GetAxis("Button_B");
		return GetButtonDown;
	}
}
