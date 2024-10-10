using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController2 : MonoBehaviour
{
    [SerializeField]
    public PathManager2 pathManager2;

    List<waypoint2> thePath;
    waypoint2 target;

    public float MoveSpeed;
    public float RotateSpeed;

    public Animator animator;
    bool isSprinting;

    bool noMoreSprinting = false;
    


    // Start is called before the first frame update
    void Start()
    {
        isSprinting = false;
        animator.SetBool("IsSprinting", isSprinting);

        thePath = pathManager2.GetPath();
        if (thePath != null && thePath.Count > 0)
        {
            //set starting target to the first waypoint
            target = thePath[0];
        }
    }

    void RotateTowardsTarget()
    {
        float stepSize = RotateSpeed * Time.deltaTime;

        Vector3 targetDir = target.pos - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void MoveForward()
    {
        
         float stepSize = Time.deltaTime * MoveSpeed;
         float distanceToTarget = Vector3.Distance(transform.position, target.pos);

         if (distanceToTarget < stepSize)
         {
                //we will overshoot the target
                //so we should do something smarter here
                return;
         }

            //take a step forward
            Vector3 moveDir = Vector3.forward;
            transform.Translate(moveDir * stepSize);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //switch to next target
        target = pathManager2.GetNextTarget();
        
        isSprinting = false;
        animator.SetBool("IsSprinting", isSprinting);
        noMoreSprinting = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && noMoreSprinting == false)
        {
            isSprinting = !isSprinting;
            
            animator.SetBool("IsSprinting", isSprinting);
        }
        if (isSprinting)
        {
            RotateTowardsTarget();
            MoveForward();
        }
    }
}
