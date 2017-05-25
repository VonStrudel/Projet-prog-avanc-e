using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_Jeu
{

    /*Comportement graphique de l'objet (affichage)*/
    /*Retourne l'image à afficher avec la position de son point topleft et son layer*/
    abstract class BaseDisplayer
    {
        char[,,,] img; //[x,y,data, frame] :    [x,y,0,#] : caractere
                       //                [x,y,1,#] : couleur lettre
                       //                [x,y,2,#] : couleur background
        WorldObject me;
        void display()
        {

        }
    }
    class GeneralDisplayer : BaseDisplayer
    {

    }
}
