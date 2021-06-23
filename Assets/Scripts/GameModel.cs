using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameModel{
    public Player[] Players;
    public Player You;
    public Bullet[] Bullets; 
    private string status;
    public  GameObject ShipPrefab;
    public  GameObject BulletPrefab;

    public static GameModel CreateFromJSON(string jsonString)
    {
        GameModel gm = JsonUtility.FromJson<GameModel>(jsonString);
        return gm;
    }

    public string Status { 
        get { return status; }
    }


}