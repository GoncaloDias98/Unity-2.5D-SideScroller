﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{
        public GameObject player;
        private GameCamera cam;

    // Start is called before the first frame update
    void Start(){
        cam = GetComponent<GameCamera>();
        SpawnPlayer();
    }

   private void SpawnPlayer(){
       cam.SetTarget((Instantiate(player, Vector3.zero, Quaternion.identity) as GameObject).transform);
   }
}
