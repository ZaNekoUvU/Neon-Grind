using UnityEngine;
using UnityEngine.UIElements;

public class BossHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth;
    public int currentHealth;

    public float deathDelay = 1f;

    private Boss bossScript;

    [SerializeField] AudioClip death;
    private AudioSource deathSound;

    [SerializeField] AudioClip hurt;
    private AudioSource hurtSound;

    private void Awake()
    {
        deathSound = gameObject.AddComponent<AudioSource>();
        deathSound.playOnAwake = false;
        deathSound.clip = death;

        hurtSound = gameObject.AddComponent<AudioSource>();
        hurtSound.playOnAwake = false;
        hurtSound.clip = hurt;

        hurtSound.volume = 0.3f;
        deathSound.volume = 0.3f;
    }

    void Start()
    {
        currentHealth = maxHealth;
        bossScript = GetComponent<Boss>();
    }

    // Detect collision with player bullets
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    // Reduces health and triggers death sequence if health hits 0
    private void TakeDamage(int damage)
    {
        hurtSound.Play();
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            StartCoroutine(HandleDeath());
        }
    }

    // Disables boss logic, waits, then notifies system and destroys boss
    private System.Collections.IEnumerator HandleDeath()
    {
        if (bossScript != null)
        {
            bossScript.enabled = false; 
        }

        yield return new WaitForSeconds(deathDelay);

        EventManager.Instance?.PostNotification(NeonGrindEvents.BOSS_DEFEATED, this);

        deathSound.Play();

        gameObject.SetActive(false);
    }
}