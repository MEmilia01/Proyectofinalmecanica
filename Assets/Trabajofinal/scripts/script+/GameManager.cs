using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [Header("Contador logica")]
    [SerializeField] private int Monedasmaximas = 15;
    private int Mondedatienes = 0;

    [Header("Temporizador")]
    [SerializeField] private float TiempoInicial = 300f;
    private float tiemporestante;

    [Header("UI")]
    [SerializeField] private TMP_Text Textocontador;
    [SerializeField] private TMP_Text TextoTemporizador;
    [SerializeField] public GameObject Textofinal;
    [SerializeField] private Slider Juegonivel;
    [SerializeField] private TMP_Text Mensajefinal;

    private void Start()
    {
        Juegonivel.value = 0f;
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
            Textocontador.text = "Cajas: " + Mondedatienes + " / " + Monedasmaximas;
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
        Juegonivel.value = Mondedatienes;
        Mensajefinal.text = "Eres increible!!";
    }
    private void Ganarmedio()
    {
        Textofinal.SetActive(true);
        Juegonivel.value = Mondedatienes;
        Mensajefinal.text = "Lo has hecho bien!";
    }
    private void Perder()
    {
        Textofinal.SetActive(true);
        Juegonivel.value = Mondedatienes;
        Mensajefinal.text = "No has conseguido casi nada!";
    }

    #endregion
}
