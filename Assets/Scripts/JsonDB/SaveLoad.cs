using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveLoad 
{
    public string path;
    public string mobilePath;

    void CheckPath()
    {

        path = Application.dataPath;
        mobilePath = Application.persistentDataPath;
        if (Application.platform == RuntimePlatform.Android)
            path = mobilePath;

    }

    public void Save(Data m)
    {
        CheckPath();
        
        string json = JsonUtility.ToJson(m);
        File.WriteAllText(path + "/" + m.fname + ".json", json); 
    }
    public Data Load(string name)
    {
        CheckPath();
        
        Data m = new Data();
        string json = File.ReadAllText(path + "/" + name + ".json");
        JsonUtility.FromJsonOverwrite(json, m);
        return m;
    }
    public Data LoadFromResources(string name)
    {
        path = "JSONFiles/Data/";
        Data m = new Data();
        path = path + name + ".json";
        string newPath = path.Replace(".json", "");
        TextAsset ta = Resources.Load<TextAsset>(newPath);
        string json = ta.text;
        JsonUtility.FromJsonOverwrite(json, m);
        

        return m;
    }
}
public class Data
{
    public string fname;
    public string[] name;
    public string[] status;
    

    public Data(string f,string[] n, string[] l)
    {
        fname = f;
        name = n;
        status = l;
        
    }
    public Data(){

    }

}
//Data m = new Data("Goblin", name, lvl, d);
        //saveload.Save(m);

//Data m=saveload.Load("Goblin");
//m.fname;