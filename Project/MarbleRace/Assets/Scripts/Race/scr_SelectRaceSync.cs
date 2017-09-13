using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

[RequireComponent(typeof(Dropdown))]

public class scr_SelectRaceSync : MonoBehaviour {

    public Text Description;
    public Text Length;
    public Text Players;
    public Text Designer;

    Dropdown m_Dropdown;

    Dictionary<int, int> m_RaceBuildIndices = new Dictionary<int, int>();  // OptionNumber, BuildIndex
    Dictionary<int, string> m_RaceNames = new Dictionary<int, string>(); // OptionNumber, Name
    int m_SelectedRace = 0;

	// Use this for initialization
	void Start () {
        m_Dropdown = GetComponent<Dropdown>();

        // Set Dropdown values to Race names
        var raceNames = new List<string>();
        int optionNr = 0;
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; ++i)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string fullName = path.Substring(path.LastIndexOf('/') + 1);
            if (!fullName.Contains("Race_")) continue;
            string name = fullName.Substring(5, fullName.LastIndexOf('.') - 5);

            raceNames.Add(name);
            m_RaceBuildIndices.Add(optionNr, i);
            m_RaceNames.Add(optionNr, name);
            ++optionNr;
        }

        m_Dropdown.ClearOptions();
        m_Dropdown.AddOptions(raceNames);

        // Add Dropdown select event
        var dropdownEvent = new Dropdown.DropdownEvent();
        dropdownEvent.AddListener(DropdownCallback);
        m_Dropdown.onValueChanged = dropdownEvent;

        // Load Race Info
        LoadRaceInfo();

        // Show Race
        ShowRace();
    }

    void DropdownCallback(int i)
    {
        m_SelectedRace = i;
        LoadRaceInfo();
        ShowRace();
    }

    public void LoadSelectedScene()
    {
        // Set Canvas and Scripts of Selection Scene inactive
        GameObject.Find("SelectionCanvas").SetActive(false);
        GameObject.Find("SelectionScripts").SetActive(false);

        // Set RaceRequirements of Race Scene active
        var raceGOs = SceneManager.GetSceneByBuildIndex(m_RaceBuildIndices[m_SelectedRace]).GetRootGameObjects();
        foreach (var go in raceGOs)
        {
            if (go.name == "RaceRequirements")
            {
                go.SetActive(true);
                break;
            }
        }

        // Set Camera Position and Size
        Vector3 pos = scr_RaceInput.GetSpawn();
        pos.z = -12;
        Camera.main.transform.position = pos;
        Camera.main.orthographicSize = 5.5f;
    }

    void LoadRaceInfo()
    {
        var path = scr_InputManager.DataPath + "/races/" + m_RaceNames[m_SelectedRace] + ".txt";
        if (!File.Exists(path))
        {
            Description.text = "No info file found for this race.";
            Length.text = "";
            Players.text = "";
            Designer.text = "";
            return;
        }

        StreamReader reader = new StreamReader(path);
        Description.text = reader.ReadLine();
        Length.text = reader.ReadLine();
        Players.text = reader.ReadLine();
        Designer.text = reader.ReadLine();
        reader.Close();
    }

    void ShowRace()
    {
        for (int i = 1; i < SceneManager.sceneCount; ++i)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i).buildIndex);
        }

        SceneManager.LoadSceneAsync(m_RaceBuildIndices[m_SelectedRace], LoadSceneMode.Additive);
    }
}
