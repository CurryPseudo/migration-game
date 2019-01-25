﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
	up = 1,
	down = -1,
	right = 10,
	left = -10,
}

public class Controller : MonoBehaviour {
	public Vector2 PositionInMap = new Vector2(0, 0);
	public float Speed;
	public Vector2 fiction = new Vector2(0.5f, 0.5f);


	private IMap iMap;
 	private Transform _transform;
	private float playerRadius = 0.25f;
	private	Vector2 direction = new Vector2(0, 0);




	private void Awake() {
		_transform = gameObject.GetComponent<Transform>();
	}

	private void Update() {
		Move();
	}





	/// <summary>
	/// 玩家控制的方向
	/// </summary>
	public Vector2 GetMoveDirection() {
		int dirValue = 0;
		Vector2 moveDir = new Vector2(0, 0);
		if (Input.GetKey(KeyCode.W)) {
			dirValue += (int)Direction.up;
		}
		if (Input.GetKey(KeyCode.A)) {
			dirValue += (int)Direction.left;
		}
		if (Input.GetKey(KeyCode.S)) {
			dirValue += (int)Direction.down;
		}
		if (Input.GetKey(KeyCode.D)) {
			dirValue += (int)Direction.right;
		}

		switch (dirValue) {
			case 1  : moveDir = new Vector2(1, 1).normalized; break;
			case -1 : moveDir = new Vector2(-1, -1).normalized; break;
			case 10 : moveDir = new Vector2(1, -1).normalized; break;
			case -10: moveDir = new Vector2(-1, 1).normalized; break;
			case 11 : moveDir = new Vector2(1, 0).normalized; break;
			case -9 : moveDir = new Vector2(0, 1).normalized; break;
			case 9  : moveDir = new Vector2(0, -1).normalized; break;
			case -11: moveDir = new Vector2(-1, 0).normalized; break;
			default: break;
		}
		direction = moveDir;
		return moveDir;
	}
	/// <summary>
	/// 碰撞检测
	/// </summary>
	public void CheckCollision() {
		Vector2 velocity = new Vector2(0, 0);
		Vector2 currentMU = new Vector2(Mathf.FloorToInt(PositionInMap.x), Mathf.FloorToInt(PositionInMap.y));
		Vector2 moveDir = GetMoveDirection();
		// 右上有障碍物
		if ((iMap.GetMapUnit((int)currentMU.x + 1, (int)currentMU.y) != null) && (Mathf.Abs(PositionInMap.x - currentMU.x) >= playerRadius)) {
			// 调整移动方向
			if (moveDir.x > 0.9f) {
				direction = new Vector2(1, -1).normalized * fiction;
			}
			if (moveDir.y > 0.9f) {
				direction = new Vector2(-1, 1).normalized * fiction;
			}
		}
		// 左上有障碍物
		if ((iMap.GetMapUnit((int)currentMU.x, (int)currentMU.y + 1) != null) && (Mathf.Abs(PositionInMap.x - currentMU.x) >= playerRadius)) {
			// 调整移动方向
			if (moveDir.x < -0.9f) {
				direction = new Vector2(-1, -1).normalized * fiction;
			}
			if (moveDir.y > 0.9f) {
				direction = new Vector2(1, 1).normalized * fiction;
			}
		}
		// 右下有障碍物
		if ((iMap.GetMapUnit((int)currentMU.x, (int)currentMU.y - 1) != null) && (Mathf.Abs(PositionInMap.x - currentMU.x) >= playerRadius)) {
			// 调整移动方向
			if (moveDir.x > 0.9f) {
				direction = new Vector2(1, 1).normalized * fiction;
			}
			if (moveDir.y < -0.9f) {
				direction = new Vector2(-1, -1).normalized * fiction;
			}
		}
		// 左下有障碍物
		if ((iMap.GetMapUnit((int)currentMU.x - 1, (int)currentMU.y) != null) && (Mathf.Abs(PositionInMap.x - currentMU.x) >= playerRadius)) {
			// 调整移动方向
			if (moveDir.x < -0.9f) {
				direction = new Vector2(-1, 1).normalized * fiction;
			}
			if (moveDir.y < -0.9f) {
				direction = new Vector2(1, -1).normalized * fiction;
			}
		}
	}

	public void Move() {
		PositionInMap.x += direction.x * Speed * Time.deltaTime;
		PositionInMap.y += direction.y * Speed * Time.deltaTime;
	}
}
