using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace project
{
    class Program
    {

        static void Main(string[] args)
        {
            Program p           = new Program();
            TestThread thread   = new TestThread();
            thread.main();

            while (true)
            {
                if (thread.complete == TestThread.Text)
                {
                    // cccを待つときはコメント
                    return;
                }
                Console.WriteLine("メインスレッド待機中");
                Thread.Sleep(30);
            }
           
        }
    }
}
