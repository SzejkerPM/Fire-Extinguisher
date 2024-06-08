using UnityEngine;

public class OuterCollider : BaseCollider
{
    protected override void ResetHit()
    {
        if (Time.time - lastHitChange >= extinguishResetTime)
        {
            gameState.isFireOuterHit = false;
            lastHitChange = Time.time;
        }
    }

    protected override void HandleCollision()
    {
        gameState.isFireOuterHit = true;
    }
}
