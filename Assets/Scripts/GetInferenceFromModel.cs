using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.UI;

public class GetInferenceFromModel : MonoBehaviour
{
    public NNModel onnxAsset;
    private DialogueScript dialogueScript;
    private Texture2D imageToRecognise;
    private IWorker worker;
    private string computedInfo;
    private readonly string[] classes = {"beach ball", "rugby ball"};
    
    // Start is called before the first frame update
    void Start()
    {
        imageToRecognise = (Texture2D)(GetComponent<RawImage>().texture);    
        dialogueScript = GetComponent<DialogueScript>();

        try
        {
            using(var worker = onnxAsset.CreateWorker())
            {
                using(var input = new Tensor(imageToRecognise, channels: 3))
                {
                    var output = worker.Execute(input).PeekOutput();
                    computedInfo = ComputeClass(output.ToReadOnlyArray());
                    dialogueScript.ShowDialogue(computedInfo);
                }
            }
        }
        catch(System.Exception e)
        {
            Debug.LogError("Error during model execution: " + e.Message);
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
