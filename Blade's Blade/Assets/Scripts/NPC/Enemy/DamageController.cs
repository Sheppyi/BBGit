using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MonsterStats))]

public class DamageController : MonoBehaviour {

    public int health = 99;


	void Start () {
        health = this.GetComponent<MonsterStats>().baseHealth;
	}

    private void Update() {
        if (health <= 0) {
            Debug.Log(health);
            Destroy(this.gameObject);
        }
    }


    public bool TakeDamage(int damage) {
        Debug.Log("Damage of " + damage + " was taken");
        health -= damage;
        if (health <= 0) {
            return true;
        }
        else {
            return false;
        }
    }



}
