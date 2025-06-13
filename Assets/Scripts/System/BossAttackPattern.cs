using UnityEngine;

public enum BossAttackType { FireFlame, EagleStrike, RocketLaunch }

[CreateAssetMenu(fileName = "BossAttackPattern", menuName = "Configs/BossAttackPattern")]
public class BossAttackPattern : ScriptableObject
{
    public BossAttackType attackType;
    public float attackDelay = 2f;
    public GameObject visualEffectPrefab;
    public AudioClip soundEffect;
}