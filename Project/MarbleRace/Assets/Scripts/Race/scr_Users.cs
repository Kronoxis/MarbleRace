using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.IO;
using System.Linq;

[RequireComponent(typeof(scr_RaceInput))]
[RequireComponent(typeof(scr_CreateMarble))]
[RequireComponent(typeof(scr_Leaderboard))]

public class scr_Users : MonoBehaviour
{
    static List<GameObject> m_Userlist = new List<GameObject>();
    List<string> m_Usernames = new List<string>();

    List<FileInfo> m_SpriteFiles = new List<FileInfo>();
    scr_CreateMarble m_CreateMarbleScript;

    Text m_CenterText = null;
    Dictionary<string, float> m_JoinedUsersTime = new Dictionary<string, float>();
    float m_LoadedMessageTime = 3.0f;

    // Use this for initialization
	void Start ()
    {
        // Clear Static Arrays
        m_Userlist.Clear();

        // Set Bools
        scr_RaceInput.IsClosed = false;

        // Get Scripts
        m_CreateMarbleScript = GetComponent<scr_CreateMarble>();

        // Get Sprites
        if (scr_InputManager.SpriteSource == scr_InputManager.SpriteSources.Local || scr_InputManager.SpriteSource == scr_InputManager.SpriteSources.Both)
        {
            // Get Files from Directory
            DirectoryInfo info = new DirectoryInfo(scr_InputManager.DataPath + "sprites");
            FileInfo[] files = info.GetFiles();
            foreach (FileInfo file in files)
            {
                // Only support JPEG, JPG and PNG
                if (file.Extension.ToLower() == ".jpeg" || 
                    file.Extension.ToLower() == ".jpg" || 
                    file.Extension.ToLower() == ".png")
                {
                    m_SpriteFiles.Add(file);
                }
            }
        }

        // Start Download loop
        StartCoroutine(DownloadWebpage());

        // Start Creation loop
        StartCoroutine(CreateUsers());

        m_CenterText = GameObject.FindGameObjectWithTag("CenterText").GetComponent<Text>();
	}

    void Update()
    {
        if (m_LoadedMessageTime > -0.99f || m_LoadedMessageTime < -1.01f)
            UpdateCenterText();
    } 

    IEnumerator DownloadWebpage()
    {
        // While Giveaway is not closed
        while (!scr_RaceInput.IsClosed)
        {
            // Download Webpage
            WWW www = new WWW(scr_InputManager.URL);
            while (!www.isDone) yield return 0;
            string users = www.text;
            StoreUsers(users, ' ');

            // Delay
            yield return new WaitForSeconds(scr_InputManager.DownloadDelay);
        }
    }

    void StoreUsers(string data, char split)
    {
        // Add non-empty entries to users
        char[] seperators = { split };
        var users = data.Split(seperators, System.StringSplitOptions.RemoveEmptyEntries);

        if (users.Length > 0)
        {
            var last = users[users.Length - 1];
            if (last.Contains("|"))
            {
                users[users.Length - 1] = last.Substring(0, last.Length - 1);
                scr_RaceInput.IsClosed = true;
            }
        }

        foreach (var user in users)
        {
            // Don't add same user again
            if (m_Usernames.Find(x => x == user) != null) continue;
            // Add to list
            m_Usernames.Add(user);
        }
    }

