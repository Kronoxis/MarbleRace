using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class scr_InputManager : scr_Singleton<scr_InputManager>
{
    public enum SpriteSources
    {
        None,
        Local,
        Twitch,
        Both
    }

    // Configuration
    public static string URL = "http://giveaway.yucibot.nl/pixelsrealm";
    public static string DataPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/MarbleRace/data/";
    public static float DownloadDelay = 1.0f;
    public static float UpdateDelay = 0.1f;
    public static float ReleaseTimer = 3.0f;
    public static SpriteSources SpriteSource = SpriteSources.Both;

    public static KeyCode Key_Start = KeyCode.Space;
    public static KeyCode Key_ZoomIn = KeyCode.LeftShift;
    public static KeyCode Key_ZoomOut = KeyCode.LeftControl;
    public static KeyCode Key_AddTest = KeyCode.Return;
    public static KeyCode Key_Move = KeyCode.Mouse1;
    public static KeyCode Key_Recenter = KeyCode.Mouse2;
    public static float MoveSpeed = 1.0f;
    public static float ZoomSpeed = 1.0f;

    // Variables
    public static List<KeyCode> Keys = new List<KeyCode>();
    public static bool IsFinished = false;

    // Version 
    static string m_Version = "2.1";

    // Application Settings
    void Awake()
    {
        Application.targetFrameRate = 60;
        Application.runInBackground = true;

        // Create Folders
        CreateFolders();

        // Create Files
        CreateFiles();
    }

    public static void CreateFolders()
    {
        // DataPath
        if (!Directory.Exists(DataPath)) Directory.CreateDirectory(DataPath);
        // Races
        if (!Directory.Exists(DataPath + "races")) Directory.CreateDirectory(DataPath + "races");
        // Settings
        if (!Directory.Exists(DataPath + "settings")) Directory.CreateDirectory(DataPath + "settings");
        // Sprites
        if (!Directory.Exists(DataPath + "sprites")) Directory.CreateDirectory(DataPath + "sprites");
        // Saves
        if (!Directory.Exists(DataPath + "saves")) Directory.CreateDirectory(DataPath + "saves");
    }

    public static void CreateFiles()
    {
        // Races
        if (!File.Exists(DataPath + "races/Small.txt")) File.Copy(Application.dataPath + "/DefaultFiles/races/Small.txt", DataPath + "races/Small.txt");
        if (!File.Exists(DataPath + "races/Big.txt")) File.Copy(Application.dataPath + "/DefaultFiles/races/Big.txt", DataPath + "races/Big.txt");
        // Sprites
        if (!File.Exists(DataPath + "sprites/mstiekema.jpeg")) File.Copy(Application.dataPath + "/DefaultFiles/sprites/mstiekema.jpeg", DataPath + "sprites/mstiekema.jpeg");
        if (!File.Exists(DataPath + "sprites/pixelsrealm.jpg")) File.Copy(Application.dataPath + "/DefaultFiles/sprites/pixelsrealm.jpg", DataPath + "sprites/pixelsrealm.jpg");
        if (!File.Exists(DataPath + "sprites/Thycon.png")) File.Copy(Application.dataPath + "/DefaultFiles/sprites/Thycon.png", DataPath + "sprites/Thycon.png");
        // Settings
        if (File.Exists(DataPath + "settings/Settings.txt")) LoadSettings();
        else SaveSettings();
    }

    public static void SaveSettings()
    {
        Debug.Log("Saving Settings...");

        // Open File
        StreamWriter writer = new StreamWriter(DataPath + "settings/Settings.txt");

        // Write Version
        writer.WriteLine(m_Version);

        // Write Config
        writer.WriteLine("URL = " + URL);
        writer.WriteLine("DataPath = " + DataPath);
        writer.WriteLine("DownloadDelay = " + DownloadDelay.ToString());
        writer.WriteLine("UpdateDelay = " + UpdateDelay.ToString());
        writer.WriteLine("ReleaseTimer = " + ReleaseTimer.ToString());
        writer.WriteLine("SpriteSource = " + SpriteSource.ToString());

        writer.WriteLine("StartKey = " + KeyCodeToString(Key_Start));
        writer.WriteLine("ZoomInKey = " + KeyCodeToString(Key_ZoomIn));
        writer.WriteLine("ZoomOutKey = " + KeyCodeToString(Key_ZoomOut));
        writer.WriteLine("AddTestKey = " + KeyCodeToString(Key_AddTest));
        writer.WriteLine("MoveKey = " + KeyCodeToString(Key_Move));
        writer.WriteLine("RecenterKey = " + KeyCodeToString(Key_Recenter));
        writer.WriteLine("MoveSpeed = " + MoveSpeed.ToString());
        writer.WriteLine("ZoomSpeed = " + ZoomSpeed.ToString());

        UpdateKeys();

        // Close File
        writer.Close();
    }

    public static void LoadSettings()
    {
        // Open File
        StreamReader reader = new StreamReader(DataPath + "settings/Settings.txt");

        // Read Version
        if (reader.ReadLine() != m_Version)
        {
            // Version doesn't match, delete file and save default
            reader.Close();
            File.Delete(DataPath + "settings/config.txt");
            SaveSettings();
            return;
        }

        // Read Config
        URL = FileStrToValueStr(reader.ReadLine());
        DataPath = FileStrToValueStr(reader.ReadLine());
        DownloadDelay = float.Parse(FileStrToValueStr(reader.ReadLine()));
        UpdateDelay = float.Parse(FileStrToValueStr(reader.ReadLine()));
        ReleaseTimer = float.Parse(FileStrToValueStr(reader.ReadLine()));
        SpriteSource = (SpriteSources)(System.Enum.Parse(typeof(SpriteSources), FileStrToValueStr(reader.ReadLine())));

        Key_Start = StringToKeyCode(FileStrToValueStr(reader.ReadLine()));
        Key_ZoomIn = StringToKeyCode(FileStrToValueStr(reader.ReadLine()));
        Key_ZoomOut = StringToKeyCode(FileStrToValueStr(reader.ReadLine()));
        Key_AddTest = StringToKeyCode(FileStrToValueStr(reader.ReadLine()));
        Key_Move = StringToKeyCode(FileStrToValueStr(reader.ReadLine()));
        Key_Recenter = StringToKeyCode(FileStrToValueStr(reader.ReadLine()));
        MoveSpeed = float.Parse(FileStrToValueStr(reader.ReadLine()));
        ZoomSpeed = float.Parse(FileStrToValueStr(reader.ReadLine()));

        UpdateKeys();

        // Close File
        reader.Close();
    }

    static void UpdateKeys()
    {
        Keys = new List<KeyCode> { Key_Start, Key_ZoomIn, Key_ZoomOut, Key_AddTest, Key_Move, Key_Recenter };
    }

    public static void SetKeysFromList()
    {
        Key_Start = Keys[0];
        Key_ZoomIn = Keys[1];
        Key_ZoomOut = Keys[2];
        Key_AddTest = Keys[3];
        Key_Move = Keys[4];
        Key_Recenter = Keys[5];
    }

    #region Helpers
    static string FileStrToValueStr(string s)
    {
        if (s == null) return null;
        return s.Substring(s.IndexOf('=') + 2);
    }

    public static KeyCode StringToKeyCode(string keyName)
    {
        if (keyName == null) return KeyCode.None;
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
            case "Caps Lock":
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
            case "Numpad 0":
                key = KeyCode.Keypad0;
                break;
            case "Numpad 1":
                key = KeyCode.Keypad1;
                break;
            case "Numpad 2":
                key = KeyCode.Keypad2;
                break;
            case "Numpad 3":
                key = KeyCode.Keypad3;
                break;
            case "Numpad 4":
                key = KeyCode.Keypad4;
                break;
            case "Numpad 5":
                key = KeyCode.Keypad5;
                break;
            case "Numpad 6":
                key = KeyCode.Keypad6;
                break;
            case "Numpad 7":
                key = KeyCode.Keypad7;
                break;
            case "Numpad 8":
                key = KeyCode.Keypad8;
                break;
            case "Numpad 9":
                key = KeyCode.Keypad9;
                break;
            case "Numpad /":
                key = KeyCode.KeypadDivide;
                break;
            case "Numpad Enter":
                key = KeyCode.KeypadEnter;
                break;
            case "Numpad =":
                key = KeyCode.KeypadEquals;
                break;
            case "Numpad -":
                key = KeyCode.KeypadMinus;
                break;
            case "Numpad *":
                key = KeyCode.KeypadMultiply;
                break;
            case "Numpad .":
                key = KeyCode.KeypadPeriod;
                break;
            case "Numpad +":
                key = KeyCode.KeypadPlus;
                break;
            case "Left Alt":
                key = KeyCode.LeftAlt;
                break;
            case "Left":
                key = KeyCode.LeftArrow;
                break;
            case "Left Ctrl":
                key = KeyCode.LeftControl;
                break;
            case "Left Shift":
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
            case "Num Lock":
                key = KeyCode.Numlock;
                break;
            case "Page Down":
                key = KeyCode.PageDown;
                break;
            case "Page Up":
                key = KeyCode.PageUp;
                break;
            case "Print":
                key = KeyCode.Print;
                break;
            case "Return":
                key = KeyCode.Return;
                break;
            case "Right Alt":
                key = KeyCode.RightAlt;
                break;
            case "Right":
                key = KeyCode.RightArrow;
                break;
            case "Right Ctrl":
                key = KeyCode.RightControl;
                break;
            case "Right Shift":
                key = KeyCode.RightShift;
                break;
            case "Scroll Lock":
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

    public static string KeyCodeToString(KeyCode key)
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
                keyName = "Caps Lock";
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
                keyName = "Numpad 0";
                break;
            case KeyCode.Keypad1:
                keyName = "Numpad 1";
                break;
            case KeyCode.Keypad2:
                keyName = "Numpad 2";
                break;
            case KeyCode.Keypad3:
                keyName = "Numpad 3";
                break;
            case KeyCode.Keypad4:
                keyName = "Numpad 4";
                break;
            case KeyCode.Keypad5:
                keyName = "Numpad 5";
                break;
            case KeyCode.Keypad6:
                keyName = "Numpad 6";
                break;
            case KeyCode.Keypad7:
                keyName = "Numpad 7";
                break;
            case KeyCode.Keypad8:
                keyName = "Numpad 8";
                break;
            case KeyCode.Keypad9:
                keyName = "Numpad 9";
                break;
            case KeyCode.KeypadDivide:
                keyName = "Numpad /";
                break;
            case KeyCode.KeypadEnter:
                keyName = "Numpad Enter";
                break;
            case KeyCode.KeypadEquals:
                keyName = "Numpad =";
                break;
            case KeyCode.KeypadMinus:
                keyName = "Numpad -";
                break;
            case KeyCode.KeypadMultiply:
                keyName = "Numpad *";
                break;
            case KeyCode.KeypadPeriod:
                keyName = "Numpad .";
                break;
            case KeyCode.KeypadPlus:
                keyName = "Numpad +";
                break;
            case KeyCode.LeftAlt:
                keyName = "Left Alt";
                break;
            case KeyCode.LeftArrow:
                keyName = "Left";
                break;
            case KeyCode.LeftControl:
                keyName = "Left Ctrl";
                break;
            case KeyCode.LeftShift:
                keyName = "Left Shift";
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
                keyName = "Num Lock";
                break;
            case KeyCode.PageDown:
                keyName = "Page Down";
                break;
            case KeyCode.PageUp:
                keyName = "Page Up";
                break;
            case KeyCode.Print:
                keyName = "Print";
                break;
            case KeyCode.Return:
                keyName = "Return";
                break;
            case KeyCode.RightAlt:
                keyName = "Right Alt";
                break;
            case KeyCode.RightArrow:
                keyName = "Right";
                break;
            case KeyCode.RightControl:
                keyName = "Right Ctrl";
                break;
            case KeyCode.RightShift:
                keyName = "Right Shift";
                break;
            case KeyCode.ScrollLock:
                keyName = "Scroll Lock";
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
    #endregion
}
