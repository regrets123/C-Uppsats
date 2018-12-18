using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    Button playButton;
    Button quitButton;

	// Use this for initialization
	void Start () {


    }

    public void LoadScene(string sceneToBeLoaded)
    {
        SceneManager.LoadScene(sceneToBeLoaded);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
