using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionAction : MonoBehaviour
{
    private TD2_2 inputAcution;

    public GameObject Retry;
    public GameObject Title;

    //音
    private AudioSource audioSource;
    public AudioClip selectAudio;

    public bool isRight = false;//右を選択しているか

    // Start is called before the first frame update
    void Start()
    {
        inputAcution = new TD2_2();
        inputAcution.Enable();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputAcution.Player.Left.IsPressed())
        {
            //audioSource.PlayOneShot(selectAudio);//音
            audioSource.clip = selectAudio;
            audioSource.Play();
            isRight = false;
        }

        if (inputAcution.Player.Right.IsPressed())
        {
            //audioSource.PlayOneShot(selectAudio);//音
            audioSource.clip = selectAudio;
            audioSource.Play();
            isRight = true;
        }
        if (isRight)
        {
            //拡大拡縮アニメーション
            Title.transform.localScale = new Vector3(2f, 2f, 2f);
            Retry.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (!isRight)
        {
            //拡大拡縮アニメーション
            Title.transform.localScale = new Vector3(1f, 1f, 1f);
            Retry.transform.localScale = new Vector3(2f, 2f, 2f);
        }
    }
}
