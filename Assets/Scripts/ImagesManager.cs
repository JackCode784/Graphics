using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class ImagesManager : MonoBehaviour
{
    private string resourceImagesPath = "Datasets/BallsDataset/test/";   // path da cui prendere le immagini
    private readonly int numImages = 5; // numero di immagini da generare
    private GameObject imagesCanvas;    // Canvas dove posizionare le immagini
    public GameObject rawImagePrefab;   // Prefab delle immagini

    // Start is called before the first frame update
    void Start()
    {
        imagesCanvas = GameObject.Find("ImagesCanvas");
        CreateImages(numImages);
        RandomizeImages(resourceImagesPath, numImages);        
    }
    
    // Crea numImages istanze di RawImage sulla ImagesCanvas
    void CreateImages(int numImages)
    {
        float spacing = 20f; // Horizontal spacing between images

        RectTransform canvasRect = imagesCanvas.GetComponent<RectTransform>();
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        // Calculate the total space occupied by all images and the spacing between them
        float totalSpacing = spacing * (numImages - 1);
        float totalImageWidth = Mathf.Min((canvasWidth - totalSpacing) / numImages, canvasHeight * 0.8f); // Image size
        float totalOccupiedWidth = totalImageWidth * numImages + totalSpacing; // Total width occupied by images and spacing

        // Starting y-position for images (centered vertically)
        float yPos = canvasHeight / 2;

        // Calculate the initial x-position to center all images horizontally
        float startXPos = -(totalOccupiedWidth / 2) + totalImageWidth / 2; // Start centered on canvas

        for (int i = 0; i < numImages; i++)
        {
            // Instantiate the RawImage prefab
            GameObject newImage = Instantiate(rawImagePrefab, imagesCanvas.transform);
            newImage.name = "Picture #" + (i + 1).ToString();
            newImage.GetComponentInChildren<TextMeshProUGUI>().text = newImage.name;
            RectTransform imageRect = newImage.GetComponent<RectTransform>();

            // Set the size of the RawImage as a square (equal width and height)
            imageRect.sizeDelta = new Vector2(totalImageWidth, totalImageWidth);

            // Calculate the x-position for each image, leaving space between them
            float xPos = startXPos + i * (totalImageWidth + spacing);

            // Set the anchored position of the image in the canvas
            imageRect.anchoredPosition = new Vector2(xPos, yPos - (canvasHeight / 2));
        }

        Debug.Log(numImages.ToString() + " images have been created and centered!");
    }


    // Genera numImages nuove immagini a caso da imagesPath
    void RandomizeImages(string imagesPath, int numImages)
    {
        // Lista di tutte le immagini vuote
        List<RawImage> children = new List<RawImage>();
        foreach(Transform child in imagesCanvas.transform)
        {
            if (child.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
                children.Add(child.GetComponent<RawImage>());
        }
        // Debug.Log("Numero figli di ImagesCanvas: " + children.Count); // bug-related

        // Lista delle immagini
        imagesPath = imagesPath.Replace("\\", "/").TrimEnd('/');

        // Restituisce i nomi di tutti i file in imagesPath, anche sottocartelle e con qualunque nome
        string[] texturePaths = Directory.GetFiles("Assets/Resources/" + imagesPath, "*.*", SearchOption.AllDirectories);
        List<string> validExtensions = new List<string>() {".png", ".jpg", ".jpeg"};
        List<string> validImagesPath = new List<string>();

        // Esclude le estensioni scorrette
        foreach(string imagePath in texturePaths)
            if(validExtensions.Contains(Path.GetExtension(imagePath).ToLower()))
                {
                    validImagesPath.Add(resourceImagesPath + Path.GetFileNameWithoutExtension(imagePath));
                }
        // Se ci sono meno immagini di quelle che si vuole mostrare
        numImages = Mathf.Min(numImages, validImagesPath.Count);

        // Carica le immagini
        for(int i = 0; i < numImages; i++)
        {
            // Indice random tra quelli validi
            int rndIdx = Random.Range(0, validImagesPath.Count);
            string assetPath = validImagesPath[rndIdx].Replace(Application.dataPath, "").Replace("\\", "/");
            validImagesPath.RemoveAt(rndIdx);   // rimuovi immagine inserita
            Texture2D texture = Resources.Load<Texture2D>(assetPath);

            // Metti l'immagine se esiste
            if(texture != null)
            {
                children[i].texture = texture;
            }
        }
        
        Debug.Log(numImages.ToString() + " immagini sono state randomizzate!");
    }

    // Distrugge tutte le immagini figlie della ImagesCanvas
    // ------------------------------------------------------
    // A causa di un bug, in realtà rimuove solo le texture delle
    // immagini figlie di imagesCanvas.
    void DestroyTextures()
    {
        /* 
        Provando a distruggere le immagini preesistenti con Destroy(child.gameObject) 
        portava a un bug: le immagini rimanevano grigie (senza texture) 
        sulla imagesCanvas, e la lista children all'interno di RandomizeImages pareva 
        pareva avesse 2*numImages elementi invece che numImages.
        */ 
        foreach(Transform child in imagesCanvas.transform)
        {
            if (child.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
            {
                // Se non ho chiuso il dialogo, lo chiudo
                DialogueScriptOnClick dialogueScript = GetComponent<DialogueScriptOnClick>();
                if (dialogueScript.HasAlreadyDialogueWindow(child.gameObject, "DialoguePanel"))
                {
                    dialogueScript.CloseDialogue(child.gameObject.transform.Find("DialoguePanel").gameObject);
                }
                child.gameObject.GetComponent<RawImage>().texture = null;   // rimozione texture
                
                // Destroy(child.gameObject);   // bug
            }
        }

        Debug.Log("Textures rimosse!");
    }

    // Pulsante di refresh
    public void RefreshImages()
    {
        DestroyTextures();
        RandomizeImages(resourceImagesPath, numImages);
    }
}
