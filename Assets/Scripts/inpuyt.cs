using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inpuyt : MonoBehaviour
{
    
    private string textMov = "some text";
    private string textTouchPhase = "some text";
    public GameObject ship;
    public Transform circle;
    public Transform outerCircle;
    private float width;
    private float height;

    private void OnGUI() {    
        GUI.Label(new Rect(0,0,100,50),textMov);
        GUI.Label(new Rect(200,0,100,50),textTouchPhase);
    }

    private void Awake() {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        circle.GetComponent<SpriteRenderer>().enabled = true;
        outerCircle.GetComponent<SpriteRenderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Began){
                //touch.deltaPosition;
            }
            Vector2 pos = touch.position;
            pos.x = (pos.x - width) / width;
            pos.y = (pos.y - height) / height;
            textTouchPhase = touch.phase.ToString();
            ship.transform.position = pos;
            //textMov = "X: " +pos.x + "\n Y: " + pos.y ;
            textMov = touch.deltaPosition.x + " " + touch.deltaPosition.y;
        }
        
        
        
    }
}
