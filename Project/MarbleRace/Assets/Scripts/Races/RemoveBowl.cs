using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Collider))]

public class RemoveBowl : MonoBehaviour {

    private Renderer m_R;
    private Collider m_C;
    private bool isStarted = false;

    GlobalManager gManager;

    // Use this for initialization
    void Start () {
        gManager = GlobalManager.Instance();

        m_R = GetComponent<Renderer>();
        m_C = GetComponent<Collider>();
    }

    void Update ()
    {
        if (!isStarted && gManager.gIsGiveawayClosed)
            StartCoroutine(Remove());
    }

    // Update is called once per frame
    IEnumerator Remove()
    {
        if (Input.GetKeyDown(gManager.gKeyStart1) || Input.GetKeyDown(gManager.gKeyStart2))
        {
            isStarted = true;
            m_R.material.color = Color.red;
            yield return new WaitForSeconds(1);
            m_R.material.color = Color.yellow;
            yield return new WaitForSeconds(1);
            m_R.material.color = Color.green;
            yield return new WaitForSeconds(1);
            m_R.enabled = false;
            m_C.enabled = false;
        }
        yield return 0;
    }
}
