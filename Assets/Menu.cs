using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Button okButton;
    public Button cancelButton;
    public Button pigDetails;
    public Button joinArenaButton;

    public Button[] skinbtns = new Button[4];


    public InputField playerNameInput;
    private string playerName;
    private string skinName;
    private GameObject pigSetup;
    public int selectedSkin;
    private string logText;

    private void OnGUI() {
        GUI.Label(new Rect(10,10,100,20),logText);
    }

    // Start is called before the first frame update
    void Start()
    {
		okButton.onClick.AddListener(ChangePlayerSetup);
		cancelButton.onClick.AddListener(ClosePlayerSetup);
        pigDetails.onClick.AddListener(OpenPigDetail);
        joinArenaButton.onClick.AddListener(JoinArenaClick);
        
        skinbtns[0].onClick.AddListener(delegate{ChangeSkin(0);});
        skinbtns[1].onClick.AddListener(delegate{ChangeSkin(1);});
        skinbtns[2].onClick.AddListener(delegate{ChangeSkin(2);});
        skinbtns[3].onClick.AddListener(delegate{ChangeSkin(3);});
        //for (int i = 0; i < skinbtns.Length; i++){}
        
        logText = "";
        playerName = PlayerPrefs.GetString("Name");
        
        selectedSkin = PlayerPrefs.GetInt("Skin");

        pigSetup = GameObject.Find("PigSetup");
        if (playerName==""){
            pigSetup.active = true;
        }else{
            playerNameInput.text = playerName;
            pigSetup.active = false;
        }
    }

    public void ChangeScene(){
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void OpenPigDetail(){
        logText="open pigDetailsBtn";
        if ( PlayerPrefs.GetString("Name")!=""){
            playerName=PlayerPrefs.GetString("Name");
        }
        pigSetup.active = true;
    }

    public void ChangePlayerSetup(){
        playerName =  playerNameInput.text;
        PlayerPrefs.SetString("Name",playerName);                        
        PlayerPrefs.SetInt("Skin",selectedSkin);
        PlayerPrefs.Save();
         logText=playerName;
        pigSetup.active=false;
    }

    public void ClosePlayerSetup(){
        logText="Case CancelBtn";
        pigSetup.active=false;
    }
    
    public void JoinArenaClick(){
        string playerName = PlayerPrefs.GetString("Name");
        if (playerName == ""){
            logText="Please, select pig name";
            OpenPigDetail();
        } else{
            ChangeScene();
        }
    }

    public void ChangeSkin(int skinIndex){
        selectedSkin = skinIndex;
        logText = "skin " + selectedSkin;
    }


}
