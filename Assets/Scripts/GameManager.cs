using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
     [SerializeField]
     private GameObject menu;
     [SerializeField]
     private GameObject pauseScreen;
	[SerializeField]
	private GameObject gameOverScreen;
	[SerializeField]
	private Text score;
	[SerializeField]
	private float gameOverDelay;
	[SerializeField]
     private SpawnPoint[] spawnpoints;

     private bool isPaused = false;

	public bool ControlsEnabled { get; private set; }
	
	[SerializeField]
     private int killCount = 0;
	public int KillCount { get => killCount; set => killCount = value; }

	// Start is called before the first frame update
	void Awake()
    {
		Time.timeScale = 0;
		ControlsEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Escape) && !menu.activeSelf)
		{
			if (isPaused)
			{
				UnPause();
                    
               }
			else
			{
				Pause();                    
			}
		}
    }

	public void StartGame()
	{
		menu.SetActive(false);
		Time.timeScale = 1;
		ControlsEnabled = true;

		// spawn a single enemy to start with
		var random = Random.Range(1, 5) - 1;
		spawnpoints[random].AddToQueue();
	}

	private void Pause()
	{
		ControlsEnabled = false;
          Time.timeScale = 0;
		pauseScreen.SetActive(true);
	}

	public void UnPause()
	{
          pauseScreen.SetActive(false);
		ControlsEnabled = true;
		Time.timeScale = 1;
	}

	public void MainMenu()
	{
		SceneManager.LoadScene(0);
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void GameOver()
	{
		score.text = "Tanks Destroyed: " + KillCount.ToString();
		Invoke("EndGame", gameOverDelay);
	}

	public void SpawnTanks()
	{
		for(var i = 0; i < 2; i++)
		{
			var random = Random.Range(1, 5) - 1;
			spawnpoints[random].AddToQueue();
		}
	}

	private void EndGame()
	{
		Time.timeScale = 0;
		gameOverScreen.SetActive(true);
	}
}
