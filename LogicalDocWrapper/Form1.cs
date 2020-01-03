using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using System.Runtime.InteropServices;

namespace LogicalDocWrapper
{
    public partial class Form1 : Form
    {
        private Process LogicalDocProc = new Process();
        private Process MariaDBProc = new Process();

        private const int SW_HIDE = 0;
        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void runLogicalDoc()
        {
            LogicalDocProc.StartInfo.UseShellExecute = false;
            // You can start any process, HelloWorld is a do-nothing example.
            LogicalDocProc.StartInfo.FileName = textBox3.Text + "\\bin\\catalina.bat";
            LogicalDocProc.StartInfo.Arguments = "run";
            LogicalDocProc.StartInfo.EnvironmentVariables["CATALINA_HOME"] = textBox3.Text;

            LogicalDocProc.StartInfo.CreateNoWindow = true;
            LogicalDocProc.StartInfo.RedirectStandardOutput = true;
            LogicalDocProc.OutputDataReceived += new DataReceivedEventHandler(SortOutputHandler);


            LogicalDocProc.Start();

            LogicalDocProc.BeginOutputReadLine();
        }

        private void runMariaDB()
        {
            MariaDBProc.StartInfo.UseShellExecute = false;
            // You can start any process, HelloWorld is a do-nothing example.
            MariaDBProc.StartInfo.FileName = textBox4.Text + "\\bin\\mysqld";
            MariaDBProc.StartInfo.Arguments = "--console --skip-grant-tables";

            MariaDBProc.StartInfo.CreateNoWindow = false;
            MariaDBProc.StartInfo.RedirectStandardOutput = true;
            //MariaDBProc.StartInfo.RedirectStandardInput = true;
            

            MariaDBProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            MariaDBProc.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            MariaDBProc.Start();

            MariaDBProc.BeginOutputReadLine();

            while (MariaDBProc.MainWindowHandle.Equals(IntPtr.Zero)) ;


            IntPtr hWnd = MariaDBProc.MainWindowHandle;
            if (!hWnd.Equals(IntPtr.Zero))
            {
                // SW_SHOWMAXIMIZED to maximize the window
                // SW_SHOWMINIMIZED to minimize the window
                // SW_SHOWNORMAL to make the window be normal size
                ShowWindowAsync(hWnd, SW_HIDE);
            }
            

    }

        static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //* Do your stuff with the output (write to console/log/StringBuilder)
            Console.WriteLine(outLine.Data);
        }

        private void stopLogicalDoc()
        {
            Process shutdownProc = new Process();
            shutdownProc.StartInfo.UseShellExecute = false;
            // You can start any process, HelloWorld is a do-nothing example.
            shutdownProc.StartInfo.FileName = textBox3.Text + "\\bin\\shutdown.bat";
            shutdownProc.StartInfo.EnvironmentVariables["CATALINA_HOME"] = textBox3.Text;

            shutdownProc.StartInfo.CreateNoWindow = true;
            shutdownProc.StartInfo.RedirectStandardOutput = true;
            //LogicalDocProc.OutputDataReceived += new DataReceivedEventHandler(SortOutputHandler);

            shutdownProc.Start();
            //LogicalDocProc.BeginOutputReadLine();
            int i = 0;
            while(!LogicalDocProc.HasExited)
            {
                System.Threading.Thread.Sleep(10);
                i++;
                if(i > 500)
                {
                    KillProcess(LogicalDocProc.Id);
                }
            }

            //KillProcess(LogicalDocProc.Id);

        }

        private int startLogicalDoc()
        {
            Process[] processes = Process.GetProcesses();
            List<int> oldPIDs = new List<int>();

            foreach (Process process in processes)
            {
                if (process.ProcessName == "java")
                {
                    virtConsole.AppendText(process.ProcessName + " - " + process.Id.ToString() + "\r\n");
                    oldPIDs.Add(process.Id);
                }
            }

            runLogicalDoc();

            LogicalDocProc = null;
            while (LogicalDocProc == null)
            {
                processes = Process.GetProcesses();
                foreach (Process process in processes)
                {
                    if (process.ProcessName == "java")
                    {

                        if (oldPIDs.Contains(process.Id) == false)
                        {
                            virtConsole.AppendText("LD Process found: " + process.ProcessName + " - " + process.Id.ToString() + "\r\n");
                            LogicalDocProc = process;
                        }
                    }
                }
            }

            return 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            runMariaDB();
            startLogicalDoc();
        }

        void SortOutputHandler(object sender, DataReceivedEventArgs e)
        {
            Trace.WriteLine(e.Data);
            this.BeginInvoke(new MethodInvoker(() =>
            {
                virtConsole.AppendText(e.Data ?? string.Empty + "\r\n");
            }));
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        public void KillProcess(int pid)
        {
            Process[] process = Process.GetProcesses();

            foreach (Process prs in process)
            {
                if (prs.Id == pid)
                {
                    prs.Kill();
                    break;
                }
            }
        }

        private void textBox3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            folderBrowserDialog1.SelectedPath = textBox3.Text;
            folderBrowserDialog1.ShowDialog();
            textBox3.Text = folderBrowserDialog1.SelectedPath;
            
            Properties.Settings.Default["LogicalDocPath"] = textBox3.Text;
            Properties.Settings.Default.Save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox3.Text = Properties.Settings.Default["LogicalDocPath"].ToString();
            textBox4.Text = Properties.Settings.Default["MariaDBPath"].ToString();
        }

        private void textBox4_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            folderBrowserDialog1.SelectedPath = textBox4.Text;
            folderBrowserDialog1.ShowDialog();
            textBox4.Text = folderBrowserDialog1.SelectedPath;

            Properties.Settings.Default["MariaDBPath"] = textBox4.Text;
            Properties.Settings.Default.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LogicalDocProc.Kill();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //stopLogicalDoc();
            MariaDBProc.CloseMainWindow();
            MariaDBProc.Dispose();
            MariaDBProc = new Process();

            stopLogicalDoc();

            LogicalDocProc.Dispose();
            LogicalDocProc = new Process();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!MariaDBProc.HasExited)  panel2.BackColor = Color.Green;
                else panel2.BackColor = Color.Red;
            }
            catch
            {
                panel2.BackColor = Color.Red;
            }

            try
            {
                if (!LogicalDocProc.HasExited) panel1.BackColor = Color.Green;
                else panel1.BackColor = Color.Red;
            }
            catch
            {
                panel1.BackColor = Color.Red;
            }
        }
    }
}
