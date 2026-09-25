using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public HealthBarUI uiVida;
    public GameObject panelMuerte;  // 🔥 Arrastrar el Canvas de muerte
    
    [Header("Muerte y Reinicio")]
    public float restartDelay = 10f;
    private bool isDead = false;
    
    void Update()
    {
        if (isDead) return;
        DañarPlayer();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            RecibirDaño();
        }
    }

    void DañarPlayer()
    {
        if(Keyboard.current.pKey.wasPressedThisFrame)
        {
            RecibirDaño();
        }
    }
    
    public void RecibirDaño()
    {
        if (isDead) return;
        
        if (uiVida == null)
        {
            Debug.LogError("❌ uiVida es null");
            return;
        }
        
        uiVida.PerderVida();
        Debug.Log($"Vida actual después del daño: {uiVida.GetVidaActual()}");
        
        if (uiVida.GetVidaActual() <= 0)
        {
            Muerte();
        }
    }
    
    void Muerte()
    {
        if (isDead) return;
        isDead = true;
        
        Debug.Log("💀 Jugador ha muerto. Pausando juego...");
        
        // 🔥 Mostrar panel de muerte
        if (panelMuerte != null)
        {
            panelMuerte.SetActive(true);
        }
        
        // Pausar el juego
        Time.timeScale = 0f;
        
        // Iniciar corrutina para reiniciar
        StartCoroutine(RestartGame());
    }
    
    IEnumerator RestartGame()
    {
        // Esperar en tiempo real (ignora Time.timeScale)
        float elapsedTime = 0f;
        while (elapsedTime < restartDelay)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        
        // Reanudar el tiempo antes de recargar
        Time.timeScale = 1f;
        
        // Recargar la escena actual
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        
        Debug.Log("🔄 Reiniciando partida...");
    }
}