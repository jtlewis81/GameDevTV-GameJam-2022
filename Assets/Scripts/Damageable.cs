using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
     [SerializeField]
     private int maxHP;
     [SerializeField]
     private int currHP;

	public int CurrHP
     {
          get { return currHP; }
          set
          {
               currHP = value;
               OnHPChange?.Invoke((float)CurrHP / MaxHP);
          }
     }

	public int MaxHP { get => maxHP; set => maxHP = value; }

     private GameManager gm;
     private Hud hud;

     public UnityEvent OnDead;
     public UnityEvent<float> OnHPChange;
     public UnityEvent OnHit, OnHeal;


	void Awake()
     {
          currHP = MaxHP;
          gm = FindObjectOfType<GameManager>();
          hud = FindObjectOfType<Hud>();
     }

	internal void Hit(int damage)
	{
          CurrHP -= damage;
          if(currHP <= 0)
		{
               OnDead?.Invoke();
               if (gameObject.tag == "Enemy")
			{
                    gm.KillCount++;
                    hud.UpdateTanksDestroyed();
                    gm.SpawnTanks();
               }
		}
		else
		{
               OnHit?.Invoke();
		}
	}

     public void Heal(int addedHP)
	{
          CurrHP += addedHP;
          CurrHP = Mathf.Clamp(currHP, 0, MaxHP);
          OnHeal?.Invoke();
	}
}
