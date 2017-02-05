using UnityEngine;
using System.Collections;

public class StartStopRotation : MonoBehaviour {

    public float Speed = 3;
    public float RotateTime = 5;
    public float StopTime = 5;
    public string Axis = "Y";

    private float m_AccuTime;
    private bool m_IsRotating = true;
    private Transform m_T;

    // Use this for initialization
    void Start () {
        m_T = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        int mulX = 0;
        int mulY = 0;
        int mulZ = 0;
        if (Axis.Contains("X") || Axis.Contains("x"))
            mulX = 1;
        if (Axis.Contains("Y" )|| Axis.Contains("y"))
            mulY = 1;
        if (Axis.Contains("Z") || Axis.Contains("z"))
            mulZ = 1;

        m_AccuTime += Time.deltaTime;

        if (m_IsRotating)
        {
            if (m_AccuTime > RotateTime)
            {
                m_AccuTime = 0.0f;
                m_IsRotating = false;
            }
        }
        else
        {
            if (m_AccuTime > StopTime)
            {
                m_AccuTime = 0.0f;
                m_IsRotating = true;
            }
        }

        if (m_IsRotating) m_T.Rotate(new Vector3(mulX * Speed, mulY * Speed, mulZ * Speed));
    }
}
