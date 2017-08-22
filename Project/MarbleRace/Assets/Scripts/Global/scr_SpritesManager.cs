using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_SpritesManager : scr_Singleton<scr_SpritesManager> {

    static private List<Texture2D> g_SpritesList = new List<Texture2D>();

    static public void AddSprite(Texture2D tex)
    {
        // Add Texture to list
        g_SpritesList.Add(tex);
    }

    static public Texture2D GetSprite(string username)
    {
        Texture2D toRet = null;
        foreach (Texture2D sprite in g_SpritesList)
        {
            // Find Sprite with matching Username
            if (sprite.name.ToLower() == username.ToLower())
            {
                toRet = sprite;
                break;
            }
        }
        return toRet;
    }
}
