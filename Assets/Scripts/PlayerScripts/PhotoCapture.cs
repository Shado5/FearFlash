using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using System;

public class PhotoCapture : MonoBehaviour
{
    [Header("Photo Taker")]
    [SerializeField] private Image photoDisplayArea;
    [SerializeField] private GameObject photoFrame;

    [Header("Monsters")]
    [SerializeField] public GameObject shadowMonster;
    [SerializeField] public GameObject wallCrawler;
    [SerializeField] public GameObject kitchenText;
    [SerializeField] public GameObject sittingPeople;


    [Header("Flash Effect")]
    [SerializeField] private GameObject cameraFlash;
    [SerializeField] private float flashTime;

    [Header("Photo Fader Effect")]
    [SerializeField] private Animator fadingAnimation;

    [Header("Audio")]
    [SerializeField] private AudioSource cameraAudio;
    [SerializeField] private AudioSource manScream;
    [SerializeField] private AudioSource outAudio;

    [SerializeField] private float _getridofphototime = 3f;
    [SerializeField] private float _reloadTime = 3f;
    [SerializeField] private float _ePrompt = 15f;
    [SerializeField] private float _removeTask = 3f;

    private Texture2D screenCapture;
    private bool viewingPhoto;
    private bool canTakePhoto = true;

    public List<GameObject> objectsToTakePicturesOf;
    private List<GameObject> takenPicturesOfObjects = new List<GameObject>();
    public Camera mainCamera;

    public TMP_Text objectsText;
    public Image backgroundImage;
    public TMP_Text shotsLeftText;
    public GameObject reloadBar;
    public TMP_Text rToReload;
    public TMP_Text ePrompt;

    private bool showText = true;

    public GameObject gameObjectToDisable;

    private int shotsLeft = 5;

    private void Start()
    {
        //turns on list of objects
        UpdateObjectsText();
        backgroundImage.gameObject.SetActive(true);
        objectsText.gameObject.SetActive(true);

        //turns off mesh of invisible items
        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        shadowMonster.SetActive(false);
        wallCrawler.SetActive(false);
        kitchenText.SetActive(false);
        sittingPeople.SetActive(false);

        //starts timer for e prompt
        StartCoroutine(EPrompt(_ePrompt));

        //turns on r prompt
        rToReload.enabled = true;
    }

    private void Update()
    {
        shotsLeftText.text = "Shots left " + shotsLeft; // display the remaining shots

        //hides UI when e is pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            showText = !showText;
            backgroundImage.gameObject.SetActive(showText);
            objectsText.gameObject.SetActive(showText);
            ePrompt.gameObject.SetActive(false);
        }

