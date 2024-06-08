using UnityEngine;

public class BoltController : MonoBehaviour
{

    private bool isSoundPlayed = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isSoundPlayed && collision.collider.CompareTag("Floor"))
        {
            AudioManager.Instance.PlayBoltSound("bolt");
            isSoundPlayed = true;
        }
    }
}
