using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public abstract class VehicleRepository
    {
        public abstract IList<Vehicle> Get();

        public async Task<Vehicle> GetAsync(int id)
        {
            Task<Vehicle> task = new Task<Vehicle>(() => Get( id));
            task.Start();
            var result = await task;
            return result;
        }

        public abstract IEnumerable<Vehicle> Get(int page , int size);

        public abstract Vehicle Get(int id);

        public abstract IList<String> SearchModel(String search);

        public abstract int Add (Vehicle vh);

        public abstract bool Update(Vehicle vh);

        public abstract bool Delete (int id);

    }
}
