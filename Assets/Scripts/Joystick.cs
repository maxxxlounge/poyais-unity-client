using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour {
    public Transform player;
    public float speed = 5.0f;
    private Vector2 pointA;
    private Vector2 pointB;
    private bool isPaused;

    public Transform circle;
    public Transform outerCircle;
    public UnityEngine.UI.Text log;

    private Vector2 dir;
    private string lastMsg;
    private string lastActionMsg;
    private string lastBtnName;
    string msg ="";
    string btnName="";
    string btnStatus="";

    private GUIStyle guiStyle = new GUIStyle();
    

    [SerializeField]
    private ServerCommunication communication;

    private void OnApplicationPause(bool pauseStatus) {
        if (pauseStatus == true){
            Time.timeScale = 0.0f;
            isPaused = true;
        }   
        isPaused = pauseStatus;        
    }

    private void Start() {
        Time.timeScale = 1.0f;
        log.text = "test";
    }

	// Update is called once per frame
	void Update () {
        if (Input.touchCount <= 0 || isPaused){
            /*if ((lastMsg != "" || msg =="") && lastMsg != msg){
                msg = lastBtnName+"release";
                sendMessage(msg);
                lastMsg = msg;
            };*/
            return;
        }
        for (int t=0; t<Input.touchCount;t++)
        {
            if (t>1){
                continue;
            }
            Touch touch = Input.GetTouch(t);
            Vector3 touchPosWorld = Camera.main.ScreenToWorldPoint(touch.position);
            Vector2 touchPosWorld2D = new Vector2(touchPosWorld.x, touchPosWorld.y);
            RaycastHit2D hitInformation = Physics2D.Raycast(touchPosWorld2D, Camera.main.transform.forward);
            if (hitInformation.collider == null) {
                continue;
            }
            GameObject touchedObject = hitInformation.transform.gameObject;
            if (touchedObject == null || touchedObject.transform.name == ""){
                continue;
            }
            btnName = touchedObject.transform.name;
            if(btnName=="Fire"){
                btnStatus = "pressed";
                if (touch.phase == TouchPhase.Ended){
                    btnStatus = "release";
                }
                msg = btnName+btnStatus;
                sendMessage(msg,lastActionMsg);            
                lastActionMsg = msg;
                continue;
            }
            
            if (touch.phase == TouchPhase.Ended){
                msg = btnName+"release";
                sendMessage(msg);
                msg = lastBtnName+"release";
                sendMessage(msg);
                lastMsg = msg;
                continue;
            }
            if (lastBtnName != btnName && lastBtnName!=""){
                msg = lastBtnName+"release";
                sendMessage(msg);
            }
            btnStatus = "pressed";
            msg = btnName+btnStatus;
            if (msg != lastMsg){
                sendMessage(msg,lastMsg);            
            }
            lastMsg = msg;
            lastBtnName = btnName;
        }
	}

    private void sendMessage(string msg, string lastMsg=""){
            if (msg != lastMsg){
                communication.SendRequest(msg);       
            }
    }
 
}