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
       
        public virtual void onCollide(WorldObject obj)
        {
            if (this.gameplay != null)
                this.gameplay.onCollide(this.gameplay);
        }
        public virtual void onReceive(Attack atk, WorldObject obj)
        {
            if (this.gameplay != null)
                this.gameplay.receive(atk, obj.gameplay);
        }
        public virtual void onInteract(Interaction i, WorldObject obj)
        {
            if (this.gameplay != null)
                this.gameplay.interact(i, obj.gameplay);
        }

        public BasePhysics physics;     //(facultatif)La physique (le comportement physique de l'objet en relation avec les autres objets)
                                        //quand implémenté, l'objet physique va vérifier que chaque déplacement effectué par l'objet est possible selon ses regles
        /// <summary>
        /// Move est un déplacement en ligne droite : 
        /// Si un objet est sur le chemin, il est heurté (onCollide) et bloque le déplacement (enfin ca dépend du comportement de sa composante physique)
        /// </summary>
        /// <param name="movement"></param>
        public void move(Vect2D movement) //Cette fonction fait trop de trucs : faudrait rapatrier tout ce qui touche aux rebonds dans le physics, le laisser vivre sa vie, et gérer la dimension "gameplay" des collisions uniquement à chaque updates
        {
            //On regarde si on va collider avec quelque chose
            if (this.physics != null) //Si y'a une composante physique on peut checker les collisions
            {
                WorldObject collided = world.getCollidedObject(this.physics, movement);
                while (collided != null) //On utilise une boucle au cas ou il y aurait des rebonds menant à d'autres collisions
                {
                    this.onCollide(collided); //Je fais mon action vers l'objet heurté
                    collided.onCollide(this); //L'objet heurté fait son action (les collision vont dans les deux sens, principe d'action réaction)

                    movement += physics.resolveCollision(collided.physics, movement); //On laisse l'objet physics gérer l'influence de la collision sur les déplacements
                    collided = world.getCollidedObject(this.physics, movement); 
                }
            }
            this.mv(movement);
        }
        /// <summary>
        /// Teleport est  un déplacement instantanné, il ne fait pas attention aux objets sur la route. Si il y a un objet à la position ou la téléportation est voulue,
        /// la collision est gérée par les objets physics
        /// </summary>
        /// <param name="movement"></param>
        public void teleport(Vect2D movement)
        {
            if(this.physics != null)
            {
                movement = physics.checkTeleportation(movement);
            }
            this.mv(movement);
            if(this.physics != null)
            {
                WorldObject collided = world.getObjectAt(this.physics.pos.pos.x, this.physics.pos.pos.y); //On regarde si on s'est tp sur quelque chose
                if (collided != null)
                {

                    this.onCollide(collided); //Je fais mon action vers l'objet heurté
                    collided.onCollide(this); //L'objet heurté fait son action (les collision vont dans les deux sens, principe d'action réaction)
                }
            }
        }

        //S'occupe de bouger l'objet
        protected void mv(Vect2D movement)
        {
            this.pos.pos += movement; //On déplace tout
            this.physics.pos.pos += movement;
            this.displayer.pos.pos += movement;
            this.gameplay.pos.pos += movement;
        }


        public BaseDisplayer displayer;//(facultatif) Le Displayer gere l'affichage de l'objet
                                       // il renvoie un objet contenant le tile (l'"image" à afficher) et la position du tile, permettant à l'afficheur du World de savoir quoi
                                       // afficher où 
        
        public void update()
        {
            this.onUpdate();
            //TODO :    on checke les collisions 
            //          on utilise gameplay.oncollide
            //          (du coup on risquerait pas de rebondir et se blesser plusieurs fois entre deux updates)
            //          (et ca rendrait tout un peu moins sale)
            //
            //          implémenter le concept de "vitesse" 
        }
    }
}
