using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    private bool created = false;
    private int counter;

    private void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(transform.gameObject);
            created = true;
            counter = 0;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0 && counter != 0)
        {
            Destroy(this.gameObject);
        }
        ++counter;
    }
}
