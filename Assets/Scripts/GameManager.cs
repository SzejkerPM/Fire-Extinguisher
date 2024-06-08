using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameState gameState;

    [SerializeField]
    private ExtinguisherController extinguisherController;

    private void Awake()
    {
        ResetAllGameStates();
    }

    void Update()
    {
        ControlGameStages();

        if (gameState.areInitialAnimationsAvaible && gameState.areAnimationsUnlocked)
        {
            GetInputAndStartAnimations();
        }

        if (!gameState.areInitialAnimationsAvaible && gameState.isInputTutorialOn)
        {
            GetInputForTextTutorial();
        }

        if (!gameState.areInitialAnimationsAvaible && gameState.isSecondStageStarted)
        {
            GetInputAndUseExtinguisher();
        }
    }

    private void GetInputForTextTutorial()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameState.textCounter++;
            AudioManager.Instance.PlayUISound("click");
        }
    }

    private void GetInputAndStartAnimations()
    {
        if (Input.GetMouseButtonDown(0) && !gameState.isCurrentlyAnimating)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.transform.CompareTag("Bolt") && !gameState.isBoltAnimationCompleted)
                {
                    gameState.isCurrentlyAnimating = true;
                    extinguisherController.InitialAnimation();
                }
                else if (hit.collider.transform.CompareTag("Hose") && gameState.isBoltAnimationCompleted && !gameState.isHoseAnimationCompleted)
                {
                    gameState.isCurrentlyAnimating = true;
                    extinguisherController.InitialAnimation();
                }
            }
        }
    }

    private void GetInputAndUseExtinguisher()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (gameState.canPlayerUseUI && hit.collider.transform.CompareTag("Holder"))
                {
                    gameState.isUsingExtinguisher = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            gameState.isUsingExtinguisher = false;
        }
    }

    private void ControlGameStages()
    {
        if (!gameState.areInitialAnimationsAvaible && !gameState.isInputTutorialOn && gameState.textCounter == 0)
        {
            gameState.isInputTutorialOn = true;
            gameState.textCounter = 1;
        }

        if (!gameState.areInitialAnimationsAvaible && !gameState.isInputTutorialOn)
        {
            gameState.isFirstStageStarted = false;
            gameState.isSecondStageStarted = true;
            gameState.canPlayerUseUI = true;
        }

        if (gameState.isFireDefeated)
        {
            gameState.isUsingExtinguisher = false;
            gameState.isSecondStageStarted = false;
            gameState.isThirdStageStarted = true;
            gameState.isGameWon = true;
            gameState.isGameLost = false;
        }
        else if (gameState.isExtinguisherEmpty)
        {
            gameState.isUsingExtinguisher = false;
            gameState.isSecondStageStarted = false;
            gameState.isThirdStageStarted = true;
            gameState.isGameWon = false;
            gameState.isGameLost = true;
        }
    }

    private void ResetAllGameStates() // for playing in Unity "fresh" start
    {
        gameState.isFirstStageStarted = false;
        gameState.areAnimationsUnlocked = false;
        gameState.isBoltAnimationCompleted = false;
        gameState.isHoseAnimationCompleted = false;
        gameState.isCurrentlyAnimating = false;
        gameState.areInitialAnimationsAvaible = true;
        gameState.isSecondStageStarted = false;
        gameState.isUIStarted = false;
        gameState.isUsingExtinguisher = false;
        gameState.isFireCoreHit = false;
        gameState.isFireOuterHit = false;
        gameState.isFireDefeated = false;
        gameState.isExtinguisherEmpty = false;
        gameState.isThirdStageStarted = false;
        gameState.isGameWon = false;
        gameState.isGameLost = false;
        gameState.isResultVisible = false;
        gameState.isInputTutorialOn = false;
        gameState.canPlayerUseUI = false;

        gameState.textCounter = 0;
    }
}