﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace OmokServer;
class HeartBeatProcessor // HeartBeat 스레드 생성 및 관리
{
    private Thread _heartBeatThread;
    private bool _isRunning = false;
    private int _checkInterval = 10000; // 체크 간격 10초

    public HeartBeatProcessor()
    {
        _heartBeatThread = new Thread(new ThreadStart(RunHeartBeatCheck));
    }

    public void Start()
    {
        _isRunning = true;
        _heartBeatThread.Start(); // 스레드 시작
    }

    public void Stop()
    {
        _isRunning = false;
        _heartBeatThread.Join(); // 스레드가 종료될 때까지 기다림
    }

    private void RunHeartBeatCheck()
    {
        while (_isRunning)
        {
            CheckHeartBeat();
            Thread.Sleep(_checkInterval); // 설정된 간격으로 쉬면서 반복
        }
    }

    private void CheckHeartBeat()
    {
        // HeartBeat 체크 로직 구현
        Console.WriteLine("Checking heartbeats...");
        
    }
}
