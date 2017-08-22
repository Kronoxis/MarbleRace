using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class scr_LoadSprites : MonoBehaviour {

    private List<FileInfo> m_Files = new List<FileInfo>();

    void Start()
    {
        // Get Directory
        DirectoryInfo info = new DirectoryInfo(scr_SettingsManager.g_SpritePath);
        // Get Files from Directory
        FileInfo[] files = info.GetFiles();

        foreach (FileInfo file in files)
        {
            // Support jpeg and png images
            if (file.Extension == ".jpg" || file.Extension == ".jpeg" || file.Extension == ".png")
            {
                m_Files.Add(file);
                GetImageFromFile(file);
            }
        }
    }

    void GetImageFromFile(FileInfo file)
    {
        WWW www = new WWW("file://" + file.Directory + "\\" + file.Name);
        StartCoroutine(WaitForDownload(www, RemoveExtension(file)));
    }

    string RemoveExtension(FileInfo file)
    {
        string toRet = file.Name;
        string ext = file.Extension;
        toRet = toRet.Substring(0, toRet.Length - ext.Length);
        return toRet;
    }

    IEnumerator WaitForDownload(WWW www, string texName)
    {
        yield return www;
        Texture2D tex = new Texture2D(128, 128);
        www.LoadImageIntoTexture(tex);
        tex.name = texName;
        scr_SpritesManager.AddSprite(tex);
    }
}
