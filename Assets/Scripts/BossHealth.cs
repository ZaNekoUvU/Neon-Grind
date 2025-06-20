using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Events;

public class BossHealth : MonoBehaviour
{
<<<<<<< Updated upstream
    [SerializeField]
    private int maxHealth = 5;
=======
    [SerializeField] public int maxHealth = 20;
>>>>>>> Stashed changes
    public int currentHealth;

    public Sprite[] healthSprites;
    [SerializeField] private Image healthBarUI;

    public float deathDelay = 3f;

    private Boss bossScript;

<<<<<<< Updated upstream
    [SerializeField] private GameManager gameManager;

    //public GameObject bossBar;

=======
>>>>>>> Stashed changes
    void Start()
    {
        currentHealth = maxHealth;
        bossScript = GetComponent<Boss>();
        UpdateSprite();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateSprite();

        if (currentHealth <= 0)
        {
            StartCoroutine(HandleDeath());
        }
    }

    void UpdateSprite()
    {
        if (healthBarUI != null && healthSprites.Length > 0)
        {
            int spriteIndex = Mathf.Clamp(maxHealth - currentHealth, 0, healthSprites.Length - 1);
            healthBarUI.sprite = healthSprites[spriteIndex];
        }
    }

    private IEnumerator HandleDeath()
    {
        if (bossScript != null)
        {
            bossScript.enabled = false;
        }

        if (gameManager != null)
        {
            gameManager.FirstBossDefeated();
        }

        yield return new WaitForSeconds(deathDelay);

        GameEvents.OnFirstBossDefeated?.Invoke(); // Fire event
        Destroy(gameObject);
    }
}