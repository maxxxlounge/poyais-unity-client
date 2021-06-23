using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// Forefront class for the server communication.
/// </summary>
public class ServerCommunication : MonoBehaviour
{
    private string log;
    private bool logVisible = true;
    private string gameStatus="connecting";
    private Touch theTouch;
    private GameModel game;
    public GameObject Ship;
    public GameObject Arrow;
    public GameObject BulletPrefab ;
    public GameObject GameObjectContainer ;
    public string status;
    public string PlayerName;
    public bool namesent=false;
    float elapsed = 0f;
    const float screenWidth = 1020;
    const float screenHeight = 763;
    private bool clientconnected = false;
    private string skinName;
    private Sprite skin;
    public Sprite[] skins =  new Sprite[4];


    private bool isPaused = false;
    // Server IP address
    [SerializeField]
    private string hostIP;

    // Server port
    [SerializeField]
    private int port = 8888;

    // Flag to use localhost
    [SerializeField]
    private bool useLocalhost = true;

    // Address used in code
    private string host => useLocalhost ? "localhost" : hostIP;
    // Final server address
    private string server;

    // WebSocket Client
    private WsClient client;

    private void OnGUI() {
        if (logVisible){
            GUI.Label(new Rect(10,10,Screen.width-20,10),log);
        }
    }

    /// <summary>
    /// Unity method called on initialization
    /// </summary>
    private void Awake()
    { 
        PlayerName = PlayerPrefs.GetString("Name");
        
        log = "Connecting to game server"; 
        server = "ws://" + host+ ":" + port + "/connect";
        Debug.Log("connecting to Server " + server);
        client = new WsClient(server);
        
    }
    private void Start() {
        Time.timeScale = 1; 
    }

    private void OnApplicationQuit() {
        client.Disconnect();
    }

    private void OnApplicationPause(bool pauseStatus) {
        Debug.Log("pause:" + pauseStatus);
        if (pauseStatus==true){
            SendRequest("pause");
            isPaused = true;
        }
    }

    private void OnApplicationFocus(bool hasFocus) {
        //Debug.Log("focus:" + hasFocus);
       if (hasFocus==false){
            SendRequest("pause");
            isPaused = true;
            return;
        } 
        SendRequest("resume");
    }

    /// <summary>
    /// Unity method called every frame
    /// </summary>
    private void Update()
    {

        if (status == "died"){
            client.Disconnect();
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
            return;
        }
        if (client.IsConnecting()){
            log="connecting";
            Debug.Log(log);
        }
        if (client.GetState()=="Closed"){
            return;
        }
        log= "server connected";
        var cqueue = client.receiveQueue;
        string msg;
        while (cqueue.TryPeek(out msg))
        {
            cqueue.TryDequeue(out msg);
            HandleMessage(msg);
        }
        //elapsed += Time.deltaTime;
        if (clientconnected==true){
            //Render();
        }
    }

    private void clearRenderBullet(){
        GameObject[] bb = GameObject.FindGameObjectsWithTag("bullet");
        foreach(GameObject b in bb){
            GameObject.Destroy(b);
        }
    }

    /// <summary>
    /// Method responsible for handling server messages
    /// </summary>
    /// <param name="msg">Message.</param>
    private void HandleMessage(string msg)
    {
        if (namesent==false){
            SendRequest("setup|"+ PlayerName);
            namesent=true;
        }
        //Debug.Log("Server: " + msg);
        game =  GameModel.CreateFromJSON(msg);
        if (game != null ){
            clientconnected = true;
        }
        //log = game.Status;
        /*
        switch (game.Status){
            case "waiting for player":
                break;
            case "ready":
                break; 
            case "play":
                break; 
            case "end":
                break;
            case "scoreboard":
                break;
            default:
                break;
        }*/
        Render();
    }

    /// <summary>
    /// Call this method to connect to the server
    /// </summary>
    public void ConnectToServer()
    {
        client.Connect();
    }

    /// <summary>
    /// Method which sends data through websocket
    /// </summary>
    /// <param name="message">Message.</param>
    public void SendRequest(string message)
    {
        //Debug.Log(message);
        client.Send(message);
    }

    public void Render(){
        if (game == null){
            return;
        }
        logVisible = false;    
        RenderPlayer();    
        RenderBullet();
    }

