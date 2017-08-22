using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;

public class scr_UserManager : scr_Singleton<scr_UserManager> {

    WebClient m_Client = new WebClient();
    string m_Reply;
    static string[] m_Users;

	// Use this for initialization
	void Start ()
    {
        // Add DownloadFileCompleted method
        m_Client.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(DownloadFileCallback);

        // Start download loop
        StartCoroutine(DownloadUntilClosed());
    }

    IEnumerator DownloadUntilClosed()
    {
        // Download while Giveaway is not closed
        while (!scr_SettingsManager.g_IsClosed)
        {
            // Download again
            DownloadFile();
            // Delay
            yield return new WaitForSeconds(scr_SettingsManager.g_DownloadDelay);
        }
    }

    void DownloadFile()
    {
        Debug.Log("Started downloading from " + scr_SettingsManager.g_URL + " to " + scr_SettingsManager.g_SavePath + "EnteredUsers.txt");

        // Download from URL
        m_Client.DownloadFileAsync((new System.Uri(scr_SettingsManager.g_URL)), scr_SettingsManager.g_SavePath + "EnteredUsers.txt");
    }

    void DownloadFileCallback(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {
        // Read file
        m_Reply = GetUsersFromFile(scr_SettingsManager.g_SavePath + "EnteredUsers.txt");
        // Split users
        SplitUsers();
        // Check for Closed symbol ("|")
        scr_SettingsManager.g_IsClosed = m_Reply.Contains("|");
    }

    string GetUsersFromFile(string path)
    {
        // Read path as one long string
        StreamReader sr = new StreamReader(path);
        string data = sr.ReadToEnd();
        sr.Close();
        return data;
    }

    void SplitUsers()
    {
        // Split by character (' ')
        char[] splits = { ' ' };
        // Add non-empty entries to users
        m_Users = m_Reply.Split(splits, System.StringSplitOptions.RemoveEmptyEntries);

        var last = m_Users[m_Users.Length - 1];
        if (last.Contains("|")) last.Remove(last.Length - 1);
    }

    static public string[] GetUsers()
    {
        return m_Users;
    }
}
