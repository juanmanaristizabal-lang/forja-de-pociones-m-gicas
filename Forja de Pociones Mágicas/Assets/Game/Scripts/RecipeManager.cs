using Cainos.LucidEditor;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameDataLoader;



public class RecipeManager : MonoBehaviour
{

    [Header("Texto de receta a hacer")]
    [SerializeField] private TextMeshProUGUI textoNombreReceta;
    [SerializeField] private TextMeshProUGUI textoObjetivos;
    [SerializeField] private TextMeshProUGUI textoInventario;
    [SerializeField] private TextMeshProUGUI textoMensaje;

    [Header("Paneles")]
    [SerializeField] private GameObject panelVictoria;

    [SectionHeader("Botones")]
    [SerializeField] private Button botonVolver; //A recolectar
    [SerializeField] private Button botonMenu; //A menu principal



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       if(panelVictoria != null)
           panelVictoria.SetActive(false);

        MostrarMensaje("");

        GameManager.Instance.OnInventarioActualizado += ActualizarUIInventario;

        ActualizarUIReceta();
        ActualizarUIInventario();

        if (botonVolver != null)
            botonVolver.onClick.AddListener(VolverARecolectar);
        if ((botonMenu != null))
             botonMenu.onClick.AddListener(VolverAlMenu);

    }


    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnInventarioActualizado -= ActualizarUIInventario;

    }

    public void ActualizarUIReceta()
    {
        RecetaData receta = GameManager.Instance.ObtenerRecetaActual();
        if(receta == null)
        {
            MostrarVictoria();
            return;
        }
        if(textoNombreReceta != null)
            textoNombreReceta.text = $"Receta: {receta.nombre}";

        if (textoObjetivos != null)
        {
            System.Text.StringBuilder sb = new();
            sb.AppendLine("Ingredientes necesarios:");
            foreach (ObjetivoReceta obj in receta.objetivos)
                sb.AppendLine($"- {obj.ingrediente}: {obj.cantidad}");
            textoObjetivos.text = sb.ToString();

        }

    }

    public void ActualizarUIInventario()
    {
        if (textoInventario is null) return;
        Dictionary<string, int> inv = GameManager.Instance.Inventario;

        if (inv.Count.Equals(0))
        {
            textoInventario.text = "Inventario vacio";
            return;
        }

        System.Text.StringBuilder sb = new();
        sb.AppendLine("Tu inventario:");
        foreach (KeyValuePair<string, int> par in inv )
            sb.AppendLine($"- {par.Key}: {par.Value}");
        textoInventario.text = sb.ToString();
    }

    private void MostrarMensaje(string mensaj)
    {
        if(textoMensaje != null) 
            textoMensaje.text = mensaj;
    }

    private void MostrarVictoria()
    {

        MostrarMensaje("Felicidades Has completado todas las recetas.");
        if (panelVictoria != null)
            panelVictoria.SetActive(true);
    }

    public void EntregarReceta()
    {
        RecetaData receta = GameManager.Instance.ObtenerRecetaActual();

        if (receta == null)
        {
            MostrarMensaje("No hay receta activa.");
            return;
        }

        if (ValidarReceta(receta))
        {
            MostrarMensaje($" ¡Receta '{receta.nombre}' completada");
            Debug.Log($"[RecipeManager] Receta completada: {receta.nombre}");

            bool hayMas = GameManager.Instance.AvanzarReceta();

            if (hayMas)
            {
                ActualizarUIReceta();
                ActualizarUIInventario();
            }
            else
            {
                MostrarVictoria();
            }
        }
        else
        {
            MostrarMensaje(" Faltan ingredientes. Vuelve a recolectar.");
            Debug.Log("[RecipeManager] Receta incompleta.");
        }
    }

    private bool ValidarReceta(RecetaData receta)
    {
        Dictionary<string, int> inventario = GameManager.Instance.Inventario;
        foreach (ObjetivoReceta objetivo in receta.objetivos)
        {
            bool tieneIngrediente = inventario.ContainsKey(objetivo.ingrediente);
            if (!tieneIngrediente)
            {
                Debug.Log($"[RecipeManager] Falta ingrediente: {objetivo.ingrediente}");
                return false;
            }
            if (inventario[objetivo.ingrediente] <objetivo.cantidad)
            {
                Debug.Log($"[Validación] No alcanza: {objetivo.ingrediente} " +
                                         $"(tiene {inventario[objetivo.ingrediente]}, necesita {objetivo.cantidad})");
                return false;
            }
        }
        return true;
    }

    private void VolverARecolectar()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Cueva");
    }

    private void VolverAlMenu()
    {
        GameManager.Instance.ReiniciarJuego();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuPrincipal");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
