using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class scr_LoadConfig : MonoBehaviour {

    GlobalManager gManager;

    void Start()
    {
        gManager = GlobalManager.Instance();

        // Entered text
        GameObject.Find("InputInputPath").GetComponent<InputField>().text = gManager.gInputPath;
        GameObject.Find("InputGiveawayURL").GetComponent<InputField>().text = gManager.gGiveawayURL;
        GameObject.Find("InputSpritePath").GetComponent<InputField>().text = gManager.gSpritePath;
        GameObject.Find("InputSavePath").GetComponent<InputField>().text = gManager.gSavePath;

        GameObject.Find("InputSaveFormat").GetComponent<Dropdown>().value = (int)gManager.gFormat;

        GameObject.Find("InputDownloadUsersDelay").GetComponent<Slider>().value = gManager.gDownloadUsersDelay;
        GameObject.Find("InputDownloadUsersDelay").transform.parent.Find("SliderValue").GetComponent<InputField>().text = gManager.gDownloadUsersDelay.ToString();

        GameObject.Find("InputScoreboardUpdateDelay").GetComponent<Slider>().value = gManager.gScoreboardUpdateDelay;
        GameObject.Find("InputScoreboardUpdateDelay").transform.parent.Find("SliderValue").GetComponent<InputField>().text = gManager.gScoreboardUpdateDelay.ToString();
        
        GameObject.Find("InputAddMarbleDelay").GetComponent<Slider>().value = gManager.gAddMarbleDelay;
        GameObject.Find("InputAddMarbleDelay").transform.parent.Find("SliderValue").GetComponent<InputField>().text = gManager.gAddMarbleDelay.ToString();

        GameObject.Find("InputManualStart").GetComponent<Toggle>().isOn = gManager.gIsManualStart;
        GameObject.Find("InputStartDelay").GetComponent<Toggle>().isOn = gManager.gIsStartDelay;

        // Placeholder text
        GameObject.Find("InputInputPath").transform.Find("Placeholder").GetComponent<Text>().text = gManager.gInputPath;
        GameObject.Find("InputGiveawayURL").transform.Find("Placeholder").GetComponent<Text>().text = gManager.gGiveawayURL;
        GameObject.Find("InputSpritePath").transform.Find("Placeholder").GetComponent<Text>().text = gManager.gSpritePath;
        GameObject.Find("InputSavePath").transform.Find("Placeholder").GetComponent<Text>().text = gManager.gSavePath;
    }
}
