using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using UnityEngine;

public class sampleCodeSnippet : MonoBehaviour
{
    public NNModel onnxAsset;
    public Texture2D imageToRecognise;
    private IWorker worker;
    
    // Start is called before the first frame update
    void Start()
    {
        worker = onnxAsset.CreateWorker();

        using(var input = new Tensor(imageToRecognise, channels: 3))
        {
            var output = worker.Execute(input).PeekOutput();
            var highestProbabilityIdx = output[0];

            UnityEngine.Debug.Log($"Image was recognised as class number: " + output[0] + " " + output[1]);
        }
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
