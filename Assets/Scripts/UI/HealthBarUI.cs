using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    public GameObject[] rayos;  // Arrastra los 3 rayos (de izquierda a derecha)
    
    public int vidaActual = 3;
    public int vidaMaxima = 3;
    
    void Start()
    {
        vidaActual = vidaMaxima;
        ActualizarUI();
    }
    
    // ESTE MÉTODO se ejecuta automáticamente cuando cambias un valor en el Inspector
    void OnValidate()
    {
        // Asegurar que la vida no se pase de los límites
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);
        ActualizarUI();
    }
    
    public void PerderVida()
    {
        if (vidaActual <= 0) return;
        
        vidaActual--;
        ActualizarUI();
        
        Debug.Log($"Rayos restantes: {vidaActual}/{vidaMaxima}");
        
        if (vidaActual <= 0)
        {
            Debug.Log("GAME OVER");
        }
    }
    
    void ActualizarUI()
    {
        // Verificar que el array no esté vacío
        if (rayos == null || rayos.Length == 0) return;
        
        for (int i = 0; i < rayos.Length; i++)
        {
            if (rayos[i] != null)
            {
                if (i < vidaActual)
                    rayos[i].SetActive(true);   // Rayo visible
                else
                    rayos[i].SetActive(false);  // Rayo desaparece
            }
        }
    }
    
    public int GetVidaActual()
    {
        return vidaActual;
    }
}