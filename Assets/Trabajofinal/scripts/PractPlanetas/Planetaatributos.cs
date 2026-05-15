using UnityEngine;

public class Planetaatributos : MonoBehaviour
{
    public float radius = 5f;
    public float massplaneta;

    public Vector3 GetGravityForce(Rigidbody body)
    {
        Vector3 dir = (transform.position - body.position);
        float distance = dir.magnitude;

        if (distance > radius || distance <= 0.01f)
            return Vector3.zero;

        dir.Normalize();

        //F = G * m1 * m2 / r^2
        float forceMag = 1f * massplaneta * body.mass / (distance * distance);
        return dir * forceMag;
    }
    public Vector3 GetGravityAcceleration(Rigidbody body)
    {
        Vector3 force = GetGravityForce(body);
        return force / body.mass;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}