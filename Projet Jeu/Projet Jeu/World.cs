using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_Jeu
{
    /*Class représentant le monde (le niveau)*/
    // GamePosition collisionChecker(BasePhysics myself, Vect2D direction);
    delegate void keyListener(ConsoleKey ckey);
    class World 
    {
        public WorldObject[,] level { get; set; }
        List<displayData> affichage;
        public keyListener keyListeners;
        public void display() {
            for (int i = 0; i < level.GetLength(0); i++)
            {
                for (int j = 0; j < level.GetLength(1); j++)
                {
                    if(level[i,j] != null && level[i, j].hasChanged)
                    {
                        affichage.Add(level[i, j].displayer.display());
                        level[i, j].hasChanged = false;

                    }
                }
             }
            
            affichage.ForEach(delegate (displayData dat) {
                for (int i=0;i<dat.img.GetLength(0);i++) //i et j parcourent l'image
                {
                    for(int j=0;j<dat.img.GetLength(1);j++)
                    {
                        int x = dat.position.x + i; 
                        int y = dat.position.y + j;
                        Console.SetCursorPosition(x, y);
                        setColor(dat.img[i, j, 1, dat.orientation], dat.img[i, j, 2, dat.orientation]);
                        Console.Write(dat.img[i, j,0,dat.orientation]);
                        Console.Clear();
                    }
                }
                
            }); 
            affichage = new List<displayData>();
        }
        private static void setColor(int backColor, int letterColor)
        {
            ConsoleColor[] tab = { ConsoleColor.DarkRed, ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.DarkMagenta, ConsoleColor.Magenta, ConsoleColor.Gray, ConsoleColor.DarkYellow, ConsoleColor.DarkGreen, ConsoleColor.DarkCyan, ConsoleColor.Black };
            if (backColor > 0)
                Console.BackgroundColor = tab[backColor - 1];
            if (letterColor > 0)
                Console.ForegroundColor = tab[letterColor - 1];
        }
        public void update()
        {
            if (Console.KeyAvailable)
            {
                keyListeners(Console.ReadKey(true).Key);
            }
            for (int i = 0; i < level.GetLength(0); i++)
            {
                for (int j = 0; j < level.GetLength(1); j++)
                {
                    if (level[i, j] != null) { level[i, j].update(); }
                }
            }
        }
        public World()
        {
            this.level = new WorldObject[20, 20];
            this.affichage = new List<displayData>();
        }
        /// <summary>
        /// Check de collision basique
        /// </summary>
        /// <param name="p">L'objet physique qui veut vérifier sa collision</param>
        /// <param name="direction">Le vecteur de direction de cet objet physique (vers ou il va)</param>
        /// <returns></returns>
        public virtual GamePosition collisionCheck(BasePhysics p, Vect2D direction )
        {
            
            Vect2D currentPosCursor = p.pos.pos.Copy();
            Vect2D normalizedDirection = direction.normalize();
            for (int i = 0; i < Vect2D.getDistance(direction, new Vect2D(0, 0)); i++)
            {
                
                currentPosCursor += normalizedDirection; 
                //On regarde si on est bien dans les boundaries du niveau
                if (currentPosCursor.x < 0)
                    return new GamePosition(currentPosCursor, p.pos.layer, p.pos.orientation);
                else if (currentPosCursor.x > this.level.GetLength(0))
                    return new GamePosition(currentPosCursor, p.pos.layer, p.pos.orientation);
                if (currentPosCursor.y < 0)
                {
                    return new GamePosition(currentPosCursor, p.pos.layer, p.pos.orientation);
                }
                else if (currentPosCursor.y > this.level.GetLength(1))
                    return new GamePosition(currentPosCursor, p.pos.layer, p.pos.orientation);


                //Si oui on regarde si là ou on veut aller, il y a déjà un objet (détection de collision très basique)
                WorldObject collidedObject = level[currentPosCursor.x, currentPosCursor.y];
                
                if (collidedObject != null && collidedObject.pos.layer == p.pos.layer)
                {
                    return new GamePosition(currentPosCursor, p.pos.layer, p.pos.orientation);
                }
                


               
            }
            return null; // pas de collision !
        }
        public void move(WorldObject objToMove, GamePosition newPos)
        { 
            for (int i = 0; i < level.GetLength(0); i++)
            {
                for (int j = 0; j < level.GetLength(1); j++)
                {
                    if(level[i, j] == objToMove)
                    {
                        
                        level[i, j] = null;Console.WriteLine(newPos.pos.y);
                        level[newPos.pos.x, newPos.pos.y] = objToMove;
                        this.affichage.Add(objToMove.displayer.display());
                    }
                }
            }
        }
        public void addListener(keyListener evL) { this.keyListeners += evL; }
        public void removeKeyListener(keyListener toRemove) { this.keyListeners -= toRemove; }


    }
    /// <summary>
    /// Nouvelle class World en construction. L'autre sera bientot obsolete
    /// Plus propre, plus logique, mais pas encore parfaite
    /// </summary>
    class World_
    {
        private int worldLength;
        private int worldHeight;
        private List<WorldObject> level;
        private WorldObject getObjectAt(int x, int y)
        {
            for (int i = 0; i < level.Count; i++)
                if (level[i].pos.pos.x == x && level[i].pos.pos.y == y)
                    return level[i];
            return null;
        }
        public keyListener keyListeners; //fonction d'objets qui attendent des entrées claviers
        public Action updateListeners; //fonctions d'objets qui ont besoin d'etre updaté (les objets qui bougent tous seuls comme les monstres par exemple)
        public void AddObject(WorldObject obj)
        {
            this.level.Add(obj);
            if(obj.needUpdate)
                this.updateListeners += obj.update;
        }
        public void RemoveObject(WorldObject obj) // Doit avoir la référence exacte de l'objet à supprimer
        {
            level.Remove(obj);
            if (obj.needUpdate)
                this.updateListeners -= obj.update;
        }

        /// <summary>
        /// Cette fonction s'occupe juste de voir si à une position, il y a une raison potentielle d'etre bloqué (un objet ou les limites du terrain)
        /// si c'est un objet, il retourne sa composante physique (et si y'a pas de composante physique il retourne null ( un objet sans compôsante physique est comme un fantome))
        /// si c'est une limite du terrain, il crée un objet temporaire qui se comporte comme un mur
        /// </summary>
        /// <param name="p">L'objet qui veut vérifier si y'a pas de collision</param>
        /// <param name="destination">L'endroit ou il veut aller</param>
        /// <returns></returns>
        public virtual BasePhysics collisionCheck(BasePhysics p, Vect2D destination)
        {
            Vect2D currentPosCursor = p.pos.pos.Copy();
            Vect2D normalizedDirection = destination.normalize();
            for (int i = 0; i < Vect2D.getDistance(destination, new Vect2D(0, 0)); i++)
            {

                currentPosCursor += normalizedDirection;
                //On regarde si on est bien dans les boundaries du niveau
                if (currentPosCursor.x < 0)
                    return new BasePhysics(-1, currentPosCursor.y, p.pos.layer);
                else if (currentPosCursor.x > this.worldLength)
                    return new BasePhysics(this.worldLength, currentPosCursor.y, p.pos.layer);
                if (currentPosCursor.y < 0)
                {
                    return new BasePhysics(currentPosCursor.x,-1, p.pos.layer);
                }
                else if (currentPosCursor.y > this.worldHeight)
                    return new BasePhysics(currentPosCursor.x, this.worldHeight, p.pos.layer);


                //Si oui on regarde si là ou on veut aller, il y a déjà un objet (détection de collision très basique)
                WorldObject collidedObject = getObjectAt(currentPosCursor.x, currentPosCursor.y);

                if (collidedObject != null && collidedObject.pos.layer == p.pos.layer)
                {
                    return collidedObject.physics;
                }
            }
            return null; // pas de collision !
        }
        /// <summary>
        /// Update tous les objets updatables du mooonde
        /// </summary>
        public virtual void update()
        {
            if(Console.KeyAvailable)//On donne les entrées clavier à ceux qui les demandent
                this.keyListeners(Console.ReadKey().Key);
            this.updateListeners();

        }

        /// <summary>
        /// Display tous les objets displayable du mooonde (enfin ceux qui ont changé d'apparence pour pas clignoter trop)
        /// </summary>
        public virtual void display()
        {

        }
        /// <summary>
        /// Ajout de fonctions qui "écoutent" les entrées clavier
        /// </summary>
        /// <param name="evL"></param>
        public void addKeyListener(keyListener evL) { this.keyListeners += evL; }
        public void removeKeyListener(keyListener toRemove) { this.keyListeners -= toRemove; }

        
    }
}
