using System;
using DAL;
using DAL.DalObject;
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
