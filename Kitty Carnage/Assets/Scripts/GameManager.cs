using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	void Awake()
	{
		if (instance == null)
		{
			DontDestroyOnLoad(gameObject);
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

    void Start()
    {
        
    }

    void Update()
    {
        
    }

	public void HideCursor()
	{
		// Set the cursor to the center of the screen
		Cursor.lockState = CursorLockMode.Locked;

		// Hide the cursor
		Cursor.visible = false;
	}

	public void ShowCursor()
	{
		// Set the cursor to the center of the screen
		Cursor.lockState = CursorLockMode.None;

		// Hide the cursor
		Cursor.visible = true;
	}
}
