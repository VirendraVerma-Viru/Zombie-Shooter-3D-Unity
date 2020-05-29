using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    [SerializeField] private GameObject TurretBullets;
    [SerializeField] private Transform TurretBulletsPosition;
    [SerializeField] private GameObject RotatingTurretpart;
    [SerializeField] private float range = 4f;
    [SerializeField] private string enemyTag = "Dummie";
    [SerializeField] private float turnSpeed = 0.05f;
    [SerializeField] private float fireCountDown = 0f;
    [SerializeField] public float fireRate = 1f;



    private Transform target;

    void Start()
    {
        range = 7f;
        turnSpeed = 5;
        fireRate = 2f;
    }

    
    void Update()
    {
        UpdateTarget();
        
        if (target == null)
        {
            return;
        }
        fireCountDown -= Time.deltaTime;

        LockOnTarget();

        if (fireCountDown <= 0f)
        {
            Shoot();
            fireCountDown = 1f / fireRate;
        }
        

    }

    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(TurretBullets, TurretBulletsPosition.position, TurretBulletsPosition.rotation);
        bulletGO.transform.localScale = Vector3.one;
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null)
            bullet.Seek(target);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemies)
        {

            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            //targetEnemy = nearestEnemy.GetComponent<enemy>();
        }
        else
        {
            target = null;
        }
    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(RotatingTurretpart.transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        RotatingTurretpart.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
