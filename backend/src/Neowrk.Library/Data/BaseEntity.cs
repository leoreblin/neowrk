using System;
using System.Collections.Generic;
using System.Text;

namespace Neowrk.Library.Data
{
    public abstract class BaseEntity<TKeyType>
    {
        public TKeyType Id { get; set; }
    }

    public abstract class BaseEntity : BaseEntity<Guid>
    {

    }

    
}
