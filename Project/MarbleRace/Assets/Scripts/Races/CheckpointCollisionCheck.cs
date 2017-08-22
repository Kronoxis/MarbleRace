using UnityEngine;
using System.Collections;

public class CheckpointCollisionCheck : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Marble")
        {
            string name = other.name;
            int nr = int.Parse(gameObject.name.Substring(10));
            GameObject.Find("Canvas").transform.Find("Panel").GetComponent<Leaderboard>().UpdateCheckpoint(name, nr);
        }
    }
}
