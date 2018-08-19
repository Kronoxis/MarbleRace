using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_RaceInput : MonoBehaviour
{
    public enum SpawnShapes
    {
        Circle,
        Rectangle
    }

    public static bool IsStarted = false;
    public static bool IsClosed = false;
    public static bool IsDoneLoading = false;
    public static bool RandomizeSpawn = true;
    public static SpawnShapes SpawnShape = SpawnShapes.Circle;
    public static Vector2 SpawnHalfSize = new Vector2(3, 3);
    public GameObject Instructions;

    static Vector3 m_Spawn = Vector3.zero;
    GameObject m_Cap = null;
    Text m_CenterText = null;

    int m_NumberOfTestMarbles = 0;

    // Use this for initialization
    void Start()
    {
        // Find Objects
        var obj = GameObject.FindGameObjectWithTag("Spawn");
        if (obj)
            m_Spawn = obj.transform.position;
        else
            Debug.Log("Could not find an object tagged as 'Spawn'. Default Spawn set at (0,0,0)");

        obj = GameObject.FindGameObjectWithTag("Cap");
        if (obj)
            m_Cap = obj;
        else
            Debug.Log("Could not find an object tagged as 'Cap'. You will not be able to open the spawn room.");

        //m_CenterText = GameObject.FindGameObjectWithTag("CenterText").GetComponent<Text>();

        // Show Instructions
        //Instructions.SetActive(true);

        // Set bools
        IsStarted = false;
        IsClosed = false;
        IsDoneLoading = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDoneLoading && IsClosed && !IsStarted && Input.GetKeyUp(scr_InputManager.Key_Start))
        {
            IsStarted = true;
            StartCoroutine(ReleaseMarbles(scr_InputManager.ReleaseTimer));
        }

        if (IsStarted && Instructions.activeInHierarchy)
        {
            Instructions.SetActive(false);
        }

        if (Input.GetKey(scr_InputManager.Key_AddTest))
        {
            ++m_NumberOfTestMarbles;
            GetComponent<scr_Users>().CreateMarble("test" + m_NumberOfTestMarbles, null);
        }

#if UNITY_EDITOR
        // DEBUG KEYS
        // TimeScale
        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            Time.timeScale *= 1.1f;
        }
        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            Time.timeScale *= 0.9f;
        }
        if (Input.GetKeyUp(KeyCode.KeypadMultiply))
        {
            Time.timeScale = 1.0f;
        }
#endif
    }

    IEnumerator ReleaseMarbles(float time)
    {
        float timer = time;
        m_CenterText.gameObject.SetActive(true);
        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            m_CenterText.text = "<size=50><b>\n\n" + ((int)timer + 1).ToString() + "</b></size>";
            m_CenterText.color = new Color(1, 1, 1, timer % 1);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        m_CenterText.gameObject.SetActive(false);
        m_Cap.SetActive(false);
    }

    public static Vector2 GetSpawnLocation()
    {
        if (!RandomizeSpawn)
            return m_Spawn;

        Vector2 p = m_Spawn;
        switch (SpawnShape)
        {
            case SpawnShapes.Circle:
                var a = Random.Range(0, Mathf.PI * 2);
                var r = Random.Range(0, SpawnHalfSize.x);
                p += new Vector2(
                    r * Mathf.Cos(a),
                    r * Mathf.Sin(a));
                break;
            case SpawnShapes.Rectangle:
                p += new Vector2(
                    Random.Range(-SpawnHalfSize.x, SpawnHalfSize.x),
                    Random.Range(-SpawnHalfSize.y, SpawnHalfSize.y));
                break;
            default:
                break;
        }

        return p;
    }

    public static Vector2 GetSpawn()
    {
        return m_Spawn;
    }
}
