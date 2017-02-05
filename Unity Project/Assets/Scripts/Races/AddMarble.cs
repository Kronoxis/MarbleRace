using UnityEngine;
using System.Collections;
using System.IO;

public class AddMarble : MonoBehaviour {

    public GameObject Marble;

    LoadSprites m_SpritesScript;

    void Start()
    {
        m_SpritesScript = GameObject.Find("ScriptManager").GetComponent<LoadSprites>();
    }

    public GameObject Create(string user, Color color)
    {
        GameObject marble = Instantiate(Marble);
        marble.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        marble.transform.position = new Vector3(-5.5f + Random.Range(-1.5f, 1.5f), 5f + Random.Range(-0.5f, 0.5f), 0f);

        Renderer ren = marble.GetComponent<Renderer>();
        Texture2D tex = m_SpritesScript.GetTex(user);
        if (tex != null)
        {
            Debug.Log("Added Texture instead of Color for user " + user);
            ren.material.mainTexture = tex;
        }
        else
        {
            ren.material.color = color;
        }

        marble.name = "M." + user;

        return marble;
    }
}
