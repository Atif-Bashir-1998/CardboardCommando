using UnityEngine;

public class GunScript : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;

    [SerializeField] private AudioClip bulletSoundClip;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.GetComponent<Rigidbody>().linearVelocity = -bulletSpawnPoint.forward * bulletSpeed;

            // play bullet sound
            SoundFXManager.instance.PlaySoundFXClip(bulletSoundClip, transform);

            Destroy(bullet, 3f); // Destroy after 3 seconds
        }
    }
}