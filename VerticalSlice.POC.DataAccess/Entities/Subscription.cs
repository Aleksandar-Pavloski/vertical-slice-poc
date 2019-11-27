using System;
using System.Collections.Generic;
using System.Text;

namespace VerticalSlice.POC.DataAccess.Entities
{
    public class Subscription : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
