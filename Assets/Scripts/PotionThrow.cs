using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionThrow : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private Transform cam;
    [SerializeField] private float maxDistance;
    [SerializeField] private float heigthLaunch = 2f;
    [SerializeField] private Transform launnchPos;
    [SerializeField] private LayerMask colliderLayer;

    [Header("Visual")]
    [SerializeField] private int visualresolution = 30;
    [SerializeField] private LineRenderer lineVisual;
    private float gravity;
    public GameObject aimTarget;
    [SerializeField] private GameObject potionPrefab;

    private void Start() {
        aimTarget.SetActive(false);
        player.OnStartAim += StartAim;
        player.OnStopAim += StopAim;
        cam  = Camera.main.transform;
        gravity = Physics.gravity.y;
    }

    private void Update() {
        
        if(!player.isAiming) return;

        RaycastHit hitinfo;
        

        if(Physics.Raycast(cam.position,cam.forward,out hitinfo,maxDistance,colliderLayer))
        {
            aimTarget.transform.position = hitinfo.point;
        }
        else
        {
            aimTarget.transform.position = cam.position + cam.forward * maxDistance;
        }

        DrawPath();
    }

    LaunchData CalculateLauchData()
    {
        Vector3 endPos = aimTarget.transform.position;
        Vector3 startPos = launnchPos.transform.position;

        float displacementY = endPos.y - startPos.y;
        float h = displacementY + heigthLaunch;
        if(h < 0)
        {
            
            h +=  - h ;
        }

        Vector3 displacemntXZ = new Vector3(endPos.x - startPos.x,0,endPos.z - startPos.z);
        float time = Mathf.Sqrt(-2*h/gravity) + Mathf.Sqrt(2*(displacementY-h)/gravity);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
        Vector3 velocityXZ = displacemntXZ/ time;

        return new LaunchData(velocityXZ + velocityY,time);
    }

    private void DrawPath()
    {
        LaunchData launchData = CalculateLauchData();
        lineVisual.positionCount = visualresolution;

        for (int i = 0; i < visualresolution; i++)
        {
            float simulationTime = i / (float) visualresolution * launchData.timeToTarget;
            Vector3 displacement = launchData.inotialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime /2f;
            Vector3 drawPoint = launnchPos.position + displacement;

            lineVisual.SetPosition(i,drawPoint);
        }
    }

    private void Launch()
    {
        var go = PoolManager.Instance.Spawn(potionPrefab,true,launnchPos.position,transform.rotation);
        Rigidbody potionRb = go.GetComponent<Rigidbody>();

        potionRb.velocity = CalculateLauchData().inotialVelocity;
        potionRb.angularVelocity = go.transform.forward * 10f;
    }

    private void StartAim()
    {
        aimTarget.SetActive(true);
        player.OnThrow += Launch;
        lineVisual.enabled = true;
    }

    private void StopAim()
    {
        aimTarget.SetActive(false);
        player.OnThrow -= Launch;
        lineVisual.enabled = false;
    }

    private void OnDisable() {
        player.OnStartAim -= StartAim;
        player.OnStopAim -= StopAim;
    }

    struct LaunchData
    {
        public readonly Vector3 inotialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.inotialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }
    }
}

