using System;
using System.Collections.Generic;
using SatisfactorySaveParser;

namespace FactoryBuilder.Util
{
    public static class IdControl {
        public static int GetNextAvailableId()
        {
            if (availableIds.Count == 0)
            {
                throw new InvalidOperationException("No available ID for next entity");
            }
            return availableIds.Dequeue();
        }

        public static void Init(SatisfactorySave save)
        {
            if (hasInit)
            {
                return;
            }
            hasInit = true;
            var occupiedIds = new HashSet<int>();
            foreach (var obj in save.Entries)
            {
                if (obj is SaveEntity)
                {
                    var nameSplit = obj.InstanceName.Split("_");
                    var idStr = nameSplit[^1];
                    if (int.TryParse(idStr, out int id))
                    {
                        occupiedIds.Add(id);
                    }
                }
            }
            availableIds = new Queue<int>();
            for (int i = startFrom; i < int.MaxValue; ++i)
            {
                availableIds.Enqueue(i);
            }
        }

        static bool hasInit = false;
        static readonly int startFrom = 2147410000;
        static Queue<int> availableIds;
    }
}
