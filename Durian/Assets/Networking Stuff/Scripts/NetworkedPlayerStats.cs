using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkedPlayerStats : NetworkBehaviour
{
    //public Text statLine;

    public int maxHp;
    public int maxDef;
    public int maxMove;

    //public GameObject flag;

    //[HideInInspector]
    [SyncVar]
    public int hp;

    //[HideInInspector]
    [SyncVar]
    public int atk;

    //[HideInInspector]
    [SyncVar]
    public int def;

    //[HideInInspector]
    [SyncVar]
    public int movement;

    //[HideInInspector]
    [SyncVar]
    public bool holdingFlag;

    // Use this for initialization
    void Start()
    {
        hp = 5;
        atk = 0;
        def = 0;
        movement = 0;
        holdingFlag = false;
    }

    private void Update()
    {
        //statLine.text = "HP: " + hp + " Atk: " + atk + " Def: " + def + " Move: " + movement;

        if (holdingFlag)
        {
            //flag.SetActive(true);
        }
        else
        {
            //flag.SetActive(false);
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
        }
        else
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

    [Command]
    public void CmdIncreaseAtk()
    {
        print("Increasing Atk in Stats...");
        atk += 1;
    }

    [Command]
    public void CmdIncreaseDef()
    {
        print("Increasing Def in Stats...");
        def+=1;
        print("Increased Def in Stats!");

        GetComponentInParent<NetworkedPlayerController>().responded = true;
        GetComponentInParent<NetworkedPlayerController>().tileEffectPassed = false;
    }

    [Command]
    public void CmdIncreaseMove()
    {
        print("Increasing Movement in Stats...");
        movement++;
    }
}
