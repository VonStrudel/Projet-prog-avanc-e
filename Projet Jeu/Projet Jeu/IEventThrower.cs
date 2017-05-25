using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_Jeu
{
    /*Chaque objet (le monde et les composants du worldobject) appellent des events 
     *dans beaucoup de situations. Un objet peut s'inscrire à l'event d'un autre objet pour etre prévenu : 
     * exemple : une porte pourrait s'inscrire à tous les events "die" des objets du niveau : elle pourrait compter les morts et ne s'ouvrir
     * qu'a partir d'un certain nombre
     */
    delegate void eventListener(EventRepresenter ev);
    class EventRepresenter //Represente un evenement
    {
        int objectId;
        ObjectType objectType;
        string eventType;
        string eventContent;
    } 
    interface IEventThrower
    {
        void addListener(eventListener evL);
        void removeListener(); // à préciser
        void throwEvent(EventRepresenter ev);
    }
}
