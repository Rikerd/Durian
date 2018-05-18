using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour {

	public GameObject p1;
	public GameObject p2;
	public GameObject p3;
	public GameObject p4;

	public Text txt;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("space")) {
			txt.text = p1.GetComponent<Player> ().Fight (p2.GetComponent<Player>());
		}
	}
}
