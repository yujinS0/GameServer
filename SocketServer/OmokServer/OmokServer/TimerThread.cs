using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OmokServer;
public class TimerThread
{
    private Timer timer;
    private int interval;
    private Action onTimerElapsed;
    private bool isRunning = false;

    public TimerThread(int interval, Action callback)
    {
        this.interval = interval;
        this.onTimerElapsed = callback;
    }

    public void Start()
    {
        if (isRunning)
            return;

        timer = new Timer(TimerCallback, null, interval, Timeout.Infinite);
        isRunning = true;
    }

    public void Stop()
    {
        timer?.Change(Timeout.Infinite, Timeout.Infinite);
        isRunning = false;
    }

    private void TimerCallback(object state)
    {
        onTimerElapsed?.Invoke();
        
        // Restart the timer if still running
        if (isRunning)
            timer.Change(interval, Timeout.Infinite);
    }
}