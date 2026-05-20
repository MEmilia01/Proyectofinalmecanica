using UnityEngine;

public class Mouse : MonoBehaviour
{
    public Vector3 MousePosition { get; private set; }
    public bool Atraccionmouse { get; private set; }

    [SerializeField] private Camera cam;
    [SerializeField] private float distanciaDesdeCamara = 10f;

    private void Awake()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    private void Update()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = distanciaDesdeCamara;
        MousePosition = cam.ScreenToWorldPoint(screenPos);

        Atraccionmouse = Input.GetMouseButton(0);
    }
}
