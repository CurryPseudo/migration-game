using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MigrationInput {

	public override Vector2 GetInputAxis() {
		Vector2 moveDir = new Vector2(0, 0);
		if (Input.GetAxisRaw("Horizontal") > 0.9f) {
			moveDir = new Vector2(1, -1).normalized;
		}
		if (Input.GetAxisRaw("Horizontal") < -0.9f) {
			moveDir = new Vector2(-1, 1).normalized;
		}
		if (Input.GetAxisRaw("Vertical") > 0.9f) {
			moveDir = new Vector2(1, 1).normalized;
		}
		if (Input.GetAxisRaw("Vertical") < -0.9f) {
			moveDir = new Vector2(-1, -1).normalized;
		}
		if (Input.GetAxisRaw("Horizontal") > 0.5f && Input.GetAxisRaw("Vertical") > 0.5f) {
			moveDir = new Vector2(1, 0).normalized;
		}
		if (Input.GetAxisRaw("Horizontal") > 0.5f && Input.GetAxisRaw("Vertical") < -0.5f) {
			moveDir = new Vector2(0, -1).normalized;
		}
		if (Input.GetAxisRaw("Horizontal") < -0.5f && Input.GetAxisRaw("Vertical") > 0.5f) {
			moveDir = new Vector2(0, 1).normalized;
		}
		if (Input.GetAxisRaw("Horizontal") < -0.5f && Input.GetAxisRaw("Vertical") < -0.5f) {
			moveDir = new Vector2(-1, 0).normalized;
		}
		if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) {
			moveDir = new Vector2(0, 0);
		}
		return moveDir;
	}

	public override bool GetInputInteraction() {
		if (Input.GetKeyDown(KeyCode.J))
			return true;
		else
			return false;
	}
	public override bool OpenTechTree() {
		if (Input.GetKeyDown(KeyCode.K))
			return true;
		else
			return false;
	}
	public override bool CloseTechTree() {
		if (Input.GetKeyDown(KeyCode.L))
			return true;
		else
			return false;
	}
}
