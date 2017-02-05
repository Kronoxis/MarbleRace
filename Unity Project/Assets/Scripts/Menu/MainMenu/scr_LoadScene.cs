using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class scr_LoadScene : MonoBehaviour {

    private int m_SceneNr = 1;

    GlobalManager gManager;
    void Start()
    {
        gManager = GlobalManager.Instance();
    }

    public void LoadAScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadARace()
    {
        string valueStr = null;
        if (m_SceneNr < 10)
            valueStr = "0";
        valueStr += m_SceneNr.ToString();
        try
        {
            SceneManager.LoadScene("Race_" + valueStr);
        }
        catch
        {
            Debug.Log("Scene " + "Race_" + valueStr + " not found!");
        }

        // Reset isClosed variable
        gManager.gIsGiveawayClosed = false;
    }

    public void SetSceneNr(int value)
    {
        m_SceneNr = value;
    }
}
