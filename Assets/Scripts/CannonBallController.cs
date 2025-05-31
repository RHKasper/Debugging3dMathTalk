using System;
using System.Threading.Tasks;
using UnityEngine;

public class CannonBallController : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionPrefab;
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Boom!");
        Destroy(gameObject);
        var explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position + Vector3.up;
        _ = DestroyExplosionAfterParticleSystemFinishes(explosion);
    }

    private async Task DestroyExplosionAfterParticleSystemFinishes(ParticleSystem particleSystem)
    {
        await Task.Delay((int)(particleSystem.main.duration * 1000));
        Destroy(particleSystem.gameObject);
    }
}
