using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceanManaer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;//frameレートの固定
    }

    // Update is called once per frame
    void Update()
    {
        int nowSceneIndexNumber = SceneManager.GetActiveScene().buildIndex;

        //リセット
        if (Input.GetKeyDown(KeyCode.R))
        {
            nowSceneIndexNumber = SceneManager.GetActiveScene().buildIndex;

            SceneManager.LoadScene(nowSceneIndexNumber);
        }
    }
}
