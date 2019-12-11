using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class levelControl : MonoBehaviour {

    public Button play;
    public Button option;
    public Button back;
    public GameObject panel;

     void Start()
    {
        Player.charHeal = 10;
        Player.speed = 5;
        Player.hardAttack = 5;
        Time.timeScale = 1f; //oyun icindeyken menuye dönersek oyunu durdurup döndüğümüz icin yeniden play ile oyuna döndüğümüzde scale ı 0 alıyordu. 
    }


    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }
    public void OptionButton()
    {
        panel.gameObject.SetActive(true);
        play.gameObject.SetActive(false);
     
    }
    public void BackButton()
    {
        panel.gameObject.SetActive(false);
        play.gameObject.SetActive(true);
     
    }

   

}
