using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceanManaer : MonoBehaviour
{
    public GameObject Clear;
    public GameObject GameOver;

    //�V�[�����ׂ����ϐ����擾���邽��
    public static bool isPlayerDead = false;
    public static bool isBossDead = false;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;//frame���[�g�̌Œ�
        Clear.SetActive(false);
        GameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        int nowSceneIndexNumber = SceneManager.GetActiveScene().buildIndex;

        //���Z�b�g
        if (Input.GetKeyDown(KeyCode.R))
        {
            nowSceneIndexNumber = SceneManager.GetActiveScene().buildIndex;

            SceneManager.LoadScene(nowSceneIndexNumber);
        }
        if (SceneManager.GetActiveScene().name == "Title")
        {
            //�^�C�g������Q�[����
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(++nowSceneIndexNumber);
            }
        }
        if (SceneManager.GetActiveScene().name == "Game")
        {
            PlayerAction player;
            GameObject obj = GameObject.Find("Player");
            player = obj.GetComponent<PlayerAction>();

            BossAction boss;
            GameObject bossObj = GameObject.Find("Boss");
            boss = bossObj.GetComponent<BossAction>();


            if (boss.isDead)
            {
                isBossDead = boss.isDead;
            }

            if (player.isDead)
            {
                isPlayerDead = player.isDead;
            }

            //�v���C���[���{�X�����񂾂烊�U���g��ʂ�
            if (player.isDead || boss.isDead)
            {
                SceneManager.LoadScene(++nowSceneIndexNumber);
            }
        }

        if (SceneManager.GetActiveScene().name == "Result")
        {
            //���U���g�Ō��ʂ�\������
            if (isBossDead)
            {
                Clear.SetActive(true);
                GameOver.SetActive(false);
            }

            if (isPlayerDead)
            {
                Clear.SetActive(false);
                GameOver.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isPlayerDead = false;
                isBossDead = false;

                OptionAction option;
                GameObject objOp = GameObject.FindWithTag("Option");
                option = objOp.GetComponent<OptionAction>();

                if (option.isRight)
                {
                    SceneManager.LoadScene("Title");
                }
                else
                {
                    SceneManager.LoadScene("Game");
                }
            }
        }
    }
}
