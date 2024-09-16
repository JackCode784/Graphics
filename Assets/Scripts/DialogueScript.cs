using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueScript : MonoBehaviour
{
    public GameObject dialogueWindowPrefab;
    private TextMeshProUGUI dialogueText;

    // Call this method to pass in the computed information from another script
    public void ShowDialogue(string computedInfo)
    {
        // Instantiate the dialogue window from prefab
        GameObject dialogueWindow = Instantiate(dialogueWindowPrefab, transform);

        // Find the TextMeshProUGUI component and update the text
        dialogueText = dialogueWindow.GetComponentInChildren<TextMeshProUGUI>();
        if(dialogueText != null)
            dialogueText.text = computedInfo;
        else Debug.LogError("DialogueText component not found!");

        // Optional: Handle the close button (if it exists)
        Button closeButton = dialogueWindow.GetComponentInChildren<Button>();
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(() => CloseDialogue(dialogueWindow));
        }
    }

    // Function to close the dialogue window
    void CloseDialogue(GameObject dialogueWindow)
    {
        Destroy(dialogueWindow); // Destroy the dialogue window to close it
    }
}
