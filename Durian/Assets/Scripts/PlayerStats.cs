using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {
    public Text statLine;

    public int maxHp;
    public int maxDef;
    public int maxMove;

    [HideInInspector]
    public int hp;

    [HideInInspector]
    public int atk;

    [HideInInspector]
    public int def;

    [HideInInspector]
    public int movement;

    // Use this for initialization
    void Start () {
        hp = 5;
        atk = 0;
        def = 0;
        movement = 0;
	}

    private void Update()
    {
        statLine.text = "HP: " + hp + " Atk: " + atk + " Def: " + def + " Move: " + movement;
    }

    public void fullHeal()
    {
        hp = maxHp;
    }

    public void halfHeal()
    {
        if (hp + 2 > maxHp)
        {
            hp = maxHp;
        }
        else
        {
            hp += 2;
        }
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

    public void increaseMove()
    {
        movement++;
    }
}
