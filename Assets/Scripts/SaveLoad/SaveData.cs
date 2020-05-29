using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;


public class SaveData : MonoBehaviour
{
    List<string> stateName = new List<string>();
    List<string> stateStatus = new List<string>();


    public static string accountID = " ";
    public static string playerName = " ";
    public static string currentSelectedState = " ";
    public static string currentSelectedStateStatus = " ";
    public static int zombieAmount = 0;
    public static bool won = false;


    public static string current_filename = "info.dat";

    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + current_filename);
        Notebook_Data data = new Notebook_Data();


        data.AccountID = accountID;
        data.PlayerName = Encrypt(playerName);
        data.CurrentSelectedState = Encrypt(currentSelectedState);
        data.CurrentSelectedStateStatus = Encrypt(currentSelectedStateStatus);
        data.ZombieAmount = zombieAmount;
        data.Won = won;

        bf.Serialize(file, data);
        file.Close();
    }

    public static void Load()
    {

        if (File.Exists(Application.persistentDataPath + "/" + current_filename))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + current_filename, FileMode.Open);/* */
            Notebook_Data data = (Notebook_Data)bf.Deserialize(file);

            accountID = data.AccountID;
            playerName = Decrypt(data.PlayerName);
            currentSelectedState = Decrypt(data.CurrentSelectedState);
            currentSelectedStateStatus = Decrypt(data.CurrentSelectedStateStatus);
            zombieAmount = data.ZombieAmount;
            won = data.Won;


            file.Close();

        }
        else
        {
            current_filename = "info.dat";
            accountID = " ";
            SaveData m = new SaveData();
            m.InitializeMapFirstTime();
            SaveData.Save();

        }
    }

    private static string hash = "9452@abc";

    public static string Encrypt(string input)
    {
        byte[] data = UTF8Encoding.UTF8.GetBytes(input);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
            {
                ICryptoTransform tr = trip.CreateEncryptor();
                byte[] results = tr.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(results, 0, results.Length);
            }
        }
    }

    public static string Decrypt(string input)
    {
        byte[] data = Convert.FromBase64String(input);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
            {
                ICryptoTransform tr = trip.CreateDecryptor();
                byte[] results = tr.TransformFinalBlock(data, 0, data.Length);
                return UTF8Encoding.UTF8.GetString(results);
            }
        }
    }

    void InputdataTOList()
    {
        

        stateName.Add("Uttar Pradesh");
        stateStatus.Add("Occupied by me");

        stateName.Add("Bihar");
        stateStatus.Add("Occupied by zombie");

        stateName.Add("Delhi");
        stateStatus.Add("Occupied by zombie");

        stateName.Add("Gujrat");
        stateStatus.Add("Under Attack");

        stateName.Add("Tamil Nadu");
        stateStatus.Add("Occupied by zombie");

        stateName.Add("Bengal");
        stateStatus.Add("Occupied by zombie");

        stateName.Add("Maharastra");
        stateStatus.Add("Occupied by zombie");

        stateName.Add("Chennai");
        stateStatus.Add("Occupied by zombie");

       

    }

    public void InitializeMapFirstTime()
    {
        InputdataTOList();
        SaveLoad saveload = new SaveLoad();
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
        print("creted");
    }

}


[Serializable]
class Notebook_Data
{
    public string AccountID;
    public string PlayerName;
    public string CurrentSelectedState;
    public string CurrentSelectedStateStatus;
    public int ZombieAmount;
    public bool Won;

}