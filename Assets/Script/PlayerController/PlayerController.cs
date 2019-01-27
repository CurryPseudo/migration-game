using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class Player {

	[NonSerialized]
	public PlayerController controller;
	public Vector2 PositionInMap = new Vector2(0, 0);
	public bool notInTechTree;
	public bool notInCollect;
	[Header("Unit")]
	public PlayerUnit u;
	public float Speed;
	public float PushingSpeed;
	public float CollectTime;
	public UIController UI;
	[Header("Move")]
	public IMap map;
	public float playerRadius = 0.25f;
	public float interactiveRadius = 0.4f;
	public WealthController wealthController;
	[Header("TechTreeButton")]
	public GameObject RecipeMenu;
    public GameObject myEventSystem;
    public GameObject firstSelectedGameObject;
	public GameObject Canvas;
	public GameObject SliderPrefab;
	[Header("Voice")]
	public AudioSource[] voice;
	public Vector2 VelocityDir = new Vector2(0, 0);
	public Vector2 Forward = new Vector2(0, 0);
	public Vector2 WorldForward;
	private Vector2 currentMapUnit;
	internal MigrationInput migrationInput;

	public void Update() {
		if(notInTechTree && notInCollect) {
			GetMoveDirection();
			CheckCollision();
			Move();
			InteracitonTrigger();
		}
		else {
			VelocityDir = Vector2.zero;
		}
		TechTreeTrigger();
		WorldForward = controller.mapController.map.MapToWorldDirection(Forward).normalized;
	}
	public void TechTreeTrigger() {
		if(migrationInput.OpenTechTree()) {
			if(notInTechTree) {
				IMapUnit unit;
				unit = UnitExist();
				if (unit != null && unit.GetController() != null) {
					if(unit.GetController().GetComponent<House>() != null) {
						OpenTechTree();
					}
				}
			}
			else {
				CloseTechTree();
			}
		}
	}


	/// <summary>
	/// 玩家控制的方向
	/// </summary>
	private Vector2 GetMoveDirection() {
		Vector2 moveDir = migrationInput.GetInputAxis();
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
		if (VelocityDir != Vector2.zero && !voice[2].isPlaying) {
			voice[2].Play();
		}
		else if (VelocityDir == Vector2.zero) {
			voice[2].Stop();
		}
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
			if(interactive != null) {
				if (migrationInput.GetInputInteraction()) {
					interactive.Interaction(controller);
				}
			}
		}
	}

	public void OpenTechTree() {
		UI.TxtUpdate();
		u.collectNum = 0;
		voice[0].Play();
		notInTechTree = false;
		myEventSystem.SetActive(true);
		RecipeMenu.SetActive(true);
        myEventSystem.GetComponent<EventSystem> ().SetSelectedGameObject(firstSelectedGameObject);
	}
	public void CloseTechTree() {
		UI.TxtUpdate();
		voice[1].Play();
		RecipeMenu.SetActive(false);
		myEventSystem.SetActive(false);
		notInTechTree = true;
	}

	public IEnumerator Collect(Wealth wealth) {
		if (u.collectNum >= u.MaxCollectNum) {
			Debug.Log("采集次数达到上限");
			yield break;
		}
		Color color = wealth.GetComponentInChildren<SpriteRenderer>().color;
		GameObject newSlider = GameObject.Instantiate(SliderPrefab, Canvas.transform);
		AudioSource collectVoice;
		if (wealth.name.Equals("Wood")) {
			collectVoice = voice[3];
		}
		else if (wealth.name.Equals("Stone") || wealth.name.Equals("Iron")) {
			collectVoice = voice[4];
		}
		else {
			collectVoice = null;
		}
		if (collectVoice != null)
			collectVoice.Play();
		newSlider.transform.position = Camera.main.WorldToScreenPoint(wealth.transform.position + new Vector3(0, 1.5f, 0));
		notInCollect = false;
		float timeCount = 0;
        while(true) {
			timeCount += Time.deltaTime;
			color.a = 1 - timeCount / CollectTime;
			wealth.GetComponentInChildren<SpriteRenderer>().color = color;
			newSlider.GetComponent<Slider>().value = timeCount / CollectTime;
			if (color.a <= 0) {
				break;
			}
            yield return null;
        }
		u.collectNum++;
		if (wealth.name.Equals("Berry")) {
			u.Energy += 25;
			u.ControlMaxEnergy();
			u.collectNum--;
		}
		else
			wealthController.AddWealth(wealth.name, wealth.count);
		GameObject.Destroy(wealth.gameObject);
		GameObject.Destroy(newSlider);
		notInCollect = true;
		if (collectVoice != null)
			collectVoice.Stop();
	}
}


[ExecuteInEditMode]
public class PlayerController : MonoBehaviour {
	public string currentStateName;
	public IEnumerator currentState;
	public MapController mapController;
	public Player player;	 
	public AnimationClipPlayer ClipPlayer {
		get {
			return GetComponentInChildren<AnimationClipPlayer>();
		}
	}
    public Vector2 Position {
        get {
            return transform.position;
        }
        set {
            transform.position = new Vector3(value.x, value.y, transform.position.z);
        }
    }

	private MigrationInput migrationInput;
	public void Start() {
		player.map = mapController.map;
		this.migrationInput = GetComponent<MigrationInput>();
		player.migrationInput = this.migrationInput;
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
	public IEnumerator Main() {
		player.controller = this;
		currentState = Idle();
		yield return StartCoroutine(currentState);
	}
	public IEnumerator Idle() {
		currentStateName = "Idle";
		while(true) {
			yield return null;
			player.PositionInMap = mapController.map.WorldToMapPoint(Position);
			if(!Application.isEditor || Application.isPlaying) {
				player.Update();
			}
			Vector2 direction = mapController.map.MapToWorldDirection(player.Forward);
			if(ClipPlayer != null && Time.timeScale > 0) {
				string clipName = "";
				if(direction.normalized.y > 0.8f) {
					clipName = "Top";
				}
				else if(direction.normalized.y < -0.8f) {
					clipName = "Down";
				}
				else if(direction.normalized.x > 0) {
					clipName = "Right";
				}
				else {
					clipName = "Left";
				}

				if(player.VelocityDir.magnitude > 0.1f) {
					clipName += "Walk";
				}
				else {
					clipName += "Stand";
				}
				ClipPlayer.PlayClip(clipName);
			}
			Position = mapController.map.MapToWorldPoint(player.PositionInMap);
		}
	}
	public bool CouldMoveHouse() {
		return true;
	}
	public void UseEnerge() {

	}
	public void ChangeState(IEnumerator next) {
		StopCoroutine(currentState);
		currentState = next;
		StartCoroutine(next);
	}
	public IEnumerator HandledHouse(House house, Vector2 local) {
		currentStateName = "HandledHouse";
		while(true) {
			yield return null;
			player.PositionInMap = house.LocalToMap(local);
			Position = mapController.map.MapToWorldPoint(player.PositionInMap);
			UnHandledInputProcess(house);
		}
	}
	private void UnHandledInputProcess(House house) {
		if(player.migrationInput.GetInputInteraction()) {
			house.UnHandled(this);
		}
	}
}