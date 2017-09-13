using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]

public class scr_FreeCamera : MonoBehaviour {

    Vector3 m_StartPosition;
    Camera m_Cam;

	// Use this for initialization
	void Start () {
        m_Cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        // First frame of mouse button down: Set begin position
        if (Input.GetKeyDown(scr_InputManager.Key_Move))
        {
            m_StartPosition = Input.mousePosition;
        }

        // Move Camera
		if (Input.GetKey(scr_InputManager.Key_Move))
        {
            var mousePos = Input.mousePosition;
            float deltaX = mousePos.x - m_StartPosition.x;
            float ndcdX = deltaX / Screen.width;
            float deltaY = mousePos.y - m_StartPosition.y;
            float ndcdY = deltaY / Screen.height;

            transform.Translate(
                ndcdX * scr_InputManager.MoveSpeed * m_Cam.orthographicSize / 5.0f, 
                ndcdY * scr_InputManager.MoveSpeed * m_Cam.orthographicSize / 5.0f, 
                0);
        }

        // Reset Position to Spawn with MMB
        if (Input.GetKeyUp(scr_InputManager.Key_Recenter))
        {
            Vector3 spawn = scr_RaceInput.GetSpawn();
            spawn.z = -10;
            transform.position = spawn;
        }

        // Zoom Camera with Scroll Wheel
        m_Cam.orthographicSize -= Input.mouseScrollDelta.y / 2.0f * scr_InputManager.ZoomSpeed * m_Cam.orthographicSize / 5.0f;

        // Zoom Camera with Keys
        if (Input.GetKey(scr_InputManager.Key_ZoomIn))
        {
            m_Cam.orthographicSize *= 1.0f - 0.05f * scr_InputManager.ZoomSpeed;
        }
        else if (Input.GetKey(scr_InputManager.Key_ZoomOut))
        {
            m_Cam.orthographicSize *= 1.0f + 0.05f * scr_InputManager.ZoomSpeed;
        }

        // Limit Zoom
        m_Cam.orthographicSize = Mathf.Clamp(m_Cam.orthographicSize, 1, 1000);
    }
}
