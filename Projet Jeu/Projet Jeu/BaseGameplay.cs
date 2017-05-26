using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_Jeu
{
    delegate void positionChanger(Vect2D moveVector);
    class BaseGameplay
    {
        public GamePosition pos;
        /*Appartient au worldObject */
        public positionChanger moveObject; //delegate permettant au gameplay de bouger son objet (bloc par bloc). Rien n'empeche 
        public positionChanger teleportObject; //delegate permettant une téléportation. C'est une autre fonction car elle gere différement la verification des collision si physique il y a
                                                // une teleportation n'est pas 
        private BaseController _controller;
        public BaseController controller
        {
            get
            {
                return this._controller;
            }
            set //Quand on ajoute un controller, on lui "donne les commandes" 
            {
                this._controller = value;           //Le controller peut :
                this._controller.gMove = this.move; //faire bouger l'objet
                this._controller.gTeleport = this.teleport; //faire se téléporter l'objet
                this._controller.gTurn = this.turn; //faire tourner l'objet
                //this._controller.gGetInv = this.; //récuperer l'inventaire (NON IMPLEMENTÉ)
                //this._controller.gEquipItem = this.; //equiper un item (NON IMPLEMENTÉ)
                //this._controller.gUseItem = this.;//utiliser un item équipé (NON IMPLEMENTÉ)
            }
        }


        public virtual Vect2D myTeleport(Vect2D position) //Fonction overrideable pour modifier le déroulement d'une téléportation
        {
            return position;
        }
        public virtual direction myTurn(direction dir)//Fonction overrideable pour modifier le déroulement d'une rotation
        {
            return dir;
        }
        public virtual void move(direction dir) // Move est directement overrideable pour permettre plus de flexibilité (appeller plusieurs fois moveObject à la suite par exemple)
        {
            Vect2D directionVector;
            switch (dir)
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
            this.moveObject(directionVector);
        }

        public void teleport(Vect2D position)
        {
            this.teleportObject(myTeleport(position));
        } 
        public void turn(direction dir) { // à implémenter
        }
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
