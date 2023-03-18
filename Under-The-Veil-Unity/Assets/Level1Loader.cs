using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1Loader : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("Level 1");
    }
}