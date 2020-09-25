﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Windows;
using System.Runtime.InteropServices;
using System.Linq;

 namespace ConsoleApplication1
{
    public class process:IComparable<process>
    {
        public int pid, time, waitTime = 0,priority,arrival,remaining;
        public string tag;
        public process(int pid, int time, string tag,int priority=-1,int arrival=-1) 
        {
            this.pid = pid;
            this.time = time;
            this.tag = tag;
            if (priority >= 0)
                this.priority = priority;
            if (arrival >= 0)
                this.arrival = arrival;
        }
        public int CompareTo(process other)
        {
            return arrival.CompareTo(other.arrival);
        }
        public string ToString()
        {
            return "pid: " +pid+ "\ntag: " +tag+ "\narrival: " +arrival+ "\npriority: " +priority+ "\nrunning: " + time+"\nremaining: "+remaining;
        }
    }
    class algo
    {

        enum type { FCFS, SJF ,Priority,srtf,round_robin};
        public unsafe void config()
        {
            string choice = "n";
            Queue<process> processes = new Queue<ConsoleApplication1.process>();
            foreach(type x in Enum.GetValues(typeof(type)))
            Console.Write("{0}:{1} \n", x, (int)x);
            Console.WriteLine("\ninput: ");
            choice = Console.ReadLine();
            switch (int.Parse(choice))
            {
                case (int)type.FCFS:
                    Queue<process> processQueue = new Queue<ConsoleApplication1.process>();
                    do
                    {
                        int pid, time;
                        string tag = input(&pid, &time);
                        processQueue.Enqueue(new process(pid, time, tag));
                        Console.WriteLine("another process?(y|n): ");
                        choice = Console.ReadLine();

                    } while (choice[0] == 'y');
                    drawChart(fcfs(processQueue));
                    break;
                case (int)type.Priority:
                    List<process> priorityList = new List<process>();
                    do
                    {
                        int pid, time,priority;
                        string tag = input(&pid, &time,&priority);
                        priorityList.Add(new process(pid, time, tag,priority));
                        Console.WriteLine("another process?(y|n): ");
                        choice = Console.ReadLine();

                    } while (choice[0] == 'y');
                    drawChart(prioritySchedulor(priorityList));
                    break;
                case (int)type.SJF:
                    List<process> processList = new List<ConsoleApplication1.process>();
                    do
                    {
                        int pid, time;
                        string tag = input(&pid, &time);
                        processList.Add(new process(pid, time, tag));
                        Console.WriteLine("another process?(y|n): ");
                        choice = Console.ReadLine();

                    } while (choice[0].Equals('y'));
                    drawChart(sjf(processList));
                    break;
               
                case (int)type.srtf:
                    List<process> srtfList = new List<process>();
                    do
                    {
                        int pid, time, priority;
                        string tag = input(&pid, &time, &priority);
                        srtfList.Add(new process(pid, time, tag, priority));
                        Console.WriteLine("another process?(y|n): ");
                        choice = Console.ReadLine();

                    } while (choice[0] == 'y');
                    drawChart(prioritySchedulor(srtfList));
                    break;
                case (int)type.round_robin:
                    Queue<process> RRQ = new Queue<process>();
                     Console.WriteLine("slice time of process: ");
                    int slice=int.Parse(Console.ReadLine());;
                    do
                    {
                        int pid, time;
                        string tag = input(&pid, &time);
                        RRQ.Enqueue(new process(pid, time, tag));
                        Console.WriteLine("another process?(y|n): ");
                        choice = Console.ReadLine();

                    } while (choice[0] == 'y');
                    drawChart(RR(RRQ,slice));
                    break;
            }


        }
        public unsafe string input(int* pid, int* burst, int* priority = null,int *arrival=null)
        {

            Console.WriteLine("name of process: ");
            string tag = Console.ReadLine();
            Console.WriteLine("burst time of process: ");
            *burst = int.Parse(Console.ReadLine());
            Console.WriteLine("pid of process: ");
            *pid = int.Parse(Console.ReadLine());
            if (priority != null)
            {
                Console.WriteLine("priority of process: ");
                *priority = int.Parse(Console.ReadLine());
            }
            if (arrival != null)
            {
                Console.WriteLine("arrival time of process: ");
                *arrival = int.Parse(Console.ReadLine());
            }
                return tag;
        }
        public process[] fcfs(Queue<process> process)
        {

            process[] p = new process[process.Count];
            for (int i = 0; process.Count > 0; i++)
            {
                p[i] = process.Dequeue();
                p[i].waitTime = i == 0 ? 0 : p[i - 1].waitTime + p[i - 1].time;
                Console.WriteLine("process name:{0}\npid:{1}\nwaiting time:{2}\nrunning time:{3}", p[i].tag, p[i].pid, p[i].waitTime, p[i].time);
            }
            return p;
        }
        public process[] sjf(List<process> p)
        {
            p.Sort((a, b) => (a.time.CompareTo(b.time)));
            for (int i = 0; i < p.Count; i++)
            {
                p[i].waitTime = i == 0 ? 0 : p[i - 1].waitTime + p[i - 1].time;
                Console.WriteLine("process name:{0}\npid:{1}\nwaiting time:{2}\nrunning time:{3}", p[i].tag, p[i].pid, p[i].waitTime, p[i].time);
            }
            return p.ToArray();
        }
        public process[] prioritySchedulor(List<process> process)
        {
            process.Sort((a, b) => (a.priority.CompareTo(b.priority)));
            return fcfs(new Queue<process>(process));
        }
        public process[] RR(Queue<process> process,int slice)
        {
            List<process> output= new List<process>();
            int i = 0;process p;
            while(process.Count>0){
                p = process.Dequeue();
                if(p.time>slice){
                    output.Add(new process(p.pid,slice,p.tag,p.priority,p.arrival));
                    p.time -= slice;
                    output[output.Count - 1].waitTime = output.Count - 1==0?0:output[output.Count - 2].waitTime + output[output.Count - 2].time;
                    
                    if(p.time>=0)
                    process.Enqueue(p);
                }
                else
                {
                    output.Add(new process(p.pid, p.time, p.tag, p.priority, p.arrival));
                    output[output.Count - 1].waitTime = output[output.Count - 2].waitTime + output[output.Count - 2].time;
                }
            }
            return output.ToArray();
        }
        [STAThread]
        public void drawChart(process[] p)
        {
            Application.EnableVisualStyles();
            Form win = new Form()
            {
                Text = "os",
                MaximizeBox = false,
                MinimizeBox = false,
                StartPosition = FormStartPosition.CenterScreen,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Width = 900,
                Height = 300
            };
            win.Paint += new PaintEventHandler((object sender, PaintEventArgs e) =>
            {
                int x, y, avg = 0;
                for (int i = 0; i < p.Length; i++)
                {
                    avg += p[i].waitTime;
                    y = 20;
                    x = 50 + (i == 0 ? 0 : ((int)(800 * ((float)p[i].waitTime / (float)(p[p.Length - 1].time + p[p.Length - 1].waitTime)))));
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(i * 50 % 250, i * 80 % 250, i * 100 % 250)), Rectangle.Round(new Rectangle(x, y, (int)(800 * ((float)p[i].time / (float)(p[p.Length - 1].time + p[p.Length - 1].waitTime))), 50)));
                    e.Graphics.DrawString("pid:" + p[i].pid, new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point), Brushes.White, x + 6, y + 15, StringFormat.GenericDefault);
                }
                e.Graphics.DrawString("average wait time: " + (float)avg / (float)p.Length, new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point), Brushes.Black, 350, 215, StringFormat.GenericDefault);

            });
            DataGridView table = new DataGridView()
            {
                ColumnCount = 5,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single,
                CellBorderStyle = DataGridViewCellBorderStyle.Single,
                RowHeadersVisible = false
            };
            table.SetBounds(50, 100, 800, 100);
            table.Columns[0].Name = "pid";
            table.Columns[1].Name = "name/tag";
            table.Columns[2].Name = "burst time";
            table.Columns[3].Name = "wait time";
            table.Columns[4].Name = "priority";
            for (int i = 0; i < table.ColumnCount; i++)
                table.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            for (int i = 0; i < p.Length; i++)
                table.Rows.Add(new string[] { p[i].pid + "", "" + p[i].tag, "" + p[i].time, "" + p[i].waitTime, (p[i].priority >= 0 ? ("" + p[i].priority) : ("n/a")) });
            win.Controls.Add(table);
            Application.Run(win);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<process> x = new List<process>() { new process(1, 20, "a", 0, 1), new process(2, 3, "aa", 2, 3), new process(3, 5, "aaa", 1, 0) };
            algo alg = new algo();
            //alg.drawChart(alg.RR(new Queue<process>(x),5));
            alg.config();
        }
    }
}
