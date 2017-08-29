using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Dropdown))]

public class scr_SelectRaceSync : MonoBehaviour {

    Dropdown m_Dropdown;

	// Use this for initialization
	void Start () {
        m_Dropdown = GetComponent<Dropdown>();
	}
	
	// Update is called once per frame
	void Update () {
        var options = m_Dropdown.options;

        // Amount of races = Amount of scenes without MainMenu, SelectRace
        int nrOfRaces = SceneManager.sceneCountInBuildSettings - 2;

        if (options.Count == nrOfRaces) return;

        List<string> raceNames = new List<string>();

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; ++i)
        {
            // Races list
        }

        m_Dropdown.ClearOptions();
        m_Dropdown.AddOptions(raceNames);
	}
}
