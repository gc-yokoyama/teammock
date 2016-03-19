using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class sceneManager : MonoBehaviour {

    public bool changeflg;

	// Use this for initialization
	void Start () {
        changeflg = false;
    }

    // Update is called once per frame
    void Update() {

        if (changeflg)
        { 
            // シーン遷移
            SceneManager.LoadScene("home");
            changeflg = false;
        }
    }

    public void changeScene()
    {
        changeflg = true;
    }
}
