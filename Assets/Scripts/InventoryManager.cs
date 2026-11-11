using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private int keyCount = 0;
    private const int totalKeys = 5;
    
    [SerializeField] private DoorController doorController;
    [SerializeField] private GameManager gameManager;
 
    public void AddKey()
    {
        keyCount++;

        gameManager.UpdateUI();


        if (HasAllKeys())
        {
            doorController.UnlockDoor();
        }

    }

    public bool HasAllKeys()
    {
        return keyCount >= totalKeys;
    }

    public int GetKeyCount()
    {
        return keyCount;
    }
}
