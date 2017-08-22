using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_CreateMarble : MonoBehaviour {

    public GameObject MarblePrefab;

	// Use this for initialization
	void Start () {
		
	}

    public GameObject CreateMarble(string username, Color color, Vector2 spawnPosition, Vector2 spawnRadii)
    {
        // Instantiate
        var marble = Instantiate(MarblePrefab);

        // Edit Scale
        float s = scr_SettingsManager.g_MarbleScale;
        marble.transform.localScale = new Vector3(s, s, s);

        // Spawn at spawnPosition with random offset
        marble.transform.position = new Vector3(
            spawnPosition.x + Random.Range(-spawnRadii.x, spawnRadii.x), 
            spawnPosition.y + Random.Range(-spawnRadii.y, spawnRadii.y), 
            0f);

        // Set Color / Texture
        Renderer ren = marble.GetComponent<Renderer>();
        Texture2D tex = scr_SpritesManager.GetSprite(username);
        if (tex != null)
            ren.material.mainTexture = tex;
        else
            ren.material.color = color;

        // Set Name
        marble.name = username;

        return marble;
    }
}
