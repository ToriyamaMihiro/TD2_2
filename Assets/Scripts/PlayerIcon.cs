using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIcon : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite damageSprite;
    private Image image;

    void Start()
    {
        // SpriteRendererコンポーネントを取得
        image = GetComponent<Image>();
    }

    void Update()
    {
        PlayerAction player;
        GameObject obj = GameObject.Find("Player");
        player = obj.GetComponent<PlayerAction>();
        if (player.isHit)
        {
            // 画像を切り替え
            image.sprite = damageSprite;
        }
        else
        {
            image.sprite = normalSprite;
        }
    }
}
