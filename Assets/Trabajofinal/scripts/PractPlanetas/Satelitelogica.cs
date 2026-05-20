using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class Satelitelogica : MonoBehaviour
{
    [Header("Variables")]
    public float JugadorVelo = 6f;
    public float RotacionVelo = 5f;
    public float FuerzaSalto = 8f;
    public float ChecardistanciaSuelo = 1.1f;
    public float Impulso = 2f;
    public float Fuerzatraccion = 10f;

    private Vector3 lastMoveDir = Vector3.zero;
    public LayerMask PlanetaSuelo;

    [Header("Importante")]
    Rigidbody rb;
    public Camera primerapov;
    public Camera tercerapov;
    [SerializeField] private Mouse ratonPri;
    [SerializeField] private Mouse ratonTer;


    public Planetaatributos[] planetas;
    public bool estasuelo;
    Planetaatributos Masfuerteplaneta;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        primerapov.enabled = false;
        tercerapov.enabled = true;
    }
    void Start()
    {
        //Busca otros planetas en escena
        planetas = FindObjectsByType<Planetaatributos>(FindObjectsSortMode.None);

    }
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.R)) SceneManager.LoadScene("PlanetGame");
        if (planetas == null || planetas.Length == 0) return;

        //se mira que planeta es mas fuerte en rango
        Vector3 totalGravityForce = Vector3.zero;
        Planetaatributos strongestAttractor = null;
        float strongestAccel = 0f;

        foreach (var attractor in planetas)
        {
            Vector3 force = attractor.GetGravityForce(rb);
            if (force != Vector3.zero)
            {
                totalGravityForce += force;

                float accelMag = attractor.GetGravityAcceleration(rb).magnitude;
                if (accelMag > strongestAccel)
                {
                    strongestAccel = accelMag;
                    strongestAttractor = attractor;
                }
            }
        }

        Masfuerteplaneta = strongestAttractor;

        rb.AddForce(totalGravityForce, ForceMode.Force);

        //se pone al jugador en forma que sus pies estan en el "suelo" del planeta
        if (Masfuerteplaneta != null && totalGravityForce != Vector3.zero)
        {
            Vector3 gravityDir = (Masfuerteplaneta.transform.position - transform.position).normalized;
            Vector3 desiredUp = -gravityDir;

            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, desiredUp) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, RotacionVelo * Time.fixedDeltaTime);
        }

        //movimiento y salto
        Movimientojugador();
        EstaenSuelo();

    }

    private void Movimientojugador()
    {
        if (Masfuerteplaneta == null) return;

        float moveForward = 0f;
        float moveRight = 0f;

        if (Input.GetKey(KeyCode.D)) moveForward += 1f;
        if (Input.GetKey(KeyCode.A)) moveForward -= 1f;
        if (Input.GetKey(KeyCode.S)) moveRight += 1f;
        if (Input.GetKey(KeyCode.W)) moveRight -= 1f;

        Vector3 up = transform.up;

        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(transform.right, up).normalized;

        Vector3 moveDir = (forward * moveForward + right * moveRight);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            moveDir = moveDir.normalized;
            lastMoveDir = moveDir;

            Vector3 desiredVelocity = moveDir * JugadorVelo;
            Vector3 currentPlanarVelocity = Vector3.ProjectOnPlane(rb.linearVelocity, up);
            Vector3 velocityChange = desiredVelocity - currentPlanarVelocity;

            rb.AddForce(velocityChange * 0.5f, ForceMode.VelocityChange);
        }

        if (Input.GetKey(KeyCode.E))
        {
            Vector3 impulsoDir = lastMoveDir.sqrMagnitude > 0.01f ? lastMoveDir : forward;
            rb.AddForce(impulsoDir * Impulso, ForceMode.Impulse);
        }
    }

    private void AttraccionMouse(Vector3 objetivoWorldPos, float fuerza)
    {
        Vector3 direccion = objetivoWorldPos - transform.position;

        if (direccion.sqrMagnitude < 0.001f) return;

        direccion.Normalize();
        rb.AddForce(direccion * fuerza, ForceMode.VelocityChange);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && estasuelo && Masfuerteplaneta != null)
        {
            Vector3 up = transform.up;
            rb.AddForce(up * FuerzaSalto, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            primerapov.enabled = true;
            tercerapov.enabled = false;
            //raton.
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            primerapov.enabled = false;
            tercerapov.enabled = true;
        }

        if (Input.GetMouseButton(0))
        {
            if (ratonPri != null && ratonPri.Atraccionmouse)
            {
                AttraccionMouse(ratonPri.MousePosition, Fuerzatraccion);
            }
            else if (ratonTer != null && ratonTer.Atraccionmouse)
            {
                AttraccionMouse(ratonTer.MousePosition, Fuerzatraccion);
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            JugadorVelo = 0;
        }
        if (!Input.GetKeyUp(KeyCode.X))
        {
            JugadorVelo = 3;
        }
    }

    private void EstaenSuelo()
    {
        Vector3 origin = transform.position;
        Vector3 down = -transform.up;

        if (Physics.Raycast(origin, down, out RaycastHit hit, ChecardistanciaSuelo, PlanetaSuelo))
        {
            estasuelo = true;
        }
        else
        {
            estasuelo = false;
        }
    }
}
