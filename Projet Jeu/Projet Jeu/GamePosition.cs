using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_Jeu
{
    class GamePosition
    {
        public Vect2D pos;
        public int layer;
        public direction orientation;
        public GamePosition(Vect2D pos, int layer, direction or)
        {
            this.pos = pos;
            this.layer = layer;
            this.orientation = or;
        }
        public GamePosition(int x, int y, int layer, direction or)
        {
            this.pos = new Vect2D(x, y);
            this.layer = layer;
            this.orientation = or;
        }
        public override string ToString()
        {
            return this.pos + "l" + layer + "d" + orientation;
        }
    }
}
