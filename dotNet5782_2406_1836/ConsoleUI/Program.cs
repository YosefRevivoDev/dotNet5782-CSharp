using System;
using DAL;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("test project");
            DAL.StartApplication apps = new StartApplication();
            apps.start();

        }
    }
}
