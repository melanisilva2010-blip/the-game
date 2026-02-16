using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.VisualScripting;

public class Dialogos : MonoBehaviour
{
    [Header("Configuración del Diálogo")]
    [SerializeField, TextArea(3, 100)] private string[] _strLineasDialogo;
    [SerializeField] private GameObject _goPanelDialogo;
    [SerializeField] private TMP_Text _tmpTextoDialogo;

  

    [Header("Opciones Extra")]
    [Tooltip("Si está marcado, el diálogo solo saldrá la primera vez.")]
    [SerializeField] private bool _esDeUnSoloUso = true; // <--- ¡AQUÍ ESTÁ LA MAGIA!

    private bool _blnDialogoActivo;
    private int _intLineaActual;
    private bool _yaSeMostro = false; // Variable para recordar si ya salió

    void Start()
    {
        _goPanelDialogo.SetActive(false);
    }

    void Update()
    {
        if (_blnDialogoActivo && Input.GetKeyDown(KeyCode.F))
        {
            if (_tmpTextoDialogo.text == _strLineasDialogo[_intLineaActual])
            {
                SiguienteLineaDialogo();
            }
            else
            {
                StopAllCoroutines();
                _tmpTextoDialogo.text = _strLineasDialogo[_intLineaActual];
            }
        }
    }

    public void ComenzarDialogo()
    {
        Time.timeScale = 0f; // Pausa el juego
        _blnDialogoActivo = true;
        _goPanelDialogo.SetActive(true);

        _intLineaActual = 0;
        StartCoroutine(MostrarLineasDialogo());
    }

    public void SiguienteLineaDialogo()
    {
        if (_intLineaActual < _strLineasDialogo.Length - 1)
        {
            _intLineaActual++;
            StartCoroutine(MostrarLineasDialogo());
        }
        else
        {
            CerrarDialogo();
        }
    }

    public void CerrarDialogo()
    {
        Time.timeScale = 1f; // Reanuda el juego
        _blnDialogoActivo = false;
        _goPanelDialogo.SetActive(false);
        _tmpTextoDialogo.text = string.Empty;
        StopAllCoroutines();
    }

    IEnumerator MostrarLineasDialogo()
    {
        _tmpTextoDialogo.text = string.Empty;
        foreach (char ch in _strLineasDialogo[_intLineaActual])
        {
            _tmpTextoDialogo.text += ch;
            yield return new WaitForSecondsRealtime(0.05f); // Usa tiempo real para ignorar la pausa
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Personaje"))
        {
            // LÓGICA DE UN SOLO USO:
            // Si está marcado como "Un solo uso" Y ya se mostró antes... NO hacemos nada.
            if (_esDeUnSoloUso && _yaSeMostro)
            {
                return; // Se sale de la función aquí mismo
            }

            // Si llegamos aquí, es porque o se puede repetir, o es la primera vez
            _yaSeMostro = true; // Lo marcamos como visto
            ComenzarDialogo();
        }
    }
}