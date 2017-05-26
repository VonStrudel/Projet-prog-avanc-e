using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_Jeu
{

    class Vect2D
    {
        public int x;
        public int y;
        public Vect2D(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Vect2D normalize()
        {
            int size = (int)Vect2D.getDistance(this, new Vect2D(0, 0));
            return new Vect2D(this.x / size, this.y / size);
        }
        public static double getDistance(Vect2D p1, Vect2D p2)
        {
            return Math.Sqrt(Math.Pow(p2.x - p1.x, 2) + Math.Pow(p2.y - p1.y, 2));
        }
        public static Vect2D operator +(Vect2D p1, Vect2D p2)
        {
            return new Vect2D(p1.x + p2.x, p1.y + p2.y);
        }
        public static Vect2D operator -(Vect2D p1, Vect2D p2)
        {
            return new Vect2D(p1.x - p2.x, p1.y - p2.y);
        }
        public static bool operator ==(Vect2D p1, Vect2D p2)
        {
            if (p1.x == p2.x && p1.y == p2.y)
                return true;
            else
                return false;
        }
        public static bool operator !=(Vect2D p1, Vect2D p2)
        {
            return !(p1 == p2);
        }
        public static Vect2D operator ++(Vect2D p)
        {
            p.x++;
            p.y++;
            return p;
        }
        public override string ToString()
        {
            return this.x + " :" + this.y;
        }
        public Vect2D Copy()
        {
            return new Vect2D(this.x, this.y);
        }
    }
}
