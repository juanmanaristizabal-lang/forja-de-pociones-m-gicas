using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameDataLoader : MonoBehaviour
{

    [SerializeField] private string nombreArchivo = "Ingredientes_Recetas";

  

    [System.Serializable]
    public class IngredienteData
    {
        public string nombre;
        public int valor;
        public string iconoId; 
    }

    [System.Serializable]
    public class ObjetivoReceta
    {
        public string ingredientes;
        public int cantidad; 
    }

    [System.Serializable]
    public class  RecetaData 
    {
        public int id;
        public string nombre;
        public List<ObjetivoReceta> objetivos;
    }

    [System.Serializable]
    public class GameData
    {
        public List<IngredienteData> ingredientes;
        public List<RecetaData> recetas;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.Instance.Ingredientes.Count > 0) return;
        string ruta = Path.Combine(Application.streamingAssetsPath, nombreArchivo + ".json");

        if (!File.Exists(ruta))
        {
            Debug.LogError($"[GameDataLoader] Archivo no encontrado: {ruta}");
            return;
        }

        string json = File.ReadAllText(ruta);
        GameData datos = JsonUtility.FromJson<GameData>(json);

        if (datos is null)
        {
            Debug.LogError("[GameDataLoader] Error al cargar datos del juego.");
            return;
        }

        GameManager.Instance.InicializarDatos(datos);
        Debug.Log("[GameDataLoader] Datos del juego cargados exitosamente.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
