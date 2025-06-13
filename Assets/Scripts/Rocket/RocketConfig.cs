using UnityEngine;

[CreateAssetMenu(fileName = "RocketConfig", menuName = "Configs/RocketConfig")]
public class RocketConfig : ScriptableObject
{
    public float speed = 10f;
    public float throwForce = 15f;
    public float explosionRadius = 2f;
    public bool explodesOnImpact = true;
    public GameObject explosionEffect;
}
