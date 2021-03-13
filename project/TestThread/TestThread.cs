using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace project
{
    class TestThread
    {
        public const string Text = "完了";
        public string complete = "";
        private object lockObj = new object();

        public async void main()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            aaa();
            anotherBbb();       // async いらん
            //await waitBbb();  // async いる
            ccc();

            sw.Stop();
            TimeSpan time = sw.Elapsed;
            Console.WriteLine("\nメインスレッドの処理時間");
            Console.WriteLine(time.Hours + "時間 " + time.Minutes + "分 " + time.Seconds + "秒 " + time.Milliseconds + "ミリ秒\n");
        }

        void aaa()
        {
            Console.WriteLine("aaaの処理を実行");
        }

        // 処理が終わってから情報を渡す非同期スレッド
        void anotherBbb()
        {
            Console.WriteLine("bbbの処理を実行");

            // Taskをコメントにしたら非同期じゃなくなる
            Task.Run(() =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                for (int i = 0; i < 50; ++i)
                {
                    Console.WriteLine("\n" + (i + 1) + "回目のbbbの処理を実行中です  ( ファイル読み込み中 )" + "\n");
                    Thread.Sleep(100);
                }

                // 下の不具合を解決するために、絶対に一緒に通らせたいところはロックして、処理が終わるまで抜け出さんようにする
                lock (lockObj)
                {
                    // ここに通った時に、メインスレッドで条件を満たして、
                    complete = Text;

                    // こっちの処理が終わってないのに条件を満たすから->のバグの可能性がある ( 終了, スキップ, setter, getter の値が違うくなったり ) 
                    sw.Stop();
                    TimeSpan time = sw.Elapsed;
                    Console.WriteLine("\n非同期スレッドの処理時間");
                    Console.WriteLine(time.Hours + "時間 " + time.Minutes + "分 " + time.Seconds + "秒 " + time.Milliseconds + "ミリ秒\n");
                }
            });
        }

        // こっちの処理が終わるまでcccの処理を待つ
        async Task waitBbb()
        {
            Console.WriteLine("bbbの処理を実行");
            await Task.Run(() =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                for (int i = 0; i < 50; ++i)
                {
                    Console.WriteLine("\n" + (i + 1) + "回目のbbbの処理を実行中です  ( ファイル読み込み中 )" + "\n");
                    Thread.Sleep(100);
                }
                // 処理を上から順にやっていくから、別にlockせんでもいい
                complete = Text;

                sw.Stop();
                TimeSpan time = sw.Elapsed;
                Console.WriteLine("\n非同期スレッドの処理時間");
                Console.WriteLine(time.Hours + "時間 " + time.Minutes + "分 " + time.Seconds + "秒 " + time.Milliseconds + "ミリ秒\n");
            });
        }

        void ccc()
        {
            Console.WriteLine("cccの処理を実行");
        }
    }
}
