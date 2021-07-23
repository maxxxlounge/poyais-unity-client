using Firesplash.UnityAssets.SocketIO;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class PoyaisConnector : MonoBehaviour
{

    public SocketIOCommunicator sioCom;
     private string log;
    private bool logVisible = true;
    private string gameStatus="connecting";
    private GameModel game;
    public GameObject TavernTemplate;
    public GameObject DwarfTemplate;
    public GameObject GameObjectContainer ;
    public string status;
    public bool namesent=false;
    float elapsed = 0f;
    const float screenWidth = 1020;
    const float screenHeight = 763;
    private bool clientconnected = false;

    [System.Serializable]
    struct Element
    {
        public int id;
        public string name;
        public float[] position;
        public float[] heading;
    }

    [System.Serializable]
    struct Game
    {
        public Element[] game;    
    }

    void Start()
    {
        sioCom.Instance.On("connect", (string data) => {
            Debug.Log("LOCAL: Hey, we are connected!");
          
        });

        sioCom.Instance.On("state", (string payload) =>
        {                        
            string payload2 = payload.Replace(@"\", ""); 
            //Debug.Log(payload2);
            //Game srv = JsonUtility.FromJson<Game>(payload2);
            //Debug.Log(srv.game[0].name);
            HandleMessage(payload2);
        });

        //When the conversation is done, the server will close our connection after we said Goodbye
        sioCom.Instance.On("disconnect", (string payload) => {
            if (payload.Equals("io server disconnect"))
            {
                Debug.Log("Disconnected from server.");   
            } 
            else
            {
                Debug.LogWarning("We have been unexpecteldy disconnected. This will cause an automatic reconnect. Reason: " + payload);
            }
        });


        //We are now ready to actually connect
        sioCom.Instance.Connect();
    }

  private void HandleMessage(string msg)
    {
        Game srv = JsonUtility.FromJson<Game>(msg);
        RenderPlayer(srv.game);
    }

    private void RenderPlayer(Element[] ggee){
                        
        foreach (var ge in ggee)
        {
            Debug.Log("render player " + ge.id);
            GameObject goge = GameObject.Find(ge.id.ToString());
            Vector3 pos = new Vector3(ge.position[0],ge.position[1],0); 
            if (goge != null){
                goge.transform.position = pos;
                continue;
            }

            
            goge = GameElement.GenerateGameObject(DwarfTemplate, GameObjectContainer,pos,ge.id.ToString());
                

            //Quaternion angle = Quaternion.Euler(0,0, p.Rotation * Mathf.Rad2Deg);
            //angle *= Quaternion.Euler(0, 0, 90);
            
        }
    }
 
}
