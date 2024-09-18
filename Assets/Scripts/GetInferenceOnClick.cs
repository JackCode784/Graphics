using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.UI;

// To be attached to main camera in the Main Level scene
public class GetInferenceOnClick : MonoBehaviour
{
    public NNModel onnxAsset;
    private DialogueScriptOnClick dialogueScript;
    private RawImage imageToRecognise;
    private IWorker worker;
    private string inference;
    private readonly string[] classes = {"beach ball", "rugby ball"};

    public void GetInference(GameObject clickedObject)
    {
        // Debug.Log("Get Inference called!");  // test
        dialogueScript = GetComponent<DialogueScriptOnClick>();
        imageToRecognise = clickedObject.GetComponent<RawImage>();

        try
        {
            using(var worker = onnxAsset.CreateWorker())
            {
                using(var input = new Tensor((Texture2D)(imageToRecognise.texture), channels: 3))
                {
                    var output = worker.Execute(input).PeekOutput();
                    inference = ComputeClass(output.ToReadOnlyArray());
                    dialogueScript.ShowDialogue(inference, imageToRecognise);
                }
            }
        }
        catch(System.Exception e)
        {
            Debug.LogError("Error during model execution " + e.Message);
        }
    }

        string ComputeClass(float[] nnOutput)
    {
        float maxProb = Mathf.Max(nnOutput);
        int maxIndex = FindMaxIndex(nnOutput);

        return "Image was recognised as " + classes[maxIndex].ToString() +
         " with probability " + (maxProb*100).ToString() + "%"; 
    }

    int FindMaxIndex(float[] arr)
    {
        int maxIdx = 0;

        for(int i = 0; i < arr.Length; i++)
        {
            if(arr[i] > arr[maxIdx])
                maxIdx = i;
        }

        return maxIdx;
    }
}
