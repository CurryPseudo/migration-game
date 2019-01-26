using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;
using Sirenix.Serialization;

public class House : Interactive {
    public IEnumerator currentState;
    public string stateName;
    public float movingSpeed;
    public MapUnitController unitController;
    public MapController mapController;
    public List<MapGridPosition> gridPositions = new List<MapGridPosition>();
    private List<PlayerController> handledPlayers = new List<PlayerController>();
    public IEnumerator Main() {
        currentState = Idle();
        StartCoroutine(currentState);
        yield break;
    }
    private Dictionary<Vector2Int, Vector2Int> playerPosSideMap = new Dictionary<Vector2Int, Vector2Int>() {
        {new Vector2Int(-1, 0), new Vector2Int(-1, 0)},
        {new Vector2Int(-1, 1), new Vector2Int(-1, 0)},
        {new Vector2Int(0, 2), new Vector2Int(0, 1)},
        {new Vector2Int(1, 2), new Vector2Int(0, 1)},
        {new Vector2Int(2, 0), new Vector2Int(1, 0)},
        {new Vector2Int(2, 1), new Vector2Int(1, 0)},
        {new Vector2Int(0, -1), new Vector2Int(0, -1)},
        {new Vector2Int(1, -1), new Vector2Int(0, -1)}
    };
    public bool HasPlayerSide(PlayerController playerController) {
        return playerPosSideMap.ContainsKey(PlayerToLocalInt(playerController));
    }
    public Vector2Int PlayerSide(PlayerController playerController) {
        return playerPosSideMap[PlayerToLocalInt(playerController)];
    }

    public override void Interaction(PlayerController playerController)
    {
        Debug.Assert(!handledPlayers.Contains(playerController));
        var playerLocal = PlayerToLocalInt(playerController);
        if(playerController.CouldMoveHouse() && PlaceEmpty(playerLocal) && HasPlayerSide(playerController)) {
            handledPlayers.Add(playerController);
            playerController.ChangeState(playerController.HandledHouse(this, playerLocal));
        }
        if(handledPlayers.Count == 2 && PlayerSameSide()) {
            Vector2Int side = PlayerSide(handledPlayers[0]);
            Vector2Int direction = new Vector2Int(-side.x, -side.y);
            if(MapEmpty(direction)) {
                ChangeState(Moving(direction));
            }
        }
    }
    private Dictionary<Vector2Int, List<Vector2Int>> directionCollisionCheckMap = 
         new Dictionary<Vector2Int, List<Vector2Int>>() {
             {new Vector2Int(1, 0), new List<Vector2Int>{new Vector2Int(2, 0), new Vector2Int(2, 1)}},
             {new Vector2Int(-1, 0), new List<Vector2Int>{new Vector2Int(-1, 0), new Vector2Int(-1, 1)}},
             {new Vector2Int(0, 1), new List<Vector2Int>{new Vector2Int(0, 2), new Vector2Int(1, 2)}},
             {new Vector2Int(0, -1), new List<Vector2Int>{new Vector2Int(0, -1), new Vector2Int(1, -1)}}
         };
    public bool MapEmpty(Vector2Int direction) {
        foreach(var collisionArea in directionCollisionCheckMap[direction]) {
            if(mapController.map.GetMapUnit(Vector2Util.RoundToInt(LocalToMap(collisionArea))) != null) {
                return false;
            }
        }
        return true;
    }
    public Vector2Int PlayerToLocalInt(PlayerController playerController) {
        Vector2 pos = playerController.player.PositionInMap;
        Vector2Int posRounded = MapToLocalInt(pos);
        return posRounded;
    }
    public Vector2 LocalToMap(Vector2 local) {
        return local + unitController.mapUnit.GetOriginPoint();
    }
    public Vector2Int MapToLocalInt(Vector2 pos) {
        return Vector2Util.RoundToInt(pos) - unitController.mapUnit.GetOriginPointInt();
    }
    public void ChangeState(IEnumerator state) {
        StopCoroutine(currentState);
        currentState = state;
        StartCoroutine(state);
    }
    public bool PlaceEmpty(Vector2Int pos) {
        foreach(var player in handledPlayers) {
            if(PlayerToLocalInt(player) == pos) {
                return false;
            }
        }
        return true;
    }
    public bool PlayerSameSide() {
        if(handledPlayers.Count == 0) return true;
        Vector2Int side = PlayerSide(handledPlayers[0]);
        for(int i = 1; i < handledPlayers.Count; i++) {
            if(PlayerSide(handledPlayers[i]) != side) return false;
        }
        return true;

    }

    public void UnHandled(PlayerController playerController) {
        if(unHandledAction != null) {
            unHandledAction(playerController);
        }
    }
    private Action<PlayerController> unHandledAction = null;
    public IEnumerator Idle() {
        stateName = "Idle";
        unHandledAction = (playerController) => {
            playerController.ChangeState(playerController.Idle());
            handledPlayers.Remove(playerController);
        };
        while(true) {
            yield return null;
        }
    }
    public IEnumerator Moving(Vector2Int direction) {
        stateName = "Moving";
        List<PlayerController> relasePlayers = new List<PlayerController>();
        unHandledAction = (playerController) => {
            if(!relasePlayers.Contains(playerController)) {
                relasePlayers.Add(playerController);
            }

        };
        while(true) {
            foreach(var player in handledPlayers) {
                player.UseEnerge();
            }
            foreach(var grid in gridPositions) {
                grid.rounded = false;
            }
            var start = unitController.mapUnit.GetOriginPointInt();
            var end = start + direction;
            Vector2 housePos = start;
            float moveTime = 1 / movingSpeed;
            {
                float timeCount = 0;
                while(timeCount < moveTime) {
                    housePos = Vector2.Lerp(start, end, timeCount / moveTime);
                    unitController.mapUnit.SetPosition(housePos);
                    yield return null;
                    timeCount += Time.deltaTime;
                }
            }
            unitController.mapUnit.SetPositionInt(end);
            foreach(var grid in gridPositions) {
                grid.rounded = true;
            }
            if(relasePlayers.Count > 0) {
                foreach(var player in relasePlayers) {
                    player.ChangeState(player.Idle());
                    handledPlayers.Remove(player);
                }
                relasePlayers.Clear();
                break;
            }
            else if(!MapEmpty(direction)) {
                break;
            }
            foreach(var player in handledPlayers) {
                if(!player.CouldMoveHouse()) {
                    player.ChangeState(player.Idle());
                    handledPlayers.Remove(player);
                    break;
                }
            }
        }
        unHandledAction = null;
        ChangeState(Idle());
    }
}