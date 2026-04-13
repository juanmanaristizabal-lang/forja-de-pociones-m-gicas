using UnityEngine;
using static GameDataLoader;

public class ItemRecolectable : MonoBehaviour
{

    private IngredienteData datos;
    private bool yaRecolectado = false;

    public void Inicializar(IngredienteData data)
    {
        datos = data;
    }

    public void SetItemNombre(string nombre)
    {
        datos = GameManager.Instance.Ingredientes.Find(ing => ing.nombre.Equals(nombre));
        if (datos == null)
            Debug.LogWarning($"[ItemRecolectable] Ingrediente no encontrado: {nombre}");
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (yaRecolectado) return;
        if (!otro.CompareTag("Player")) return;
        
        yaRecolectado =true;
        if (datos is null)
        {
            Debug.LogError("[ItemRecolectable] Datos no inicializados.");
            Destroy(gameObject);
            return;
        }

        GameManager.Instance.AgregarAlInventario(datos.nombre);
        Debug.Log($"[Recolección] +1 {datos.nombre} (valor: {datos.valor})");
        Destroy(gameObject);
    }

    public string NombreIngrediente
    {
        get { return datos != null ? datos.nombre : ""; }
        set { if (datos != null) datos.nombre = value; }
    }

    public int ValorIngrediente
    {
        get { return datos != null ? datos.valor : 0; }
        set { if (datos != null) datos.valor = value; }
    }

    public string IconoId
    {
        get { return datos != null ? datos.iconoId : ""; }
        set { if (datos != null) datos.iconoId = value; }
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
