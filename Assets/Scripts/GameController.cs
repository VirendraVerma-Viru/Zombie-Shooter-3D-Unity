 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    List<string> stateName = new List<string>();
    List<string> stateStatus = new List<string>();

    public Text zombiecounterText;

    public Transform[] zombieSpawnPoints;
    public GameObject Zombie;

    public float timer;
    public float interval;

    private bool slowMotion=false;
    private bool turretBuildEnable = false;
    private int zombieAmount = 10;

    private Transform tempNode;

    [Header("BuildOptionTurret")]
    [SerializeField]private GameObject TurretOptionCanvas;

    [Header("Turret Variables")]
    [SerializeField] private GameObject MachineTurretGameObject;

    public SaveLoad saveload;

    void Start()
    {
        saveload = new SaveLoad();
        SaveData.Load();
        zombieAmount = SaveData.zombieAmount;
        timer = 1;
        interval =1;
        slowMotion = false;
        turretBuildEnable = false;
        TurretOptionCanvas.SetActive(false);
    }
    void Save()
    {
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
    // Update is called once per frame
    void Update()
    {
        if (zombieAmount < 0)
        {
            return;
        }
        else if (zombieAmount == 0)
        {
            Load();
            //clear the state from json

            for (int i = 0; i < stateStatus.Count; i++)
            {
                if (SaveData.currentSelectedState == stateName[i])
                {
                    print(i + "Fount");
                    stateStatus[i] = "Occupied by me";
                }
                else
                {
                    print(SaveData.currentSelectedState + "||" + stateName[i] );
                }
            }
            Save();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
                //SceneManager.LoadScene(0);
        }

        ZombieSpawnSystem();
        SlowMotionFunctionality();
        BuildFunctionality();

        if (Input.GetKeyDown(KeyCode.G))
        {
            OnMachineTurretPressed();
        }
    }

    private void ZombieSpawnSystem()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = interval;
            zombieAmount--;
            //spawn zombie
            zombiecounterText.text = zombieAmount.ToString();
            int random = Random.Range(0, zombieSpawnPoints.Length);
            GameObject go = Instantiate(Zombie, zombieSpawnPoints[random].position, zombieSpawnPoints[random].rotation);

        }
    }

    private void SlowMotionFunctionality()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (slowMotion == true)
            {
                slowMotion = false;
                Time.timeScale = 1;
                Time.fixedDeltaTime = 0.02F;
            }
            else
            {
                slowMotion = true;
                Time.timeScale = 0.3f;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
            }
        }
    }

    private void BuildFunctionality()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (turretBuildEnable)
                turretBuildEnable = false;
            else
                turretBuildEnable = true;
        }

        if (turretBuildEnable)
        {
            turretBuildEnable = true;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.transform.tag == "TurretNode")
                {
                    TurretOptionCanvas.SetActive(true);
                    tempNode = hit.collider.transform;
                }
            }
        }
        else
        {
            TurretOptionCanvas.SetActive(false);
        }

    }

    public void OnMachineTurretPressed()
    {
        GameObject go = Instantiate(MachineTurretGameObject, tempNode.position, tempNode.rotation);
        turretBuildEnable = false;
    }
}
