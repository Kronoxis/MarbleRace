using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_CreateMarble : MonoBehaviour {

    public GameObject Marble;
    public Transform Parent;

    public GameObject CreateMarble(string name, Texture2D tex)
    {
        // Create Marble Instance
        GameObject m = Instantiate(Marble, Parent);
        // Set Name
        m.name = name;
        // Set Position
        m.transform.position = scr_RaceInput.GetSpawnLocation();
        // Set Sprite
        if (tex) m.GetComponent<Renderer>().material.mainTexture = tex;
        // Set Color
        else m.GetComponent<Renderer>().material.color = Random.ColorHSV(0, 1, 0.3f, 1, 0.3f, 0.9f);

        return m;
    }
}
