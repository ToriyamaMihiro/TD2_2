using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DustIconManager : MonoBehaviour
{

    [SerializeField] GameObject[] icon=new GameObject[6];

    bool[] isSet = new bool[6];
    [SerializeField] int leftIconNum;
    [SerializeField] int rightIconNum;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
