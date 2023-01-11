using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DemoApp
{
    /// <summary>
    /// https://www.youtube.com/watch?v=RqJESGHEMDY
    /// https://www.youtube.com/watch?v=wErS2ODmkNQ
    ///
    /// Every object that has a GetAwaiter method can come after a await keyword.
    /// </summary>
    class DemoStateMachine
    {
        public void Run()
        {

            int __state = 0;
            TaskAwaiter __delayAwaiter = default(TaskAwaiter);

            Action doIt = default(Action);

            doIt = () =>
            {
                Trace.WriteLine("Enter doIt");
                switch (__state)
                {
                    case 0:
                        Trace.WriteLine("Enter State = 0");

                        __delayAwaiter = Task.Delay(100).GetAwaiter();
                        if (__delayAwaiter.IsCompleted)

                        {
                            goto case 1;
                        }
                        else
                        {
                            __state = 1;
                            __delayAwaiter.OnCompleted(doIt);
                        }

                        break;
                        case 1:
                            Trace.WriteLine("Enter State = 1");
                            __delayAwaiter.GetResult();

                        break;
                }
            };

            
            doIt();

            Trace.WriteLine("Exit Run");
        }
    }
}
