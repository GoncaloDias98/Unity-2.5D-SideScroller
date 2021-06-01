using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sawblade : MonoBehaviour{
    public float speed = 300;



    // Update is called once per frame
    void Update(){
        transform.Rotate(Vector3.forward * speed * Time.deltaTime,Space.World);
    }

    void OnTriggerEnter(Collider c){
        if(c.tag == "Player"){
            c.GetComponent<enemies>().TakeDamage(1);
        }
    }
}
