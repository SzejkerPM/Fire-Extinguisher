using UnityEngine;
using UnityEngine.UI;

public class ExtinguisherUI : MonoBehaviour, IActivateUI, IDeactivateUI
{
    [SerializeField]
    private GameState gameState;

    [SerializeField]
    private ExtinguisherController extinguisherController;

    [SerializeField]
    private Slider heightSlider;

    [SerializeField]
    private Slider powderBar;

    [SerializeField]
    private Text powderValueText;

    private void Start()
    {
        heightSlider.onValueChanged.AddListener(extinguisherController.ChangePosition);
    }

    private void Update()
    {
        ControlHeightSlider();

        if (gameState.isUIStarted)
        {
            UpdateExtinguisherPowderBar();
        }
    }

    private void UpdateExtinguisherPowderBar()
    {
        float powderAmount = extinguisherController.GetExtinguisherPowder();
        powderBar.value = powderAmount;

        if (powderAmount == 10)
        {
            powderValueText.text = "FULL";
        }
        else if (powderAmount <= 0)
        {
            powderValueText.text = "EMPTY";
        }
        else
        {
            powderValueText.text = powderAmount.ToString("F1") + "/10";
        }
    }

    private void ControlHeightSlider()
    {
        if (!gameState.canPlayerUseUI && heightSlider.interactable)
        {
            heightSlider.interactable = false;
        }
        else if (gameState.canPlayerUseUI && !heightSlider.interactable)
        {
            heightSlider.interactable = true;
        }
    }

    public void ActivateUI()
    {
        heightSlider.gameObject.SetActive(true);
        powderBar.gameObject.SetActive(true);
    }

    public void DeactivateUI()
    {
        heightSlider.gameObject.SetActive(false);
        powderBar.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        heightSlider.onValueChanged.RemoveListener(extinguisherController.ChangePosition);
    }
}
