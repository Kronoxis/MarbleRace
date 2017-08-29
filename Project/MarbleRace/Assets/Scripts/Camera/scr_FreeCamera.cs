using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]

public class scr_FreeCamera : MonoBehaviour {

    public float MoveSpeed = 1;
    public float ZoomSpeed = 1;

    Vector3 m_StartPosition;
    Camera m_Cam;

	// Use this for initialization
	void Start () {
        m_Cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        // First frame of mouse button down: Set begin position
        if (Input.GetMouseButtonDown(1))
        {
            m_StartPosition = Input.mousePosition;
        }

		if (Input.GetMouseButton(1))
        {
            var mousePos = Input.mousePosition;
            float deltaX = mousePos.x - m_StartPosition.x;
            float ndcdX = deltaX / Screen.width;
            float deltaY = mousePos.y - m_StartPosition.y;
            float ndcdY = deltaY / Screen.height;

            transform.Translate(
                ndcdX * MoveSpeed * m_Cam.orthographicSize / 5.0f, 
                ndcdY * MoveSpeed * m_Cam.orthographicSize / 5.0f, 
                0);
        }

        if (Input.GetMouseButtonUp(2))
        {
            Vector3 spawn = scr_RaceInput.GetSpawn();
            spawn.z = -10;
            transform.position = spawn;
        }

        m_Cam.orthographicSize -= Input.mouseScrollDelta.y / 2.0f * ZoomSpeed * m_Cam.orthographicSize / 5.0f;
        m_Cam.orthographicSize = Mathf.Clamp(m_Cam.orthographicSize, 1, 1000);
    }
}
