using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_SettingsManager : scr_Singleton<scr_SettingsManager> {

    // User Settings
    static public float g_MarbleScale = 0.5f;
    static public string g_URL = "http://giveaway.yucibot.nl/pixelsrealm";
    static public string g_SpritePath = "C:/Users/quinten/AppData/Roaming/MarbleRace/data/sprites/";
    static public string g_SavePath = "C:/Users/quinten/AppData/Roaming/MarbleRace/data/saves/";
    static public float g_DownloadDelay = 1.0f;

    // Global Variables
    static public bool g_IsClosed = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
