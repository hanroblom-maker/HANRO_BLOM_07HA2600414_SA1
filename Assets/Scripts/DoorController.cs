using UnityEngine;

public class DoorController : MonoBehaviour
{   
    // For keeping the door locked or unlocked
    private bool isUnlocked = false;

    // Refereces back to The GameManager
    [SerializeField] private GameManager gameManager;

    public void UnlockDoor()
    {
        isUnlocked = true;

        Debug.Log("Door unlocked");
    }

    public void AttemptExit()
    {
        if (isUnlocked)
        {
            gameManager.CheckWinCondition();
        }
        else
        {
            gameManager.SetStatusMessage("The door is locked.");
        }
    }

    public bool IsUnlocked()
    {
        return isUnlocked;
    }
}