        //takes photo
        if (Input.GetMouseButtonDown(0))
        {
            if (shotsLeft > 0) // check if there are shots left
            {
                if (!viewingPhoto && canTakePhoto)
                {
                    TakePicture();
                    shotsLeft--; // decrement shots left
                    //starts timers
                    StartCoroutine(GetRidOfPhoto(_getridofphototime));
                    StartCoroutine(CameraFlashEffect());
                    StartCoroutine(CapturePhoto());
                }
            }
        }
        //tells you to reload
        if(shotsLeft == 0)
        {
            rToReload.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.R)) // check if the player presses "R"
        {
            //reload timer starts
            if (shotsLeft == 0)
            {
                rToReload.enabled = false;
                StartCoroutine(ReloadTime(_reloadTime));
                reloadBar.SetActive(true);
            }
        }
    }
    //checks if all objects have photos taken
    private void CheckForCompletion()
    {
        if (objectsToTakePicturesOf.Count == 0)
        {
            if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Tutorial"))
            {
                objectsText.text = "Enter House";
            }
        }
    }

    //reloads shots
    void Reload()
    {
        shotsLeft = 5;
        reloadBar.SetActive(false);
    }

    //takes picture of object that needs to be photographed
    void TakePicture()
    {
        if (objectsToTakePicturesOf.Count > 0)
        {
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                if (objectsToTakePicturesOf.Contains(hitObject))
                {
                    Debug.Log("Taking picture of: " + hitObject.name);
                    takenPicturesOfObjects.Add(hitObject);
                    objectsToTakePicturesOf.Remove(hitObject);
                    string objectsTextString = "\n";
                    foreach (GameObject obj in objectsToTakePicturesOf)
                    {

                        objectsTextString += takenPicturesOfObjects.Contains(obj) ? "<s>" + obj.name + "</s>\n" : obj.name + "\n";

                    }
                    
                    objectsText.text = objectsTextString;
                    objectsText.text = "<s>" + hitObject.name + "</s>" + objectsTextString ; //crosses out object
                    

                }

                //object does not need a photo
                else
                {
                    Debug.Log("Camera not pointed at an object that needs a picture taken.");
                }

            }

        }
        //all objects have been photographed
        else
        {
            Debug.Log("All objects have been taken pictures of.");
        }
    }

    //removes name when photo is taken
    void UpdateObjectsText()
    {
        string objectsTextString = "\n";
        foreach (GameObject obj in objectsToTakePicturesOf)
        {
            
            objectsTextString += takenPicturesOfObjects.Contains(obj) ? "<s>" + obj.name + "</s>\n" : obj.name + "\n";
            
        }
        objectsText.text = objectsTextString;
    }

    //shows invisible items when photo is taken
    IEnumerator CapturePhoto()
    {
        //CameraUI Set False
        viewingPhoto = true;
        canTakePhoto = false;

        // Make the game object visible
        shadowMonster.SetActive(true);
        wallCrawler.SetActive(true);
        kitchenText.SetActive(true);
        sittingPeople.SetActive(true);
        yield return new WaitForEndOfFrame();

        Rect reigonToRead = new Rect(0, 0, Screen.width, Screen.height);

        screenCapture.ReadPixels(reigonToRead, 0, 0, false);
        screenCapture.Apply();
        ShowPhoto();
    }
    //displays photo taken
    void ShowPhoto()
    {
        Sprite photoSprite = Sprite.Create(screenCapture, new Rect(0.0f, 0.0f, screenCapture.width, screenCapture.height), new Vector2(0.5f, 0.5f), 100.0f);
        photoDisplayArea.sprite = photoSprite;

        photoFrame.SetActive(true);

        StartCoroutine(ResetCanTakePhotoAfterDelay(flashTime));
        fadingAnimation.Play("PhotoFade");

        shadowMonster.SetActive(true);
        shadowMonster.SetActive(false);

        wallCrawler.SetActive(true);
        wallCrawler.SetActive(false);

        kitchenText.SetActive(true);
        kitchenText.SetActive(false);

        sittingPeople.SetActive(true);
        sittingPeople.SetActive(false);
    }
    //removes photo taken
    IEnumerator RemovePhotoAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RemovePhoto();
    }
    //can take photo after previous one
    IEnumerator ResetCanTakePhotoAfterDelay(float delay)
    {
        yield return new WaitForSeconds(1);
        canTakePhoto = true;
    }
    //camera flash
    IEnumerator CameraFlashEffect()
    {
        cameraAudio.Play();
        cameraFlash.SetActive(true);
        yield return new WaitForSeconds(flashTime);
        cameraFlash.SetActive(false);
    }
    
    void RemovePhoto()
    {
        viewingPhoto = false;
        photoFrame.SetActive(false);

        shadowMonster.SetActive(false);

        wallCrawler.SetActive(false);

        kitchenText.SetActive(false);

        sittingPeople.SetActive(false);
    }
    //removes photo
    public IEnumerator GetRidOfPhoto(float t)
    {
        yield return new WaitForSeconds(t);

            if (viewingPhoto)
            {
                RemovePhoto();
            }
            CheckForCompletion();
            if (objectsToTakePicturesOf.Count == 0)
            {
                gameObjectToDisable.SetActive(false);
            }
        

    }

    public IEnumerator ReloadTime(float t)
    {
        yield return new WaitForSeconds(t);
        Reload(); // reload the camera
    }
    public IEnumerator EPrompt(float t)
    {
        yield return new WaitForSeconds(t);

        ePrompt.gameObject.SetActive(false); //turns off e prompt
    }
}