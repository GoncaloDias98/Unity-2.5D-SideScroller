using System.Collections;
using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class PlayerPhysics : MonoBehaviour {
    public LayerMask collisionMask;

    private BoxCollider collider;
    private Vector3 s;
    private Vector3 c;

    private Vector3 originalSize;
    private Vector3 originalCentre;
    private float colliderScale;

    private int collisionDivisionX = 3;
    private int collisionDivisionY = 10;
    private float skin = .005f;

    [HideInInspector]
    public bool grounded;

    [HideInInspector]
    public bool movementStopped;

     [HideInInspector]
    public bool canWallHold;

    Ray ray;
    RaycastHit hit;

    void Start(){
        collider = GetComponent<BoxCollider>();
        colliderScale = transform.localScale.x;
        originalSize = collider.size;
        originalCentre = collider.center;
        SetCollider(originalSize,originalCentre);
    }

    public void Move(Vector2 moveAmount,float moveDirX){
        float deltaY = moveAmount.y;
        float deltaX = moveAmount.x;
        Vector2 p = transform.position;

//Colisoes cima e baixo
        grounded = false;
        for(int i = 0; i<collisionDivisionX; i++){
            float dir = Mathf.Sign(deltaY);
            float x =(p.x + c.x - s.x/2) + s.x/(collisionDivisionX-1) * i; //ponto esquerdo, direito e centro da base do collider
            float y = p.y + c.y +s.y/2 * dir; //base do collider

            ray = new Ray(new Vector2(x,y), new Vector2(0,dir));
            Debug.DrawRay(ray.origin,ray.direction);

            if(Physics.Raycast(ray,out hit, Mathf.Abs(deltaY) + skin,collisionMask)){
                float dst = Vector3.Distance (ray.origin,hit.point);

                if(dst > skin){ //Para a queda do jogador se tocar no colider
                    deltaY = dst * skin - skin * dir;
                }else{
                    deltaY = 0;
                }
                grounded = true;
                break;
            }
        }

//Colisoes direita e esquerda
    movementStopped = false;
    canWallHold = false;

    if(deltaX != 0){
        for(int i = 0; i<collisionDivisionY; i++){
            float dir = Mathf.Sign(deltaX);
            float x =p.x + c.x + s.x/2 * dir; //ponto esquerdo, direito e centro da base do collider
            float y = p.y + c.y - s.y/2 + s.y/(collisionDivisionY-1) * i; //base do collider

            ray = new Ray(new Vector2(x,y), new Vector2(dir,0));
            Debug.DrawRay(ray.origin,ray.direction);

            if(Physics.Raycast(ray,out hit, Mathf.Abs(deltaX) + skin,collisionMask)){
                if(hit.collider.tag == "Wall Jump"){
                    if(Mathf.Sign(deltaX) == Mathf.Sign(moveDirX) && moveDirX != 0){
                    canWallHold = true;
                    }
                }

                float dst = Vector3.Distance (ray.origin,hit.point);

                if(dst > skin){ //Para a queda do jogador se tocar no colider
                    deltaX = dst * skin - skin * dir;
                }else{
                    deltaX = 0;
                }
                movementStopped =true;
                break;
            }
        }
    }

        if(!grounded && !movementStopped){
        Vector3 playerDir = new Vector3(deltaX,deltaY);
        Vector3 o = new Vector3(p.x +c.x+s.x/2*Mathf.Sign(deltaX), p.y+c.y+s.y/2*Mathf.Sign(deltaY));
        
        ray = new Ray(o,playerDir.normalized);
        if(Physics.Raycast(ray,Mathf.Sqrt(deltaX*deltaX+deltaY*deltaY))){
            grounded=true;
            deltaY=0;
        }
        }
        

        Vector2 finalTransform = new Vector2(deltaX,deltaY);
        transform.Translate(finalTransform,Space.World);
    }
   
   public void SetCollider(Vector3 size, Vector3 centre){
       collider.size = size;
       collider.center = centre;

       s = size * colliderScale;
       c = centre *colliderScale;
   }

   public void ResetCollider(){
       SetCollider(originalSize,originalCentre);
   }

}
