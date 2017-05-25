using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_Jeu
{
    /*Class représentant le monde (le niveau)*/
    // GamePosition collisionChecker(BasePhysics myself, Vect2D direction);
    class World : IEventThrower
    {
        WorldObject[,] level;
        void display() { }
        void update() { }
        public World()
        {

        }
        /// <summary>
        /// Check de collision basique
        /// </summary>
        /// <param name="p">L'objet physique qui veut vérifier sa collision</param>
        /// <param name="direction">Le vecteur de direction de cet objet physique (vers ou il va)</param>
        /// <returns></returns>
        public virtual GamePosition collisionCheck(BasePhysics p, Vect2D direction )
        {
            Vect2D currentPosCursor = new Vect2D(0, 0);
            Vect2D normalizedDirection = direction.normalize();
            for (int i = 0; i < Vect2D.getDistance(direction, new Vect2D(0, 0)); i++)
            { 
                WorldObject collidedObject = level[currentPosCursor.x, currentPosCursor.y];
                if (collidedObject != null && collidedObject.pos.layer == p.pos.layer)
                {
                    return new GamePosition(currentPosCursor, p.me.pos.layer, p.me.pos.orientation);
                }
                currentPosCursor += normalizedDirection;
            }
            return null; // pas de collision !
        }
        //Gestion du systeme d'"évenement"
        //(les entrées clavier)
        public void addListener(eventListener evL) { }
        public void removeListener() { }
        public void throwEvent(EventRepresenter ev) { }

    }
}
