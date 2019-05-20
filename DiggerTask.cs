using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Digger
{
    public class Terrain : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public string GetImageFileName()
        {
            return "Terrain.png";
        }
    }

    public class Player : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            var result = new CreatureCommand();
            switch (Game.KeyPressed)
            {
                case System.Windows.Forms.Keys.Left:
                    if (x - 1 >= 0 && !(Game.Map[x - 1, y] is Sack))
                        result.DeltaX = -1;
                    break;
                case System.Windows.Forms.Keys.Up:
                    if (y - 1 >= 0 && !(Game.Map[x, y - 1] is Sack))
                        result.DeltaY = -1;
                    break;
                case System.Windows.Forms.Keys.Down:
                    if (y + 1 < Game.MapHeight && !(Game.Map[x, y + 1] is Sack))
                        result.DeltaY = 1;
                    break;
                case System.Windows.Forms.Keys.Right:
                    if (x + 1 < Game.MapWidth && !(Game.Map[x + 1, y] is Sack))
                        result.DeltaX = 1;
                    break;
                default:
                    break;
            }
            return result;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Sack || conflictedObject is Monster;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public string GetImageFileName()
        {
            return "Digger.png";
        }
    }

    public class Sack : ICreature
    {
        public int Way = 0;
        public CreatureCommand Act(int x, int y)
        {
            var act = new CreatureCommand();
            if ((y + 1 < Game.MapHeight) &&
                (Game.Map[x, y + 1] == null ||
                    ((Game.Map[x, y + 1] is Player || Game.Map[x, y + 1] is Monster) 
                        && Way > 0)))
            {
                act.DeltaY += 1;
                Way++;
            }
            else if (Way > 1)
            {
                act.TransformTo = (ICreature)new Gold();
            }
            else
                Way = 0;
            return act;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return (conflictedObject is Sack);
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public string GetImageFileName()
        {
            return "Sack.png";
        }
    }

    public class Gold : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            var result = new CreatureCommand();

            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Player)
            {
                Game.Scores += 10;
                return true;
            }
            else
                return conflictedObject is Monster;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public string GetImageFileName()
        {
            return "Gold.png";
        }
    }

    public class Monster : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            var act = new CreatureCommand();
            var playerLocationX = -1;
            var playerLocationY = -1;
            for (int i = 0; i < Game.MapWidth; i++)
                for (int j = 0; j < Game.MapHeight; j++)
                    if (Game.Map[i, j] is Player)
                    {
                        playerLocationX = i;
                        playerLocationY = j;
                    }
            if (playerLocationX == -1 && playerLocationY == -1)
                return act;
            if (playerLocationX > x && x + 1 < Game.MapWidth && !(Game.Map[x + 1, y] is Sack || Game.Map[x + 1, y] is Terrain || Game.Map[x + 1, y] is Monster))
                act.DeltaX = 1;
            else if (playerLocationX < x && x - 1 >= 0 && !(Game.Map[x - 1, y] is Sack || Game.Map[x - 1, y] is Terrain || Game.Map[x - 1, y] is Monster))
                act.DeltaX = -1;
            else if (playerLocationY < y && y - 1 >= 0 && !(Game.Map[x, y - 1] is Sack || Game.Map[x, y - 1] is Terrain || Game.Map[x, y - 1] is Monster))
                act.DeltaY = -1;
            else if (playerLocationY > y && y + 1 < Game.MapHeight && !(Game.Map[x, y + 1] is Sack || Game.Map[x, y + 1] is Terrain || Game.Map[x, y + 1] is Monster))
                act.DeltaY = 1;
            return act;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Sack || conflictedObject is Monster;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public string GetImageFileName()
        {
            return "Monster.png";
        }
    }
}
