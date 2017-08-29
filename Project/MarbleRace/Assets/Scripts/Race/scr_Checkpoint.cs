using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Checkpoint : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Marble")
        {
            string name = other.name;
            int nr = int.Parse(gameObject.name.Substring(12, gameObject.name.Length - 13)); // Naming Convention: Checkpoint (n)
            scr_Leaderboard.UpdateCheckpoint(name, nr);
        }
    }
}
