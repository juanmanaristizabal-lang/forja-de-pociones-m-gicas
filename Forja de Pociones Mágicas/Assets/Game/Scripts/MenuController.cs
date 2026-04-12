using Cainos.LucidEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    [Header("Panel de instrucciones")]
    [SerializeField] private GameObject panelInstrucciones;

    public void onJugar()
    {
        GameManager.Instance.ReiniciarJuego();
        SceneManager.LoadScene("Cueva");
    }

    public void onInstrucciones()
    {
        if(panelInstrucciones != null)
            panelInstrucciones.SetActive(!panelInstrucciones.activeSelf);
    }

    public void OnSalir()
    {
        Application.Quit();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
