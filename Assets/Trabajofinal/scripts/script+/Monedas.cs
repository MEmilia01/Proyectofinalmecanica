using UnityEngine;

public class Monedas : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entra");
            GameManager.Instance.RegistrarMoneda();
            Destroy(this.gameObject);
        }
    }
    //esto es para que llame el registrarmoneda
    //GameManager.Instance.RegistrarMoneda();
}
