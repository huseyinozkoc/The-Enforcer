using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class References : MonoBehaviour {

    public static int level;
    
	void Start ()
    {
        if (SceneManager.GetActiveScene().buildIndex > PlayerPrefs.GetInt("levelCounter"))
        {
            PlayerPrefs.SetInt("levelCounter", SceneManager.GetActiveScene().buildIndex);
        }
    }


    void Update () {
		
	}
}
