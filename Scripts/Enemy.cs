using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR  
using UnityEditor;
#endif

public class Enemy : MonoBehaviour
{

    public int health;
    public Animator anim;
    public float speed = 0.1f;
    Vector3 plpos;
    Vector3 mypos;
    public GameObject pl;
    int damage = 5;
    public float Movespeed = 2;
    public float startStun;
   

    void Awake()
    {
        anim = GetComponent<Animator>();

    }
    void Start()
    {

    }

    void Update()
    {
        

      
        plpos = pl.transform.position;
        mypos = transform.position;
        float a = Vector3.Distance(plpos, mypos);

        if (a < 20f)
        {
            transform.Translate((plpos - mypos).normalized * Movespeed * Time.deltaTime);
        }
       
       
        if ((plpos - mypos).x < 0)
        {
            transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
        }
        else
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }

        if (health <= 0)
        {
            anim.SetTrigger("die");
          
            Destroy(gameObject, 1.2f);

        }

    }

    public void takeDamage(int damage)
    {
        damage = Player.hardAttack;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(5f, 2f), ForceMode2D.Impulse);
        health -= damage;


    }









}