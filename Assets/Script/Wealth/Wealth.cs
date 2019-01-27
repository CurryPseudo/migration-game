using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wealth : Interactive {

  public new string name;
	public int count;
  public bool isCollected = false;

    public override void Interaction(PlayerController playerController)
    {
		  playerController.StartCoroutine(playerController.player.Collect(this));
    }
}
