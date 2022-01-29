using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FishSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        for(int i=0;i<fishTypes.Length;i++)
        {
            int num=0;
            while(num< fishTypes[i].fishCount)
            {
                Fish fish=Instantiate<Fish>(fishPrefab);
                fish.Type=fishTypes[i];
                fish.ResetFish();
                num++;
            }
        }
    }

    [SerializeField]
    private Fish fishPrefab;
    [SerializeField]
    private Fish.FishType[] fishTypes;

    // Update is called once per frame
    void Update()
    {
        
    }
}
