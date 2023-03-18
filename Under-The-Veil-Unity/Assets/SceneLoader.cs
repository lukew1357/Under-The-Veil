using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public float loadTimer;
    public string sceneName;
    public void Update()
    {
        loadTimer -= Time.deltaTime;
        if (loadTimer <= 0)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
