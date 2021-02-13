

namespace WinService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;
    using System.Threading;
    using System.Timers;

    class NeuHomeWorkWinService : ServiceBase
    {
        private Worker worker;
        private Thread workerThread;

        #region Component Designer generated code
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }



        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.ServiceName = "WinService";
        }

        #endregion

        System.Timers.Timer timer = new System.Timers.Timer();


        public NeuHomeWorkWinService()
        {
#if TRACE
            Logger.WriteMessage(LogLevel.Trace, "NeuHomeWorkWinService.cTor");
#endif
            InitializeComponent();
            worker = new Worker();
            workerThread = new Thread(new ThreadStart(worker.Start));
            workerThread.Name = "Worker Thread";
        }

        protected override void OnStart(string[] args)
        {
#if TRACE
            WriteToFile("Service Başladı " + DateTime.Now);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 5000;
            timer.Enabled = true;

            Logger.WriteMessage(LogLevel.Trace, "NeuHomeWorkWinService.OnStart");
#endif
            if (workerThread != null && !workerThread.IsAlive)
            {
                workerThread.Start();
            }
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            WriteToFile("Service Message !Logger Run :    " + DateTime.Now);
            Logger.WriteMessage("Service Message !Logger Run :    " + DateTime.Now);
        }

        protected override void OnStop()
        {
#if TRACE
            Logger.WriteMessage(LogLevel.Trace, "NeuHomeWorkWinService.OnStop");
#endif
            if (workerThread != null && workerThread.IsAlive)
            {
                worker.Stop();
                workerThread.Abort();
            }
        }

        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }

        internal void Start(string[] args)
        {
#if TRACE
            Logger.WriteMessage(LogLevel.Trace, "NeuHomeWorkWinService.Start");
#endif
            this.OnStart(args);
        }
    }
}
