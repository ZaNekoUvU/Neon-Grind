using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [SerializeField]
    public int maxHealth = 20;
    public int currentHealth;

    public Sprite[] healthSprites;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    public Image healthBarUI;

    public float deathDelay = 3f;

    private Boss bossScript;

    public GameObject bossBar;

    void Start()
    {
        bossBar.SetActive(false);
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        bossScript = GetComponent<Boss>();

        UpdateSprite();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Debug.Log("Damage taken" + currentHealth + "/" + maxHealth);
            Destroy(other.gameObject);
        }
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateSprite();

        if (currentHealth <= 0)
        {
           StartCoroutine(HandleDeath());
        }
    }
    void UpdateSprite()
    {
        if (spriteRenderer != null && currentHealth >= 0 && currentHealth <healthSprites.Length)
        {
            spriteRenderer.sprite = healthSprites[currentHealth];
        }

        if (healthBarUI != null && currentHealth >= 0 && currentHealth < healthSprites.Length)
        {
            healthBarUI.sprite = healthSprites[currentHealth];
        }
    }

    private System.Collections.IEnumerator HandleDeath()
    {
        if (bossScript != null)
        {
            bossScript.enabled = false;
        }

        yield return new WaitForSeconds(deathDelay);

        Destroy(gameObject);
    }
}
