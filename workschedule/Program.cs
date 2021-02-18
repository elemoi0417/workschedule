using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace workschedule
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Mod Start WataruT 2021.02.18 多重起動を禁止し、画面の最小化表示を可能とする
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Login());

            //Mutex名を決める（必ずアプリケーション固有の文字列に変更すること！）
            string mutexName = "MyApplicationName";
            //Mutexオブジェクトを作成する
            System.Threading.Mutex mutex = new System.Threading.Mutex(false, mutexName);

            bool hasHandle = false;
            try
            {
                try
                {
                    //ミューテックスの所有権を要求する
                    hasHandle = mutex.WaitOne(0, false);
                }
                //.NET Framework 2.0以降の場合
                catch (System.Threading.AbandonedMutexException)
                {
                    //別のアプリケーションがミューテックスを解放しないで終了した時
                    hasHandle = true;
                }
                //ミューテックスを得られたか調べる
                if (hasHandle == false)
                {
                    //得られなかった場合は、すでに起動していると判断して終了
                    MessageBox.Show("勤務表管理システムは既に起動しています。");
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Login());
            }
            finally
            {
                if (hasHandle)
                {
                    //ミューテックスを解放する
                    mutex.ReleaseMutex();
                }
                mutex.Close();
            }
            // Mod End   WataruT 2021.02.18 多重起動を禁止し、画面の最小化表示を可能とする
        }
    }
}
