﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_Jeu
{

    class mystery { }
    enum direction { up=0, down=1, left=2, right=3}
    enum ActionType { move, turn, openInventory, interact, useItem }

    delegate void GameplayMove(direction dir);//Pour se déplacer dans une direction (zqsd)
    delegate void GameplayTurn(direction dir);//Pour s'orienter dans une direction (haut bas gauche droite)
    delegate Inventory GameplayGetInventory();//Retourne l'inventaire, et permet au controlleur de faire des trucs dedans
    delegate void GameplayEquipItem(Item item);//Permet au controlleur d'équiper un item
    delegate void GameplayInteract();//Interagit avec l'objet en face de nous
    delegate void GameplayUseItem(int slotId);//Utilise le slotId eme item équipé
    /// <summary>
    /// Controlleur du worldObject. Le "cerveau"
    /// </summary>
    abstract class BaseController
    {
        public  GameplayMove gMove;
        protected  GameplayTurn gTurn;
        protected  GameplayGetInventory gGetInv;
        protected  GameplayInteract gInteract;
        protected  GameplayUseItem gUseItem;
        public abstract void onInteract(WorldObject obj);
        public abstract void update();

    }

    /// <summary>
    /// Contient le type de l'action que le controlleur veut effectuer (bouger, interagir, tourner, utiliser un item)
    /// et une variable précisant l'information (direction, numero de l'item)
    /// </summary>
    class ControllerAction
    {
        public ActionType action;
        public int value;//Si action est move ou turn : peut etre casté en enum direction
                  //Si getInventory : inutile
                  //Si interact : inutile
                  //Si useItem : directement utilisé en int
        public ControllerAction(ActionType action, int value)
        {
            this.action = action;
            this.value = value;
        }
        public ControllerAction(ActionType action)
        {
            this.action = action;
            this.value = 0;
        }
    }
    /// <summary>
    /// Controleur de joueur : pas d'ia, récupere les touches clavier, et est relié directement au GUI
    /// </summary>
    class PlayerController : BaseController
    {
        public Dictionary<ConsoleKey, ControllerAction> keyboardAssignment;

        void setDefaultDictionary()
        {
            keyboardAssignment = new Dictionary<ConsoleKey, ControllerAction>();
            keyboardAssignment.Add(ConsoleKey.UpArrow, new ControllerAction(ActionType.turn, (int)direction.up));
            keyboardAssignment.Add(ConsoleKey.DownArrow, new ControllerAction(ActionType.turn, (int)direction.down));
            keyboardAssignment.Add(ConsoleKey.LeftArrow, new ControllerAction(ActionType.turn, (int)direction.left));
            keyboardAssignment.Add(ConsoleKey.RightArrow, new ControllerAction(ActionType.turn, (int)direction.right));
            keyboardAssignment.Add(ConsoleKey.Z, new ControllerAction(ActionType.move, (int)direction.up));
            keyboardAssignment.Add(ConsoleKey.S, new ControllerAction(ActionType.move, (int)direction.down));
            keyboardAssignment.Add(ConsoleKey.Q, new ControllerAction(ActionType.move, (int)direction.left));
            keyboardAssignment.Add(ConsoleKey.D, new ControllerAction(ActionType.move, (int)direction.right));
            keyboardAssignment.Add(ConsoleKey.E, new ControllerAction(ActionType.interact, 0));
            keyboardAssignment.Add(ConsoleKey.I, new ControllerAction(ActionType.openInventory, 0));

        }
        public override void onInteract(WorldObject obj)
        {
            throw new NotImplementedException();
        }
        public override void update()
        {
        }
        void onKeyPress(ConsoleKey cKey)
        {
            ControllerAction whatDo = keyboardAssignment[cKey];
            switch(whatDo.action)
            {
                case ActionType.move:
                    gMove((direction)whatDo.value);
                    break;
                case ActionType.turn:
                    gTurn((direction)whatDo.value);
                    break;
                case ActionType.openInventory:
                    gGetInv();//faudra gérer l'ouverture d'un gui
                    break;
                case ActionType.interact:
                    gInteract();
                    break;
                case ActionType.useItem:
                    gUseItem(whatDo.value);//ca sera gameplay qui s'occupera de regarder la position et l'orientation du joueur
                    break;
            }
        }
    }
}