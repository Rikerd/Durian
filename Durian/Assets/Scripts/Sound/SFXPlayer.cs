using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour {

	public AudioClip fight;
	public AudioClip levelup;
	public AudioClip tap;
	public AudioClip monster;
	public AudioClip flag;

	private AudioSource src;

	// Use this for initialization
	void Start () {
		src = GetComponent<AudioSource> ();
	}

	public void FightSound() {
		src.clip = fight;
		src.Play ();
	}

	public void LevelUpSound() {
		src.clip = levelup;
		src.Play ();
	}

	public void TapSound() {
		src.clip = tap;
		src.Play ();
	}

	public void MonsterSound() {
		src.clip = monster;
		src.Play ();
	}

	public void FlagSound() {
		src.clip = flag;
		src.Play ();
	}
}
