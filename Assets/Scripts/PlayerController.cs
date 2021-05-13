using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerPhysics))]

public class PlayerController : MonoBehaviour{
    public float gravity = 20;
    public float walkSpeed = 8;
    public float runSpeed = 12;
    public float acceleration = 30;
    public float jumpHeight = 12;
    public float slideDeceleration = 10;

    private float animationSpeed;
    private float currentSpeed;
    private float targetSpeed;
    private Vector2 amountToMove;

    private bool jumping;
    private bool sliding;

    private PlayerPhysics playerPhysics;

    private Animator animator;

    // Start is called before the first frame update
    void Start(){
        playerPhysics = GetComponent<PlayerPhysics>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update(){

        if(playerPhysics.movementStopped){
            targetSpeed = 0;
            currentSpeed = 0;
        }

        if(playerPhysics.grounded){
            amountToMove.y = 0;

            if(jumping){
                jumping = false;
                animator.SetBool("Jumping",false);
            }

            if(sliding){
                if(Mathf.Abs(currentSpeed) < .25f){
                    sliding = false;
                    animator.SetBool("Sliding",false);
                    playerPhysics.ResetCollider();
                }
            }

            if(Input.GetButtonDown("Jump")){
                amountToMove.y = jumpHeight;
                jumping = true;
                animator.SetBool("Jumping",true);
            }

            if(Input.GetButtonDown("Slide")){
                sliding = true;
                animator.SetBool("Sliding",true);
                targetSpeed=0;

                playerPhysics.SetCollider(new Vector3(0.96f,0.77f,1.49f), new Vector3(0,0.37f,0.16f));
            }
        }

       
    
    //Animator Parameters
        animationSpeed=IncrementTowards(animationSpeed,Mathf.Abs(targetSpeed),acceleration);
        animator.SetFloat("Speed",animationSpeed);

    //Inputs
        if(!sliding){
            float speed =(Input.GetButton("Run")? runSpeed : walkSpeed);
            targetSpeed = Input.GetAxisRaw("Horizontal") * speed;
            currentSpeed = IncrementTowards(currentSpeed,targetSpeed,acceleration);
            float moveDir= Input.GetAxisRaw("Horizontal");
            if(moveDir == 0){
                transform.eulerAngles=Vector3.up * -180 ;
            }else{
                transform.eulerAngles=(moveDir >0)?Vector3.up * 90:Vector3.up * -90;
            }
        }else{
            currentSpeed = IncrementTowards(currentSpeed,targetSpeed,slideDeceleration);
        }
    
        amountToMove.x = currentSpeed;
        amountToMove.y -= gravity * Time.deltaTime;
        playerPhysics.Move(amountToMove * Time.deltaTime);


        
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
