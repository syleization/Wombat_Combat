using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScreenChanger : MonoBehaviour {

	// Use this for initialization
	public void Arena()
    {
	    SceneManager.LoadScene( "TheArena");
	}

    public void Shop()
    {
        SceneManager.LoadScene("Shop");
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Twoplayers()
    {
        SceneManager.LoadScene("HandTest");
    }

    public void Threeplayers()
    {
        SceneManager.LoadScene("HandTest");
    }

    public void Fourplayers()
    {
        SceneManager.LoadScene("HandTest");
    }

    // Update is called once per frame
    void Update () {
	
	}
}