    IEnumerator CreateUsers()
    {
        // Loop while Giveaway is open 
        // or when it's closed and not all usernames have been added yet
        while (m_Userlist.Count != m_Usernames.Count || !scr_RaceInput.IsClosed)
        {
            for (int i = 0; i < m_Usernames.Count; ++i)
            {
                string name = m_Usernames[i];
                // Don't add same user again
                if (m_Userlist.Find(x => x.name == name)) continue;

                // Marble Skin
                Texture2D tex = null;
                WWW www = null;
                switch (scr_InputManager.SpriteSource)
                {
                    case scr_InputManager.SpriteSources.Local:
                        {
                            // LOCAL
                            // Get file
                            FileInfo file = m_SpriteFiles.Find(x => x.Name.Substring(0, x.Name.IndexOf('.')).ToLower() == name);
                            if (file != null)
                            {
                                // Convert file to tex
                                tex = new Texture2D(128, 128, TextureFormat.RGB24, false);
                                byte[] imageData = File.ReadAllBytes(file.FullName);
                                tex.LoadImage(imageData);
                            }
                        }
                        break;
                    case scr_InputManager.SpriteSources.Twitch:
                        {
                            // TWITCH
                            // Get link
                            www = new WWW("http://api.yucibot.nl/user/pf/" + name);
                            while (!www.isDone) yield return 0;
                            string link = www.text;
                            if (link != "")
                            {
                                // Convert link to tex
                                tex = new Texture2D(128, 128, TextureFormat.RGB24, false);
                                www = new WWW(link);
                                while (!www.isDone) yield return 0;
                                www.LoadImageIntoTexture(tex);
                                tex.name = name;
                            }
                        }
                        break;
                    case scr_InputManager.SpriteSources.Both:
                        {
                            // LOCAL
                            // Get file
                            FileInfo file = m_SpriteFiles.Find(x => x.Name.Substring(0, x.Name.IndexOf('.')).ToLower() == name);
                            if (file != null)
                            {
                                // Convert file to tex
                                tex = new Texture2D(128, 128, TextureFormat.RGB24, false);
                                byte[] imageData = File.ReadAllBytes(file.FullName);
                                tex.LoadImage(imageData);
                            }
                            // TWITCH
                            else
                            {
                                www = new WWW("http://api.yucibot.nl/user/pf/" + name);
                                while (!www.isDone) yield return 0;
                                string link = www.text;
                                if (link != "")
                                {
                                    // Convert link to tex
                                    tex = new Texture2D(128, 128, TextureFormat.RGB24, false);
                                    www = new WWW(link);
                                    while (!www.isDone) yield return 0;
                                    www.LoadImageIntoTexture(tex);
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }

                // Set Texture name
                if (tex) tex.name = name;

                CreateMarble(name, tex);
            }

            // Delay
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void CreateMarble(string name, Texture2D tex)
    {
        // Create Marble
        var marble = m_CreateMarbleScript.CreateMarble(name, tex);

        // Add to list
        m_Userlist.Add(marble);

        // Add to Leaderboard
        scr_Leaderboard.AddUserToLeaderboard(marble);

        // Add to timer
        m_JoinedUsersTime.Add(name, 6.0f);
    }

    void UpdateCenterText()
    {
        // Adding Users
        if (!scr_RaceInput.IsClosed || m_JoinedUsersTime.Count > 0)
        {
            for (int i = 0; i < m_JoinedUsersTime.Count; ++i)
            {
                var key = m_JoinedUsersTime.ElementAt(i).Key;
                // Update time
                m_JoinedUsersTime[key] -= Time.deltaTime;
                // Remove when out of time
                if (m_JoinedUsersTime[key] <= 0.0f)
                {
                    m_JoinedUsersTime.Remove(key);
                    --i;
                }
            }

            // Sort by time
            var list = m_JoinedUsersTime.ToList();
            list.Sort((x, y) => x.Value.CompareTo(y.Value));

            // Display in CenterText
            m_CenterText.text = "<color=#E5B456FF><b>Loading Users...\n\n</b></color>";
            foreach (var user in list)
            {
                string name = user.Key;
                string hexColor = scr_Leaderboard.GetHexColor(name);
                if (hexColor == null) hexColor = "FFFFFF50";
                m_CenterText.text += "<color=#" + hexColor + ">" + name + "</color> has joined!\n";
            }
        }
        // No new users and closed
        else if (scr_RaceInput.IsClosed && m_JoinedUsersTime.Count == 0 && m_Userlist.Count > 0 && m_LoadedMessageTime > 0.0f)
        {
            m_CenterText.text = "<color=#00FF00><b>Users Loaded!</b></color>";
            m_LoadedMessageTime -= Time.deltaTime;
        }
        // Clear
        else if (m_LoadedMessageTime <= 0.0f)
        {
            m_CenterText.text = "<color=#101010><b>Press <i>" + scr_InputManager.KeyCodeToString(scr_InputManager.Key_Start) + "</i> to Start!</b></color>";
            m_LoadedMessageTime = -1.0f;
            scr_RaceInput.IsDoneLoading = true;
        }
    }

    public static int GetMarbleCount()
    {
        return m_Userlist.Count;
    }
}
