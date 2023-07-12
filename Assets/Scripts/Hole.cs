using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Hole : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball != null)
        {
            ball.Destroy();
        }
    }
}
