using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    List<string> stateName = new List<string>();
    List<string> stateStatus = new List<string>();

    [Header("State Selecter")]
    public GameObject SelecterImage;
    public GameObject AttackButton;
    public GameObject DefendButton;
    public Text StateNameText;

    public Text dataText; 

    public SaveLoad saveload;
    // Start is called before the first frame update
    void Start()
    {
        saveload = new SaveLoad();
        SelecterImage.SetActive(false);
        SaveData.Load();
        LoadMap();
        
        
    }

    #region save and load from json

    void Save()
    {
        print("1");
        string[] sname = new string[stateName.Count];
        string[] sstatus = new string[stateStatus.Count];
        int d = 0;
        foreach (string s in stateName)
        {
            sname[d] = s;
            d++;
        }
        d = 0;
        foreach (string s in stateStatus)
        {
            sstatus[d] = s;
            d++;
        }
        Data m = new Data("States", sname, sstatus);
        saveload.Save(m);
    }

    void Load()
    {
        Data m = saveload.Load("States");
        for (int i = 0; i < m.name.Length; i++)
        {
            stateName.Add(m.name[i]);
            stateStatus.Add(m.status[i]);
        }
    }

    #endregion

    public void SaveJ()
    {
        print("UnderAttack");
        stateStatus[7] = "Under Attack";
        Save();
        //string[] name=new string[2];
        //string[] lvl = new string[2];


        //name[0] = "Raju";
        //name[1] = "Rohit";
        //lvl[0] = "hg";

        //lvl[1] = "fg";
        
        //Data m = new Data("Goblin", name, lvl);
        //saveload.Save(m);
        //LoadJ();
        
    }
    
    public void LoadJ()
    {
        Data m=saveload.Load("Goblin");
        print(m.fname);
        foreach (string s in m.name)
        {
            print(s);
            dataText.text += s;
        }
    }
    public void LoadF()
    {
        Data m=saveload.LoadFromResources("Goblin");
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.tag == "States")
                {
                    SelecterImage.SetActive(true);
                    
                    //check if the state is normal or infected
                    if (hit.collider.gameObject.GetComponent<Renderer>().material.name == "Infected (Instance)")
                    {
                        //if infected show attack button
                        AttackButton.SetActive(true);
                        DefendButton.SetActive(false);
                        
                    }
                    else if (hit.collider.gameObject.GetComponent<Renderer>().material.name == "Normal (Instance)")
                    {
                        //if normal dont show attack button or defend button
                        AttackButton.SetActive(false);
                        DefendButton.SetActive(false);
                    }
                    else if (hit.collider.gameObject.GetComponent<Renderer>().material.name == "UnderAttack (Instance)")
                    {
                        //if normal dont show attack button or defend button
                        AttackButton.SetActive(false);
                        DefendButton.SetActive(true);
                    }
                    SaveData.currentSelectedState = hit.collider.gameObject.name;
                    SaveData.currentSelectedStateStatus = hit.collider.gameObject.GetComponent<Renderer>().material.name;
                    showSelectedStateDetail();
                }
            }
        }
    }
    
   

    void showSelectedStateDetail()
    {
        StateNameText.text = SaveData.currentSelectedState;
    }

    #region create the map and colour

    [Header("Elements for map states")]
    public GameObject[] AllStates;

    public Material InfectedMaterial;
    public Material NormalMaterial;
    public Material UnderAttackMaterial;

    void LoadMap()
    {
        Load();

        if (AllStates.Length == stateName.Count)
        {
            for (int i = 0; i < stateName.Count; i++)
            {
                if (stateStatus[i] == "Occupied by zombie")
                {
                    //place infected area
                    AllStates[i].GetComponent<Renderer>().material = InfectedMaterial;
                }
                else if (stateStatus[i] == "Occupied by me")
                {
                    //place normal area
                    AllStates[i].GetComponent<Renderer>().material = NormalMaterial;
                }
                else if (stateStatus[i] == "Under Attack")
                {
                    //place underattack area
                    AllStates[i].GetComponent<Renderer>().material = UnderAttackMaterial;
                }
            }
        }
        else
        {
            print("State Mismatch");
        }
    }

    #endregion

    public void AttackButtonPressed()
    {
        //generate random zombie and enter to game
        SaveData.zombieAmount = Random.RandomRange(1, 3);
        SaveData.Save();
       // SceneManager.LoadScene(1);
    }

    public void CancelButtonPressed()
    {
        SelecterImage.SetActive(false);
    }
}
