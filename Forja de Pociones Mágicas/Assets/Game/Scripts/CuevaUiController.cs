
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CuevaUiController : MonoBehaviour
{

    [Header("UI - Inventario")]
    [SerializeField] private TextMeshProUGUI textoInventario;

    [Header("Texto - Receta objetivo")]
    [SerializeField] private TextMeshProUGUI textoObjetivoReceta;

    [Header("Boton ir al caldero")]
    [SerializeField] private Button botonIrAlCaldero;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.OnInventarioActualizado += RefrescarUIInventario;
        if(botonIrAlCaldero != null)
            botonIrAlCaldero.onClick.AddListener(IrAlCaldero);
       
        MostrarObjetivoReceta();
        RefrescarUIInventario();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnInventarioActualizado -= RefrescarUIInventario; ;
    }


    private void RefrescarUIInventario()
    {
        if (textoInventario is null) return;
        Dictionary<string, int> inv = GameManager.Instance.Inventario;

        if (inv.Count.Equals(0))
        {
            textoInventario.text = "Inventario: vacio";
            return;
        }
        System.Text.StringBuilder sb = new();
        sb.AppendLine("Inventario:");
        foreach (KeyValuePair<string, int> par in inv)
            sb.AppendLine($"- {par.Key}: {par.Value}");

        textoInventario.text = sb.ToString();

    }

    private void MostrarObjetivoReceta()
    {
        if (textoObjetivoReceta is null) return;
        var receta = GameManager.Instance.ObtenerRecetaActual();
        if (receta is null)
        {
            textoObjetivoReceta.text = "No hay receta objetivo";
            return;
        }
        System.Text.StringBuilder sb = new();
        sb.AppendLine($"Receta objetivo: {receta.nombre}"); sb.AppendLine("Necesitas:");
        foreach (var obj in receta.objetivos)
            sb.AppendLine($"  • {obj.ingrediente} x{obj.cantidad}");

        textoObjetivoReceta.text = sb.ToString();
    }

    private void IrAlCaldero()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Caldero"); //Hace que permanezca el inventario en el cambio de escena
    }
}