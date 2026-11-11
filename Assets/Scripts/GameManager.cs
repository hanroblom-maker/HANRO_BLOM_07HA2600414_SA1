using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Keeps track of player Health
    private int playerHealth = 3;

    private bool gameWon = false;
    private bool gameLost = false;

    // Stores messages for the players current status in the game
    private string statusMessage;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text keysText;
    [SerializeField] private TMP_Text statusText;

    //Call upon Inventory Manager to check the coin count
    [SerializeField] private InventoryManager inventoryManager;

    //To add the Panels for when the player wins or loses
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    private void Start()
    {
        Time.timeScale = 1f;

        UpdateUI();

        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }
    public void TakeDamage()
    {
        playerHealth--;

        Debug.Log("Player damaged");

        UpdateUI();

        CheckLoseCondition();
    }

    public void CheckWinCondition()
    {
        statusMessage = "You escaped!";
        statusText.text = statusMessage;

        winPanel.SetActive(true);

        Time.timeScale = 0f;

        Debug.Log("Game won");
    }

    public void CheckLoseCondition()
    {
        if (playerHealth <= 0)
        {
            statusMessage = "Game Over";
            statusText.text = statusMessage;

            losePanel.SetActive(true);

            Time.timeScale = 0f;

            Debug.Log("Game lost");
        }
    }

    public void UpdateUI()
    {
        healthText.text = "Health: " + playerHealth + " / 3";

        keysText.text = "Keys: " + inventoryManager.GetKeyCount() + " / 5";

        if (playerHealth <= 0)
        {
            statusMessage = "Game Over";
        }
        else if (inventoryManager.HasAllKeys())
        {
            statusMessage = "Door unlocked!";
        }
        else
        {
            statusMessage = "Find the keys.";
        }

        statusText.text = statusMessage;
    }
    public void SetStatusMessage(string newMessage)
    {
        statusMessage = newMessage;
        statusText.text = statusMessage;
    }

    public int GetHealth()
    {
        return playerHealth;
    }
}
