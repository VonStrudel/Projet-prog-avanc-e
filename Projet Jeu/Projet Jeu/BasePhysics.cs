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
    delegate BasePhysics collisionChecker(BasePhysics myself, Vect2D direction);
    /// <summary>
    /// Comportement physique du worldObject.
    /// </summary>
    class BasePhysics
    {
        int speed;//Nombre de tics entre chaque mouvement
        int actualTime;//Nombre de tics actuels
        public collisionChecker checkForCollision; // ca sera une fonction de world
        public GamePosition pos; //Un objet physique possede sa propre position pour etre indépendant de son WorldObject
        public void update()
        {
            actualTime++; 
        }
        public BasePhysics()
        {
            this.speed = 10;
        }
        /// <summary>
        /// Constructeur pour un collider uniquement (utile pour les murs, ou les bords de map)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public BasePhysics(int x, int y, int layer)
        {
            this.pos = new GamePosition(new Vect2D(x, y), layer, direction.none);
        }
        public Vect2D checkMove(Vect2D movement)
        {
            //verifie si il peut bouger
            Vect2D newMovement = new Vect2D(0, 0);
            this.pos.pos += newMovement;
            return newMovement;
        }
        public Vect2D checkTeleportation(Vect2D movement)
        {
            //Verifie si il peut se tp là. Si il y a quelque chose, la teleportation ne se fait pas
            Vect2D newMovement;
            newMovement = movement;
            BasePhysics collided = this.checkForCollision(this, movement);

            newMovement = new Vect2D(0, 0);
            this.pos.pos += newMovement;
            return newMovement;
        }
        public Vect2D onCollide(BasePhysics obj, Vect2D hisTrajectory)
        {
            return new Projet_Jeu.Vect2D(0, 0);
        }
        public Vect2D onTpCollide(BasePhysics obj, Vect2D hisTrajectory)
        {

            return new Projet_Jeu.Vect2D(0, 0);
        }
    }


    class PlayerPhysics : BasePhysics
    {
    }
}
