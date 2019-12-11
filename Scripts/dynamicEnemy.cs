using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR  
using UnityEditor;
#endif

public class dynamicEnemy : MonoBehaviour {
    GameObject[] waypoint;
    Vector3 distanceVec;
    bool distance = true;   // ileri veya geri ne zaman gideceğimizi kontrol et
    bool ismoved = true;
    int counter = 0;

	void Start ()
    {
        waypoint = new GameObject[transform.childCount];	

        for(int i = 0; i < waypoint.Length; i++)
        {
            waypoint[i] = transform.GetChild(0).gameObject; 
            waypoint[i].transform.SetParent(transform.parent); 
        }
            
            
    }
	
	void FixedUpdate ()
    {
      
        landPoint();
	}


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
        for(int i = 0; i < transform.childCount; i++)
        {
            Gizmos.color = Color.red; 
            Gizmos.DrawWireSphere(transform.GetChild(i).transform.position,1); 
        }
        for (int i = 0; i < transform.childCount-1; i++)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.GetChild(i).transform.position, transform.GetChild(i + 1).transform.position); 
        }
    }


#endif

}




#if UNITY_EDITOR
[CustomEditor(typeof(dynamicEnemy))]
[System.Serializable]
class dynamicEnemyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        dynamicEnemy script = (dynamicEnemy)target;
        if (GUILayout.Button("ADD",GUILayout.MinWidth(100),GUILayout.Width(100))) 
        {
            GameObject add = new GameObject();  
            add.transform.parent = script.transform; 
            add.transform.position = script.transform.position;
            add.name = script.transform.childCount.ToString();
        }
    }
    
}
#endif