using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class Bullet{
    public string ID;
    public float X;
    public float Y;
    public bool active;
    public string Owner;
    public float Rotation;
    public float Damage;
    public float Speed;
    public bool Exhausted;
}