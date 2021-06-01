using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider))]

public class BoxGuizmos : MonoBehaviour{

    public Color gizmoColour;

    void OnDrawGizmos() {
        Gizmos.color = gizmoColour;
        Gizmos.DrawCube(transform.position + GetComponent<BoxCollider>().center, GetComponent<BoxCollider>().size);
    }
   
}
