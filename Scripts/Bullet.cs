using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    ShooterEnemy shooterEnemy;
    Rigidbody2D rb;
	void Start ()
    {
        shooterEnemy = GameObject.FindGameObjectWithTag("shooterEnemy").GetComponent<ShooterEnemy>();
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(shooterEnemy.getPos() * 1000);
        
	}
}
