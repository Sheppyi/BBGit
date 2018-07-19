using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    GameObject[] player;

    private void Start() {
        player = GameObject.FindGameObjectsWithTag("Player");
        if (player.Length != 1) {
            Debug.LogWarning("More than one player in scene!");
        }

    }


    void Update () {
        transform.position = player[0].transform.position;
        transform.position += new Vector3(0, 1.5f, -10);
	}
}
