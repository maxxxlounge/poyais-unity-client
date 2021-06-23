using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameElement {
    public int id;
    public string name;
    public PoyaisType type;
    public PoyaisState state;
    public float[] position;
    public float[] heading;
    

    public Vector2 GetPosition(){
        return new Vector2(this.position[0],this.position[1]);
    }

    public GameObject GenerateGameGameObject(GameObject Template, GameObject parent){
        GameObject s = Instantiate(Ship,position,Quaternion.identity);   
        s.name=this.name;
        s.transform.parent = parent;
        return s;
    } 
}