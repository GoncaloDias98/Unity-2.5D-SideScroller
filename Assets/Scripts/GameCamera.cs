using System.Collections;
using UnityEngine;

public class GameCamera : MonoBehaviour{
  
    private Transform target;
    private float trackSpeed = 10;

    public void SetTarget(Transform t){
        target = t;
        transform.position = new Vector3(t.position.x, t.position.y, transform.position.z);
    }

    void LateUpdate() {
        if(target){
            float x =IncrementTowards(transform.position.x, target.position.x, trackSpeed);
            float y =IncrementTowards(transform.position.y, target.position.y, trackSpeed);
            transform.position = new Vector3(x,y,transform.position.z);
        }
    }

private float IncrementTowards(float n, float target, float a){
        if(n == target){ //se o speed for igual ao target, retorna o speed
            return n;
        }else{
            float dir = Mathf.Sign(target - n); //
            n += a * Time.deltaTime * dir;
            return (dir == Mathf.Sign(target-n))? n : target; // se o speed passar o target entao retorna o target senao retorna o speed
        }

    }

}
