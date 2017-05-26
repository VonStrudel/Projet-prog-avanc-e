using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_Jeu
{

    /*Comportement graphique de l'objet (affichage)*/
    /*Retourne l'image à afficher avec la position de son point topleft et son layer*/
    class displayData
    {
        public char[,,,] img { get; private set; }
        public int orientation;
        public Vect2D position { get; private set; }
        public displayData(char[,,,]img, Vect2D position, int orientation)
        {
            this.position = position;
            this.orientation = orientation;
            this.img = img;
        }
    }
    class BaseDisplayer
    {
        char[,,,] img; //[x,y,data, frame] :    [x,y,0,#] : caractere
                       //                [x,y,1,#] : couleur lettre
                       //                [x,y,2,#] : couleur background
                       //                [#,#,#,enum direction] : frame correspondant à l'orientation
        public WorldObject me;
        public virtual displayData display()
        {
            return new displayData(img, me.pos.pos, (int)me.pos.orientation);
        }
        public BaseDisplayer(char c, char col, char back) //tests
        {
            this.img = new char[1,1,3,4];
            for(int i=0;i<4;i++)
            {
                this.img[0, 0, 0, i] = 'x';
                this.img[0, 0, 1, i] = col;
                this.img[0, 0, 2, i] = back;
            }
        }
    }
}
