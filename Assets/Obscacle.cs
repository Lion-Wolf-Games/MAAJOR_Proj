using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Obscacle : MonoBehaviour
{
    private bool isPlayerInside;
    private bool isPassingObstacle;
    
    private PlayerController player;

    public float passDuration;
    public AnimationCurve velocityCurve;
    private float PassTime;

    private Vector3 endPos;
    [SerializeField] private Transform pointA,pointB;
    [SerializeField] private Collider obstacleColider;
    [SerializeField] private Transform visual;
    [SerializeField] private ParticleSystem fx;

    private void Start() {
        isPlayerInside = false;
    }

    //Detect when player enter
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        {
            isPlayerInside = true;
            player = other.GetComponent<PlayerController>();
            Debug.Log("PlayerEnter");
        }
    }
    private void Update() {

        if(!isPlayerInside) return;

        if(player.isDashing)
        {
            //Get playerClosest point
            float distA = Vector3.Distance(player.transform.position,pointA.position);
            float distB = Vector3.Distance(player.transform.position,pointB.position);

            if (distA >= distB)
            {
                endPos = pointA.position;
            } else
            {
                endPos = pointB.position;
            }

            //Disable input
            player.DisableInput();
        
            obstacleColider.enabled = false;
            isPassingObstacle = true;
            PassTime = Time.time + passDuration;

            //Visual
            player.transform.LookAt(transform.position);
            visual.DOShakeScale(0.5f,0.5f);
            fx.Play();
        }

    }

    private void LateUpdate() {
        
        if (isPassingObstacle)
        {
            float t = (PassTime - Time.time) / passDuration;
            t = Mathf.Abs(t-1);
            float velocity = velocityCurve.Evaluate(t);

            Vector3 dir = (endPos - player.transform.position).normalized;

            //MovePlayer to other Point
            player.Move(dir * velocity * Time.deltaTime);

            //activate input
            if(PassTime < Time.time)
            {
                obstacleColider.enabled = true;
                isPassingObstacle = false;
                player.EnableInput();
            }
        }
    }

    //Detect when Player leave
    private void OnTriggerExit(Collider other) {
        if(other.tag == "Player")
        {
            isPlayerInside = false;
            player = null;
            Debug.Log("PlayerExit");
        }
    }
}
