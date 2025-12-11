using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class XPBarUI : MonoBehaviour
{
    [Header("UI References")]
    public Slider xpSlider;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;

    [Header("Level Up Effect (Optional)")]
    public GameObject levelUpFlash;
    public float flashDuration = 1.5f;

    private void Start()
    {
        if (levelUpFlash != null)
        {
            levelUpFlash.SetActive(false);
        }

        StartCoroutine(BindToProgression());
    }

    private IEnumerator BindToProgression()
    {
        yield return null;

        if (PlayerProgression.Instance != null)
        {
            UpdateXPBar(
                PlayerProgression.Instance.currentXP,
                PlayerProgression.Instance.xpToNextLevel,
                PlayerProgression.Instance.currentLevel
            );

            PlayerProgression.Instance.OnXPChanged += UpdateXPBar;
            PlayerProgression.Instance.OnLevelUp += ShowLevelUpEffect;
        }
        else
        {
            Debug.LogWarning("[XPBarUI] No PlayerProgression.Instance found");
        }
    }

    private void OnDestroy()
    {
        if (PlayerProgression.Instance != null)
        {
            PlayerProgression.Instance.OnXPChanged -= UpdateXPBar;
            PlayerProgression.Instance.OnLevelUp -= ShowLevelUpEffect;
        }
    }

    private void UpdateXPBar(int currentXP, int xpToNext, int level)
    {
        if (xpSlider != null)
        {
            xpSlider.maxValue = xpToNext;
            xpSlider.value = currentXP;
        }

        if (levelText != null)
        {
            levelText.text = $"Lv {level}";
        }

        if (xpText != null)
        {
            xpText.text = $"{currentXP} / {xpToNext} XP";
        }
    }

    private void ShowLevelUpEffect(int newLevel)
    {
        if (levelText != null)
        {
            levelText.text = $"Lv {newLevel}";
        }

        if (levelUpFlash != null)
        {
            StartCoroutine(FlashLevelUp());
        }
    }

    private IEnumerator FlashLevelUp()
    {
        levelUpFlash.SetActive(true);
        yield return new WaitForSeconds(flashDuration);
        levelUpFlash.SetActive(false);
    }
}