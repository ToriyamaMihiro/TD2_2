using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaColor : MonoBehaviour
{
    private Image image;
    public float low=0.15f;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.DOFade(low,1f).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
