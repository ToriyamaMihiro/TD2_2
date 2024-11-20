using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPImage : MonoBehaviour
{
    public Image image;

    public bool isDamage;


    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAction player;
        GameObject obj = GameObject.Find("Player");
        player = obj.GetComponent<PlayerAction>();

        //�v���C���[�Ƀ_���[�W�������Ă��邩���ڂ̂Ƃ�
        if (player.isHit && !isDamage)
        {
            isDamage = true;
            //1�Ń}�b�N�X�Ȃ̂łP����HP��������
            image.fillAmount -= 0.1f;
        }
        //if (!player.isHit)
        //{

        //    isDamage = false;
        //}

    }
}
