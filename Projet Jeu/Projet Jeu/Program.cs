using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_Jeu
{
    
    class Program
    {
        static void Main(string[] args)
        {
           // Console.CursorVisible = false;
            // On fait disparaitre les scrollbars en donnant à la console la meme taille que le buffer
            int WINDOWWIDTH = Console.LargestWindowWidth - 2; //dépend de la taille de l'ecran
            int WINDOWHEIGHT = Console.LargestWindowHeight - 2;
            Console.SetWindowSize(WINDOWWIDTH, WINDOWHEIGHT);
            Console.BufferWidth = WINDOWWIDTH;
            Console.BufferHeight = WINDOWHEIGHT;
            Console.Clear();
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            World w = new World();
            WorldObject wt = new WorldObject(ObjectType.player, new GamePosition(0, 0, 0, direction.right), w);
            
            PlayerController c = new PlayerController();
            w.addListener(c.onKeyPress);
            BaseDisplayer d = new BaseDisplayer('x', (char)0, (char)1);
            PlayerPhysics p = new Projet_Jeu.PlayerPhysics();
            PlayerGameplay g = new Projet_Jeu.PlayerGameplay();
            wt.controller = c;
            wt.displayer = d;
            wt.gameplay = g;
            wt.physics = p;
            Console.WriteLine(wt.makeConnections());

            while (true)
            {
                w.update();
                w.display();
            }
        }
    }



    


    



}
