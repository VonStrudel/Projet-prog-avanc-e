﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_Jeu
{
    
    class Program
    {
        static void Main(string[] args)
        {
            WorldObject wo = new WorldObject();
            wo.addListener(dic);
            wo.addListener(dic);
        }
        static void dic(EventRepresenter ev)
        {
            Console.WriteLine("a");
        }
    }



    


    


    /*Comportement graphique de l'objet (affichage)*/
    /*Retourne l'image à afficher avec la position de son point topleft et son layer*/
    abstract class BaseDisplayer
    {

    }
    class GeneralDisplayer : BaseDisplayer
    {

    }

}
