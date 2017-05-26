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
        //Gestion du systeme d'"évenement"
        //(les entrées clavier)
        public void addListener(keyListener evL) { this.keyListeners += evL; }
        public void removeListener() { }

    }
}
