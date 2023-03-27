using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneOver : MonoBehaviour
{
    public float _cutsceneTime = 30f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CutSceneTime(_cutsceneTime));
    }

    public IEnumerator CutSceneTime(float t)
    {
        yield return new WaitForSeconds(t);

        SceneManager.LoadScene("Tutorial");
    }
}
