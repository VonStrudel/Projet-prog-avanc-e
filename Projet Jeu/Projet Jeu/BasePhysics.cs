using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_Jeu
{

    class Rectangle
    {
        Vect2D p1;
        Vect2D p2;
        Vect2D topLeft
        {
            get
            {
                if (p1.x < p2.x) return p1;
                return p2;
            }
            /*set
            {
                if (value.x != p2.x && value.y != p2.y)
                {
                    if (p1.x < p2.x)
                    {
                        p1 = value;
                    }
                    p2 = value;
                }
            }*/
        }
        Vect2D botRight
        {
            get
            {
                if (p1.x > p2.x) return p1;
                return p2;
            }
           /* set
            {
                if (value.x != p2.x && value.y != p2.y)
                {
                    if (p1.x > p2.x)
                    {
                        p1 = value;
                    }
                    p2 = value;
                }
            }*/
        }

        public bool checkCollision(Rectangle rect)
        {
            if(rect.topLeft.y >= this.botRight.y && rect.botRight.y <= this.topLeft.y)
                if (rect.topLeft.x >= this.botRight.x && rect.botRight.x <= this.topLeft.x)
                    return true;
            return false;
        }
        public bool checkCollision(Vect2D p)
        {
            if (p.x >= p1.x && p.x <= p2.x && p.y >= p1.y && p.y <= p2.y)
                return true;
            return false;
        }
    }
    /*
    class Circle
    {
        Vect2D p;
        int radius;
        public bool checkCollision(Circle c)
        {
            if (Vect2D.getDistance(c.p, this.p) > radius)
                return false;
            return true;
        }

    }*/
    delegate GamePosition collisionChecker(BasePhysics myself, Vect2D direction);
    /// <summary>
    /// Comportement physique du worldObject.
    /// </summary>
    class BasePhysics
    {
        int speed;//Nombre de tics entre chaque mouvement
        int actualTime;//Nombre de tics actuels
        public collisionChecker checkForCollision; // ca sera une fonction de world
        public GamePosition pos;
        public void update()
        {
            actualTime++; 

        }
        public BasePhysics()
        {
            this.speed = 10;
        }
        public virtual GamePosition move(Vect2D dir)
        {
            if (actualTime >= speed) //on peut bouger
            {

                actualTime = 0;
                GamePosition collision = checkForCollision(this, dir); //on regarde si y'a une collision ou pas. Si y'en a une on a sa position
                if (collision != null)
                {
                    pos.pos = collision.pos - dir.normalize(); //on place l'objet un "bloc" avant la collision
                }
                else
                {
                    pos.pos += dir;
                }
            }
           
            return new GamePosition(new Vect2D(0, 0), 0, (direction)0);
        }
        public virtual GamePosition teleport(GamePosition pos)
        {
            return new GamePosition(new Vect2D(0, 0), 0, (direction)0);
        }
        public virtual GamePosition changeLayer(int newLayer)
        {
            return new GamePosition(new Vect2D(0, 0), 0, (direction)0);
        }
    }
    class PlayerPhysics : BasePhysics
    {

    }
    class WallPhysics : BasePhysics
    {

    }
}

/*
     delegate GamePosition PhysicsMove(Vect2D dir);
    delegate GamePosition PhysicsTeleport(GamePosition pos);
    delegate GamePosition PhysicsChangeLayer(int newLayer);
     */
