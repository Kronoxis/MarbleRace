using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class GlobalManager : Singleton<GlobalManager> {

    // Leaderboard Format
    public enum LeaderboardFormat
    {
        HHMMSS_YYYYMMDD,
        HHMMSS_YYYYDDMM,
        HHMMSS_DDMMYYYY,
        HHMMSS_MMDDYYYY,
        HHMM_YYYYMMDD,
        HHMM_YYYYDDMM,
        HHMM_DDMMYYYY,
        HHMM_MMDDYYYY
    }

    // Controls
    public KeyCode gKeyStart1;
    public KeyCode gKeyStart2;
    public KeyCode gKeyRight1;
    public KeyCode gKeyRight2;
    public KeyCode gKeyLeft1;
    public KeyCode gKeyLeft2;
    public KeyCode gKeyUp1;
    public KeyCode gKeyUp2;
    public KeyCode gKeyDown1;
    public KeyCode gKeyDown2;
    public KeyCode gKeyZoomIn1;
    public KeyCode gKeyZoomIn2;
    public KeyCode gKeyZoomOut1;
    public KeyCode gKeyZoomOut2;

    // Config
    public string gInputPath;
    public string gGiveawayURL;
    public string gSpritePath;
    public string gSavePath;
    public LeaderboardFormat gFormat;
    public float gDownloadUsersDelay;
    public float gScoreboardUpdateDelay;
    public float gAddMarbleDelay;

    // Other global variables
    public bool gIsGiveawayClosed;
    public Vector2 gReferenceResolution;

    void Awake()
    {
        // Default Init
        DefaultInit();
        // Create defaults when there are none to read from
        if (!Directory.Exists(gInputPath))
        {       
            // Create data directories and files
            CreateFiles();
        }
        else
        {
            // Init from file
            VariablesInit();
        }
    }

    void DefaultInit()
    {
        gKeyStart1 = KeyCode.Space;
        gKeyStart2 = KeyCode.Return;
        gKeyRight1 = KeyCode.D;
        gKeyRight2 = KeyCode.RightArrow;
        gKeyLeft1 = KeyCode.A;
        gKeyLeft2 = KeyCode.LeftArrow;
        gKeyUp1 = KeyCode.W;
        gKeyUp2 = KeyCode.UpArrow;
        gKeyDown1 = KeyCode.S;
        gKeyDown2 = KeyCode.DownArrow;
        gKeyZoomIn1 = KeyCode.KeypadPlus;
        gKeyZoomIn2 = KeyCode.LeftShift;
        gKeyZoomOut1 = KeyCode.KeypadMinus;
        gKeyZoomOut2 = KeyCode.LeftControl;

        gInputPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/MarbleRace/data/input/";
        gGiveawayURL = "http://www.pixelbot.nl/giveaway";
        gSpritePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/MarbleRace/data/sprites/";
        gSavePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/MarbleRace/data/saves/";
        gFormat = LeaderboardFormat.HHMMSS_YYYYMMDD;
        gDownloadUsersDelay = 1.0f;
        gScoreboardUpdateDelay = 0.05f;
        gAddMarbleDelay = 0.1f;

        gIsGiveawayClosed = false;
        gReferenceResolution = new Vector2(800, 600);
    }

    public void CreateFiles()
    {
        // Directories
        Directory.CreateDirectory(gInputPath);
        Directory.CreateDirectory(gSpritePath);
        Directory.CreateDirectory(gSavePath);

        // Files
        string controls, config = null;
        GenerateDefaultText(out controls, out config);

        File.WriteAllText(gInputPath + "Controls.txt", controls);
        File.WriteAllText(gInputPath + "Config.txt", config);
    }

    void GenerateDefaultText(out string controls, out string config)
    {
        controls = null;
        controls += "Start: " + KeyCodeToString(gKeyStart1) + "," + KeyCodeToString(gKeyStart2) + "," +
            "null" + "," + "null";
        controls += "\n";
        controls += "Horizontal: " + KeyCodeToString(gKeyRight1) + "," + KeyCodeToString(gKeyRight2) + "," + 
            KeyCodeToString(gKeyLeft1) + "," + KeyCodeToString(gKeyLeft2);
        controls += "\n";
        controls += "Vertical: " + KeyCodeToString(gKeyUp1) + "," + KeyCodeToString(gKeyUp2) + "," +
            KeyCodeToString(gKeyDown1) + "," + KeyCodeToString(gKeyDown2);
        controls += "\n";
        controls += "Zoom: " + KeyCodeToString(gKeyZoomIn1) + "," + KeyCodeToString(gKeyZoomIn2) + "," +
            KeyCodeToString(gKeyZoomOut1) + "," + KeyCodeToString(gKeyZoomOut2);

        config = null;
        config += "InputPath: " + gInputPath;
        config += "\n";
        config += "GiveawayURL: " + gGiveawayURL;
        config += "\n";
        config += "SpritePath: " + gSpritePath;
        config += "\n";
        config += "SavePath: " + gSavePath;
        config += "\n";
        config += "SaveFormat: " + gFormat.ToString();
        config += "\n";
        config += "DownloadUsersDelay: " + gDownloadUsersDelay.ToString();
        config += "\n";
        config += "ScoreboardUpdateDelay: " + gScoreboardUpdateDelay.ToString();
        config += "\n";
        config += "AddMarbleDelay: " + gAddMarbleDelay.ToString();
    }

    void VariablesInit()
    {
        // Get controls from controls file
        StreamReader sr = new StreamReader(gInputPath + "Controls.txt");
        string all = null;
        string line = null;

        // Read line by line, cut AxisName
        while (!sr.EndOfStream)
        {
            line = sr.ReadLine();
            line = line.Substring(line.IndexOf(": ") + 2);
            line += ",";
            all += line;
        }
        sr.Close();
        // Add every control to its global variable
        char[] splits = { ',' };
        string[] keys = all.Split(splits, System.StringSplitOptions.RemoveEmptyEntries);

        gKeyStart1   = StringToKeyCode(keys[0]);
        gKeyStart2   = StringToKeyCode(keys[1]);
        gKeyRight1   = StringToKeyCode(keys[4]);
        gKeyRight2   = StringToKeyCode(keys[5]);
        gKeyLeft1    = StringToKeyCode(keys[6]);
        gKeyLeft2    = StringToKeyCode(keys[7]);
        gKeyUp1      = StringToKeyCode(keys[8]);
        gKeyUp2      = StringToKeyCode(keys[9]);
        gKeyDown1    = StringToKeyCode(keys[10]);
        gKeyDown2    = StringToKeyCode(keys[11]);
        gKeyZoomIn1  = StringToKeyCode(keys[12]);
        gKeyZoomIn2  = StringToKeyCode(keys[13]);
        gKeyZoomOut1 = StringToKeyCode(keys[14]);
        gKeyZoomOut2 = StringToKeyCode(keys[15]);

        // Get config from config file
        sr = new StreamReader(gInputPath + "Config.txt");
        all = null;
        line = null;

        // Read line by line, cut VariableName
        while (!sr.EndOfStream)
        {
            line = sr.ReadLine();
            line = line.Substring(line.IndexOf(": ") + 2);
            line += ",";
            all += line;
        }
        sr.Close();
        // Add every config to its global variable
        string[] configs = all.Split(splits, System.StringSplitOptions.RemoveEmptyEntries);

        gInputPath              = ModifyPath(configs[0]);
        gGiveawayURL            = configs[1];
        gSpritePath             = ModifyPath(configs[2]);
        gSavePath               = ModifyPath(configs[3]);
        gFormat                 = StringToFormatEnum(configs[4]);
        gDownloadUsersDelay     = float.Parse(configs[5]);
        gScoreboardUpdateDelay  = float.Parse(configs[6]);
        gAddMarbleDelay         = float.Parse(configs[7]);
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

    public string ModifyPath(string path)
    {
        // Replace backslashes
        if (path.Contains("\\"))
        {
            string pathCopy = null;
            while (path.Contains("\\"))
            {
                // Add data up till first backslash
                pathCopy += path.Substring(0, path.IndexOf("\\"));
                // Add forward slash instead of backslash
                pathCopy += "/";
                // Cut data and backslash from path
                path = path.Substring(path.IndexOf("\\") + 1);
            }

            // Make sure data after last backslash isn't skipped
            if (path.Length > 0)
                pathCopy += path;

            path = pathCopy;
        }

        // Full path ? If no, has begin slash?
        if (!path.Contains(":"))
        {
            string pathCopy = path;
            if (path.StartsWith("/"))
            {
                path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/MarbleRace" + pathCopy;
            }
            else
            {
                path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/MarbleRace/" + pathCopy;
            }
        }

        // End slash?
        if (!path.EndsWith("/"))
        {
            path += "/";
        }

        return path;
    }

    public string ModifyUrl(string url)
    {
        if (url.Contains("http://") || url.Contains("https://"))
            return url;
        else
            return "http://" + url;
    }

    public LeaderboardFormat StringToFormatEnum(string formatStr)
    {
        LeaderboardFormat format = LeaderboardFormat.HHMMSS_YYYYMMDD;

        switch (formatStr)
        {
            case "HHMMSS_YYYYMMDD":
                format = LeaderboardFormat.HHMMSS_YYYYMMDD;
                break;
            case "HHMMSS_YYYYDDMM":
                format = LeaderboardFormat.HHMMSS_YYYYDDMM;
                break;
            case "HHMMSS_DDMMYYYY":
                format = LeaderboardFormat.HHMMSS_DDMMYYYY;
                break;
            case "HHMMSS_MMDDYYYY":
                format = LeaderboardFormat.HHMMSS_MMDDYYYY;
                break;
            case "HHMM_YYYYMMDD":
                format = LeaderboardFormat.HHMM_YYYYMMDD;
                break;
            case "HHMM_YYYYDDMM":
                format = LeaderboardFormat.HHMM_YYYYDDMM;
                break;
            case "HHMM_DDMMYYYY":
                format = LeaderboardFormat.HHMM_DDMMYYYY;
                break;
            case "HHMM_MMDDYYYY":
                format = LeaderboardFormat.HHMM_MMDDYYYY;
                break;
            default:
                break;
        }

        return format;
    }
}
