using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject sphere, safeZone, LWall, RWall,stopTimeParticle;
    public int vidas = 3;
    public GameObject[] spikesTopBot;
    private Rigidbody2D rb;

    private Rigidbody rbBall;

    public SpikeSpawner left, right;
    private Animator pAnimator;

    public static bool leftWall;
    private bool startGame;
    private bool dead;
    public bool CanStopTime = true;
    private int chargeStopTime = 3;

    public static Player Instance;

    private float stopTime;
    private float size = 1f;
    private bool inmortal = false;

    private bool stoping;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        leftWall = false;
        rb = GetComponent<Rigidbody2D>();
        rbBall = transform.GetChild(0).GetComponent<Rigidbody>();
        pAnimator = sphere.GetComponent<Animator>();
        RWall.GetComponent<Animator>().SetTrigger("On");
    }

    void Update()
    {
        if (dead == false)
        {
            if (rb.velocity.x > 3 || rb.velocity.y > 10)
            {
                stoping = true;
            }

            if (stoping == true)
            {
                stopTime += Time.fixedDeltaTime;

                //Debug.Log(stopTime);

                if (stopTime > 0.25f)
                {
                    rb.velocity = new Vector2(Mathf.Abs(rb.velocity.x / 2.5f) * Mathf.Clamp(rb.velocity.x, -1, 1), Mathf.Abs(rb.velocity.y / 2.5f) * Mathf.Clamp(rb.velocity.y, -1, 1));
                    stopTime = 0;
                    stoping = false;
                }
            }

            else
            {
                stopTime = 0;
            }

            if (Input.GetMouseButtonDown(0))
            {
                //Si el jugador no ha empezado la partida, se le aplica gravedad al jugador, se le añade una fuerza lateral, una fuerza vertical para que el jugador no se caiga y se desactiva el panel del canvas de lobby.
                if (startGame == false)
                {
                    rb.gravityScale = 1;
                    rb.AddForce(Vector2.right * 3, ForceMode2D.Impulse);
                    rb.AddForce(Vector2.up * 4, ForceMode2D.Impulse);
                    GameManager.instance.StartGame();
                    startGame = true;
                }
                //Si no este simplemente agrega una fuerza hacia arriba, ya que este siempre tiene una velocidad constante lateral que se dio en el inicio, esta velocidad no se pierde ya que el jugador tiene un material con Bouncinnes en 1.
                //else
                //{
                //    rb.velocity = new Vector2(rb.velocity.x, 0);
                //    rb.AddForce(Vector2.up * 4, ForceMode2D.Impulse);
                //}
            }
            #region celularInput
            if (Input.touchCount == 1)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    //Si el jugador no ha empezado la partida, se le aplica gravedad al jugador, se le añade una fuerza lateral y se desactiva el panel del canvas de lobby.
                    if (startGame == false)
                    {
                        rb.gravityScale = 1;
                        rb.AddForce(Vector2.right * 3, ForceMode2D.Impulse);
                        rb.AddForce(Vector2.up * 4, ForceMode2D.Impulse);
                        GameManager.instance.StartGame();
                        startGame = true;
                    }
                    //Si no este simplemente agrega una fuerza hacia arriba, ya que este siempre tiene una velocidad constante lateral que se dio en el inicio, esta velocidad no se pierde ya que el jugador tiene un material con Bouncinnes en 1.
                    //else
                    //{
                    //    rb.velocity = new Vector2(rb.velocity.x, 0);
                    //    rb.AddForce(Vector2.up * 4, ForceMode2D.Impulse);
                    //}
                }
            }
            #endregion
        }
    }

    public void MoveSwipePlayer(Vector3 movement)
    {
       CanStopTime = false;
       chargeStopTime = 0;
        stopTimeParticle.GetComponent<Animator>().SetTrigger("Off");
             rb.velocity = new Vector2(rb.velocity.x, 0);
      
         float factor = 3f + ((1f- size)*2f);
        Vector3 impulse = (movement - transform.position) * factor;
        impulse =  new Vector3(Mathf.Clamp(impulse.x, -17f ,17f),Mathf.Clamp(impulse.y, -17f ,17f),Mathf.Clamp(impulse.z, -17f ,17f) ) ;
          
        rb.AddForce(impulse, ForceMode2D.Impulse);

        }
       
    

    public void MoveTapPlayer()
    {
        
        if(chargeStopTime < 3) {
            chargeStopTime ++;
             if(chargeStopTime == 3) 
             {
                 CanStopTime = true;
                 stopTimeParticle.GetComponent<Animator>().SetTrigger("On");
             }
             
        }
        float factor = 5f + ((1f- size)*2f);
        rb.velocity = new Vector2(factor * Mathf.Clamp(rb.velocity.x, -1, 1), 0);
        rb.AddForce(Vector2.up * factor, ForceMode2D.Impulse);
    }
    /// <summary>
    /// OnTriggerStay is called once per frame for every Collider other
    /// that is touching the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "SafeZone")
        {
            inmortal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.tag == "SafeZone")
        {
            inmortal = false;

            safeZone.transform.GetChild(0).gameObject.SetActive(false);
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), spikesTopBot[0].GetComponent<Collider2D>(), false);
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), spikesTopBot[1].GetComponent<Collider2D>(), false);

        print("notinmortal");
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
          rb.velocity = new Vector2((3f + ((1f- size)*2f)) * Mathf.Clamp(rb.velocity.x, -1, 1), 0);
        if (dead == false)
        {
            if (collision.collider.tag == "LeftWall" && !inmortal)
            {
                if (leftWall == true)
                {
                    GameManager.instance.AddPoint();
                    leftWall = !leftWall;
                    LWall.GetComponent<Animator>().SetTrigger("Off");
                    RWall.GetComponent<Animator>().SetTrigger("On");
                    rbBall.angularVelocity = Vector3.zero;
                    rbBall.AddTorque(new Vector3(0, 0, -360 * Mathf.Clamp(rb.velocity.x, -1, 1)));

                    if (leftWall == true)
                    {
                        right.DeactivateSpikes();
                        left.ActivateSpikes();

                    }
                    else
                    {
                        left.DeactivateSpikes();
                        right.ActivateSpikes();
                    }
                }
            }

            if (collision.collider.tag == "RightWall" && !inmortal)
            {
                if (leftWall == false)
                {
                    GameManager.instance.AddPoint();
                    leftWall = !leftWall;
                    RWall.GetComponent<Animator>().SetTrigger("Off");
                    LWall.GetComponent<Animator>().SetTrigger("On");
                    rbBall.angularVelocity = Vector3.zero;
                    rbBall.AddTorque(new Vector3(0, 0, -360 * Mathf.Clamp(rb.velocity.x, -1, 1)));
                  
                    if (leftWall == true)
                    {
                        right.DeactivateSpikes();
                        left.ActivateSpikes();

                    }
                    else
                    {
                        left.DeactivateSpikes();
                        right.ActivateSpikes();
                    }
                }
            }


            if (collision.collider.tag == "Spikes")
            {
                if (!inmortal)
                {

                   Crash();
                   
             if(collision.transform.parent.name == "Bot")
             {
                
                  rb.AddForce(Vector2.up * 5f, ForceMode2D.Impulse);
                  return;
             }
             if(collision.transform.parent.name == "Top")
             {
                 rb.AddForce(Vector2.down * 5f, ForceMode2D.Impulse);
                  return;
             }
                   GameManager.instance.ExplodeObstacle(collision.transform.position);
              collision.transform.parent.gameObject.SetActive(false);
            
              
                }



            }
            if (collision.collider.tag == "Obstacle")
            {
             Crash();
                GameManager.instance.ExplodeObstacle(collision.transform.position);
          collision.transform.parent.gameObject.SetActive(false);
       
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (dead == false && !inmortal)
        {
            if (collision.tag == "Spikes")
            {              
              Crash();
                 GameManager.instance.ExplodeObstacle(collision.transform.position);
              collision.transform.parent.gameObject.SetActive(false);
           
            }
        }

        if (collision.gameObject.tag == "SafeZone")
        {
            safeZone.transform.GetChild(0).gameObject.SetActive(true);
               Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), spikesTopBot[0].GetComponent<Collider2D>(), true);
                 Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), spikesTopBot[1].GetComponent<Collider2D>(), true);
            print("SafeZoneIn");
        }
        if (collision.gameObject.tag == "Obstacle")
        {
          Crash();
               GameManager.instance.ExplodeObstacle(collision.transform.position);
              collision.transform.parent.gameObject.SetActive(false);
            
           
        }
     

    }

    public void Crash()
    {
         
        vidas --;
        switch(vidas)
        {
            case 0 :
            Die();
            break;
            case 1 : 
            pAnimator.SetTrigger("Crash"); 
            StartCoroutine(Resize(0.5f));           
            break;
            case 2:
            pAnimator.SetTrigger("Crash"); 
              StartCoroutine(Resize(0.75f));
            break;
        }
    }
   IEnumerator Resize(float minSize) 
{  
    while(size >= minSize)
    {
         size -= 0.01f;
         sphere.transform.GetChild(0).localScale = Vector3.one * size  ;
          yield return new WaitForSeconds(.01f);
          StartCoroutine(Resize(minSize));
    }   
}

    public void Die ()
    {
       pAnimator.SetTrigger("Die");   
       rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY ;
       sphere.GetComponent<Rigidbody>().freezeRotation = true;
       GetComponent<BoxCollider2D>().enabled = false;
       sphere.transform.GetChild(0).gameObject.SetActive(false);
        GameManager.instance.Lose();
         dead = true;
    }

}
