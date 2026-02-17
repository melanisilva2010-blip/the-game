using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VolverMenuGameOver : MonoBehaviour
{
    public void VolveraJugar()
    {
        SceneManager.LoadScene(0);
    }
}
