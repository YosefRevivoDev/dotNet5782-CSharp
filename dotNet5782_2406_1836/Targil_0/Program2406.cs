using System;

namespace Targil_0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome2406();
            Welcome1836();
        }
        static partial void Welcome1836();
        private static void Welcome2406()
        {
            Console.WriteLine("Enter your name: ");
            string username = Console.ReadLine();
            Console.WriteLine(username + ", welcome to my first console application");
        }
    }
}
