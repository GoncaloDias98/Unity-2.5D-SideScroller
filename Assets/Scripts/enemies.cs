using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemies : MonoBehaviour{

    public float health;
    public GameObject ragdoll;

    public void TakeDamage(float dmg){
        health -= dmg;

        if(health <= 0){
            Die();
        }
    }

    public void Die(){
        Ragdoll r = (Instantiate(ragdoll, transform.position, transform.rotation) as GameObject).GetComponent<Ragdoll>();
        r.CopyPose(transform);
        Destroy(this.gameObject);
    }

}
