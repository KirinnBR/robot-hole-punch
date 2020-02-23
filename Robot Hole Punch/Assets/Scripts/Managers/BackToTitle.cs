using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToTitle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Title());
    }

    IEnumerator Title()
    {
        yield return new WaitForSeconds(20);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }
}
