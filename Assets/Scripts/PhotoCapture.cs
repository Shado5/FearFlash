using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private bool showText = true;

    public GameObject gameObjectToDisable;

    private int shotsLeft = 5;

    private void Start()
    {
        UpdateObjectsText();
        backgroundImage.gameObject.SetActive(true);
        objectsText.gameObject.SetActive(true);

        screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        shadowMonster.SetActive(false);
        wallCrawler.SetActive(false);
        kitchenText.SetActive(false);
        sittingPeople.SetActive(false);
    }



    private void Update()
    {
        shotsLeftText.text = "Shots left " + shotsLeft; // display the remaining shots

        if (shotsLeft <= 0 && !cameraAudio.isPlaying) // check if there are no shots left and audio is not playing
        {
            outAudio.Play(); // play the audio
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            showText = !showText;
            backgroundImage.gameObject.SetActive(showText);
            objectsText.gameObject.SetActive(showText);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (shotsLeft > 0) // check if there are shots left
            {
                TakePicture();
                shotsLeft--; // decrement shots left
                StartCoroutine(GetRidOfPhoto(_getridofphototime));

                if (!viewingPhoto && canTakePhoto)
                {
                    StartCoroutine(CameraFlashEffect());
                    StartCoroutine(CapturePhoto());
                }
            }
        }
        

        if (Input.GetKeyDown(KeyCode.R)) // check if the player presses "R"
        {
            StartCoroutine(ReloadTime(_reloadTime));
            reloadBar.SetActive(true);
            

        }
    }
    private void CheckForCompletion()
    {
        if (objectsToTakePicturesOf.Count == 0)
        {
            objectsText.text = "Enter House";


        }
    }

    void Reload()
    {
        shotsLeft = 5;
        reloadBar.SetActive(false);
    }

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
                    UpdateObjectsText();

                    //if (objectsToTakePicturesOf.Count == objectsToTakePicturesOf.Count / 3)
                    //{
                    //    manScream.Play();
                    //}
                }

                else
                {
                    Debug.Log("Camera not pointed at an object that needs a picture taken.");
                }

            }

        }
        else
        {
            Debug.Log("All objects have been taken pictures of.");
        }
    }
    void UpdateObjectsText()
    {
        string objectsTextString = "\n";
        foreach (GameObject obj in objectsToTakePicturesOf)
        {
            objectsTextString += takenPicturesOfObjects.Contains(obj) ? "<s>" + obj.name + "</s>\n" : obj.name + "\n";
        }
        objectsText.text = objectsTextString;
    }

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

        //StartCoroutine(RemovePhotoAfterDelay(5f));

    }
    IEnumerator RemovePhotoAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        RemovePhoto();
    }

    IEnumerator ResetCanTakePhotoAfterDelay(float delay)
    {
        yield return new WaitForSeconds(1);
        canTakePhoto = true;
    }

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
}
