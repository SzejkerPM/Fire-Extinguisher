using UnityEngine;
using UnityEngine.UI;

public class FireUI : MonoBehaviour, IActivateUI, IDeactivateUI
{
    [SerializeField]
    private GameState gameState;

    [SerializeField]
    private FireController fireController;

    [SerializeField]
    private Slider fireHealthBar;

    private void Update()
    {
        if (gameState.isUIStarted)
        {
            UpdateFireHealthBar();
        }
    }

    private void UpdateFireHealthBar()
    {
        fireHealthBar.value = fireController.GetFireHealth;
    }

    public void ActivateUI()
    {
        fireHealthBar.gameObject.SetActive(true);
    }

    public void DeactivateUI()
    {
        fireHealthBar.gameObject.SetActive(false);
    }

}
