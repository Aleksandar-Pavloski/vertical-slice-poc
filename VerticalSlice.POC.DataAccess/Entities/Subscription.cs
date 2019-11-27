using System.Collections.Generic;

namespace VerticalSlice.POC.DataAccess.Entities
{
    public class Subscription : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
