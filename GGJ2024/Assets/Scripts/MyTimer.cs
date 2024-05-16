using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer
{

    private float _timer = 0;
    private float _duration;

    public MyTimer(float duration)
    {
        _duration = duration;
    }

    public void Tick()
    {
        _timer += Time.deltaTime;
    }
    public void Reset()
    {
        _timer = 0;
    }

    public bool IsOver()
    {
        return _timer >= _duration;
    }



}
