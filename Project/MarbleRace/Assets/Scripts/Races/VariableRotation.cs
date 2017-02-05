using UnityEngine;
using System.Collections;

public class VariableRotation : MonoBehaviour {

    public float SpeedMin = 1;
    public float SpeedMax = 10;
    public float Speed = 5;
    public string Axis = "Y";

    public bool RandomDelay = false;
    public float DelayMin = 1;
    public float DelayMax = 5;
    public float Delay = 2;

    private float m_AccuTime;
    private Transform m_T;

    // Use this for initialization
    void Start () {
        Speed = Random.Range(SpeedMin, SpeedMax);
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

        // Delay == -1 -> Speed only set once
        if (Delay != -1)
        {
            m_AccuTime += Time.deltaTime;
            if (m_AccuTime >= Delay)
            {
                // Set random speed once time has passed
                m_AccuTime -= Delay;
                Speed = Random.Range(SpeedMin, SpeedMax);

                // Set random delay if wanted
                if (RandomDelay)
                {
                    Delay = Random.Range(DelayMin, DelayMax);
                }
            }
        }

        m_T.Rotate(new Vector3(mulX * Speed, mulY * Speed, mulZ * Speed));
    }
}
