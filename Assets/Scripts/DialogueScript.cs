using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueScript : MonoBehaviour
{
    public GameObject dialogueWindowPrefab;
    public TextMeshProUGUI dialogueText;
    private string computedInfo;

    // Start is called before the first frame update
    void Start()
    {
        computedInfo = "Your score is: " + Random.Range(0, 100).ToString();
    }

    public void ShowDialogue()
    {
        GameObject dialogueWindow = Instantiate(dialogueWindowPrefab, transform);

        dialogueText = dialogueWindow.GetComponentInChildren<TextMeshProUGUI>();
        dialogueText.text = computedInfo;

        // Close button
        Button closeButton = dialogueWindow.GetComponentInChildren<Button>();
        if(closeButton != null)
            closeButton.onClick.AddListener(() => CloseDialogue(dialogueWindow));
    }

    void CloseDialogue(GameObject dialogueWindow)
    {
        Destroy(dialogueWindow);
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
