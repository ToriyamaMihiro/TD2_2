using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PracticeBossAction : MonoBehaviour
{
    int time;

    // Start is called before the first frame update
    void Start()
    {
        //登場イージング
      //  transform.DOScale(new Vector3(3, 3, 1), 3f).SetEase(Ease.InOutCubic).SetLink(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        TyutorialManager tyutorial;
        GameObject objT = GameObject.Find("TyutorialManager");
        tyutorial = objT.GetComponent<TyutorialManager>();

        //if (tyutorial.isXAttack)
        //{
            time += 1;
            if (time == 50)
            {
                //退場イージング
                transform.DOScale(new Vector3(0, 0, 1), 3f).SetEase(Ease.InOutCubic).SetLink(gameObject);
            }
        //}

        if (transform.localScale.x <= 0)
        {
            Destroy(gameObject);
        }

    }
}
