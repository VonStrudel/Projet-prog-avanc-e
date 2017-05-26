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
    public delegate void updater();
    /*Base de tous les objets*/
    class WorldObject
    {
        public bool hasMoved;
        public bool needUpdate;
        public static int count { get; protected set; } // Nombre total d'objets (pour avoir leur id )
        public int id { get; protected set; } //Id de l'objet
        public ObjectType type { get; protected set; } // Ce qu'est l'objet (une porte, un monstre, le joueur?..)
        public GamePosition pos { get; protected set; } // La position instantannée de l'objet dans le niveau
        public GamePosition startPos { get; protected set; } //Position de l'objet à sa création
        private World _world;
        public World world {
            get { return this._world; }
            set { this._world = value;
                this._world.AddObject(this);
            }
        } //Monde dans le quel cet objet est contenu

        public updater onUpdate; //Delegate pour tous les composants ayant besoin de s'update

        /*Constructeurs*/
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

        public virtual void onCollide(WorldObject obj, Vect2D direction) 
        {

        }
        public virtual void onReceive(Attack atk, WorldObject obj)
        {

        }
        public virtual void onInteract(Interaction i, WorldObject obj)
        {

        }

        /*Composition de l'objet*/
        private BaseGameplay _gameplay;
        public BaseGameplay gameplay {
            get { return this._gameplay; }
            set {
                this._gameplay = value;
                this._gameplay.moveObject = move;
                this._gameplay.teleportObject = teleport;
            }

        }   //Le gameplay (tout ce qui gere vie, inventaire, status, comment l'entité se déplace, comment elle réagit, etc...) mais aussi ce qui décide de ce que va faire
             //l'objet si un controller est implémenté
        public void move(Vect2D movement)
        {
            if(this.physics != null)
            {
                movement = physics.checkMove(movement);
            }
            this.mv(movement);
        }
        public void teleport(Vect2D movement)
        {
            if(this.physics != null)
            {
                movement = physics.checkTeleportation(movement);
            }
            this.mv(movement);
        }
        protected void mv(Vect2D movement)
        {
            this.pos.pos += movement; //On déplace tout
            this.physics.pos.pos += movement;
            this.displayer.pos.pos += movement;
            this.gameplay.pos.pos += movement;
        }

        public BasePhysics physics;     //(facultatif)La physique (le comportement physique de l'objet en relation avec les autres objets)
                                        //quand implémenté, l'objet physique va vérifier que chaque déplacement effectué par l'objet est possible selon ses regles
        public BaseDisplayer displayer;//(facultatif) Le Displayer gere l'affichage de l'objet
                                       // il renvoie un objet contenant le tile (l'"image" à afficher) et la position du tile, permettant à l'afficheur du World de savoir quoi
                                       // afficher où 
        //Obsolete
        public bool isComplete() //Verifie si l'objet possede bien un controller, un displayer, un gameplay et un physics 
        {
            if (/*this.controller != null &&*/ this.displayer != null && this.gameplay != null && this.physics != null)
                return true;
            return false;
        }
        public bool makeConnections()
        {
            if (this.isComplete())
            {
                //On ajoute les update au delegate
                if (this.physics != null)
                {
                    this.onUpdate += this.physics.update;
                    //On superpose l'objet physique à l'objet réel
                    this.physics.pos = this.pos;
                }


                //this.controller.gMove += this.gameplay.move;
                //this.controller.gTurn += this.gameplay.
                this.physics.checkForCollision += this.world.collisionCheck;
                return true;
            }
            return false;
        }
        public void update()
        {
            this.onUpdate();
            if (physics.pos != this.pos)
            {
                this.pos = physics.pos;
            }
        }
    }
}
