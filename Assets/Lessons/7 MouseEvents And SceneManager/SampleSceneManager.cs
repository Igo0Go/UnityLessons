using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleSceneManager : MonoBehaviour {

    public int sceneIdForLoad;
    public SceneBufer bufer;
    public Material[] materials;

    private Vector3 standartScale;
    private int currentMaterial;
    private MeshRenderer renderer;

	// Use this for initialization
	void Start () {
        renderer = GetComponent<MeshRenderer>();
        renderer.material = bufer.material;
        standartScale = transform.localScale;
	}

    private void OnMouseEnter()
    {
        transform.localScale = standartScale * 2;
    }

    private void OnMouseExit()
    {
        transform.localScale = standartScale;
    }

    private void OnMouseDown()
    {
        NextMaterial();   
    }

    private void OnMouseUpAsButton()
    {
        bufer.SceneId = sceneIdForLoad;
        bufer.material = materials[currentMaterial];
        SceneManager.LoadScene(0);
    }

    private void NextMaterial()
    {
        currentMaterial++;
        if(currentMaterial > materials.Length - 1)
        {
            currentMaterial = 0;
        }
        renderer.material = materials[currentMaterial];
    }
}
