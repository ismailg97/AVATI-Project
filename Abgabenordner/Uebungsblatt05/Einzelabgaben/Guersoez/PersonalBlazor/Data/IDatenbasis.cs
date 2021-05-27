using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlazor.Data
{
    public interface IDatenbasis
    {
        public List<Skill> LoadJson();
    }
}
