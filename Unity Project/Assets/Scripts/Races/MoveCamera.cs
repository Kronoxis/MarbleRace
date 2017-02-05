using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Camera))]

public class MoveCamera : MonoBehaviour {

    private Camera m_C;
    private Transform m_T;

    GlobalManager gManager;

    private float m_TimeKeyDown = 0.5f;

    // Use this for initialization
    void Start () {
        gManager = GlobalManager.Instance();

        m_C = GetComponent<Camera>();
        m_T = GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {

        bool isCorrectKeyDown = false;

        float speed = m_TimeKeyDown * Time.deltaTime * 5;

        // Move camera
        if (Input.GetKey(gManager.gKeyRight1) || Input.GetKey(gManager.gKeyRight2))
        {
            m_T.Translate(new Vector3(speed, 0.0f, 0.0f));
            m_TimeKeyDown += Time.deltaTime;
            isCorrectKeyDown = true;
        }
        if (Input.GetKey(gManager.gKeyLeft1) || Input.GetKey(gManager.gKeyLeft2))
        {
            m_T.Translate(new Vector3(-speed, 0.0f, 0.0f));
            m_TimeKeyDown += Time.deltaTime;
            isCorrectKeyDown = true;
        }

        if (Input.GetKey(gManager.gKeyUp1) || Input.GetKey(gManager.gKeyUp2))
        {
            m_T.Translate(new Vector3(0.0f, speed, 0.0f));
            m_TimeKeyDown += Time.deltaTime;
            isCorrectKeyDown = true;
        }
        if (Input.GetKey(gManager.gKeyDown1) || Input.GetKey(gManager.gKeyDown2))
        {
            m_T.Translate(new Vector3(0.0f, -speed, 0.0f));
            m_TimeKeyDown += Time.deltaTime;
            isCorrectKeyDown = true;
        }

        if (Input.GetKey(gManager.gKeyZoomIn1) || Input.GetKey(gManager.gKeyZoomIn2))
        {
            m_C.orthographicSize = m_C.orthographicSize - Time.deltaTime * 10;
            m_TimeKeyDown += Time.deltaTime;
            isCorrectKeyDown = true;
        }
        if (Input.GetKey(gManager.gKeyZoomOut1) || Input.GetKey(gManager.gKeyZoomOut2))
        {
            m_C.orthographicSize = m_C.orthographicSize + Time.deltaTime * 10;
            m_TimeKeyDown += Time.deltaTime;
            isCorrectKeyDown = true;
        }

        // Reset speed when keys are released
        if (!isCorrectKeyDown)
            m_TimeKeyDown = 0.5f;

        // Limit speed
        if (m_TimeKeyDown >= 3.0f)
            m_TimeKeyDown = 3.0f;

        // Prevent Camera going through course
        if (m_C.orthographicSize < 0.5f)
            m_C.orthographicSize = 0.5f;
	}
}
