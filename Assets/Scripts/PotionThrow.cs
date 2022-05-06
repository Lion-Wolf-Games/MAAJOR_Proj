using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class PotionThrow : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Potion selectedPotion;
    private Transform cam;
    [SerializeField] private float maxDistance;
    [SerializeField] private float heigthLaunch = 2f;
    [SerializeField] private Transform launchPos;
    [SerializeField] private LayerMask colliderLayer;
    [SerializeField] private Animator anims;
    [SerializeField] private float aimSpeed = 1f; 
    private float lanchTime;

    [Header("Visual")]
    [SerializeField] private int visualresolution = 30;
    [SerializeField] private LineRenderer lineVisual;
    [SerializeField] private GameObject RangeVisual;
    [SerializeField] private Material visualMaterial;
    private float gravity;
    public GameObject aimTarget;
    [SerializeField] private GameObject potionPrefab;

    private Dictionary<Potion,float> cooldowns;
    public Action<float> OnThrowPotion; 
    public Action OnThrowPotionInCd; 

    private Vector3 aimPos;

    [SerializeField] private AK.Wwise.Event potionInCd;
    

    private void Start() {
        aimTarget.SetActive(false);
        player.OnStartAim += StartAim;
        player.OnStopAim += StopAim;
        cam  = Camera.main.transform;
        gravity = Physics.gravity.y;

        cooldowns = new Dictionary<Potion, float>();
        SetPotion(selectedPotion);
        
    }

    private void Update() {
        
        if(!player.isAiming)
        {
            aimTarget.transform.position = Vector3.Lerp(aimTarget.transform.position,transform.position,Time.deltaTime * aimSpeed);
            return;
        }

        //RaycastHit hitinfo;

        // if(Physics.Raycast(launnchPos.position,cam.forward,out hitinfo,maxDistance,colliderLayer))
        // {
        //     aimTarget.transform.position = hitinfo.point;
        // }
        // else
        // {
        //     aimTarget.transform.position = launnchPos.position + cam.forward * maxDistance;
        // }

        aimTarget.transform.position = Vector3.Lerp(aimTarget.transform.position,transform.position + aimPos,Time.deltaTime * aimSpeed);
        

        DrawPath();
    }

    public void MoveAimTarget(InputAction.CallbackContext value)
    {
        Vector2 input = value.ReadValue<Vector2>();
        aimPos = (Vector3.forward * input.y + Vector3.right * input.x) * selectedPotion.launchRange;
        
    }

    #region LaunchCalculation
    LaunchData CalculateLauchData()
    {
        Vector3 endPos = aimTarget.transform.position;
        Vector3 startPos = launchPos.transform.position;

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
            Vector3 displacement = launchData.initialVelocity * simulationTime + Vector3.up * gravity * simulationTime * simulationTime /2f;
            Vector3 drawPoint = launchPos.position + displacement;

            lineVisual.SetPosition(i,drawPoint);
        }
    }
    #endregion

    private void Launch()
    {   
        if (cooldowns[selectedPotion] <= Time.time)
        {
            var go = PoolManager.Instance.Spawn(potionPrefab,true,launchPos.position,transform.rotation);
            Rigidbody potionRb = go.GetComponent<Rigidbody>();

            potionRb.velocity = CalculateLauchData().initialVelocity;
            potionRb.angularVelocity = go.transform.forward * 10f;

            go.GetComponent<PotionBehavior>().SetUpPotion(selectedPotion);
            
            cooldowns[selectedPotion] = Time.time + selectedPotion.cooldown;
            
            OnThrowPotion?.Invoke(selectedPotion.cooldown);
            anims.SetTrigger("Throw");
        }
        else
        {
            OnThrowPotionInCd?.Invoke();

            //potionInCd.Post(gameObject);
        }
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

    public void SetPotion(Potion newPotion)
    {
        selectedPotion = newPotion;
        maxDistance = selectedPotion.launchRange;

        RangeVisual.transform.DOScale( Vector3.one * selectedPotion.effectRange * 2f, 0.1f);

        Debug.Log(selectedPotion.name + " selected");

        visualMaterial.DOColor(selectedPotion.launchPreviewColor,"_Emission",0.2f);

        if(!cooldowns.ContainsKey(selectedPotion))
        { 
            cooldowns.Add(selectedPotion,0);
        }
    }

    private void OnDisable() {
        player.OnStartAim -= StartAim;
        player.OnStopAim -= StopAim;
    }

    struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }
    }
}

