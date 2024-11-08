using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceanManaer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;//frame���[�g�̌Œ�
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

            //�v���C���[���{�X�����񂾂烊�U���g��ʂ�
            if (player.isDead)
            {
                SceneManager.LoadScene(++nowSceneIndexNumber);
            }
        }
        if (SceneManager.GetActiveScene().name == "Result")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Title");
            }
        }
    }
}
