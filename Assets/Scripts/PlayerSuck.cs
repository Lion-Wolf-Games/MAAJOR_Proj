using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerSuck : MonoBehaviour
{
    PlayerController player;
    bool isSucking;

    [SerializeField] private GameObject suckRangDisplay;
    private float _suckRange;
    [SerializeField] private float maxSuckRange;
    [SerializeField] private float expandTime;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
        player.OnStartSuck += StartSuck;
        player.OnStopSuck += StopSuck;

        suckRangDisplay.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if(isSucking)
        {
            //GetCurrange range
            _suckRange = suckRangDisplay.transform.localScale.x;

             
        }
    }

    private void StartSuck()
    {
        DOTween.Kill(suckRangDisplay.transform);
        suckRangDisplay.transform.localScale = Vector3.zero;
        suckRangDisplay.transform.DOScale(maxSuckRange,expandTime);

        suckRangDisplay.SetActive(true);
        

    }

    private void StopSuck()
    {
        DOTween.Kill(suckRangDisplay.transform);
        //suckRangDisplay.SetActive(false);
        suckRangDisplay.transform.DOScale(0,0.2f).OnComplete(()=> {suckRangDisplay.SetActive(false);});
    }
}
