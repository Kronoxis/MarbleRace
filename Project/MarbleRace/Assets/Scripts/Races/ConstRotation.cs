using UnityEngine;
using System.Collections;

public class ConstRotation : MonoBehaviour {

    public float Speed = 0;
    public string Axis = "Y";
    private Transform m_T;

	// Use this for initialization
	void Start () {
        if (Speed == 0)
        {
            while (Speed < 0.1f && Speed > -0.1f)
                Speed = Random.Range(-10.0f, 10.0f);
        }
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
        
        m_T.Rotate(new Vector3(mulX * Speed, mulY * Speed, mulZ * Speed));
    }
}
