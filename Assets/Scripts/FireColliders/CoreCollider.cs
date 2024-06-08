using UnityEngine;

public class CoreCollider : BaseCollider
{
    protected override void ResetHit()
    {
        if (Time.time - lastHitChange >= extinguishResetTime)
        {
            gameState.isFireCoreHit = false;
            lastHitChange = Time.time;
        }
    }

    protected override void HandleCollision()
    {
        gameState.isFireCoreHit = true;
    }
}