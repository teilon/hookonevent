﻿using calcevent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(TestEM.GetCurrentDate());

            Test test = new Test();
            //test.TestLists();
            test.TestDict();



            //delay
            Console.WriteLine("\nend.");
            Console.ReadLine();
        }
        
    }
}
