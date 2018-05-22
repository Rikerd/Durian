using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {
    public Text statLine;

    public int maxHp;
    public int maxDef;
    public int maxMove;

    public GameObject flag;

    [HideInInspector]
    public int hp;

    [HideInInspector]
    public int atk;

    [HideInInspector]
    public int def;

    [HideInInspector]
    public int movement;

    [HideInInspector]
    public bool holdingFlag;

    private int deathTimer;

    // Use this for initialization
    void Start () {
        hp = 5;
        atk = 0;
        def = 0;
        movement = 0;
        holdingFlag = false;
        deathTimer = 0;
	}

    private void Update()
    {
        statLine.text = "HP: " + hp + " Atk: " + atk + " Def: " + def + " Move: " + movement;

        if (holdingFlag)
        {
            flag.SetActive(true);
        } else
        {
            flag.SetActive(false);
        }
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

    public void grabFlag()
    {
        holdingFlag = true;
    }

    public void releaseFlag()
    {
        holdingFlag = false;
    }

    public bool isDead()
    {
        return hp <= 0;
    }

    public void increaseDeathTimer()
    {
        deathTimer++;

        if (deathTimer >= 2)
        {
            hp = 5;
            deathTimer = 0;
        }
    }
}
