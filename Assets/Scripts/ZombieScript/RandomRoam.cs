 using UnityEngine;
using UnityEngine.AI;

public class RandomRoam : MonoBehaviour
{
    private NavMeshAgent _navmeshAgent;
    public Animator anim;
    public GameObject myPlayer;

    public float minDistance = 0.8f;
    private float enemyObserverange = 50;


    private void Awake()
    {
        enemyObserverange = 500;
        minDistance = 1f;
        _navmeshAgent = GetComponent<NavMeshAgent>();
        myPlayer=GameObject.Find("Player").gameObject;
        //anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (gameObject.GetComponent<Robot>().isDead == false)
        {
            Vector3 direction = myPlayer.transform.position - this.transform.position;
            direction.y = 0;
            //this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), 0.5f);

            //anim.SetBool ("isIdle", false);
           
            if (direction.magnitude > minDistance && direction.magnitude < enemyObserverange)
            {
                //_navmeshAgent.enabled = true;
                anim.SetBool("isAttacking", false);
                anim.SetBool("isRunning", true);
                anim.SetBool("isIdle", false);
                anim.SetBool("isWalking", false);
                _navmeshAgent.SetDestination(myPlayer.transform.position);
            }
            else if (direction.magnitude < minDistance)
            {
                //_navmeshAgent.enabled = false;
                //isOnAttackState = true;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.5f);
                anim.SetBool("isWalking", false);
                anim.SetBool("isIdle", false);
                anim.SetBool("isAttacking", true);
                anim.SetBool("isRunning", false);

            }
            else
            {
                //_navmeshAgent.enabled = true;
                //go back to idle state
                anim.SetBool("isAttacking", false);
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("isIdle", true);

            }



            if (_navmeshAgent.enabled == false)
                return;

            //if (_navmeshAgent.hasPath == false || _navmeshAgent.remainingDistance < 1f)
            //ChooseNewPosition();
        }
    }

    private void ChooseNewPosition()
    {
        Vector3 randomOffset = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
        var destination = transform.position + randomOffset;
        _navmeshAgent.SetDestination(destination);
    }
}