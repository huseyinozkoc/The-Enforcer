using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour {

    private float attackTime;
    private float startAttackTime;
    public Transform attackPos;
    public float attackRange;
    public LayerMask whatEnemy; // focus on colliders corresponding layer mask allows to ignore collider
    public int damage;
    public Text hitPoint;

	void Update ()
    {

        if (attackTime <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftShift))       
            {
                Collider2D[] enemiesDamage = Physics2D.OverlapCircleAll(attackPos.position,attackRange,whatEnemy);
                for(int i = 0; i < enemiesDamage.Length; i++)
                {
                    if (enemiesDamage[i].isTrigger)
                    {
                        enemiesDamage[i].GetComponent<Enemy>().takeDamage(damage);

                     

                    }
                  

                }
                
            }

            attackTime = startAttackTime;


        }
        else
        {
            attackTime -= startAttackTime;
        }

	}
     void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);

    }
}
