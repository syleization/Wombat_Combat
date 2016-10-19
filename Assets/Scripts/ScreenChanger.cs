using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScreenChanger : MonoBehaviour {

	// Use this for initialization
	public void Play()
    {
	    SceneManager.LoadScene( "Test");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
