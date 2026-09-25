using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    // ========== REFERENCIAS UI ==========
    [Header("=== PANEL DE VICTORIA ===")]
    public GameObject panelVictoria;           // Panel que se muestra al ganar
    public TMP_Text resultadoVictoria;         // "¡VICTORIA!"
    public TMP_Text puntajeVictoria;           // Puntaje total
    public TMP_Text detallesVictoria;          // Desglose detallado

    [Header("=== PANEL DE DERROTA ===")]
    public GameObject panelDerrota;            // Panel que se muestra al perder
    public TMP_Text resultadoDerrota;          // "DERROTA"
    public TMP_Text puntajeDerrota;            // Puntaje total
    public TMP_Text detallesDerrota;           // Desglose detallado

    // ========== VARIABLES DE SEGUIMIENTO ==========
    private int zonasPintadas = 0;
    private int enemigosConvertidos = 0;
    private int municionRecolectada = 0;
    private float tiempoSobrevivido = 0f;
    private bool partidaTerminada = false;

    // ========== PESOS PARA EL PUNTAJE ==========
    private const int PUNTOS_POR_ZONA = 10;
    private const int PUNTOS_POR_ENEMIGO = 50;
    private const int PUNTOS_POR_MUNICION = 20;
    private const int PUNTOS_POR_SEGUNDO = 5;

    // Singleton
    public static ScoreManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Ocultar ambos paneles al inicio
        if (panelVictoria != null)
            panelVictoria.SetActive(false);
        if (panelDerrota != null)
            panelDerrota.SetActive(false);
        
        tiempoSobrevivido = 0f;
        partidaTerminada = false;
    }

    void Update()
    {
        if (!partidaTerminada)
        {
            tiempoSobrevivido += Time.deltaTime;
        }
    }

    // ========== MÉTODOS PARA REGISTRAR ACCIONES ==========
    public void RegistrarZonaPintada()
    {
        zonasPintadas++;
    }

    public void RegistrarEnemigoConvertido()
    {
        enemigosConvertidos++;
    }

    public void RegistrarMunicionRecolectada()
    {
        municionRecolectada++;
    }

    // ========== CÁLCULO DEL PUNTAJE ==========
    public int CalcularPuntajeTotal()
    {
        int puntaje = 0;
        puntaje += zonasPintadas * PUNTOS_POR_ZONA;
        puntaje += enemigosConvertidos * PUNTOS_POR_ENEMIGO;
        puntaje += municionRecolectada * PUNTOS_POR_MUNICION;
        puntaje += Mathf.FloorToInt(tiempoSobrevivido) * PUNTOS_POR_SEGUNDO;
        return puntaje;
    }

    // ========== FINALIZAR PARTIDA (VICTORIA) ==========
    public void FinalizarPartidaVictoria()
    {
        if (partidaTerminada) return;
        partidaTerminada = true;

        int puntajeTotal = CalcularPuntajeTotal();

        // Mostrar panel de victoria, ocultar derrota
        if (panelVictoria != null)
            panelVictoria.SetActive(true);
        if (panelDerrota != null)
            panelDerrota.SetActive(false);

        // Mostrar datos en panel de VICTORIA
        if (resultadoVictoria != null)
        {
            resultadoVictoria.text = "¡VICTORIA!";
            resultadoVictoria.color = Color.green;
        }

        if (puntajeVictoria != null)
        {
            puntajeVictoria.text = $"TOTAL: {puntajeTotal}";
        }

        if (detallesVictoria != null)
        {
            detallesVictoria.text = ObtenerDesglosePuntaje(puntajeTotal);
        }

        Debug.Log($"=== VICTORIA ===\n{ObtenerDesglosePuntaje(puntajeTotal)}");
    }

    // ========== FINALIZAR PARTIDA (DERROTA) ==========
    public void FinalizarPartidaDerrota()
    {
        if (partidaTerminada) return;
        partidaTerminada = true;

        int puntajeTotal = CalcularPuntajeTotal();

        // Mostrar panel de derrota, ocultar victoria
        if (panelDerrota != null)
            panelDerrota.SetActive(true);
        if (panelVictoria != null)
            panelVictoria.SetActive(false);

        // Mostrar datos en panel de DERROTA
        if (resultadoDerrota != null)
        {
            resultadoDerrota.text = "PERDISTE";
            resultadoDerrota.color = Color.red;
        }

        if (puntajeDerrota != null)
        {
            puntajeDerrota.text = $"TOTAL: {puntajeTotal}";
        }

        if (detallesDerrota != null)
        {
            detallesDerrota.text = ObtenerDesglosePuntaje(puntajeTotal);
        }

        Debug.Log($"=== DERROTA ===\n{ObtenerDesglosePuntaje(puntajeTotal)}");
    }

    // ========== OBTENER DESGLOSE (para no repetir código) ==========
    private string ObtenerDesglosePuntaje(int puntajeTotal)
    {
        return $"Zonas pintadas: {zonasPintadas} x {PUNTOS_POR_ZONA} = {zonasPintadas * PUNTOS_POR_ZONA}\n" +
               $"Enemigos convertidos: {enemigosConvertidos} x {PUNTOS_POR_ENEMIGO} = {enemigosConvertidos * PUNTOS_POR_ENEMIGO}\n" +
               $"Munición recolectada: {municionRecolectada} x {PUNTOS_POR_MUNICION} = {municionRecolectada * PUNTOS_POR_MUNICION}\n" +
               $"Tiempo sobrevivido: {Mathf.FloorToInt(tiempoSobrevivido)}s x {PUNTOS_POR_SEGUNDO} = {Mathf.FloorToInt(tiempoSobrevivido) * PUNTOS_POR_SEGUNDO}\n";
    }

    // ========== MÉTODOS PÚBLICOS PARA ACCEDER ==========
    public int GetZonasPintadas() => zonasPintadas;
    public int GetEnemigosConvertidos() => enemigosConvertidos;
    public int GetMunicionRecolectada() => municionRecolectada;
    public float GetTiempoSobrevivido() => tiempoSobrevivido;
}