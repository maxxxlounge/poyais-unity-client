using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameElement  : MonoBehaviour{
    public int id;
    public string name;
    public PoyaisType type;
    public PoyaisState state;
    public float[] position;
    public float[] heading;
    



    public static GameObject GenerateGameObject( GameObject template, GameObject parent, Vector3 pos,string name){
        GameObject s = Instantiate(template, pos,Quaternion.identity);    
        s.name=name;
        s.transform.parent = parent.transform;
        return s;
    } 
}