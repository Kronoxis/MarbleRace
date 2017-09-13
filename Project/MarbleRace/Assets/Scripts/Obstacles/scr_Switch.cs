using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Switch : MonoBehaviour {
    
    public GameObject ToHide;
    public GameObject ToReveal;
    public int NeededTouches = 1;
    public bool Unique = false;

    bool m_IsOpened = false;
    int m_TouchCount = 0;
    List<GameObject> m_TouchedUsers = new List<GameObject>();

    void Start()
    {
        ToHide.SetActive(true);
        ToReveal.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        // Ignore if Collider is not a Marble
        if (other.tag != "Marble") return;

        // Ignore same user if unique touches required
        if (Unique)
            if (m_TouchedUsers.Find(x => x.Equals(other.gameObject))) return;

        // Register touch
        ++m_TouchCount;
        if (Unique) m_TouchedUsers.Add(other.gameObject);

        // Ignore when already open
        if (m_IsOpened) return;

        // Open when Needed Touches is reached
        if (NeededTouches > scr_Users.GetMarbleCount() - 5) NeededTouches = scr_Users.GetMarbleCount() - 5;
        if (NeededTouches < 1) NeededTouches = 1;
        if (m_TouchCount < NeededTouches) return;
        ToHide.SetActive(false);
        ToReveal.SetActive(true);
        m_IsOpened = true;
    }
}
