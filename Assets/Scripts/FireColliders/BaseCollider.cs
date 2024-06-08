using UnityEngine;

public abstract class BaseCollider : MonoBehaviour
{
    [SerializeField]
    protected GameState gameState;

    [SerializeField]
    protected FireController fireController;

    protected float lastHitChange = 0f;

    protected readonly float extinguishResetTime = 0.2f;

    private void Update()
    {
        if (gameState.isSecondStageStarted && !gameState.isFireDefeated)
        {
            ResetHit();
        }
    }

    protected abstract void ResetHit();

    private void OnParticleCollision(GameObject other)
    {
        HandleCollision();
        lastHitChange = Time.time;
    }

    protected abstract void HandleCollision();
}