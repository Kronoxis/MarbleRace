using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class scr_Controls : MonoBehaviour {

    GlobalManager gManager;

    void Start()
    {
        gManager = GlobalManager.Instance();
        SetInputs();
    }

    // Used in Settings
    public void SetInputs()
    {
        GameObject.Find("InputStart1").transform.Find("Text").GetComponent<Text>().text =      gManager.KeyCodeToString(gManager.gKeyStart1);
        GameObject.Find("InputStart2").transform.Find("Text").GetComponent<Text>().text =      gManager.KeyCodeToString(gManager.gKeyStart2);
        GameObject.Find("InputRight1").transform.Find("Text").GetComponent<Text>().text =      gManager.KeyCodeToString(gManager.gKeyRight1);
        GameObject.Find("InputRight2").transform.Find("Text").GetComponent<Text>().text =      gManager.KeyCodeToString(gManager.gKeyRight2);
        GameObject.Find("InputLeft1").transform.Find("Text").GetComponent<Text>().text =       gManager.KeyCodeToString(gManager.gKeyLeft1);
        GameObject.Find("InputLeft2").transform.Find("Text").GetComponent<Text>().text =       gManager.KeyCodeToString(gManager.gKeyLeft2);
        GameObject.Find("InputUp1").transform.Find("Text").GetComponent<Text>().text =         gManager.KeyCodeToString(gManager.gKeyUp1);
        GameObject.Find("InputUp2").transform.Find("Text").GetComponent<Text>().text =         gManager.KeyCodeToString(gManager.gKeyUp2);
        GameObject.Find("InputDown1").transform.Find("Text").GetComponent<Text>().text =       gManager.KeyCodeToString(gManager.gKeyDown1);
        GameObject.Find("InputDown2").transform.Find("Text").GetComponent<Text>().text =       gManager.KeyCodeToString(gManager.gKeyDown2);
        GameObject.Find("InputZoomIn1").transform.Find("Text").GetComponent<Text>().text =     gManager.KeyCodeToString(gManager.gKeyZoomIn1);
        GameObject.Find("InputZoomIn2").transform.Find("Text").GetComponent<Text>().text =     gManager.KeyCodeToString(gManager.gKeyZoomIn2);
        GameObject.Find("InputZoomOut1").transform.Find("Text").GetComponent<Text>().text =    gManager.KeyCodeToString(gManager.gKeyZoomOut1);
        GameObject.Find("InputZoomOut2").transform.Find("Text").GetComponent<Text>().text =    gManager.KeyCodeToString(gManager.gKeyZoomOut2);
    }

    public void SetKeys()
    {
        gManager.gKeyStart1 =      gManager.StringToKeyCode(GameObject.Find("InputStart1").transform.Find("Text").GetComponent<Text>().text);  
        gManager.gKeyStart2 =      gManager.StringToKeyCode(GameObject.Find("InputStart2").transform.Find("Text").GetComponent<Text>().text);  
        gManager.gKeyRight1 =      gManager.StringToKeyCode(GameObject.Find("InputRight1").transform.Find("Text").GetComponent<Text>().text);  
        gManager.gKeyRight2 =      gManager.StringToKeyCode(GameObject.Find("InputRight2").transform.Find("Text").GetComponent<Text>().text);  
        gManager.gKeyLeft1 =       gManager.StringToKeyCode(GameObject.Find("InputLeft1").transform.Find("Text").GetComponent<Text>().text);   
        gManager.gKeyLeft2 =       gManager.StringToKeyCode(GameObject.Find("InputLeft2").transform.Find("Text").GetComponent<Text>().text);   
        gManager.gKeyUp1 =         gManager.StringToKeyCode(GameObject.Find("InputUp1").transform.Find("Text").GetComponent<Text>().text);     
        gManager.gKeyUp2 =         gManager.StringToKeyCode(GameObject.Find("InputUp2").transform.Find("Text").GetComponent<Text>().text);     
        gManager.gKeyDown1 =       gManager.StringToKeyCode(GameObject.Find("InputDown1").transform.Find("Text").GetComponent<Text>().text);   
        gManager.gKeyDown2 =       gManager.StringToKeyCode(GameObject.Find("InputDown2").transform.Find("Text").GetComponent<Text>().text);   
        gManager.gKeyZoomIn1 =     gManager.StringToKeyCode(GameObject.Find("InputZoomIn1").transform.Find("Text").GetComponent<Text>().text); 
        gManager.gKeyZoomIn2 =     gManager.StringToKeyCode(GameObject.Find("InputZoomIn2").transform.Find("Text").GetComponent<Text>().text); 
        gManager.gKeyZoomOut1 =    gManager.StringToKeyCode(GameObject.Find("InputZoomOut1").transform.Find("Text").GetComponent<Text>().text);
        gManager.gKeyZoomOut2 =    gManager.StringToKeyCode(GameObject.Find("InputZoomOut2").transform.Find("Text").GetComponent<Text>().text);
    }

    public void SaveKeys()
    {
        SetKeys();
        // Add path if not there yet
        if (!System.IO.Directory.Exists(gManager.gInputPath))
            System.IO.Directory.CreateDirectory(gManager.gInputPath);

        StreamWriter sw = new StreamWriter(gManager.gInputPath + "Controls.txt");
        sw.WriteLine("Start1: " + gManager.KeyCodeToString(gManager.gKeyStart1));
        sw.WriteLine("Start1: " + gManager.KeyCodeToString(gManager.gKeyStart2));
        sw.WriteLine("Right1: " + gManager.KeyCodeToString(gManager.gKeyRight1));
        sw.WriteLine("Right2: " + gManager.KeyCodeToString(gManager.gKeyRight2));
        sw.WriteLine("Left1: " + gManager.KeyCodeToString(gManager.gKeyLeft1));
        sw.WriteLine("Left2: " + gManager.KeyCodeToString(gManager.gKeyLeft2));
        sw.WriteLine("Up1: " + gManager.KeyCodeToString(gManager.gKeyUp1));
        sw.WriteLine("Up2: " + gManager.KeyCodeToString(gManager.gKeyUp2));
        sw.WriteLine("Down1: " + gManager.KeyCodeToString(gManager.gKeyDown1));
        sw.WriteLine("Down2: " + gManager.KeyCodeToString(gManager.gKeyDown2));
        sw.WriteLine("ZoomIn1: " + gManager.KeyCodeToString(gManager.gKeyZoomIn1));
        sw.WriteLine("ZoomIn2: " + gManager.KeyCodeToString(gManager.gKeyZoomIn2));
        sw.WriteLine("ZoomOut1: " + gManager.KeyCodeToString(gManager.gKeyZoomOut1));
        sw.WriteLine("ZoomOut2: " + gManager.KeyCodeToString(gManager.gKeyZoomOut2));
        sw.Close();
    }
}
