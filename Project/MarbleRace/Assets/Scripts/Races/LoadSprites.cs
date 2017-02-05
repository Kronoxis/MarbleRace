using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LoadSprites : MonoBehaviour {

    GlobalManager gManager;
    private List<FileInfo> m_Files = new List<FileInfo>();
    private List<Texture2D> m_Sprites = new List<Texture2D>();

    void Start()
    {
        // Get Directory
        gManager = GlobalManager.Instance();
        DirectoryInfo info = new DirectoryInfo(gManager.gSpritePath);
        // Get Files from Directory
        FileInfo[] files = info.GetFiles();

        foreach (FileInfo file in files)
        {
            // Remove meta files
            if (file.Extension != ".meta")
            {
                m_Files.Add(file);
                GetImageFromFile(file);
            }
        }
    }

    public Texture2D GetTex(string user)
    {
        Texture2D toRet = null;

        foreach (Texture2D sprite in m_Sprites)
        {
            // Get Name without extension
            if (sprite.name.ToLower() == user.ToLower())
            {
                toRet = sprite;
                break;
            }
        }
        return toRet;
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
        m_Sprites.Add(tex);
    }
}
