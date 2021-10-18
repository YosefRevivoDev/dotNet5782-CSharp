using System;
using DAL;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("test");
            DAL.StartApplication apps = new StartApplication();
            apps.start();

        }
    }
}
