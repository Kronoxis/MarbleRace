using UnityEngine;
using System.Collections;

public class RotatingLiftPad : MonoBehaviour {

    public float TimePerCycle = 20.0f;

    private int m_StepOffset = 0;
    private int m_CurrentStep = 0;

    private bool m_IsRotating = false;

    private const int STEPS = 14;
    private const float RADIUS = 0.75f;

    private float m_AccuTime;

    private Transform m_T;

    // Use this for initialization
    void Start() {
        m_T = GetComponent<Transform>();

        string nrInName = this.name.Substring(this.name.Length - 2);
        int offset = int.Parse(nrInName);
        m_StepOffset = offset;
        m_CurrentStep = m_StepOffset;
    }

    // Update is called once per frame
    void Update()
    {
        m_IsRotating = (m_CurrentStep == 0 || m_CurrentStep == 1 || m_CurrentStep == 7 || m_CurrentStep == 8);

        m_AccuTime += Time.fixedDeltaTime;
        if (m_AccuTime >= TimePerCycle / STEPS)
        {
            m_AccuTime -= TimePerCycle / STEPS;

            ++m_CurrentStep;
            m_CurrentStep %= STEPS;

        }

        float stepTime = TimePerCycle / STEPS;
        //float dist = RADIUS * Mathf.PI / 2;

        if (m_IsRotating)
        {
            m_T.Rotate(new Vector3(0.0f, 0.0f, 90.0f / stepTime) * Time.fixedDeltaTime);
        }
        else
        {
            if (m_T.localRotation.z >= 0.95)
            {
                m_T.localRotation = new Quaternion(0.0f, 0.0f, 1.0f, 0.0f);
            }
            else if (m_T.localRotation.z <= 0.05)
            {
                m_T.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
            }

            float d = RADIUS * Mathf.PI / 2;
            m_T.Translate(new Vector3(0.0f, (d / stepTime) * Time.fixedDeltaTime, 0.0f));
        }
    }
}
