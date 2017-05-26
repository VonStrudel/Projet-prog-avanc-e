using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_Jeu
{
    delegate GamePosition PhysicsMove(Vect2D dir);
    delegate GamePosition PhysicsTeleport(GamePosition pos);
    delegate GamePosition PhysicsChangeLayer(int newLayer);
    abstract class BaseGameplay
    {
        public PhysicsMove pMove;
        public PhysicsMove pTurn;
        public PhysicsTeleport pTeleport;
        public PhysicsChangeLayer pChangeLayer;
        public virtual void move(direction dir)
        {
            
            Vect2D directionVector;
            switch(dir)
            {
                case direction.up:
                    directionVector = new Vect2D(0, 1); 
                    break;
                case direction.down:
                    directionVector = new Vect2D(0, -1);
                    break;
                case direction.left:
                    directionVector = new Vect2D(-1, 0);
                    break;
                case direction.right:
                    directionVector = new Vect2D(1, 0);
                    break;
                default:
                    directionVector = new Vect2D(0, 0);
                    break;
            }
           
            pMove(directionVector);
        }
    }
    class PlayerGameplay : BaseGameplay
    {

    }
}
/*
 
    delegate void GameplayMove(direction dir);//Pour se déplacer dans une direction (zqsd)
    delegate void GameplayTurn(direction dir);//Pour s'orienter dans une direction (haut bas gauche droite)
    delegate Inventory GameplayGetInventory();
    delegate void GameplayEquipItem(Item item);
    delegate void GameplayInteract();//Interagit avec l'objet en face de nous
    delegate void GameplayUseItem(int slotId);//Utilise le slotId eme item équipé
     */
