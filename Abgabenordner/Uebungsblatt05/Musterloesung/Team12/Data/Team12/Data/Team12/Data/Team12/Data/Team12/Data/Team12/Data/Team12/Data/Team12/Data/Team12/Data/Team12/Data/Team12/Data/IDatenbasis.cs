using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team12.Data
{
    public interface IDatenbasis
    {
        public List<Skill> LoadJson();
    }
}
