using UnityEngine;
using UnityEngine.UI;

public class ButtonClickHandler : MonoBehaviour
{
    public AudioSource clickSound;
    public GameObject Controlpanel;

    private Button button;
    private bool panelIsActive;


    void Start()
    {
        // Get reference to button component
        button = GetComponent<Button>();

        // Attach click listener to button
        button.onClick.AddListener(OnClick);

        // Set panel to inactive at the start
        Controlpanel.SetActive(false);

        // Set panelIsActive to false at the start
        panelIsActive = false;

    }

    void Update()
    {
        // Check if Escape key is pressed and panel is active
        if (Input.GetKeyDown(KeyCode.Escape) && panelIsActive)
        {
            // Deactivate panel
            Controlpanel.SetActive(false);

            // Set panelIsActive to false
            panelIsActive = false;
        }
    }

    public void OnClick()
    {
        // Play click sound
        clickSound.Play();

        panelIsActive = !panelIsActive;

        // Activate or deactivate panel based on panelIsActive
        Controlpanel.SetActive(panelIsActive);
    }
}
