using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class MyMMTimer
    {
        // 定义委托类型
        public delegate void TimerCallbackDelegate(uint uTimerID, uint uMsg, UIntPtr dwUser, UIntPtr dw1, UIntPtr dw2);

        // 定义回调方法
       // private void TimerCallbackMethod(uint uTimerID, uint uMsg, UIntPtr dwUser, UIntPtr dw1, UIntPtr dw2)
        //{
            //Console.WriteLine($"Timer callback at {DateTime.Now:HH:mm:ss.fff}");
       // }

        // 定义TimerCallbackDelegate类型的委托字段
        private TimerCallbackDelegate timerCallbackDelegateThis;

        // 定义定时器ID字段
        private uint timerId;

        // 创建定时器并注册回调
        public void CreateTimer(TimerCallbackDelegate timerCallbackDelegate, uint tick)
        {
            // 创建委托实例
            timerCallbackDelegateThis = timerCallbackDelegate;

            // 注册定时器回调
            timerId = timeSetEvent(tick, 1, timerCallbackDelegate, UIntPtr.Zero, TIME_PERIODIC);
        }

        // 销毁定时器
        public void DestroyTimer()
        {
            // 解除定时器回调
            timeKillEvent(timerId);
        }

        // 导入winmm.dll
        [DllImport("winmm.dll")]
        private static extern uint timeSetEvent(uint uDelay, uint uResolution, TimerCallbackDelegate lpTimeProc, UIntPtr dwUser, uint fuEvent);

        [DllImport("winmm.dll")]
        private static extern uint timeKillEvent(uint uTimerID);

        private const uint TIME_PERIODIC = 0x1;

        // 需要在应用程序关闭时调用
        public void Cleanup()
        {
            // 解除委托引用，防止垃圾回收
            timerCallbackDelegateThis = null;
        }
    }

}
