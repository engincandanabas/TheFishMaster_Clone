using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Hook : MonoBehaviour
{
    public Transform hookedTransform;

    private Camera mainCamera;
    private Collider2D collidertwod;

    private int length;
    private int strength;
    private int fishCount;
    private bool canMove=true;
    private Tweener cameraTween;
    private List<Fish> hookedFishes;
    void Awake()
    {
        mainCamera=Camera.main;
        collidertwod=GetComponent<Collider2D>();
        hookedFishes = new List<Fish>();
    }

    void Update()
    {
        if(canMove && Input.GetMouseButton(0))
        {
            Vector3 vector = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position= transform.position;
            position.x=vector.x;
            transform.position=position;
        }
    }
    public void StartFishing()
    {
        length= IdleManager.instance.length-20;
        strength = IdleManager.instance.strength;
        fishCount = 0;
        float time = (-length) * 0.1f;

        cameraTween = mainCamera.transform.DOMoveY(length,1+ time * 0.25f , false).OnUpdate(delegate{
            if(mainCamera.transform.position.y <= -11)
            {
                transform.SetParent(mainCamera.transform);
            }
        }).OnComplete(delegate{
            collidertwod.enabled=true;
            cameraTween = mainCamera.transform.DOMoveY(0,time * 5,false).OnUpdate(delegate{
                if(mainCamera.transform.position.y >=-25f)
                {
                    StopFishing();
                }
            });
        });
        ScreensManager.instance.ChangeScreen(Screens.GAME);
        collidertwod.enabled=false;
        canMove=true;
        hookedFishes.Clear();
    }
    public void StopFishing()
    {
        canMove=false;
        cameraTween.Kill(false);
        cameraTween = mainCamera.transform.DOMoveY(0,2,false).OnUpdate(delegate{
            if(mainCamera.transform.position.y >=-11f)
            {
                transform.SetParent(null);
                transform.position = new Vector2(transform.position.x,-6f);
            }
        }).OnComplete(delegate{
            transform.position= Vector2.down * 6;
            collidertwod.enabled=true;
            int num=0;
            for(int i=0;i< hookedFishes.Count;i++)
            {
                hookedFishes[i].transform.SetParent(null);
                hookedFishes[i].ResetFish();
                num+=hookedFishes[i].Type.price;
            }
            IdleManager.instance.totalGain=num;
            ScreensManager.instance.ChangeScreen(Screens.END);
        });
    }
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.CompareTag("Fish") && fishCount != strength)
        {
            fishCount++;
            Fish component= collider2D.GetComponent<Fish>();
            component.Hooked();
            hookedFishes.Add(component);
            collider2D.transform.SetParent(transform);
            collider2D.transform.position = hookedTransform.position;
            collider2D.transform.rotation = hookedTransform.rotation;
            collider2D.transform.localScale = Vector3.one;

            collider2D.transform.DOShakeRotation(5,Vector3.forward*45,10,90,false).SetLoops(1,LoopType.Yoyo).OnComplete(delegate{
                collider2D.transform.rotation = Quaternion.identity;
            });
            if(fishCount == strength)
            {
                StopFishing();
            }
        }
    }
}
