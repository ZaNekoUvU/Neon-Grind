using UnityEngine;
using UnityEngine.UIElements;

public class BossHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth;
    public int currentHealth;

    public float deathDelay = 1f;

    private Boss bossScript;

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

        gameObject.SetActive(false);
    }
}