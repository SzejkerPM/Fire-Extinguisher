using UnityEngine;

[CreateAssetMenu(fileName = "GameState", menuName = "ScriptableObjects/GameState")]

public class GameState : ScriptableObject
{
    #region First Stage

    public bool isFirstStageStarted;

    public bool areAnimationsUnlocked;

    public bool isBoltAnimationCompleted;
    public bool isHoseAnimationCompleted;

    public bool isCurrentlyAnimating;
    public bool areInitialAnimationsAvaible;

    public bool isInputTutorialOn;
    public int textCounter;

    public bool canPlayerUseUI;

    #endregion

    #region Second Stage

    public bool isSecondStageStarted;

    public bool isUIStarted;

    public bool isUsingExtinguisher;

    public bool isFireCoreHit;
    public bool isFireOuterHit;

    public bool isFireDefeated;
    public bool isExtinguisherEmpty;

    #endregion

    #region Third Stage

    public bool isThirdStageStarted;

    public bool isGameWon;
    public bool isGameLost;

    public bool isResultVisible;

    #endregion
}

// First stage stands for initial animations and preparations
// Seconds stage stands for actual game
// Third stage stands for closing game
