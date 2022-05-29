using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TankController : MonoBehaviour
{
     [SerializeField]
     private Rigidbody2D rb;
     [SerializeField]
     private Transform turretBase;
     [SerializeField]
     private Transform projectileSpawn;
     [SerializeField]
     private GameObject projectilePrefab;

     [SerializeField]
     private float moveSpeed;
     [SerializeField]
     private float turnSpeed;
     [SerializeField]
     private float turretSpeed;
     [SerializeField]
     private float reloadTimer;

     private Vector2 movementVector;
     private bool canShoot = true;
     [SerializeField]
     private float currentDelay;
     private Collider2D[] tankColliders;

	public float ReloadTimer { get => reloadTimer; set => reloadTimer = value; }

     public UnityEvent OnShoot, OnCanShoot, OnCantShoot;
     public UnityEvent<float> OnReloading;

	void Awake()
    {
          tankColliders = GetComponentsInParent<Collider2D>();
    }

	private void Start()
	{
          OnReloading?.Invoke(currentDelay);
	}
	private void Update()
	{
		if (!canShoot)
		{
               OnCantShoot?.Invoke();
               currentDelay -= Time.deltaTime;
               OnReloading?.Invoke(currentDelay);

               if (currentDelay <= 0)
			{
                    canShoot = true;
                    OnCanShoot?.Invoke();
			}
		}
	}

	void FixedUpdate()
     {
          rb.velocity = (Vector2)transform.up * movementVector.y * moveSpeed * Time.fixedDeltaTime;
          rb.MoveRotation(transform.rotation * Quaternion.Euler(0, 0, -movementVector.x * turnSpeed * Time.fixedDeltaTime));
     }

     public void HandleShoot()
     {
          if (canShoot)
          {
               canShoot = false;
               currentDelay = ReloadTimer;

               GameObject projectile = Instantiate(projectilePrefab);
               projectile.transform.position = projectileSpawn.position;
               projectile.transform.localRotation = projectileSpawn.rotation;
               projectile.GetComponent<Projectile>().Initialize();

               foreach (var collider in tankColliders)
               {
                    Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), collider);
               }

               OnShoot?.Invoke();
               OnReloading?.Invoke(currentDelay);
          }

     }    
     


     public void HandleBodyMovement(Vector2 moveVector)
	{
          this.movementVector = moveVector;
	}

     public void HandleTurretMovement(Vector2 pointerPosition)
	{
          var turretDirection = (Vector3)pointerPosition - transform.position;
          var targetAngle = Mathf.Atan2(turretDirection.y, turretDirection.x) * Mathf.Rad2Deg;
          var rotationStep = turretSpeed * Time.deltaTime;
          turretBase.rotation = Quaternion.RotateTowards(turretBase.rotation, Quaternion.Euler(0, 0, targetAngle), rotationStep);
	}
}
