using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BossHealthUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject bossHealthPanel;        // The whole panel (to show/hide)
    public Slider healthSlider;
    public TextMeshProUGUI bossNameText;
    public Image fillImage;                   // Optional: to change color at low health

    [Header("Boss Reference")]
    public BossHealth bossHealth;

    [Header("Settings")]
    public string bossName = "STONE GOLEM";
    public float showDelay = 0.5f;            // Delay before bar appears
    public bool hideWhenDead = true;
    public float hideDelay = 2f;              // How long to show bar after death

    [Header("Low Health Warning (Optional)")]
    public bool changeFillColorOnLowHealth = true;
    public float lowHealthThreshold = 0.25f;  // 25%
    public Color normalColor = Color.red;
    public Color lowHealthColor = new Color(1f, 0.3f, 0f);  // Orange-red

    [Header("Animation (Optional)")]
    public bool animateOnDamage = true;
    public float damageShakeIntensity = 5f;
    public float damageShakeDuration = 0.1f;

    private float lastHealth;
    private RectTransform panelRect;
    private Vector3 originalPanelPosition;
    private bool isInitialized = false;

    private void Start()
    {
        // Hide panel initially
        if (bossHealthPanel != null)
        {
            panelRect = bossHealthPanel.GetComponent<RectTransform>();
            if (panelRect != null)
            {
                originalPanelPosition = panelRect.anchoredPosition;
            }
            bossHealthPanel.SetActive(false);
        }

        // Try to find boss if not assigned
        if (bossHealth == null)
        {
            bossHealth = FindObjectOfType<BossHealth>();
        }

        if (bossHealth != null)
        {
            StartCoroutine(InitializeWithDelay());
        }
        else
        {
            Debug.LogWarning("[BossHealthUI] No BossHealth found in scene");
        }
    }

    private IEnumerator InitializeWithDelay()
    {
        yield return new WaitForSeconds(showDelay);

        // Set boss name
        if (bossNameText != null)
        {
            bossNameText.text = bossName;
        }

        // Initialize slider
        if (healthSlider != null)
        {
            healthSlider.maxValue = bossHealth.health;
            healthSlider.value = bossHealth.currentHealth;
        }

        // Set initial fill color
        if (fillImage != null)
        {
            fillImage.color = normalColor;
        }

        lastHealth = bossHealth.currentHealth;
        isInitialized = true;

        // Show the panel
        if (bossHealthPanel != null)
        {
            bossHealthPanel.SetActive(true);
        }
    }

    private void Update()
    {
        if (!isInitialized || bossHealth == null) return;

        // Update slider
        if (healthSlider != null)
        {
            healthSlider.value = bossHealth.currentHealth;
        }

        // Check for damage taken
        if (bossHealth.currentHealth < lastHealth)
        {
            OnBossDamaged();
        }
        lastHealth = bossHealth.currentHealth;

        // Update fill color based on health percentage
        if (changeFillColorOnLowHealth && fillImage != null)
        {
            float healthPercent = bossHealth.currentHealth / bossHealth.health;
            if (healthPercent <= lowHealthThreshold)
            {
                fillImage.color = lowHealthColor;
            }
            else
            {
                fillImage.color = normalColor;
            }
        }

        // Hide when boss is dead
        if (hideWhenDead && bossHealth.isDead && bossHealthPanel.activeSelf)
        {
            StartCoroutine(HideAfterDelay());
        }
    }

    private void OnBossDamaged()
    {
        if (animateOnDamage && panelRect != null)
        {
            StartCoroutine(ShakeBar());
        }
    }

    private IEnumerator ShakeBar()
    {
        float elapsed = 0f;
        while (elapsed < damageShakeDuration)
        {
            float offsetX = Random.Range(-damageShakeIntensity, damageShakeIntensity);
            float offsetY = Random.Range(-damageShakeIntensity, damageShakeIntensity);
            panelRect.anchoredPosition = originalPanelPosition + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }
        panelRect.anchoredPosition = originalPanelPosition;
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);
        if (bossHealthPanel != null)
        {
            bossHealthPanel.SetActive(false);
        }
    }

    // Call this if boss spawns mid-game
    public void SetBoss(BossHealth boss)
    {
        bossHealth = boss;
        StartCoroutine(InitializeWithDelay());
    }
}