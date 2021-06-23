using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    private ServerCommunication communication;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
       communication.ConnectToServer();
    }


}
