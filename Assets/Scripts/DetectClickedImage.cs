using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DetectClickedImage : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
            DetectImage();
    }

    void DetectImage()
    {
        // Check if the mouse is over any UI element
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        // Raycast to all UI elements under the mouse
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        // Loop through all results
        foreach (RaycastResult result in raycastResults)
        {
            GameObject clickedObject = result.gameObject;
         
            // Exclude close buttons based on their layer
            if (clickedObject.layer == LayerMask.NameToLayer("Ignore Raycast"))
            {
                // Debug.Log("Ignored button!");
                continue;
            }

            // Check if the clicked object has an Image component
            if (clickedObject.GetComponent<Image>() != null || clickedObject.GetComponent<RawImage>() != null)
            {
                GetComponent<GetInferenceOnClick>().GetInference(clickedObject);
            }
        }
    }   
}
