using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using static GameDataLoader;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<IngredienteData> Ingredientes { get; private set; } = new();
    public List<RecetaData> Recetas { get; private set; } = new();

    //Inventario
    public Dictionary<string, int> Inventario { get; private set; } = new();


    //Progreso
    public int RecetaActualIndex { get; private set; } = 0;
    public bool JuegoTerminado { get; private set; } = false;


    public System.Action OnInventarioActualizado;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }


    public void InicializarDatos(GameData datos)
    {
        Ingredientes = datos.ingredientes;
        Recetas = datos.recetas;
        Debug.Log($"[GameManager] Cargados: {Ingredientes.Count}ingredientes," + $" {Recetas.Count} recetas.");
    }

    public void AgregarAlInventario(string nombre)
    {
        if (Inventario.ContainsKey(nombre))
            Inventario[nombre]++;
        else
            Inventario[nombre] = 1;

        Debug.Log($"[Inventario] +1 {nombre} (total; {Inventario[nombre]})");
        OnInventarioActualizado?.Invoke();
    }

    public void LimpiarInventario()
    {
        Inventario.Clear();
        OnInventarioActualizado?.Invoke();
    }

    public RecetaData ObetenerRecetaActual()
    {
        if (RecetaActualIndex < Recetas.Count)
            return Recetas[RecetaActualIndex];
        return null; 
    }

    public bool AvanzarReceta()
    {
        RecetaActualIndex++;
        if (RecetaActualIndex >= Recetas.Count)
        {
            JuegoTerminado = true;
            return false;
        }
        LimpiarInventario();
        return true; 
    }

    public void ReiniciarJuego()
    {
        RecetaActualIndex = 0;
        JuegoTerminado = false;
        LimpiarInventario();
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
