using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
     public Text HP, FireRate, TanksDestroyed;
     
     private GameObject player;
     private Damageable playerHealth;
     private TankController playerTankController;
     private GameManager gm;

    void Awake()
    {
          player = GameObject.Find("Player");
          playerHealth = player.GetComponent<Damageable>();
          playerTankController = player.GetComponent<TankController>();
          gm = FindObjectOfType<GameManager>();
          UpdateHP();
          UpdateFireRate();
          UpdateTanksDestroyed();
    }

	public void UpdateHP()
	{
          HP.text = playerHealth.CurrHP.ToString() + " / " + playerHealth.MaxHP.ToString();
	}

     public void UpdateFireRate()
	{
          FireRate.text = playerTankController.ReloadTimer.ToString() + " s";
	}

	public void UpdateTanksDestroyed()
	{
          TanksDestroyed.text = gm.KillCount.ToString();
	}
}
