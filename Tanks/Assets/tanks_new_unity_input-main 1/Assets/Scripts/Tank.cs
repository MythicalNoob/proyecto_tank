using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tank : MonoBehaviour
{
    public Vector3 spawnPosition;
    Vector2 tankMovementSpeed;
    public float tankSpeed = 1f;
    public float tankRotSpeed = 10f;
    public int maxHealth = 100;
    public HealthBar healthBar;
    public int forcePunch = 30000;
    public GameObject Tanque;
    public Renderer chasis;
    public Renderer top;

   // public Material tank;

    private int currHealth;

    public GameObject bulletPrefab;
    //public GameObject bulletPrefab;
    public Transform spawnBulletPos;

    private void Awake()
    {
        chasis.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        top.material.color = chasis.material.color;
    }

    // Start is called before the first frame update
    void Start()
    {
        currHealth = maxHealth;
        chasis = GetComponent<Renderer>();
        top = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(tankSpeed * tankMovementSpeed.y * Time.deltaTime * Vector3.forward);
        transform.Rotate(tankRotSpeed * tankMovementSpeed.x * Time.deltaTime * Vector3.up);

        if(currHealth <= 0)
        {
            SetInitialPos(spawnPosition);
            currHealth = maxHealth;
            healthBar.UpdateHealth(100f);
        }
    }

    public void SetInitialPos(Vector3 position)
    {
        spawnPosition = position;
        transform.position = position;
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started) return;
        if (!gameObject.activeInHierarchy) return;

        /*Rigidbody bullet = Instantiate(bulletPrefab, spawnBulletPos.position, spawnBulletPos.rotation) as Rigidbody;
        bullet.velocity = 50 * spawnBulletPos.forward;*/
        var bullet = Instantiate(bulletPrefab, spawnBulletPos.position, spawnBulletPos.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * forcePunch);

        
    }

    public void Move(InputAction.CallbackContext context)
    {
        tankMovementSpeed = context.ReadValue<Vector2>();
        //transform.Translate(10 * context.ReadValue<Vector2>().y * Time.deltaTime * Vector3.forward);
    }

    public void TakeDamage(int damage)
    {
        currHealth -= damage;
        healthBar.UpdateHealth((float)currHealth / (float)maxHealth);
    }

    /* private void OnCollisionEnter(Collision col)
     {
         if (col.gameObject.CompareTag("bullet"))
         {
             TakeDamage(10);
             Destroy(col.gameObject);
         }
     }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("bullet"))
        {
            TakeDamage(10);
            Destroy(other.gameObject);
            
            
        }
    }

}
