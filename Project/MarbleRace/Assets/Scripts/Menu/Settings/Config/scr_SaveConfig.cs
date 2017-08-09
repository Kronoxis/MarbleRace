using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class scr_SaveConfig : MonoBehaviour {

    GlobalManager gManager;
    void Start()
    {
        gManager = GlobalManager.Instance();
    }


    public void SaveConfig()
    {
        SaveConfigToGlobal();
        if (Directory.Exists(gManager.gInputPath) && Directory.Exists(gManager.gSpritePath) && Directory.Exists(gManager.gSavePath))
            SaveConfigToFile();   
        else
        {
            gManager.CreateFiles();
        }
    }

    public void SaveConfigToGlobal()
    {
        string value = GameObject.Find("InputInputPath").GetComponent<InputField>().text;
        gManager.gInputPath = gManager.ModifyPath(value);

        value = GameObject.Find("InputGiveawayURL").GetComponent<InputField>().text;
        gManager.gGiveawayURL = gManager.ModifyUrl(value);

        value = GameObject.Find("InputSpritePath").GetComponent<InputField>().text;
        gManager.gSpritePath = gManager.ModifyPath(value);

        value = GameObject.Find("InputSavePath").GetComponent<InputField>().text;
        gManager.gSavePath = gManager.ModifyPath(value);

        int format = GameObject.Find("InputSaveFormat").GetComponent<Dropdown>().value;
        gManager.gFormat = (GlobalManager.LeaderboardFormat)format;

        value = GameObject.Find("InputDownloadUsersDelay").GetComponent<Slider>().value.ToString();
        gManager.gDownloadUsersDelay = float.Parse(value);

        value = GameObject.Find("InputScoreboardUpdateDelay").GetComponent<Slider>().value.ToString();
        gManager.gScoreboardUpdateDelay = float.Parse(value);

        value = GameObject.Find("InputAddMarbleDelay").GetComponent<Slider>().value.ToString();
        gManager.gAddMarbleDelay = float.Parse(value);

        bool enabled = GameObject.Find("InputManualStart").GetComponent<Toggle>().isOn;
        gManager.gIsManualStart = enabled;

        enabled = GameObject.Find("InputStartDelay").GetComponent<Toggle>().isOn;
        gManager.gIsStartDelay = enabled;
    }

    void SaveConfigToFile()
    {
        StreamWriter sw = new StreamWriter(gManager.gInputPath + "Config.txt");

        sw.WriteLine("InputPath: " + gManager.gInputPath);

        sw.WriteLine("GiveawayURL: " + gManager.gGiveawayURL);

        sw.WriteLine("SpritePath: " + gManager.gSpritePath);

        sw.WriteLine("SavePath: " + gManager.gSavePath);

        sw.WriteLine("SaveFormat: " + gManager.gFormat.ToString());

        sw.WriteLine("DownloadUsersDelay: " + gManager.gDownloadUsersDelay.ToString());

        sw.WriteLine("ScoreboardUpdateDelay: " + gManager.gScoreboardUpdateDelay.ToString());

        sw.WriteLine("AddMarbleDelay: " + gManager.gAddMarbleDelay.ToString());

        sw.WriteLine("ManualStart: " + gManager.gIsManualStart.ToString());

        sw.WriteLine("StartDelay: " + gManager.gIsStartDelay.ToString());
        
        sw.Close();
    }
}
