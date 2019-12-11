using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

    public class Player : MonoBehaviour
{

    #region integers
    public static int speed = 5;
    public static int charHeal = 10;
    public static int hardAttack = 5;
    public int totalHealth = 0;
    public int totalSpeed = 0;
    public int totalAttack = 0;
    int levelCounter;
    #endregion
    #region bools
    private bool attack;
    private bool dash;
    private bool isJumped;
    private bool perspective;
    bool isDash = true;
    public static bool paused = false;
    #endregion
    #region Texts
    public Text playerHealth;
    public Text textAttack;
    public Text speedText;
    #endregion
    #region others
     private Animator myAnim;
     public GameObject myCamera;
     Vector3 myPosition;
     Vector3 lastPosition;    
     Rigidbody2D rb;

    #endregion
    #region AudioSources
    public AudioSource src;
    public AudioSource jumpSrc;
    public AudioSource dashSrc;
    public AudioSource boostSrc;
    public AudioSource harmSrc;
    public AudioSource attackGemSrc;
    public AudioSource speedGemSrc;
    public AudioSource healthGemSrc;
    #endregion
    #region panelVar
    public Button resume;
    public Button menu;
    public GameObject onPlayPanel;
   

    #endregion
    void Awake()
    {
       //PlayerPrefs.DeleteAll();
      
    
        rb = GetComponent<Rigidbody2D>();
        perspective = true;    
        myAnim = GetComponent<Animator>();
        myCamera=GameObject.FindGameObjectWithTag("MainCamera");
        PlayerPrefs.GetInt("totalHealth", charHeal);
        PlayerPrefs.GetInt("totalSpeed ", speed);
        PlayerPrefs.GetInt("totalAttack", hardAttack);

       

    }

    void Start()
    {
        playerHealth.text = " Health  " + charHeal;
        textAttack.text = " Hard Attack " + hardAttack;
        speedText.text = " Speed " + GetComponent<PlayerAttack>().damage;
        myPosition =myCamera.transform.position - transform.position;
        
     
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        movement(horizontal);
        turnBack(horizontal);
    
      

    }
    void Update()
    {
        paused = false;
       


        CameraSetting();   
        InputIssue();
        Canvas();
        PlayerPrefs.SetInt("totalHealth", charHeal);
        PlayerPrefs.SetInt("totalSpeed", speed);
        PlayerPrefs.SetInt("totalAttack", hardAttack);

        Died();

    }

    IEnumerator died()
    {
        yield return new WaitForSeconds(3);
      
        Destroy(gameObject);
        SceneManager.LoadScene(0);

    }

    public void Died()
    {
        if (charHeal <= 0)
        {

            charHeal = 0;
            myAnim.SetBool("die", true);
            StartCoroutine(died());
            Debug.Log("Died");
          
           

        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        paused = false;
        onPlayPanel.SetActive(false);
    }

    public void Stop()
    {
        Time.timeScale = 0f;
        paused = true;
    }

    public void MenuButton()
    {
        SceneManager.LoadScene(0);
    }


    private void CameraSetting(){
        myCamera.transform.position = new Vector3(transform.position.x,-0.7f,-10 );
}

     public void movement(float horizontal)
    {

        rb.velocity = new Vector2(horizontal*speed, rb.velocity.y);

        myAnim.SetFloat("speed", Mathf.Abs(horizontal));   
       
     }

    private void turnBack(float horizontal)
    {
        if (horizontal > 0 && !perspective || horizontal < 0 && perspective)
        {
            perspective = !perspective;           
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

        }
    }
    
    private void InputIssue()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (References.level == 0)
            {
                myAnim.SetTrigger("attack");
                src.Play();
               
            }
            else
            {
                myAnim.SetTrigger("attackLevel");
                src.Play();
            }
           

          

        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onPlayPanel.SetActive(true);
            if (paused)
            {
              
                Resume();
            

            }
            else
            {
              
                Stop();
            }
          
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashSrc.Play();
            myAnim.SetTrigger("dash");
            GetComponent<BoxCollider2D>().size = new Vector2(1, 0.5f);
            GetComponent<BoxCollider2D>().offset = new Vector2(0, -1);

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            GetComponent<BoxCollider2D>().size = new Vector2(1, 1.4f);
            GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.5f);
        }
       
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isJumped)
            {
                jumpSrc.Play();
               
                rb.AddForce(new Vector2(0, 370));

                isJumped = true;
            }
            isJumped = false;
        }
       
    }


    private void OnCollisionEnter2D()
    {
        isJumped = true;
    }

    private void OnTriggerEnter2D(Collider2D col){

        if(col.gameObject.tag=="healGem")
        {
            healthGemSrc.Play();
            col.GetComponent<PolygonCollider2D>().enabled=false;
             Destroy(col.gameObject,0.1f);
             charHeal+=2;
           

        }
        if (col.gameObject.tag == "bullet")
        {
            harmSrc.Play();
            col.GetComponent<CircleCollider2D>().enabled = false;
            Destroy(col.gameObject, 0.1f);
            charHeal -= 1;     
            if (charHeal <= 0)
            {
                charHeal = 0;
            }
        }

        if(col.gameObject.tag=="speedGem"){
            col.GetComponent<PolygonCollider2D>().enabled=false;
            Destroy(col.gameObject,0.1f);
            speed+=1;
          
        }

        if(col.gameObject.tag=="attackGem"){
            attackGemSrc.Play();
            col.gameObject.GetComponent<PolygonCollider2D>().enabled=false;
            Destroy(col.gameObject,0.1f);
            hardAttack += 2;

            GetComponent<PlayerAttack>().damage += 2;
          
    }
        if (col.gameObject.tag == "stair")
        {
            //saveSpeed = speed;
            //speedText.text = " " + saveSpeed;
            speed = 5;

           
        }

        

        if (col.gameObject.tag == "finish")
        {
            References.level++;

            myAnim.SetBool("win", true);
            StartCoroutine(goNextLevel());
           
           
        }

      

    }

    IEnumerator goNextLevel()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);          // buildteki diğer scene e gecirmek icin..
       
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "shooterEnemy")
        {
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(collision.gameObject, 0.1f);
        }

        if (collision.gameObject.tag == "jumpBoost")
        {
            boostSrc.Play();
            rb.AddForce(new Vector2(0,2f), ForceMode2D.Impulse);     // 2-3 f ideal gibi duruyor.
        }
    }

    void Canvas()
    {

        playerHealth.text = "" + charHeal;
        textAttack.text = "" + hardAttack;
        speedText.text = " " + speed;

    }












}





