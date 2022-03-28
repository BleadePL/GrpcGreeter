using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcGreeterClient
{
    public static class ProgressBar
    {
        public static Dictionary<int, bool> ProgressBarDict = new Dictionary<int, bool>()
        {
            { 10, false },{ 20, false }, { 30, false }, { 40, false }, { 50, false }, { 60, false } ,{ 70, false }, { 80, false },{ 90, false },{ 100, false }
        };

        public static void draw(int actPos, int size)
        {
            double progress = (double)((double)actPos / (double)size) * 100;


            switch (progress)
            {
                case <= 10.0:
                    if (!ProgressBarDict[10])
                    {
                        Console.Clear();
                        Console.WriteLine("Sending Data to server");
                        Console.WriteLine("[*---------] Progress");
                        ProgressBarDict[10] = true;
                    }
                   break;

                case <= 20.0:
                    if (!ProgressBarDict[20])
                    {
                        Console.Clear();
                        Console.WriteLine("Sending Data to server");
                        Console.WriteLine("[**--------] Progress");
                        ProgressBarDict[20] = true;
                    }
                    break;

                case <= 30.0:
                    if (!ProgressBarDict[30])
                    {
                        Console.Clear();
                        Console.WriteLine("Sending Data to server");
                        Console.WriteLine("[***-------] Progress");
                        ProgressBarDict[30] = true;
                    }
                    break;

                case <= 40.0 :
                    if (!ProgressBarDict[40])
                    {
                        Console.Clear();
                        Console.WriteLine("Sending Data to server");
                        Console.WriteLine("[****------] Progress");
                        ProgressBarDict[40] = true;
                    }
                    break;

                case <= 50.0:
                    if (!ProgressBarDict[50])
                    {
                        Console.Clear();
                        Console.WriteLine("Sending Data to server");
                        Console.WriteLine("[*****-----] Progress");
                        ProgressBarDict[50] = true;
                    }
                    break;

                case <= 60.0:
                    if (!ProgressBarDict[60])
                    {
                        Console.Clear();
                        Console.WriteLine("Sending Data to server");
                        Console.WriteLine("[******----] Progress");
                        ProgressBarDict[60] = true;
                    }
                    break;

                case <= 70.0:
                    if (!ProgressBarDict[70])
                    {
                        Console.Clear();
                        Console.WriteLine("Sending Data to server");
                        Console.WriteLine("[********---] Progress");
                        ProgressBarDict[70] = true;
                    }
                    break;

                case <= 80.0:
                    if (!ProgressBarDict[80])
                    {
                        Console.Clear();
                        Console.WriteLine("Sending Data to server");
                        Console.WriteLine("[********--] Progress");
                        ProgressBarDict[80] = true;
                    }
                    break;

                case <= 90:
                    if (!ProgressBarDict[90])
                    {
                        Console.Clear();
                        Console.WriteLine("Sending Data to server");
                        Console.WriteLine("[*********-] Progress");
                        ProgressBarDict[90] = true;
                    }
                    break;
                case 100.0:
                    Console.Clear();
                    Console.WriteLine("Data was sent");
                    Console.WriteLine("[**********] Done");

                    foreach (var item in ProgressBarDict.Keys)
                    {
                        ProgressBarDict[item] = false;
                    }

                    break;

            }


        }

    }
}
