using GgAccel;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint;

    [SerializeField] private float bulletForce = 10f;

    [SerializeField] private Bullet bulletPrefab;

    private float fireTimer;

    private Vector3 mousePos;

    [Header("Refer instance")] [SerializeField]
    private Camera cam;

    [SerializeField] private PlayerStat stat;
    [SerializeField] private AudioClip shootingSound;

    void Update()
    {
        if (fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
        }

        if (Input.GetMouseButton(0))
        {
            LaunchBullet();
        }

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        var mouseDir = mousePos - firePoint.position;
        var angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg - 90f;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void LaunchBullet()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        if (fireTimer > 0)
        {
            return;
        }

        AudioManager.PlaySound(shootingSound);
        var point = firePoint;
        switch (stat.TotalNumberOfBullet)
        {
            case 1:
                Launch(firePoint.up);
                break;
            case 2:
                Launch(firePoint.up);
                Launch(-firePoint.up);
                break;
            case 3:
                Launch(firePoint.up);
                Launch(firePoint.AThirdOfCircle());
                Launch(firePoint.TwoThirdOfCircle());
                break;
            case 4:
                Launch(firePoint.up);
                Launch(-firePoint.up);
                Launch(firePoint.right);
                Launch(-firePoint.right);
                Launch(new Vector3(1, 1));
                Launch(new Vector3(-1, 1));
                Launch(new Vector3(1, -1));
                Launch(new Vector3(-1, -1));
                break;
            default:
                break;
        }

        fireTimer = 1 / stat.TotalFireRate;
    }

    private void Launch(Vector3 direction)
    {
        var bulletGO = Pool.Get(bulletPrefab);
        bulletGO.InitBulletEvent(() => OnDisableBullet(bulletGO));
        bulletGO.transform.position = transform.position;
        var rb = bulletGO.GetComponent<Rigidbody2D>();

        rb.AddForce(direction * bulletForce, ForceMode2D.Impulse);
    }

    private void OnDisableBullet(Bullet bullet)
    {
        Pool.Release(bullet);
    }
}