using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueScriptOnClick : MonoBehaviour
{
    public GameObject dialogueWindowPrefab;
    private TextMeshProUGUI dialogueText;

    public void ShowDialogue(string modelInference, RawImage imageToRecognise)
    {
        // Debug.Log("Show Dialogue Called!");  // test

        // Instantiate the dialogue window from prefab
        if(HasAlreadyDialogueWindow(imageToRecognise.gameObject, "DialoguePanel"))
            return;

        GameObject dialogueWindow = Instantiate(dialogueWindowPrefab, imageToRecognise.transform);
        dialogueWindow.name = "DialoguePanel";  // prevent "(Clone)" text in prefab's name

        // Find the TextMeshProUGUI component and update the text
        dialogueText = dialogueWindow.GetComponentInChildren<TextMeshProUGUI>();
        if(dialogueText != null)
            dialogueText.text = modelInference;
        else Debug.LogError("DialogueText component not found!");

        // Optional: Handle the close button (if it exists)
        Button closeButton = dialogueWindow.GetComponentInChildren<Button>();
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(() => CloseDialogue(dialogueWindow));
        }
    }

    // Function to close the dialogue window
    public void CloseDialogue(GameObject dialogueWindow)
    {
        Destroy(dialogueWindow); // Destroy the dialogue window to close it
    }

    bool HasAlreadyDialogueWindow(GameObject parent, string prefabName)
    {
        foreach(Transform child in parent.transform)
        {
            if(child.gameObject.name == prefabName)
                return true;
        }

        return false;
    }
}
