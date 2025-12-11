using UnityEngine;

public class EnemyXP : MonoBehaviour
{
    [Header("XP Settings")]
    public int xpValue = 25;

    private EnemyHealth enemyHealth;
    private bool hasGrantedXP = false;

    private void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    private void Update()
    {
        if (enemyHealth != null && enemyHealth.isDead && !hasGrantedXP)
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
        else
        {
            Debug.LogWarning("[EnemyXP] No PlayerProgression.Instance found!");
        }
    }
}