using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public int atk;
	public int def;
	public int pos;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Move () {
		int dist = Random.Range (1, 6);
		pos += dist;
	}

	public string Fight (Player opponent) {
		int myRoll = Random.Range (1, 6) + atk;
		int vsRoll = Random.Range (1, 6) + opponent.def;

		string resultstring = null;

		if (myRoll > vsRoll) {
			resultstring = myRoll + " vs " + vsRoll + ", " +
			this.gameObject.name + " wins!";
		} else if (vsRoll > myRoll) {
			resultstring = myRoll + " vs " + vsRoll + ", " +
			opponent.gameObject.name + " wins!";
		} else if (vsRoll == myRoll) {
			resultstring = "It's a tie!!";
		}

		return resultstring;
	}
}
