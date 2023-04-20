using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpscareIntoEndscreen : MonoBehaviour
{
    public float _jumpscareLength = 5f;

    private void Awake()
    {
        StartCoroutine(JumpScareToEnd(_jumpscareLength));
    }

    public IEnumerator JumpScareToEnd(float t)
    {
        yield return new WaitForSeconds(t);

        SceneManager.LoadScene("EndScreen");
    }
}
