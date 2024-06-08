using System.Collections;
using UnityEngine;

public class FireController : MonoBehaviour
{
    [SerializeField]
    private GameState gameState;

    [SerializeField]
    private FireUI fireUI;

    [SerializeField]
    private ParticleSystem fireParticle;

    [SerializeField]
    private float fireHealth = 6f;

    [SerializeField]
    private float timeToRegenerateFire = 2f;

    private Coroutine fireRegenerateCoroutine;


    private void Update()
    {
        if (gameState.isSecondStageStarted)
        {
            if (!AudioManager.Instance.fireSource.isPlaying)
            {
                AudioManager.Instance.PlayFireSound("fire");
            }
            else if (gameState.isFireDefeated)
            {
                AudioManager.Instance.fireSource.Stop();
            }

            ControlFire();
        }
    }

    private void ExtinguishFire()
    {
        if (gameState.isFireOuterHit && gameState.isFireCoreHit)
        {
            gameState.isFireOuterHit = false; // to avoid double hp loss
        }

        if (gameState.isFireOuterHit)
        {
            DecreaseFire(3f);
        }
        else if (gameState.isFireCoreHit)
        {
            DecreaseFire(0f);
        }

    }

    private void DecreaseFire(float minHealth)
    {
        CancelRegeneration();

        if (fireHealth >= minHealth)
        {
            if (gameState.isUsingExtinguisher)
            {
                fireHealth -= Time.deltaTime;
            }
            else
            {
                fireHealth -= Time.deltaTime / 6f;
                // This "6f" is a safeguard to prevent players from extinguishing the fire with quick short clicks
            }

        }
    }

    private void CancelRegeneration()
    {
        if (fireRegenerateCoroutine != null)
        {
            StopCoroutine(fireRegenerateCoroutine);
            fireRegenerateCoroutine = null;
        }
    }

    private void RegenerateFire()
    {
        if (!gameState.isFireOuterHit && !gameState.isFireCoreHit && fireHealth < 6f && fireHealth != 0f)
        {
            if (fireRegenerateCoroutine == null)
            {
                fireRegenerateCoroutine = StartCoroutine(WaitAndRegenerateFire());
            }
        }
    }

    private IEnumerator WaitAndRegenerateFire()
    {
        yield return new WaitForSeconds(timeToRegenerateFire);

        while (fireHealth < 6f)
        {
            fireHealth += Time.deltaTime;
            ChangeFireParticleEmission();

            yield return null;
        }

        fireHealth = 6f;
    }

    private void ChangeFireParticleEmission()
    {
        var emission = fireParticle.emission;
        emission.rateOverTime = 10f * (fireHealth / 6f);
    }

    private void ChangeFireVolume()
    {
        AudioManager.Instance.fireSource.volume = fireHealth / 15f;
    }

    private void CheckFireHealth()
    {
        if (fireHealth <= 0)
        {
            fireHealth = 0;
            gameState.isFireDefeated = true;
        }
    }

    private void ControlFire()
    {
        if (gameState.isSecondStageStarted && !gameState.isFireDefeated)
        {
            CheckFireHealth();
            ChangeFireVolume();
            ChangeFireParticleEmission();
            ExtinguishFire();
            RegenerateFire();
        }
    }

    public float GetFireHealth { get { return fireHealth; } }
}
