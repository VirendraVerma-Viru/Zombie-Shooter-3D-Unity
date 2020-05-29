using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    private Transform target;
    public GameObject ImpactEffect;
    public float speed = 70f;
    public int damage = 25;
    public float explosionRadius = 0f;

    public void Seek(Transform _target)
    {
        target = _target;
        speed = 1f;
        
    }

	// Use this for initialization
	void Start () {
        
        speed = 1f;
        explosionRadius = 1f;
	}
	
	// Update is called once per frame
	void Update () {
	    if(target==null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed + Time.deltaTime;

        if(dir.magnitude<=distanceThisFrame)
        {
            HitTarget();
            return;
        }
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    void HitTarget()
    {
        GameObject effectIns = (GameObject)Instantiate(ImpactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 2f);

        if (explosionRadius >= 0f)
        {
            
            Explode();
        }
        else
        {
            
            Damage(target);
        }

        
        Destroy(gameObject);
    }

    void Explode()
    {
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider collider in colliders)
        {
            
           // print(collider.gameObject.name+" "+collider.transform.tag+"|"+collider.transform.position);
            if (collider.gameObject.tag == "Dummie")
            {
                
                Damage(collider.transform);
                
            }
        }
    }

    void Damage(Transform Enemy)
    {
        Robot e = Enemy.GetComponent<Robot>();
        
        if (e!=null)
        {
            
            e.TakeDamage(damage);
        }
       
    }
}
