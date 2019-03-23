using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleSceneLoad : MonoBehaviour {

    public SceneBufer bufer;

	// Use this for initialization
	void Start () {
        SceneManager.LoadSceneAsync(bufer.SceneId, LoadSceneMode.Single);	
	}


    private void Update()
    {
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(bufer.SceneId);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
