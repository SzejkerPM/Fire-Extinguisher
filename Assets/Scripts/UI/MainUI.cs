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
        textTutorial = "\n\nKliknij w ekran, aby kontyunowaæ.";

        textWelcome = "Witaj w mojej grze edukacyjnej!\nTa gra ma na celu pokazaæ podstawy obs³ugi gaœnicy w sposób interaktywny i anga¿uj¹cy. " +
            "Pamiêtaj jednak, ¿e jest to uproszczone przedstawienie i nie zast¹pi profesjonalnego szkolenia z zakresu BHP. " +
            "Zawsze zasiêgnij porady eksperta w przypadku rzeczywistych sytuacji zwi¹zanych z po¿arem.";

        textBolt = "Najpierw musisz odbezpieczyæ gaœnicê przez wyci¹gniêcie zawleczki zabezpieczaj¹cej przed przypadkowym uruchomieniem." +
            "\n\nKliknij na zawleczkê, aby to zrobiæ.";

        textHose = "Teraz musisz ustawiæ w¹¿ gaœnicy przed ni¹, aby ukierowaæ go na ogieñ." +
            "\n\nKliknij w koñcówkê wê¿a, aby to zrobiæ.";

        textHeight = "Za pomoc¹ suwaka po lewej stronie ekranu bêdziesz móg³/mog³a sterowaæ wysokoœci¹ gaœnicy, aby odpowiednio j¹ ustawiæ." +
            "\nSuwak jest jeszcze zablokowany." + textTutorial;

        textHolder = "Z gaœnicy mo¿esz korzystaæ przez zaciœniêcie r¹czki. W grze symulujemy to przez przytrzymanie w tym miejscu lewego przycisku myszy." +
            "\nGaœnica jest jeszcze zablokowana." + textTutorial;

        textPowder = "Proszek w gaœnicy wystarczy na oko³o 10 sekund, pod gaœnic¹ znajduje siê pasek przedstawiaj¹cy ile proszku Ci pozosta³o." + textTutorial;

        textFire = "Ogieñ w grze ugasisz po oko³o 6 sekundach ci¹g³ego u¿ywania gaœnicy. Pamiêtaj, ¿e musisz celowaæ w podstawê ognia. " +
            "Inaczej nie dasz rady ugasiæ go w ca³oœci i wznieci siê na nowo! Poni¿ej p³on¹cego obiektu znajdziesz pasek przedstawiaj¹cy wytrzyma³oœæ ognia."
            + textTutorial;

        textGoodluck = "To tyle!\n\nPowodzenia!\n\nKliknij w ekran, aby rozpocz¹æ grê.";

        textWin = "Gratulacje!\n\nUda³o Ci siê ugasiæ po¿ar!";
        textLost = "Niestety!\n\nNie uda³o Ci siê. \n\nSpróbuj ponownie!";
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
