using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    [SerializeField]
    private GameState gameState;

    [SerializeField]
    private ExtinguisherUI extinguisherUI;

    [SerializeField]
    private FireUI fireUI;

    [SerializeField]
    private Button startButton;

    [SerializeField]
    private Button restartButton;

    #region Tutorial

    private string textWelcome, textTutorial, textBolt, textHose, textHolder, textHeight, textPowder, textFire, textGoodluck, textWin, textLost, textMe;

    [SerializeField]
    private GameObject tutorialArrowBolt;

    [SerializeField]
    private GameObject tutorialArrowHose;

    [SerializeField]
    private GameObject tutorialArrowFire;

    [SerializeField]
    private GameObject tutorialArrowHolder;

    [SerializeField]
    private GameObject textPrefab;

    [SerializeField]
    private GameObject parentTextObject;

    private GameObject currentText;

    #endregion


    private void Start()
    {
        SetStrings();
        CreateTextPanel(textWelcome);
    }

    private void Update()
    {
        ShowFirstTextsAndArrows();
        ShowTutorialWithCounter();

        if (gameState.isThirdStageStarted)
        {
            DeavtiveAllUI();
            ShowResult();
        }
    }

    private void CreateTextPanel(string text)
    {
        if (currentText != null)
        {
            Destroy(currentText);
        }
        currentText = Instantiate(textPrefab, parentTextObject.transform);
        currentText.GetComponentInChildren<Text>().text = text;
    }

    private void SetStrings()
    {
        textTutorial = "\n\nKliknij w ekran, aby kontyunowa�.";

        textWelcome = "Witaj w mojej grze edukacyjnej!\nTa gra ma na celu pokaza� podstawy obs�ugi ga�nicy w spos�b interaktywny i anga�uj�cy. " +
            "Pami�taj jednak, �e jest to uproszczone przedstawienie i nie zast�pi profesjonalnego szkolenia z zakresu BHP. " +
            "Zawsze zasi�gnij porady eksperta w przypadku rzeczywistych sytuacji zwi�zanych z po�arem.";

        textBolt = "Najpierw musisz odbezpieczy� ga�nic� przez wyci�gni�cie zawleczki zabezpieczaj�cej przed przypadkowym uruchomieniem." +
            "\n\nKliknij na zawleczk�, aby to zrobi�.";

        textHose = "Teraz musisz ustawi� w�� ga�nicy przed ni�, aby ukierowa� go na ogie�." +
            "\n\nKliknij w ko�c�wk� w�a, aby to zrobi�.";

        textHeight = "Za pomoc� suwaka po lewej stronie ekranu b�dziesz m�g�/mog�a sterowa� wysoko�ci� ga�nicy, aby odpowiednio j� ustawi�." +
            "\nSuwak jest jeszcze zablokowany." + textTutorial;

        textHolder = "Z ga�nicy mo�esz korzysta� przez zaci�ni�cie r�czki. W grze symulujemy to przez przytrzymanie w tym miejscu lewego przycisku myszy." +
            "\nGa�nica jest jeszcze zablokowana." + textTutorial;

        textPowder = "Proszek w ga�nicy wystarczy na oko�o 10 sekund, pod ga�nic� znajduje si� pasek przedstawiaj�cy ile proszku Ci pozosta�o." + textTutorial;

        textFire = "Ogie� w grze ugasisz po oko�o 6 sekundach ci�g�ego u�ywania ga�nicy. Pami�taj, �e musisz celowa� w podstaw� ognia. " +
            "Inaczej nie dasz rady ugasi� go w ca�o�ci i wznieci si� na nowo! Poni�ej p�on�cego obiektu znajdziesz pasek przedstawiaj�cy wytrzyma�o�� ognia."
            + textTutorial;

        textGoodluck = "To tyle!\n\nPowodzenia!\n\nKliknij w ekran, aby rozpocz�� gr�.";

        textWin = "Gratulacje!\n\nUda�o Ci si� ugasi� po�ar!";
        textLost = "Niestety!\n\nNie uda�o Ci si�. \n\nSpr�buj ponownie!";
    }

    private void ActiveAllUI()
    {
        if (!gameState.isUIStarted)
        {
            extinguisherUI.ActivateUI();
            fireUI.ActivateUI();
            gameState.isUIStarted = true;
        }
    }

    private void DeavtiveAllUI()
    {
        if (gameState.isUIStarted)
        {
            extinguisherUI.DeactivateUI();
            fireUI.DeactivateUI();
            gameState.isUIStarted = false;
        }
    }

    private void ShowFirstTextsAndArrows()
    {
        if (!gameState.isBoltAnimationCompleted && gameState.isFirstStageStarted)
        {
            CreateTextPanel(textBolt);
            tutorialArrowBolt.SetActive(true);
        }

        if (gameState.isBoltAnimationCompleted && !gameState.isHoseAnimationCompleted)
        {
            CreateTextPanel(textHose);
            Destroy(tutorialArrowBolt);
            tutorialArrowHose.SetActive(true);
        }
    }

    private void ShowTutorialWithCounter()
    {
        switch (gameState.textCounter)
        {
            case 1:
                Destroy(tutorialArrowHose);
                ActiveAllUI();
                CreateTextPanel(textHeight);
                break;
            case 2:
                CreateTextPanel(textHolder);
                tutorialArrowHolder.SetActive(true);
                break;
            case 3:
                Destroy(tutorialArrowHolder);
                CreateTextPanel(textPowder);
                break;
            case 4:
                CreateTextPanel(textFire);
                tutorialArrowFire.SetActive(true);
                break;
            case 5:
                Destroy(tutorialArrowFire);
                CreateTextPanel(textGoodluck);
                break;
            case 6:
                Destroy(currentText);
                gameState.isInputTutorialOn = false;
                gameState.textCounter = -1;
                break;
        }
    }

    private void ShowResult()
    {
        if (gameState.isGameLost && !gameState.isResultVisible)
        {
            AudioManager.Instance.PlayUISound("lose");
            CreateTextPanel(textLost);
            gameState.isResultVisible = true;
        }
        else if (gameState.isGameWon && !gameState.isResultVisible)
        {
            AudioManager.Instance.PlayUISound("win");
            CreateTextPanel(textWin);
            gameState.isResultVisible = true;
        }

        restartButton.gameObject.SetActive(true);
    }

    public void OnClickStartGame()
    {
        AudioManager.Instance.PlayUISound("click");
        gameState.isFirstStageStarted = true;
        gameState.areAnimationsUnlocked = true;
        startButton.gameObject.SetActive(false);
        CreateTextPanel(textBolt);
    }

    public void OnClickRestartGame()
    {
        AudioManager.Instance.PlayUISound("click");
        SceneManager.LoadScene("Game");
    }

}
