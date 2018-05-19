using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {
    public Text statLine;

    public int maxHp;

    [HideInInspector]
    public int hp;

    [HideInInspector]
    public int atk;

    [HideInInspector]
    public int def;

	// Use this for initialization
	void Start () {
        hp = 5;
        atk = 1;
        def = 1;
	}

    private void Update()
    {
        statLine.text = "HP: " + hp + " Atk: " + atk + " Def: " + def;
    }

    public void fullHeal()
    {
        hp = maxHp;
    }

    public void takeDamage(int dmg)
    {
        if (dmg > hp)
        {
            hp = 0;
        } else
        {
            hp -= dmg;
        }
    }

    public void increaseAtk()
    {
        atk++;
    }

    public void increaseDef()
    {
        def++;
    }
}
