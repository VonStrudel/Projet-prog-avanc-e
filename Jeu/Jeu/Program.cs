using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeu
{
    public delegate int alpha(int a);
    class Program
    {
        public static int fct (int a)
        {
            Console.WriteLine("A"+ a);
            return 0;
        }

        static void Main(string[] args)
        {
            alpha fonct = fct;
            
            B c = new B("Bernard");
            A b = new A(fonct);
            alpha fonct2 = c.myName;
            b.a += fonct2;
            b.a(1);
        }
    }
    
        class A
        {
            public alpha a;
            public A(alpha fctDeleguee)
            {
                a += fctDeleguee;
            }

        }
    class B
    {
        public string name;
        public B(string name)
        {
            this.name = name;
        }
        public int myName(int a)
        {
            Console.WriteLine(name + a);
            name += a;
            return 0;
        }
    }
    
}
