using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
// using Unity.VisualScripting; // No es estrictamente necesario, lo comento para limpiar

public class Dialogos : MonoBehaviour
{
    [Header("Configuración del Diálogo")]
    [SerializeField, TextArea(3, 100)] private string[] _strLineasDialogo;
    [SerializeField] private GameObject _goPanelDialogo;
    [SerializeField] private TMP_Text _tmpTextoDialogo;

    [Header("Imágenes de Personajes")]
    [SerializeField] private GameObject _sprAvatar; // Arrastra aquí la imagen del Jugador
    [SerializeField] private GameObject _sprNpc;    // Arrastra aquí la imagen del NPC

    [Tooltip("Si está marcado, el diálogo solo saldrá la primera vez.")]
    [SerializeField] private bool _esDeUnSoloUso = true;

    private bool _blnDialogoActivo;
    private int _intLineaActual;
    private bool _yaSeMostro = false;

    void Start()
    {
        _goPanelDialogo.SetActive(false);

        // Opcional: Aseguramos que las imágenes arranquen ocultas si están fuera del panel
        if (_sprAvatar != null) _sprAvatar.SetActive(false);
        if (_sprNpc != null) _sprNpc.SetActive(false);
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
        Time.timeScale = 0f;
        _blnDialogoActivo = true;
        _goPanelDialogo.SetActive(true);

        // --- CORRECCIÓN: Activamos las imágenes explícitamente ---
        if (_sprAvatar != null) _sprAvatar.SetActive(true);
        if (_sprNpc != null) _sprNpc.SetActive(true);
        // ---------------------------------------------------------

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
        Time.timeScale = 1f;
        _blnDialogoActivo = false;

        // --- CORRECCIÓN: Desactivamos las imágenes al cerrar ---
        if (_sprAvatar != null) _sprAvatar.SetActive(false);
        if (_sprNpc != null) _sprNpc.SetActive(false);
        // -------------------------------------------------------

        _goPanelDialogo.SetActive(false);
        _tmpTextoDialogo.text = string.Empty;
        StopAllCoroutines();
        Destroy(gameObject);
    }

    IEnumerator MostrarLineasDialogo()
    {
        _tmpTextoDialogo.text = string.Empty;
        foreach (char ch in _strLineasDialogo[_intLineaActual])
        {
            _tmpTextoDialogo.text += ch;
            yield return new WaitForSecondsRealtime(0.05f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Personaje"))
        {
            if (_esDeUnSoloUso && _yaSeMostro)
            {
                return;
            }

            _yaSeMostro = true;
            ComenzarDialogo();
        }
    }
}