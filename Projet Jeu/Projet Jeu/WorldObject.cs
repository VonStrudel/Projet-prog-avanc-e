using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_Jeu
{
    //Types d'objets possibles
    [Flags]
    enum ObjectType
    {
        unknown = 0,
        door = 1,
        wall = 2,
        npc = 4,
        player = 8,
        monster = 16,
        world = 32
    };
    /*Base de tous les objets*/
    class WorldObject
    {
        public static int count { get; private set; } // Nombre total d'objets (pour avoir leur id )
        public int id { get; private set; } //Id de l'objet
        public ObjectType type { get; private set; } // Ce qu'est l'objet (une porte, un monstre, le joueur?..)
        public GamePosition pos { get; private set; } // La position instantannée de l'objet dans le niveau
        public GamePosition startPos { get; private set; } //Position de l'objet à sa création
        public World world { get; private set; } //Monde dans le quel cet objet est contenu

        public WorldObject(ObjectType type, GamePosition startPos, World world)
        {
            WorldObject.count++;
            this.type = type;
            this.pos = startPos;
            this.startPos = startPos;
            this.world = world;
        }
        public WorldObject(ObjectType type, int x, int y, int layer, direction orientation, World world) : this(type, new GamePosition(x, y, layer, orientation), world)
        { }
        public WorldObject():this(ObjectType.unknown,0,0,0,(direction)0, new World())
        { }

        public virtual void onCollide(WorldObject obj, Vect2D direction) 
        {

        }
        public virtual void onReceive(Attack atk, WorldObject obj)
        {

        }
        public virtual void onInteract(Interaction i, WorldObject obj)
        {

        }
    }
    /*Objet plus pratique à manipuler*/
    class WorldThing :WorldObject, IEventThrower
    {
        /*Composition de l'objet*/
        public BaseController controller;// Le controlleur (le cerveau)
        public BaseDisplayer displayer;//Le Displayer (l'affichage)
        public BaseGameplay gameplay;//Le gameplay (tout ce qui gere vie, inventaire, status, comment l'entité se déplace, etc...)
        public BasePhysics physics; //La physique (le comportement physique de l'objet en relation avec les autres objets)


        /*
            update() : 
            demande à component ce qu'il doit faire
            le passe à gameplay. Demande à gameplay quoi faire (si action physique)
            le passe à physics. Physics fait les modifications
        */

        public WorldThing(ObjectType type, GamePosition startPos, World world):base(type, startPos, world)
        {
        }   
        public WorldThing(ObjectType type, int x, int y, int layer, direction orientation, World world):this(type, new GamePosition(x, y, layer, orientation), world)
        { }
        public WorldThing():this(ObjectType.unknown,0,0,0,(direction)0, new World())
        { }
        public bool isComplete() //Verifie si l'objet possede bien un controller, un displayer, un gameplay et un physics 
        {
            if (this.controller != null && this.displayer != null && this.gameplay != null && this.physics != null)
                return true;
            return false;
        }


        void display() { }

        //Implémentation de IEventThrower
        public event eventListener evListeners; // Liste des fonctions qui ecoutent les events de cet objet
        public void addListener(eventListener evL) { evListeners += evL; }
        public void removeListener() { }
        public void throwEvent(EventRepresenter ev) { }
    }
}
