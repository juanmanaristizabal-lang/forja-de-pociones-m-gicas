using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static GameDataLoader;

public class ItemSpawner : MonoBehaviour
{

    [Header("Prefab Generico")]
    [SerializeField] private GameObject itemPrefab;

    [Header("Puntos de spawn  (GameObjects del mapa)")]
    [SerializeField] private List<Transform> spawnPoints;

    [Header("Configuración")]
    [SerializeField] private bool spawnAutomatico = true;

    private List<GameObject> itemsActivos = new List<GameObject>();



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (spawnAutomatico)
            SpawnItems();
    }

    private void OnDrawGizmos()
    {
        if (spawnPoints is null) return;
        Gizmos.color = Color.green;
        foreach (Transform punto in spawnPoints)
        {
            if (punto is null) continue;
            Gizmos.DrawSphere(punto.position, 0.3f);
            Gizmos.DrawWireSphere(punto.position, 0.5f);
        }
         
    }

    private void SpawnItems()
    {
        if (itemPrefab == null || spawnPoints.Count.Equals(0)) return;

        List<IngredienteData> ingredientes = GameManager.Instance.Ingredientes;

        if (ingredientes == null || ingredientes.Count.Equals(0))
        {
            Debug.LogError("[ItemSpawner] No hay ingredientes cargados.");
            return;
        }

        foreach (Transform punto in spawnPoints)
        {
            if (punto == null) continue;
         
            IngredienteData data = ingredientes[Random.Range(0, ingredientes.Count)];
            SpawnItem(punto.position, data.nombre);
        }

        Debug.Log($"[ItemSpawner] Spawneados {spawnPoints.Count} items.");
    }

    public void SpawnParaReceta(RecetaData receta)
    {
        if(itemPrefab is null || spawnPoints.Count.Equals(0)) return;

        List<string> garantizados = new List<string>();
        foreach(ObjetivoReceta objetivo in receta.objetivos)
            for(int i = 0; i <objetivo.cantidad; i++)
                garantizados.Add(objetivo.ingrediente);

        List<IngredienteData> todosIngredientes = GameManager.Instance.Ingredientes;
        for(int i = 0; i < spawnPoints.Count; i++)
        {
            string nombre;
            if(i<garantizados.Count)
                nombre = garantizados[i];
            else
                nombre = todosIngredientes[Random.Range(0, todosIngredientes.Count)].nombre;
            SpawnItem(spawnPoints[i].position,nombre);
        }
        Debug.Log($"[ItemSpawner] Spawn para receta '{receta.nombre}': " +
                  $"{garantizados.Count} garantizados + {spawnPoints.Count - garantizados.Count} aleatorios.");
    }


    private void SpawnItem(Vector2 posicion ,string nombre)
    {
        GameObject item = Instantiate(itemPrefab,posicion,Quaternion.identity);
        
        //buscar datos completos del ingrediente
        IngredienteData data = GameManager.Instance.Ingredientes.Find(ing => ing.nombre.Equals(nombre));

        ItemRecolectable recolectable = item.GetComponent<ItemRecolectable>();
        if (recolectable != null && data != null)

            recolectable.Inicializar(data);
        else
            Debug.LogWarning($"[ItemSpawner] No se pudo inicializar el item '{nombre}'.");

        if ( data != null)
        {
            Sprite sprite = Resources.Load<Sprite>($"Sprites/{data.iconoId}");
            if(sprite != null)
            {
                SpriteRenderer sr = item.GetComponent<SpriteRenderer>();
                if ((sr != null)) sr.sprite = sprite;
                
            }
            else 
                Debug.LogWarning($"[ItemSpawner] No se encontró sprite para '{data.iconoId}'");
        }
        item.name = $"Item_{nombre}";
        itemsActivos.Add( item );
    }

    public void LimpiarItems()
    {
        foreach(GameObject item in itemsActivos) 
        if(item != null) Destroy(item);

        itemsActivos.Clear();
            Debug.Log($"[ItemSpawner] Items limpiados.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
