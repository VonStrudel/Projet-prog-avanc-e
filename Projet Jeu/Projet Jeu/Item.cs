using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Projet_Jeu
{
    //delegate void itemAction(GamePosition itemPos, GamePosition pos);
    interface IReceiveAttack
    {
        Attack receiveAttack(Attack atk); //Par exemple une armure
    }

    class Attack
    {
        int strength;
        string type;
    }

    abstract class Item
    {
        private int variable;
        private int range;
        //private itemAction iAction;
        bool use(GamePosition itemPos, GamePosition strikePos)
        {
            if (Vect2D.getDistance(itemPos.pos, strikePos.pos) < range)
            {
                action(itemPos, strikePos);
                return true;
            }
            return false;
        }
        protected abstract void action(GamePosition itemPos, GamePosition strikePos);
    }

}
