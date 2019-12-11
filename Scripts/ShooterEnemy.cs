using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR  
using UnityEditor;
#endif
public class ShooterEnemy : MonoBehaviour {
    public RaycastHit2D rc;
     GameObject character;
    public LayerMask layerMask;        //kendi colliderını ignorla

    public GameObject bullet;
    float fireTime;
    public Transform spawn;
    public int shooterHealth = 3;

    // collider verdiğimizde ray cizerken kendi colliderına carpıyor bu yüzden karaktere doğru ray cizemiyor layermask ile kendi colliderını ignorelucaz ...
    // bu enemy ye shooterEnemy diye bir layer tag atadık ve inspector dan shooterEnemy olan seceneği cıkardık.
    #region editorVariables
    GameObject[] waypoint;
    Vector3 distanceVec;
    bool distance = true;   // ileri veya geri ne zaman gideceğimizi kontrol et
    bool ismoved = true;
    int counter = 0;
    #endregion
    void Start ()

    {
        character = GameObject.Find("Player");
        #region dynamicIn
        waypoint = new GameObject[transform.childCount];

        for (int i = 0; i < waypoint.Length; i++)
        {
            waypoint[i] = transform.GetChild(0).gameObject;
            waypoint[i].transform.SetParent(transform.parent);
        }
        #endregion
    }


    void Update ()
    {
        SeeMe();
        if (rc.collider.tag=="player")         
        {
            Fire();
            Debug.Log("See Me");                  
        }
        else
        {
            Debug.Log("Dont see me");
        }

        landPoint();
    }

    void SeeMe()
    {
        Vector3 rayPos = character.transform.position - transform.position;           // aradaki mesafeyi hesapladık
         rc = Physics2D.Raycast(transform.position, rayPos, 300,layerMask);         // 1000 mesafe
        Debug.DrawLine(transform.position, rc.point, Color.red);                      // düsman beni görüyor mu diye cizgi cekerek kontrol ettirdik.
    }

    void Fire()
    {
        fireTime += Time.deltaTime;
        if(fireTime > 1f)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
            fireTime = 0;
 
        }
       
    }

    public Vector3 getPos()
    {
        return (character.transform.position - transform.position).normalized;
    }
    #region dynamicMovement
    void landPoint()
   
    {
        if (distance)
        {
            distanceVec = (waypoint[counter].transform.position - transform.position).normalized; //ilk gideceğimiz yeri bulduk.
            distance = false;
        }
        float dist = Vector3.Distance(transform.position, waypoint[counter].transform.position);  // gittiğimiz yerde yeni pozisyon aldık.
        transform.position += distanceVec * Time.deltaTime * 6;

        if (dist < 0.5f)
        {
            distance = true;
            if (counter == waypoint.Length - 1)
            {
                ismoved = false;
            }
            else if (counter == 0)
            {
                ismoved = true;
            }


            if (ismoved)
            {
                counter++;    // ulasacağımız son noktaya gelmediysek ileri gitmeye devam.
                distance = true;
            }
            else
            {
                counter--;  //buradan sonra geri gidecek
            }
        }


    }
  

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position, 1);
        }
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i + 1).transform.position);
        }
    }


#endif
#if UNITY_EDITOR
    [CustomEditor(typeof(ShooterEnemy))]
    [System.Serializable]
    class ShooterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            ShooterEnemy script = (ShooterEnemy)target;
            if (GUILayout.Button("ADD", GUILayout.MinWidth(100), GUILayout.Width(100)))
            {
                GameObject add = new GameObject();
                add.transform.parent = script.transform;
                add.transform.position = script.transform.position;
                add.name = script.transform.childCount.ToString();
            }
        }

    }
#endif
    #endregion



}
