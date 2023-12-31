﻿using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameManager:MonoBehaviour
    {
        public static GameManager Instance;

        public bool IsPaused = true;

        public void Awake()
        {
            Instance = this;
            Application.targetFrameRate = 60;
        }

        //Пауза
        public void ChangeState()
        {
            IsPaused = !IsPaused;
        }
    }
}