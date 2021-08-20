using SatisfactorySaveParser;

namespace FactoryBuilder.Util
{
    public class CloneHelper
    {
        public static void CloneEntity(SaveEntity entity)
        {
            var clone = (SaveEntity)entity.Clone();
        }
    }
}