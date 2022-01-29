using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Fish : MonoBehaviour
{
    private Fish.FishType type;
    private CircleCollider2D collidertwod;
    private SpriteRenderer spriteRenderer;
    private float screenLeft;
    private Tweener tweener;

    public Fish.FishType Type
    {
        get
        {
            return type;
        }
        set
        {
            type=value;
            collidertwod.radius=type.colliderRadius;
            spriteRenderer.sprite = type.fishSprite;
        }
    }


    [System.Serializable]
    public class FishType
    {
        public int price;
        public float fishCount,minLength,maxLength,colliderRadius;
        public Sprite fishSprite;
    }

    void Awake()
    {
        collidertwod = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        screenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
    }
    public void ResetFish()
    {
        if(tweener != null)
            tweener.Kill(false);
        
        float num=Random.Range(type.minLength,type.maxLength);
        collidertwod.enabled=true;

        Vector3 position = transform.position;
        position.y = num;
        position.x = screenLeft;
        transform.position = position;

        float num2 = 1;
        float y = Random.Range(num - num2,num + num2);
        Vector2 vector2=new Vector2(-position.x,y);

        float num3 = 3;
        float delay = Random.Range(0, 2 * num3);
        tweener = transform.DOMove(vector2,num3,false).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.Linear).SetDelay(delay).OnStepComplete(delegate{
            Vector3 localScale = transform.localScale;
            localScale.x = -localScale.x;
            transform.localScale = localScale;
        });
    }

    public void Hooked()
    {
        collidertwod.enabled=false;
        tweener.Kill(false);
    }


    void Update()
    {
        
    }
}