    private void RenderPlayer(){
        Player p;
        GameObject s;
        GameObject arrow;
        string pName;
        string arrowPName;
        TextMesh playerNameTextObject;
        TextMesh arrowNameTextObject; 
        TextMesh textObject;
        Transform skin;

        for (int bi = 0; bi < game.Players.Length; bi++){        
            p = game.Players[bi];            
            pName = "ship_" + bi;
            s = GameObject.Find(pName);
            Quaternion angle = Quaternion.Euler(0,0, p.Rotation * Mathf.Rad2Deg);
            angle *= Quaternion.Euler(0, 0, 90);
            Vector3 position = new Vector3(p.X,p.Y,0);    
            if (s==null){
                s = Instantiate(Ship,position,Quaternion.identity);   
                skin = s.transform.Find("Skin");             
                s.name=pName;
                s.transform.parent = GameObjectContainer.transform;
                if (p.UUID == game.You.UUID){
                    int skinIndex = 0;
                    if (PlayerPrefs.HasKey("Skin")){
                        skinIndex = PlayerPrefs.GetInt("Skin");
                        Debug.Log("set player skin index to " + skinIndex);
                    }   
                    skin.GetComponent<SpriteRenderer>().sprite = skins[skinIndex];
                }   
            }else{
                s.transform.position = position;    
            }
            skin = s.transform.Find("Skin");
            skin.rotation = angle;
            playerNameTextObject = s.transform.Find("Name").GetComponent<TextMesh>();
            playerNameTextObject.text = p.Name;

            if (p.Life <= 0){
                status="died";
                GameObject.Destroy(s);
                return;            
            }
            textObject = s.transform.Find("Life").GetComponent<TextMesh>(); 
            textObject.text = p.Life.ToString();        
            
            if (p.UUID == game.You.UUID)  {
                Camera.main.transform.position = new Vector3(p.X,p.Y,-10);
            }else{                
                arrowPName = "arrow_" + bi;
                arrow = GameObject.Find(arrowPName);
                Vector3 playerPosition = new Vector3(p.X,p.Y,0);
                Vector3 screenPos = Camera.main.WorldToViewportPoint(playerPosition); 
                if(screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1){
                    if (arrow!=null){
                        arrow.active = false;
                    }
                    continue;
                }
                if (arrow!=null){
                    arrow.active = true;
                }
                Vector2 onScreenPos = new Vector2(screenPos.x-0.5f, screenPos.y-0.5f)*2;
                float max = Mathf.Max(Mathf.Abs(onScreenPos.x), Mathf.Abs(onScreenPos.y));
                onScreenPos = (onScreenPos/(max*2))+new Vector2(0.5f, 0.5f);
                Vector3 vtowp = Camera.main.ViewportToWorldPoint(onScreenPos);
                float deltaX = 4;
                float deltaY = 4;
                if (screenPos.x<0){
                    deltaX = -deltaX;
                }            
                if (screenPos.y<0){
                    deltaY = -deltaY;
                }
                Vector3 arrowPosition = new Vector3(vtowp.x-deltaX,vtowp.y-deltaY,0);
                if (arrow==null){
                    arrow = Instantiate(Arrow,arrowPosition,Quaternion.identity); 
                    arrowNameTextObject = arrow.transform.Find("Name").GetComponent<TextMesh>();
                    arrowNameTextObject.text = p.Name;
                    arrow.name=arrowPName;
                    arrow.transform.parent = Camera.main.transform;
                }else{
                    arrow.transform.position = arrowPosition;
                }

                SpriteRenderer asp = arrow.transform.Find("ArrowSprite").GetComponent<SpriteRenderer>();  
                if (asp != null ){                   
                    Vector3 relativePos = playerPosition-asp.transform.position;
                    float rotationZ = Mathf.Atan2(relativePos.x, relativePos.y) * (Mathf.Rad2Deg);
                    asp.transform.rotation =  Quaternion.Euler(0.0f, 0.0f, -rotationZ);
                }
            }
        }
    }

    private void RenderBullet(){        
            Bullet b;
            GameObject bulletObj;
            string bulletObjectName;
            List<string> bulletsList = new List<string>();
            for (int bi = 0; bi < game.Bullets.Length; bi++)
            {
                b = game.Bullets[bi];
                bulletObjectName = "bullet_"+b.ID;
                bulletsList.Add(bulletObjectName);
                bulletObj = GameObject.Find(bulletObjectName);
                Vector3 position = new Vector3(b.X,b.Y,0);
                Quaternion angle = Quaternion.Euler(0,0, b.Rotation * Mathf.Rad2Deg);
                if (bulletObj == null && !b.Exhausted){
                    bulletObj = Instantiate(BulletPrefab,position,angle);
                    bulletObj.name = bulletObjectName;
                    bulletObj.transform.parent = GameObjectContainer.transform;
                    bulletObj.tag = "bullet";             
                }else {               
                    if (b.Exhausted){
                        GameObject.Destroy(bulletObj);
                        continue;
                    }
                    bulletObj.transform.position = position;
                    bulletObj.transform.rotation = angle;            
                }
            }
            
            GameObject[] allbullets = GameObject.FindGameObjectsWithTag("bullet");
            if (allbullets == null){    
                return;
            }
            foreach (var bulletGameObject in allbullets)
            {
                bool a = bulletsList.Contains(bulletGameObject.name);
                if (!a){
                    GameObject.Destroy(bulletGameObject);
                }
            }

            
    }

    
}