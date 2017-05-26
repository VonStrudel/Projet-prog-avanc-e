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
        public bool hasChanged;
        public static int count { get; protected set; } // Nombre total d'objets (pour avoir leur id )
        public int id { get; protected set; } //Id de l'objet
        public ObjectType type { get; protected set; } // Ce qu'est l'objet (une porte, un monstre, le joueur?..)
        public GamePosition pos { get; protected set; } // La position instantannée de l'objet dans le niveau
        public GamePosition startPos { get; protected set; } //Position de l'objet à sa création
        private World _world;
        public World world {
            get { return this._world; }
            set { this._world = value;
                this._world.level[pos.pos.x, pos.pos.y] = this;
            }
        } //Monde dans le quel cet objet est contenu
        public BaseDisplayer displayer;//Le Displayer (l'affichage)
        public WorldObject(ObjectType type, GamePosition startPos, World world)
        {
            //this.hasChanged = true;
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
        public virtual void update()
        { 
        }
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
    class WorldThing :WorldObject
    {
        /*Composition de l'objet*/
        public BaseController controller;// Le controleur (le cerveau)
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
        public bool makeConnections()
        {
            if (this.isComplete())
            {
                this.controller.gMove += this.gameplay.move;
                this.controller.world = this.world;
                //this.controller.gTurn += this.gameplay.
                this.gameplay.pMove += this.physics.move;
                this.physics.checkForCollision += this.world.collisionCheck;
                this.physics.pos = this.pos;
                this.displayer.me = this;
                return true;
            }
            return false;
        }
        public override void update()
        {
            physics.update();
            if (physics.pos != this.pos)
            {
                world.move(this, physics.pos);
                this.pos = physics.pos;
            }
            controller.update();
            //gameplay.update();
        }

    }
}
