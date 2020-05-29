using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Robot : MonoBehaviour
{
    [SerializeField] private GameObject _ragdoll;
    [SerializeField] private GameObject _animatedModel;
    [SerializeField] private NavMeshAgent _navmeshAgent;

    [SerializeField]
    private GameObject HealthCanvas;
    [SerializeField]
    private Image healthImage;
    
    private bool _dead;
    public bool isDead;

    public float health = 100;
    private GameObject myPlayer;
    private void Awake()
    {
        HealthCanvas.SetActive(true);
        _ragdoll.gameObject.SetActive(false);
        health = 100;
        isDead = false;
        myPlayer = GameObject.Find("Player").gameObject;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        HealthCanvas.transform.LookAt(myPlayer.transform);
        healthImage.rectTransform.localScale = new Vector3(health / 100, 1, 1);
        if (health <= 0 && !isDead)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            isDead = true;
            HealthCanvas.SetActive(false);

            ToggleDead();
            Destroy(gameObject, 10);
        }
    }


    [ContextMenu("ToggleDead")]
    private void ToggleDead()
    {
        _dead = !_dead;

        if (_dead)
        {
            CopyTransformData(_animatedModel.transform, _ragdoll.transform, _navmeshAgent.velocity);
            _ragdoll.gameObject.SetActive(true);
            _animatedModel.gameObject.SetActive(false);
            _navmeshAgent.velocity = Vector3.zero;
            _navmeshAgent.enabled = false;
        }
        else
        {
            // Switch back to the model and disable the ragdoll
            _ragdoll.gameObject.SetActive(false);
            _animatedModel.gameObject.SetActive(true);
            _navmeshAgent.enabled = true;    
        }
        
    }

    private void CopyTransformData(Transform sourceTransform, Transform destinationTransform, Vector3 velocity)
    {
        if (sourceTransform.childCount != destinationTransform.childCount)
        {
            Debug.LogWarning("Invalid transform copy, they need to match transform hierarchies");
            return;
        }

        for (int i = 0; i < sourceTransform.childCount; i++)
        {
            var source = sourceTransform.GetChild(i);
            var destination = destinationTransform.GetChild(i);
            destination.position = source.position;
            destination.rotation = source.rotation;
            var rb = destination.GetComponent<Rigidbody>();
            if (rb != null)
                rb.velocity = velocity;
            
            CopyTransformData(source, destination, velocity);
        }
    }
}