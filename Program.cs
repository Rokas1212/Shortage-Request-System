using System;
using ShortageRequests.Util;

namespace ShortageRequests
{
    public class Program
    {

        public static void Main()
        {
            Console.Write("Enter your name: ");
            var name = Console.ReadLine() ?? "";
            if (name == "")
            {
                Console.WriteLine("Invalid name!");
                return;
            }
            Operations.Start(name);
        }
    }   
}