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
    }
}
