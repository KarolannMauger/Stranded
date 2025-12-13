using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("Book UI")]
    [SerializeField] private GameObject bookCanvas;
    
    [Header("Help Menu UI")]
    [SerializeField] private GameObject helpCanvas;
    
    [Header("Controllers")]
    [SerializeField] private MonoBehaviour cameraController;
    [SerializeField] private MonoBehaviour playerController;

    public static bool IsBookOpen { get; private set; }
    public static bool IsHelpOpen { get; private set; }

    void Start()
    {
        // Hide both UIs on game start
        if (bookCanvas != null)
            bookCanvas.SetActive(false);
        if (helpCanvas != null)
            helpCanvas.SetActive(false);
            
        IsBookOpen = false;
        IsHelpOpen = false;
    }

    void Update()
    {
        // Tab will open/close book
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleBook();
        }
        
        // ESC will open/close help menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleHelp();
        }
    }

    public void ToggleBook()
    {
        // Closes Help If Opened
        if (IsHelpOpen)
        {
            CloseHelp();
        }
        
        IsBookOpen = !IsBookOpen;
        bookCanvas.SetActive(IsBookOpen);
        
        UpdateControllers();
    }
    
    public void ToggleHelp()
    {
        // Closes Book If Opened
        if (IsBookOpen)
        {
            CloseBook();
        }
        
        IsHelpOpen = !IsHelpOpen;
        helpCanvas.SetActive(IsHelpOpen);
        
        UpdateControllers();
    }
    
    void CloseBook()
    {
        IsBookOpen = false;
        bookCanvas.SetActive(false);
        UpdateControllers();
    }
    
    void CloseHelp()
    {
        IsHelpOpen = false;
        helpCanvas.SetActive(false);
        UpdateControllers();
    }
    
    void UpdateControllers()
    {
        bool anyUIOpen = IsBookOpen || IsHelpOpen;
        
        if (cameraController != null)
            cameraController.enabled = !anyUIOpen;
        
        if (playerController != null)
            playerController.enabled = !anyUIOpen;
        
        // Make cursor visible when any UI is open
        Cursor.visible = anyUIOpen;
        Cursor.lockState = anyUIOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
