using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WealthController : MonoBehaviour {
	[System.Serializable]
	public struct Wealth {
		public string name;
		public int count;
	}
	public Wealth[] wealth;

	public void AddWealth(string name, int count) {
		int i = 0;
		foreach (var item in wealth) {
			if (item.name.Equals(name)) {
				break;
			}
			i++;
		}
		wealth[i].count += count;
	}

	public void SpendWealth(string name, int count) {
		int i = 0;
		foreach (var item in wealth) {
			if (item.name.Equals(name)) {
				break;
			}
			i++;
		}
		wealth[i].count += count;
	}

}
