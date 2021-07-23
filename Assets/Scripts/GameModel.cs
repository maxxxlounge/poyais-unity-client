using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameModel{
    public GameElement[] game;

    public static GameElement[] CreateFromJSON(string jsonString)
    {
        Debug.Log(jsonString);
        GameElement[] ggee = JsonUtility.FromJson<GameElement[]>(jsonString);
        Debug.Log(ggee);
        return ggee;
        
    }

    public GameElement[] GetGameElements(){
        return game;
    }

}