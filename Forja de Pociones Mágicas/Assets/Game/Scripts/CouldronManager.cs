using Cainos.LucidEditor;
using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    [Header("Referenica del recipeManager")]
    [SerializeField] private RecipeManager recipeManager;

    [SectionHeader("Segudnos entre cambio de validacion")] // para el spam
    [SerializeField] private float coolDownValidacion = 3f;

    private bool enCooldown = false;
    private float timerCooldown = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enCooldown)
        {
            timerCooldown += Time.deltaTime;
            if (timerCooldown >= 0f)
            {
                enCooldown = false;
                timerCooldown = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D otro)
    {
        if (!otro.CompareTag("Player")) return;

        if (enCooldown) return;

        if ((recipeManager is null))
        {
            Debug.LogError("[CouldronManager] RecipeManager no asignado en inspector");
            return;
        }

        enCooldown = true;
        timerCooldown = coolDownValidacion;

        Debug.Log("[CauldronManager] Jugador entro al sprite");
        recipeManager.EntregarReceta();

    }
}
