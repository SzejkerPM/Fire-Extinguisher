using System.Collections;
using UnityEngine;

public class ExtinguisherController : MonoBehaviour, IRunInitialAnimation, IOnSliderChangePosition
{
    [SerializeField]
    private GameState gameState;

    [SerializeField]
    private ExtinguisherUI extinguisherUI;

    private Animator extinguisherAnimator;

    #region Bolt and Seal

    [SerializeField]
    private GameObject bolt;

    [SerializeField]
    private GameObject seal;

    private Vector3 targetBoltPosition;
    private Vector3 targetSealPosition;

    private readonly float boltAndSealTargetPositionZ = -8.2f;

    [SerializeField]
    private float boltSealAnimationSpeed = 1f;

    #endregion

    #region Hose

    [SerializeField]
    private LineRenderer hoseLineRenderer;

    [SerializeField]
    private float hoseAnimationSpeed = 1f;

    [SerializeField]
    private Vector3[] hoseStartPoints;

    [SerializeField]
    private Vector3[] hoseEndPoints;

    #endregion

    #region Powder

    [SerializeField]
    private ParticleSystem powderParticle;

    [SerializeField]
    private float particleDelay = 0f;

    private Coroutine particleCoroutine;

    [SerializeField]
    private float extinguisherPowder = 10f;

    #endregion


    private void Start()
    {
        extinguisherAnimator = transform.GetComponent<Animator>();

        targetBoltPosition = new Vector3(bolt.transform.position.x, transform.position.y, boltAndSealTargetPositionZ);
        targetSealPosition = new Vector3(seal.transform.position.x, transform.position.y, boltAndSealTargetPositionZ);
    }

    private void Update()
    {
        if (gameState.isSecondStageStarted)
        {
            UseExtinguisher();
        }

        if (gameState.isGameLost || gameState.isGameWon)
        {
            StopExtinguisher();
            ChangePosition(0f);
        }

        if (gameState.isUsingExtinguisher && !AudioManager.Instance.extinguisherSource.isPlaying)
        {
            AudioManager.Instance.PlayExtinguisherSound("powder");
            AudioManager.Instance.extinguisherSource.loop = true;
        }
        else if (!gameState.isUsingExtinguisher && AudioManager.Instance.extinguisherSource.isPlaying)
        {
            AudioManager.Instance.extinguisherSource.Pause();
        }
    }

    public void InitialAnimation()
    {
        if (!gameState.isBoltAnimationCompleted)
        {
            StartCoroutine(BoltAnimation());
        }
        else
        {
            StartCoroutine(HoseAnimation());
        }
    }

    // MoveTowards animation
    private IEnumerator BoltAnimation()
    {
        while (Vector3.Distance(bolt.transform.position, targetBoltPosition) > 0.01f)
        {
            bolt.transform.position = Vector3.MoveTowards(bolt.transform.position, targetBoltPosition, boltSealAnimationSpeed * Time.deltaTime);
            seal.transform.position = Vector3.MoveTowards(seal.transform.position, targetSealPosition, boltSealAnimationSpeed * Time.deltaTime);

            yield return null;
        }

        bolt.transform.position = targetBoltPosition;
        seal.transform.position = targetSealPosition;

        yield return new WaitForSeconds(0.1f);

        bolt.transform.SetParent(null);
        seal.transform.SetParent(null);
        bolt.GetComponent<Rigidbody>().useGravity = true;
        seal.GetComponent<Rigidbody>().useGravity = true;

        gameState.isCurrentlyAnimating = false;
        gameState.isBoltAnimationCompleted = true;
    }

    // MoveTowards animation
    private IEnumerator HoseAnimation()
    {
        while (Vector3.Distance(hoseStartPoints[0], hoseEndPoints[0]) > 0.01f)
        {
            for (int i = 0; i < hoseStartPoints.Length; i++)
            {
                hoseStartPoints[i] = Vector3.MoveTowards(hoseStartPoints[i], hoseEndPoints[i], hoseAnimationSpeed * Time.deltaTime);
                hoseLineRenderer.SetPosition(i, hoseStartPoints[i]);
            }

            yield return null;
        }

        for (int i = 0; i < hoseStartPoints.Length; i++)
        {
            hoseLineRenderer.SetPosition(i, hoseEndPoints[i]);
        }

        // Setting new position for particles
        Vector3 newParticlePosition = hoseLineRenderer.GetPosition(0);
        powderParticle.transform.localPosition = newParticlePosition;

        gameState.isCurrentlyAnimating = false;
        gameState.isHoseAnimationCompleted = true;
        gameState.areInitialAnimationsAvaible = false;
    }

    private void StartExtinguisher()
    {
        if (gameState.isUsingExtinguisher && !extinguisherAnimator.GetBool("isPressed"))
        {
            extinguisherAnimator.SetBool("isPressed", true);
            StartPowder();
        }
    }

    private void StopExtinguisher()
    {
        if (!gameState.isUsingExtinguisher && extinguisherAnimator.GetBool("isPressed"))
        {
            extinguisherAnimator.SetBool("isPressed", false);
            StopPowder();
        }
    }

    private void ReducePowder()
    {
        if (gameState.isUsingExtinguisher)
        {
            extinguisherPowder -= Time.deltaTime;
        }
    }

    private void CheckExtinguisherPowder()
    {
        if (extinguisherPowder <= 0f)
        {
            extinguisherPowder = 0f;
            gameState.isExtinguisherEmpty = true;
            gameState.isUsingExtinguisher = false;
            StopExtinguisher();
        }
    }

    private void UseExtinguisher()
    {
        CheckExtinguisherPowder();
        StartExtinguisher();
        ReducePowder();
        StopExtinguisher();
    }

    private void StartPowder()
    {
        if (gameState.isUsingExtinguisher && particleCoroutine == null)
        {
            particleCoroutine = StartCoroutine(UsePowderCoroutine(true));
        }
    }

    private void StopPowder()
    {
        if (!gameState.isUsingExtinguisher && particleCoroutine != null)
        {
            StopCoroutine(particleCoroutine);
            particleCoroutine = StartCoroutine(UsePowderCoroutine(false));
        }
    }

    private IEnumerator UsePowderCoroutine(bool isActive)
    {
        yield return new WaitForSeconds(particleDelay);

        if (isActive)
        {
            powderParticle.Play();
        }
        else
        {
            powderParticle.Stop();
            particleCoroutine = null;
        }
    }

    // Changes Extinguisher height by slider value
    public void ChangePosition(float value)
    {
        transform.position = new Vector3(transform.position.x, value, transform.position.z);
    }

    public float GetExtinguisherPowder() { return extinguisherPowder; }

}
