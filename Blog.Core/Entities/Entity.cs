using Blog.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Entities
{
    public abstract class Entity : IEntity
    {
        public int Id { get; set; }
    }
}
