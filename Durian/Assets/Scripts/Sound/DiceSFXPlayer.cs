using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSFXPlayer : MonoBehaviour {

	public AudioClip dice1;
	public AudioClip dice2;
	public AudioClip dice3;

	private AudioSource src;

	// Use this for initialization
	void Start () {
		src = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DiceRollSound() {
		int whichone = Random.Range (1, 3);

		switch (whichone) {
		case 1:
			src.clip = dice1;
			src.Play ();
			break;
		case 2:
			src.clip = dice2;
			src.Play ();
			break;
		case 3:
			src.clip = dice3;
			src.Play ();
			break;
		}
	}

}
