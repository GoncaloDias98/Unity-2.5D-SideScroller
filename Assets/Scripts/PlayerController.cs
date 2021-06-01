using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerPhysics))]

public class PlayerController : enemies{

    //Player
    public float gravity = 20;
    public float walkSpeed = 8;
    public float runSpeed = 12;
    public float acceleration = 30;
    public float jumpHeight = 12;
    public float slideDeceleration = 10;

    private float initiateSlideThreshold = 9;
//System
    private float animationSpeed;
    private float currentSpeed;
    private float targetSpeed;
    private Vector2 amountToMove;
    private float moveDirX;
//Estados
    private bool jumping;
    private bool sliding;
    private bool wallHolding;
    private bool stopSliding;

//Components
    private PlayerPhysics playerPhysics;
    private Animator animator;
    private GameManager manager;

    // Start is called before the first frame update
    void Start(){
        playerPhysics = GetComponent<PlayerPhysics>();
        animator = GetComponent<Animator>();
        manager = Camera.main.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update(){

        if(playerPhysics.movementStopped){
            targetSpeed = 0;
            currentSpeed = 0;
        }

        

        if(playerPhysics.grounded){
            amountToMove.y = 0;
            if (wallHolding){
                wallHolding = false;
                animator.SetBool("WallHold",false);
            }
            if(jumping){
                jumping = false;
                animator.SetBool("Jumping",false);
            }

            if(sliding){
                if(Mathf.Abs(currentSpeed) < .25f || stopSliding){
                    stopSliding = false;
                    sliding = false;
                    animator.SetBool("Sliding",false);
                    playerPhysics.ResetCollider();
                }
            }
//inputs
            if(Input.GetButtonDown("Jump")){
                amountToMove.y = jumpHeight;
                jumping = true;
                animator.SetBool("Jumping",true);
            }

            if(Input.GetButtonDown("Slide")){
                if(Mathf.Abs(currentSpeed)> initiateSlideThreshold){
                sliding = true;
                animator.SetBool("Sliding",true);
                targetSpeed=0;

                playerPhysics.SetCollider(new Vector3(0.96f,0.77f,1.49f), new Vector3(0,0.37f,0.16f));
                }
            }
        }else{
            if(!wallHolding){
                if(playerPhysics.canWallHold){
                    wallHolding = true;
                    animator.SetBool("WallHold",true);
                }
            }
        }

        if(Input.GetButtonDown("Jump")){
            if(sliding){
                stopSliding = true;
            } else if(playerPhysics.grounded || wallHolding){
                amountToMove.y = jumpHeight;
                jumping = true;
                animator.SetBool("Jumping",true);

                if(wallHolding){
                    wallHolding = false;
                    animator.SetBool("WallHold", false);
                }

                }
        }

       
    
    //Animator Parameters
        animationSpeed=IncrementTowards(animationSpeed,Mathf.Abs(targetSpeed),acceleration);
        animator.SetFloat("Speed",animationSpeed);

    //Inputs
    moveDirX = Input.GetAxisRaw("Horizontal");
        if(!sliding){
            float speed =(Input.GetButton("Run")? runSpeed : walkSpeed);
            targetSpeed = moveDirX * speed;
            currentSpeed = IncrementTowards(currentSpeed,targetSpeed,acceleration);

        //Para que lado vira
            if(moveDirX == 0){
                transform.eulerAngles=Vector3.up * -180 ;
            }else{
                transform.eulerAngles=(moveDirX >0)?Vector3.up * 90:Vector3.up * -90;
            }
        }else{
            currentSpeed = IncrementTowards(currentSpeed,targetSpeed,slideDeceleration);
        }
    
        amountToMove.x = currentSpeed;

        if(wallHolding){
            amountToMove.x = 0;
            if(Input.GetAxisRaw("Vertical") != -1){
                amountToMove.y = 0;
            }
        }

        amountToMove.y -= gravity * Time.deltaTime;
        playerPhysics.Move(amountToMove * Time.deltaTime, moveDirX);


        
    }

    void OnTriggerEnter(Collider c){
        if(c.tag =="Checkpoint"){
            manager.SetCheckpoint(c.transform.position);
        }
        if(c.tag =="Finish"){
            manager.EndLevel();
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
