using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testtext : MonoBehaviour {

	public GameObject p1;
	public GameObject p2;
	public GameObject p3;
	public GameObject p4;

	public Text txt;
	
	// Update is called once per frame
	void Update () {
		txt.text = "Player 1: " + getInfo (p1) +
		"Player 2: " + getInfo (p2) +
		"Player 3: " + getInfo (p3) +
		"Player 4: " + getInfo (p4);
	}

	string getInfo(GameObject p) {
		string result_string;
		result_string = p.GetComponent<Player> ().atk + " atk, "
			+ p.GetComponent<Player> ().def + " def, " +
			p.GetComponent<Player> ().pos + " pos \n";
		return result_string;
	}
}
