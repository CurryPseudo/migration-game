using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player {

	public Vector2 PositionInMap = new Vector2(0, 0);
	public float Speed;
	// public Vector2 fiction = new Vector2(0.5f, 0.5f);
	public IMap map;
	public float playerRadius = 0.25f;
	public float interactiveRadius = 0.4f;

	private	Vector2 VelocityDir = new Vector2(0, 0);
	private Vector2 Forward = new Vector2(0, 0);
	private Vector2 currentMapUnit;


	public void Update() {
		GetMoveDirection();
		CheckCollision();
		Move();
		InteracitonTrigger();
	}


	/// <summary>
	/// 玩家控制的方向
	/// </summary>
	private Vector2 GetMoveDirection() {
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
		VelocityDir = moveDir;
		return moveDir;
	}
	/// <summary>
	/// 碰撞检测
	/// </summary>
	private void CheckCollision() {
		Vector2 moveDir = GetMoveDirection();
		Vector2 collisionDir = new Vector2(0, 0);
		int i = 0;
		Vector2[] vertex = new Vector2[] {
			PositionInMap + new Vector2(playerRadius, playerRadius),
			PositionInMap + new Vector2(playerRadius, -playerRadius),
			PositionInMap + new Vector2(-playerRadius, -playerRadius),
			PositionInMap + new Vector2(-playerRadius, playerRadius),
		};
		List<Vector2> vertexInMapUnit = new List<Vector2>();
		foreach (var item in vertex) {
			if (map.GetMapUnit(Mathf.FloorToInt(item.x + 0.5f), Mathf.FloorToInt(item.y + 0.5f)) != null) {
				// Debug.Log(Mathf.FloorToInt(item.x + 0.5f) + "," + Mathf.FloorToInt(item.y + 0.5f));
				i++;
				vertexInMapUnit.Add(item);
			}
		}
		if (i == 3) {
			Vector2 t_Vector = new Vector2(0, 0);
			foreach (Vector2 item in vertexInMapUnit) {
				t_Vector += item;
			}
			collisionDir = -(t_Vector - 3 * PositionInMap).normalized;
		}
		else if (i == 2) {
			Vector2 t_Vector = new Vector2(0, 0);
			foreach (Vector2 item in vertexInMapUnit) {
				t_Vector += item;
			}
			if (Mathf.Abs(Vector2.Angle(moveDir, collisionDir)) < 10) {
				collisionDir = -(t_Vector - 2 * PositionInMap).normalized;
			}
			else {
				collisionDir = -(t_Vector - 2 * PositionInMap).normalized * Mathf.Cos((float)Math.PI / 4);
			}
			// collisionDir = -(t_Vector - 2 * PositionInMap).normalized * Mathf.Cos(Vector2.Angle(moveDir, collisionDir));
		}
		else if (i == 1) {
			Vector2 x_Aix = new Vector2(1, 0);
			Vector2 y_Aix = new Vector2(0, 1);
			Vector2 checkedUnit = new Vector2(Mathf.FloorToInt(vertexInMapUnit[0].x + 0.5f), Mathf.FloorToInt(vertexInMapUnit[0].y + 0.5f));
			float angle = Vector2.SignedAngle(x_Aix, vertexInMapUnit[0] - checkedUnit);
			// Debug.Log(i + " , " + angle);
			if (angle <= 45 && angle >-45) collisionDir = x_Aix * Mathf.Sin((float)Math.PI / 4);
			else if (angle <= 135 && angle >45) collisionDir = y_Aix * Mathf.Sin((float)Math.PI / 4);
			else if (angle <= -45 && angle >-135) collisionDir = -y_Aix * Mathf.Sin((float)Math.PI / 4);
			else collisionDir = -x_Aix * Mathf.Sin((float)Math.PI / 4);
			if (moveDir.x == 0 || moveDir.y == 0) {
				collisionDir = collisionDir * Mathf.Pow(2, 0.5f);
			}
			// Debug.Log(collisionDir);
		}
		VelocityDir = moveDir + collisionDir;
	}
	public void Move() {
		PositionInMap.x += VelocityDir.x * Speed * Time.deltaTime;
		PositionInMap.y += VelocityDir.y * Speed * Time.deltaTime;
	}

	public IMapUnit UnitExist() {
		if (GetMoveDirection() != Vector2.zero) {
			Forward = GetMoveDirection().normalized;
		}
		Vector2 checkedPoint = PositionInMap + Forward * interactiveRadius;
		Vector2 checkedUnit = new Vector2(Mathf.FloorToInt(checkedPoint.x + 0.5f), Mathf.FloorToInt(checkedPoint.y + 0.5f));
		if (map.GetMapUnit((int)checkedUnit.x, (int)checkedUnit.y) != null) {
			return map.GetMapUnit((int)checkedUnit.x, (int)checkedUnit.y);
		}
		return null;
	}
	public void InteracitonTrigger() {
		IMapUnit unit;
		Interactive interactive;
		unit = UnitExist();
		if (unit != null && unit.GetController() != null) {
			interactive = unit.GetController().GetComponent<Interactive>();
			if (Input.GetKeyDown(KeyCode.Space)) {
				interactive.Interaction();
			}
		}
	}
}


public class PlayerController : MonoBehaviour {
	public MapController mapController;
	public Player player;	 
    public Vector2 Position {
        get {
            return transform.position;
        }
        set {
            transform.position = new Vector3(value.x, value.y, transform.position.z);
        }
    }
	public void Start() {
		player.map = mapController.map;
	}
	private void OnDrawGizmosSelected() {
		Vector2[] borders = new Vector2[]{
			new Vector2(-1f, -1f),
			new Vector2(1f, -1f),
			new Vector2(1f, 1f),
			new Vector2(-1f, 1f),
		};
		int[] starts = new int[] {
			0, 1, 2, 3
		};
		int[] ends = new int[] {
			1, 2, 3, 0
		};
		for(int i = 0; i < 4; i++) {
			var start = borders[starts[i]] * player.playerRadius;
			start = mapController.map.MapToWorldPoint(start + player.PositionInMap);
			var end = borders[ends[i]] * player.playerRadius;
			end = mapController.map.MapToWorldPoint(end + player.PositionInMap);
			Gizmos.DrawLine(start, end);
		}
	}
	public void Update() {
		player.PositionInMap = mapController.map.WorldToMapPoint(Position);
		player.Update();
		Position = mapController.map.MapToWorldPoint(player.PositionInMap);
	}
}