using InitialProject.Repository.Implementations;
using InitialProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Resources.Injector
{
    public class Injector
    {

        private static Dictionary<Type, object> _dependencies = new Dictionary<Type, object>
        {
            { typeof(ITourRepository), new TourCSVRepository() },
            { typeof(ICheckpointRepository), new CheckpointCSVRepository() },
            { typeof(ILocationRepository), new LocationCSVRepository() }

        };

        public static T CreateInstance<T>() 
        {
            Type type = typeof(T);

            if (_dependencies.ContainsKey(type)) 
            {
                return (T)_dependencies[type];
            }

            throw new ArgumentException($"No implementation found for type {type}");
        }
    }
}
