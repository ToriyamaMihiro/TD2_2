using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
   
    [SerializeField] private Transform cam;
    [SerializeField] private Vector3 positionStrength=new(0,0,0);
    [SerializeField] private Vector3 rotationStrength=new(2,2,2);
    private float shakeDuration = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void CameraShaker()
    {
       
        cam.DOComplete();
        cam.DOShakePosition(shakeDuration, positionStrength);
        cam.DOShakeRotation(shakeDuration, rotationStrength);
    }

    // Update is called once per frame
    void Update()
    {
        //ここにシェイクのタイミング描く
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CameraShaker();
        }
    }
}
