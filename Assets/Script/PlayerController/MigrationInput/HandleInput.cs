using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleInput : MigrationInput {

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
		if (Input.GetAxis( "Button_A" ) > 0.2)
			return true;
		else
			return false;
	}
}
