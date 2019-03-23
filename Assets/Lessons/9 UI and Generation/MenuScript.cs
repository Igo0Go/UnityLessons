using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {

    public PlayerReactor playerReactor;
    public GameObject menuPanel;

	// Use this for initialization
	void Start () {
        menuPanel.SetActive(false);
	}

    private void Update()
    {
        ToMenu();
    }

    private void ToMenu()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            menuPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void Continue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        menuPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Spawn()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        menuPanel.SetActive(false);
        playerReactor.transform.position = playerReactor.safePos;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
