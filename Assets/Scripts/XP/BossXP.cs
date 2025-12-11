using UnityEngine;

public class BossXP : MonoBehaviour
{
    [Header("XP Settings")]
    public int xpValue = 200;

    private BossHealth bossHealth;
    private bool hasGrantedXP = false;

    private void Start()
    {
        bossHealth = GetComponent<BossHealth>();
    }

    private void Update()
    {
        if (bossHealth != null && bossHealth.isDead && !hasGrantedXP)
        {
            GrantXP();
            hasGrantedXP = true;
        }
    }

    private void GrantXP()
    {
        if (PlayerProgression.Instance != null)
        {
            PlayerProgression.Instance.GainXP(xpValue);
        }
    }
}