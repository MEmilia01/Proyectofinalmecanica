using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [Header("Contador logica")]
    [SerializeField] private int Monedasmaximas = 15;
    private int Mondedatienes = 0;

    [Header("Temporizador")]
    [SerializeField] private float TiempoInicial = 180;
    [SerializeField] private Satelitelogica player;
    private float tiemporestante;

    [Header("UI")]
    [SerializeField] private TMP_Text Textocontador;
    [SerializeField] private TMP_Text TextoTemporizador;
    [SerializeField] public GameObject Textofinal;
    [SerializeField] private TMP_Text Mensajefinal;
    [SerializeField] public Slider Juegonivel;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // Si ya existe una instancia, destruimos el nuevo GameObject
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Si no hay instancia, nos asignamos como la única
        Instance = this;
    }

    private void Start()
    {
        Textofinal.SetActive(false);
        //Juegonivel.value = 0f;
        tiemporestante = TiempoInicial;
        ActualizarTextoMonedas();
        Actualizartexto();
    }

    private void Update()
    {
        if (tiemporestante > 0f)
        {
            tiemporestante -= Time.deltaTime;

            if (tiemporestante < 0f || tiemporestante == 0f)
            {
                tiemporestante = 0f;
                GanaroPerder();
            }

            Actualizartexto();
        }
        
        if (Mondedatienes == Monedasmaximas) { GanaroPerder(); }
    }

    #region Contador
    public void RegistrarMoneda()
    {
        Mondedatienes++;
        ActualizarTextoMonedas();
    }

    private void ActualizarTextoMonedas()
    {
        if (Textocontador != null)
            Textocontador.text = "Monedas: " + Mondedatienes + " / " + Monedasmaximas;
    }

    #endregion

    #region Temporizador

    private void Actualizartexto()
    {
        int minutos = Mathf.FloorToInt(tiemporestante / 60f);
        int segundos = Mathf.FloorToInt(tiemporestante % 60f);
        TextoTemporizador.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    #endregion

    #region Ganar/Perder
    private void GanaroPerder()
    {
        if(Mondedatienes == Monedasmaximas)
        { Ganarganar(); }
        else if(7 < Mondedatienes && Mondedatienes < Monedasmaximas)
        { Ganarmedio(); }
        else { Perder(); }
    }
    private void Ganarganar()
    {
        Textofinal.SetActive(true);
        tiemporestante = 0f;
        Juegonivel.value = Mondedatienes;
        Mensajefinal.text = "Eres increible!!";
    }
    private void Ganarmedio()
    {
        Textofinal.SetActive(true);
        tiemporestante = 0f;
        Juegonivel.value = Mondedatienes;
        Mensajefinal.text = "Lo has hecho bien!";
    }
    private void Perder()
    {
        Textofinal.SetActive(true);
        tiemporestante = 0f;
        Juegonivel.value = Mondedatienes;
        Mensajefinal.text = "No has conseguido casi nada!";
    }

    #endregion
}
