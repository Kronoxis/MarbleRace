using UnityEngine;
using System.Collections;

public class scr_ManagerLoader : MonoBehaviour {

    void Start()
    {
        scr_SettingsManager.Instance();
        scr_SpritesManager.Instance();
        scr_UserManager.Instance();
    }
}
