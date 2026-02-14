using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{

    public Image barraDeVida;
    private Personaje personaje;
    private float vidaMaxima = 100f;


    void Start()
    {
        personaje = GameObject.Find("Player").GetComponent<Personaje>();
        vidaMaxima = personaje.vida;
    }
    void Update()
    {
        barraDeVida.fillAmount = personaje.vida / vidaMaxima;
    }

   
    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");

    }

}
