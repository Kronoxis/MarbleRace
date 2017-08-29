using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class scr_InputManager : scr_Singleton<scr_InputManager> {

    // Configuration
    public static string URL = "http://giveaway.yucibot.nl/pixelsrealm";
    public static string DataPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/MarbleRace/data/";
    public static float DownloadDelay = 1.0f;
    public static float UpdateDelay = 0.1f;

    // Keys
    public KeyCode Key_Start = KeyCode.Space;

    // Version 
    static string m_Version = "2.0";

    // Application Settings
    void Awake()
    {
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
    }

    // Use this for initialization
    void Start()
    {
        //// Remove old Input folder
        //if (Directory.Exists(DataPath + "input"))
        //{
        //    if (File.Exists(DataPath + "input/controls.txt")) File.Delete(DataPath + "input/controls.txt");
        //    if (File.Exists(DataPath + "input/config.txt")) File.Delete(DataPath + "input/config.txt");
        //    Directory.Delete(DataPath + "input");
        //}
        // Create Folders
        if (!Directory.Exists(DataPath + "settings")) Directory.CreateDirectory(DataPath + "settings");
        if (!Directory.Exists(DataPath + "sprites")) Directory.CreateDirectory(DataPath + "sprites");
        if (!Directory.Exists(DataPath + "saves")) Directory.CreateDirectory(DataPath + "saves");

        // If Settings File exist
        // Load Settings at start
        // Else, Save Default
        if (File.Exists(DataPath + "settings/Config.txt")) LoadConfig();
        else SaveConfig();
        if (File.Exists(DataPath + "settings/Keys.txt")) LoadKeys();
        else SaveKeys();
	}

    public void SaveConfig()
    {
        Debug.Log("Saving Config...");

        // Open File
        StreamWriter writer = new StreamWriter(DataPath + "settings/config.txt");

        // Write Version
        writer.WriteLine(m_Version);

        // Write Config
        writer.WriteLine("URL = " + URL);
        writer.WriteLine("DataPath = " + DataPath);
        writer.WriteLine("DownloadDelay = " + DownloadDelay.ToString());
        writer.WriteLine("UpdateDelay = " + UpdateDelay.ToString());

        // Close File
        writer.Close();
    }

    public void LoadConfig()
    {
        // Open File
        StreamReader reader = new StreamReader(DataPath + "settings/config.txt");

        // Read Version
        if (reader.ReadLine() != m_Version)
        {
            // Version doesn't match, delete file and save default
            reader.Close();
            File.Delete(DataPath + "settings/config.txt");
            SaveConfig();
        }

        // Read Config
        URL = FileStrToValueStr(reader.ReadLine());
        DataPath = FileStrToValueStr(reader.ReadLine());
        DownloadDelay = float.Parse(FileStrToValueStr(reader.ReadLine()));
        UpdateDelay = float.Parse(FileStrToValueStr(reader.ReadLine()));

        // Close File
        reader.Close();
    }

    public void SaveKeys()
    {
        Debug.Log("Saving Keys...");

        // Open File
        StreamWriter writer = new StreamWriter(DataPath + "settings/keys.txt");

        // Write Version
        writer.WriteLine(m_Version);

        // Write Keys
        writer.WriteLine("Start = " + KeyCodeToString(Key_Start));

        // Close File
        writer.Close();
    }

    public void LoadKeys()
    {
        // Open File
        StreamReader reader = new StreamReader(DataPath + "settings/keys.txt");

        // Read Version
        if (reader.ReadLine() != m_Version)
        {
            // Version doesn't match, delete file and save default
            reader.Close();
            File.Delete(DataPath + "settings/keys.txt");
            SaveKeys();
        }

        // Read Keys
        Key_Start = StringToKeyCode(FileStrToValueStr(reader.ReadLine()));

        // Close File
        reader.Close();
    }

    // Helper Methods
    string FileStrToValueStr(string s)
    {
        return s.Substring(s.IndexOf('=') + 2);
    }

    public KeyCode StringToKeyCode(string keyName)
    {
        KeyCode key = KeyCode.None;
        switch (keyName)
        {
            case "0":
                key = KeyCode.Alpha0;
                break;
            case "1":
                key = KeyCode.Alpha1;
                break;
            case "2":
                key = KeyCode.Alpha2;
                break;
            case "3":
                key = KeyCode.Alpha3;
                break;
            case "4":
                key = KeyCode.Alpha4;
                break;
            case "5":
                key = KeyCode.Alpha5;
                break;
            case "6":
                key = KeyCode.Alpha6;
                break;
            case "7":
                key = KeyCode.Alpha7;
                break;
            case "8":
                key = KeyCode.Alpha8;
                break;
            case "9":
                key = KeyCode.Alpha9;
                break;
            case "AltGr":
                key = KeyCode.AltGr;
                break;
            case "Backspace":
                key = KeyCode.Backspace;
                break;
            case "Break":
                key = KeyCode.Break;
                break;
            case "CapsLock":
                key = KeyCode.CapsLock;
                break;
            case "Clear":
                key = KeyCode.Clear;
                break;
            case "Delete":
                key = KeyCode.Delete;
                break;
            case "Down":
                key = KeyCode.DownArrow;
                break;
            case "End":
                key = KeyCode.End;
                break;
            case "Escape":
                key = KeyCode.Escape;
                break;
            case "F1":
                key = KeyCode.F1;
                break;
            case "F2":
                key = KeyCode.F2;
                break;
            case "F3":
                key = KeyCode.F3;
                break;
            case "F4":
                key = KeyCode.F4;
                break;
            case "F5":
                key = KeyCode.F5;
                break;
            case "F6":
                key = KeyCode.F6;
                break;
            case "F7":
                key = KeyCode.F7;
                break;
            case "F8":
                key = KeyCode.F8;
                break;
            case "F9":
                key = KeyCode.F9;
                break;
            case "F10":
                key = KeyCode.F10;
                break;
            case "F11":
                key = KeyCode.F11;
                break;
            case "F12":
                key = KeyCode.F12;
                break;
            case "Help":
                key = KeyCode.Help;
                break;
            case "Home":
                key = KeyCode.Home;
                break;
            case "Insert":
                key = KeyCode.Insert;
                break;
            case "Num0":
                key = KeyCode.Keypad0;
                break;
            case "Num1":
                key = KeyCode.Keypad1;
                break;
            case "Num2":
                key = KeyCode.Keypad2;
                break;
            case "Num3":
                key = KeyCode.Keypad3;
                break;
            case "Num4":
                key = KeyCode.Keypad4;
                break;
            case "Num5":
                key = KeyCode.Keypad5;
                break;
            case "Num6":
                key = KeyCode.Keypad6;
                break;
            case "Num7":
                key = KeyCode.Keypad7;
                break;
            case "Num8":
                key = KeyCode.Keypad8;
                break;
            case "Num9":
                key = KeyCode.Keypad9;
                break;
            case "Num/":
                key = KeyCode.KeypadDivide;
                break;
            case "NumEnter":
                key = KeyCode.KeypadEnter;
                break;
            case "Num=":
                key = KeyCode.KeypadEquals;
                break;
            case "Num-":
                key = KeyCode.KeypadMinus;
                break;
            case "Num*":
                key = KeyCode.KeypadMultiply;
                break;
            case "Num.":
                key = KeyCode.KeypadPeriod;
                break;
            case "Num+":
                key = KeyCode.KeypadPlus;
                break;
            case "LAlt":
                key = KeyCode.LeftAlt;
                break;
            case "Left":
                key = KeyCode.LeftArrow;
                break;
            case "LCtrl":
                key = KeyCode.LeftControl;
                break;
            case "LShift":
                key = KeyCode.LeftShift;
                break;
            case "LMB":
                key = KeyCode.Mouse0;
                break;
            case "RMB":
                key = KeyCode.Mouse1;
                break;
            case "MMB":
                key = KeyCode.Mouse2;
                break;
            case "NumLock":
                key = KeyCode.Numlock;
                break;
            case "PgDn":
                key = KeyCode.PageDown;
                break;
            case "PgUp":
                key = KeyCode.PageUp;
                break;
            case "Print":
                key = KeyCode.Print;
                break;
            case "Return":
                key = KeyCode.Return;
                break;
            case "RAlt":
                key = KeyCode.RightAlt;
                break;
            case "Right":
                key = KeyCode.RightArrow;
                break;
            case "RCtrl":
                key = KeyCode.RightControl;
                break;
            case "RShift":
                key = KeyCode.RightShift;
                break;
            case "ScrollLock":
                key = KeyCode.ScrollLock;
                break;
            case "Space":
                key = KeyCode.Space;
                break;
            case "SysReq":
                key = KeyCode.SysReq;
                break;
            case "Tab":
                key = KeyCode.Tab;
                break;
            case "Up":
                key = KeyCode.UpArrow;
                break;
        }

        if (key == KeyCode.None)
            key = (KeyCode)System.Enum.Parse(typeof(KeyCode), keyName);

        return key;
    }

    public string KeyCodeToString(KeyCode key)
    {
        string keyName = null;
        switch (key)
        {
            case KeyCode.Alpha0:
                keyName = "0";
                break;
            case KeyCode.Alpha1:
                keyName = "1";
                break;
            case KeyCode.Alpha2:
                keyName = "2";
                break;
            case KeyCode.Alpha3:
                keyName = "3";
                break;
            case KeyCode.Alpha4:
                keyName = "4";
                break;
            case KeyCode.Alpha5:
                keyName = "5";
                break;
            case KeyCode.Alpha6:
                keyName = "6";
                break;
            case KeyCode.Alpha7:
                keyName = "7";
                break;
            case KeyCode.Alpha8:
                keyName = "8";
                break;
            case KeyCode.Alpha9:
                keyName = "9";
                break;
            case KeyCode.AltGr:
                keyName = "AltGr";
                break;
            case KeyCode.Backspace:
                keyName = "Backspace";
                break;
            case KeyCode.Break:
                keyName = "Break";
                break;
            case KeyCode.CapsLock:
                keyName = "CapsLock";
                break;
            case KeyCode.Clear:
                keyName = "Clear";
                break;
            case KeyCode.Delete:
                keyName = "Delete";
                break;
            case KeyCode.DownArrow:
                keyName = "Down";
                break;
            case KeyCode.End:
                keyName = "End";
                break;
            case KeyCode.Escape:
                keyName = "Escape";
                break;
            case KeyCode.F1:
                keyName = "F1";
                break;
            case KeyCode.F2:
                keyName = "F2";
                break;
            case KeyCode.F3:
                keyName = "F3";
                break;
            case KeyCode.F4:
                keyName = "F4";
                break;
            case KeyCode.F5:
                keyName = "F5";
                break;
            case KeyCode.F6:
                keyName = "F6";
                break;
            case KeyCode.F7:
                keyName = "F7";
                break;
            case KeyCode.F8:
                keyName = "F8";
                break;
            case KeyCode.F9:
                keyName = "F9";
                break;
            case KeyCode.F10:
                keyName = "F10";
                break;
            case KeyCode.F11:
                keyName = "F11";
                break;
            case KeyCode.F12:
                keyName = "F12";
                break;
            case KeyCode.Help:
                keyName = "Help";
                break;
            case KeyCode.Home:
                keyName = "Home";
                break;
            case KeyCode.Insert:
                keyName = "Insert";
                break;
            case KeyCode.Keypad0:
                keyName = "Num0";
                break;
            case KeyCode.Keypad1:
                keyName = "Num1";
                break;
            case KeyCode.Keypad2:
                keyName = "Num2";
                break;
            case KeyCode.Keypad3:
                keyName = "Num3";
                break;
            case KeyCode.Keypad4:
                keyName = "Num4";
                break;
            case KeyCode.Keypad5:
                keyName = "Num5";
                break;
            case KeyCode.Keypad6:
                keyName = "Num6";
                break;
            case KeyCode.Keypad7:
                keyName = "Num7";
                break;
            case KeyCode.Keypad8:
                keyName = "Num8";
                break;
            case KeyCode.Keypad9:
                keyName = "Num9";
                break;
            case KeyCode.KeypadDivide:
                keyName = "Num/";
                break;
            case KeyCode.KeypadEnter:
                keyName = "NumEnter";
                break;
            case KeyCode.KeypadEquals:
                keyName = "Num=";
                break;
            case KeyCode.KeypadMinus:
                keyName = "Num-";
                break;
            case KeyCode.KeypadMultiply:
                keyName = "Num*";
                break;
            case KeyCode.KeypadPeriod:
                keyName = "Num.";
                break;
            case KeyCode.KeypadPlus:
                keyName = "Num+";
                break;
            case KeyCode.LeftAlt:
                keyName = "LAlt";
                break;
            case KeyCode.LeftArrow:
                keyName = "Left";
                break;
            case KeyCode.LeftControl:
                keyName = "LCtrl";
                break;
            case KeyCode.LeftShift:
                keyName = "LShift";
                break;
            case KeyCode.Mouse0:
                keyName = "LMB";
                break;
            case KeyCode.Mouse1:
                keyName = "RMB";
                break;
            case KeyCode.Mouse2:
                keyName = "MMB";
                break;
            case KeyCode.Numlock:
                keyName = "NumLock";
                break;
            case KeyCode.PageDown:
                keyName = "PgDn";
                break;
            case KeyCode.PageUp:
                keyName = "PgUp";
                break;
            case KeyCode.Print:
                keyName = "Print";
                break;
            case KeyCode.Return:
                keyName = "Return";
                break;
            case KeyCode.RightAlt:
                keyName = "RAlt";
                break;
            case KeyCode.RightArrow:
                keyName = "Right";
                break;
            case KeyCode.RightControl:
                keyName = "RCtrl";
                break;
            case KeyCode.RightShift:
                keyName = "RShift";
                break;
            case KeyCode.ScrollLock:
                keyName = "ScrollLock";
                break;
            case KeyCode.Space:
                keyName = "Space";
                break;
            case KeyCode.SysReq:
                keyName = "SysReq";
                break;
            case KeyCode.Tab:
                keyName = "Tab";
                break;
            case KeyCode.UpArrow:
                keyName = "Up";
                break;
        }

        if (keyName == null)
            keyName = key.ToString();
        return keyName;
    }
}
