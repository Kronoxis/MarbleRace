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
        GameObject.Find("InputStart1").transform.FindChild("Text").GetComponent<Text>().text =      gManager.KeyCodeToString(gManager.gKeyStart1);
        GameObject.Find("InputStart2").transform.FindChild("Text").GetComponent<Text>().text =      gManager.KeyCodeToString(gManager.gKeyStart2);
        GameObject.Find("InputRight1").transform.FindChild("Text").GetComponent<Text>().text =      gManager.KeyCodeToString(gManager.gKeyRight1);
        GameObject.Find("InputRight2").transform.FindChild("Text").GetComponent<Text>().text =      gManager.KeyCodeToString(gManager.gKeyRight2);
        GameObject.Find("InputLeft1").transform.FindChild("Text").GetComponent<Text>().text =       gManager.KeyCodeToString(gManager.gKeyLeft1);
        GameObject.Find("InputLeft2").transform.FindChild("Text").GetComponent<Text>().text =       gManager.KeyCodeToString(gManager.gKeyLeft2);
        GameObject.Find("InputUp1").transform.FindChild("Text").GetComponent<Text>().text =         gManager.KeyCodeToString(gManager.gKeyUp1);
        GameObject.Find("InputUp2").transform.FindChild("Text").GetComponent<Text>().text =         gManager.KeyCodeToString(gManager.gKeyUp2);
        GameObject.Find("InputDown1").transform.FindChild("Text").GetComponent<Text>().text =       gManager.KeyCodeToString(gManager.gKeyDown1);
        GameObject.Find("InputDown2").transform.FindChild("Text").GetComponent<Text>().text =       gManager.KeyCodeToString(gManager.gKeyDown2);
        GameObject.Find("InputZoomIn1").transform.FindChild("Text").GetComponent<Text>().text =     gManager.KeyCodeToString(gManager.gKeyZoomIn1);
        GameObject.Find("InputZoomIn2").transform.FindChild("Text").GetComponent<Text>().text =     gManager.KeyCodeToString(gManager.gKeyZoomIn2);
        GameObject.Find("InputZoomOut1").transform.FindChild("Text").GetComponent<Text>().text =    gManager.KeyCodeToString(gManager.gKeyZoomOut1);
        GameObject.Find("InputZoomOut2").transform.FindChild("Text").GetComponent<Text>().text =    gManager.KeyCodeToString(gManager.gKeyZoomOut2);
    }

    public void SetKeys()
    {
        gManager.gKeyStart1 =      gManager.StringToKeyCode(GameObject.Find("InputStart1").transform.FindChild("Text").GetComponent<Text>().text);  
        gManager.gKeyStart2 =      gManager.StringToKeyCode(GameObject.Find("InputStart2").transform.FindChild("Text").GetComponent<Text>().text);  
        gManager.gKeyRight1 =      gManager.StringToKeyCode(GameObject.Find("InputRight1").transform.FindChild("Text").GetComponent<Text>().text);  
        gManager.gKeyRight2 =      gManager.StringToKeyCode(GameObject.Find("InputRight2").transform.FindChild("Text").GetComponent<Text>().text);  
        gManager.gKeyLeft1 =       gManager.StringToKeyCode(GameObject.Find("InputLeft1").transform.FindChild("Text").GetComponent<Text>().text);   
        gManager.gKeyLeft2 =       gManager.StringToKeyCode(GameObject.Find("InputLeft2").transform.FindChild("Text").GetComponent<Text>().text);   
        gManager.gKeyUp1 =         gManager.StringToKeyCode(GameObject.Find("InputUp1").transform.FindChild("Text").GetComponent<Text>().text);     
        gManager.gKeyUp2 =         gManager.StringToKeyCode(GameObject.Find("InputUp2").transform.FindChild("Text").GetComponent<Text>().text);     
        gManager.gKeyDown1 =       gManager.StringToKeyCode(GameObject.Find("InputDown1").transform.FindChild("Text").GetComponent<Text>().text);   
        gManager.gKeyDown2 =       gManager.StringToKeyCode(GameObject.Find("InputDown2").transform.FindChild("Text").GetComponent<Text>().text);   
        gManager.gKeyZoomIn1 =     gManager.StringToKeyCode(GameObject.Find("InputZoomIn1").transform.FindChild("Text").GetComponent<Text>().text); 
        gManager.gKeyZoomIn2 =     gManager.StringToKeyCode(GameObject.Find("InputZoomIn2").transform.FindChild("Text").GetComponent<Text>().text); 
        gManager.gKeyZoomOut1 =    gManager.StringToKeyCode(GameObject.Find("InputZoomOut1").transform.FindChild("Text").GetComponent<Text>().text);
        gManager.gKeyZoomOut2 =    gManager.StringToKeyCode(GameObject.Find("InputZoomOut2").transform.FindChild("Text").GetComponent<Text>().text);
    }

    public void SaveKeys()
    {
        SetKeys();
        // Add path if not there yet
        if (!System.IO.Directory.Exists(gManager.gInputPath))
            System.IO.Directory.CreateDirectory(gManager.gInputPath);

        StreamWriter sw = new StreamWriter(gManager.gInputPath + "Controls.txt");
        string[] inputs = { "Start", "Horizontal", "Vertical", "Zoom" };
        foreach (string input in inputs)
        {
            string line = GetLine(input);
            sw.WriteLine(line);
        }
        sw.Close();
    }

    string GetLine(string inputName)
    {
        string line = inputName + ": ";
        GameObject childPos = GameObject.Find(inputName).transform.FindChild("NamePos").gameObject;
        string inputStr = "Input";
        switch (inputName)
        {
            case "Start":
                inputStr += "Start";
                break;
            case "Horizontal":
                inputStr += "Right";
                break;
            case "Vertical":
                inputStr += "Up";
                break;
            case "Zoom":
                inputStr += "ZoomIn";
                break;
        }
        if (childPos.activeSelf)
        {
            line += childPos.transform.FindChild(inputStr + "1").FindChild("Text").GetComponent<Text>().text + ",";
            line += childPos.transform.FindChild(inputStr + "2").FindChild("Text").GetComponent<Text>().text + ",";
        }
        else
        {
            line += "null,null,";
        }

        GameObject childNeg = GameObject.Find(inputName).transform.FindChild("NameNeg").gameObject;
        inputStr = "Input";
        switch (inputName)
        {
            case "Start":
                inputStr += "Start";
                break;
            case "Horizontal":
                inputStr += "Left";
                break;
            case "Vertical":
                inputStr += "Down";
                break;
            case "Zoom":
                inputStr += "ZoomOut";
                break;
        }
        if (childNeg.activeSelf)
        {
            line += childNeg.transform.FindChild(inputStr + "1").FindChild("Text").GetComponent<Text>().text + ",";
            line += childNeg.transform.FindChild(inputStr + "2").FindChild("Text").GetComponent<Text>().text;
        }
        else
        {
            line += "null,null";
        }
        return line;
    }
}
