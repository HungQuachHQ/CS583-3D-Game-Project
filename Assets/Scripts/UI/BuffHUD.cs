using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffHUD : MonoBehaviour
{
    [Header("Attack")]
    public GameObject attackRow;
    public TMP_Text attackText;

    [Header("Defense")]
    public GameObject defenseRow;
    public TMP_Text defenseText;

    [Header("Stamina")]
    public GameObject staminaRow;
    public TMP_Text staminaText;

    [Header("Background")]            // NEW
    public Image backgroundImage;     // NEW

    private PlayerBuffController buffs;
    private PlayerStats stats;

    private void OnEnable()
    {
        Rebind();
    }

    private void Start()
    {
        Rebind();
    }

    private void Rebind()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            buffs = null;
            stats = null;
            return;
        }

        buffs = player.GetComponent<PlayerBuffController>();
        stats = player.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (buffs == null || stats == null)
        {
            SetRowActive(attackRow, false);
            SetRowActive(defenseRow, false);
            SetRowActive(staminaRow, false);

            // If we have no player/buffs, hide background too
            if (backgroundImage != null)
                backgroundImage.enabled = false;

            return;
        }

        bool anyActive = false;   // NEW – track if at least one buff is shown

        // ---- ATTACK BUFF ----
        if (buffs.AttackBuffRemaining > 0f && stats.AttackBuffFromPotions > 0)
        {
            SetRowActive(attackRow, true);
            if (attackText != null)
                attackText.text = $"+{stats.AttackBuffFromPotions} ATK ({buffs.AttackBuffRemaining:0}s)";
            anyActive = true;
        }
        else
        {
            SetRowActive(attackRow, false);
        }

        // ---- DEFENSE BUFF ----
        if (buffs.DefenseBuffRemaining > 0f && stats.DefenseBuffFromPotions > 0)
        {
            SetRowActive(defenseRow, true);
            if (defenseText != null)
                defenseText.text = $"+{stats.DefenseBuffFromPotions} DEF ({buffs.DefenseBuffRemaining:0}s)";
            anyActive = true;
        }
        else
        {
            SetRowActive(defenseRow, false);
        }

        // ---- STAMINA / SPEED BUFF ----
        if (buffs.StaminaBuffRemaining > 0f)
        {
            SetRowActive(staminaRow, true);
            if (staminaText != null)
                staminaText.text = $"Speed Boost ({buffs.StaminaBuffRemaining:0}s)";
            anyActive = true;
        }
        else
        {
            SetRowActive(staminaRow, false);
        }

        // ---- TOGGLE BACKGROUND ----
        if (backgroundImage != null)
        {
            backgroundImage.enabled = anyActive;
        }
    }

    private void SetRowActive(GameObject row, bool active)
    {
        if (row != null && row.activeSelf != active)
            row.SetActive(active);
    }
}
