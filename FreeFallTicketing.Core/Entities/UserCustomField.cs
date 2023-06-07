using SkyDiveTicketing.Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyDiveTicketing.Core.Entities
{
    public class UserCustomField : BaseEntity
    {
        public UserCustomField()
        {

        }

        public UserCustomField(string key, string value) : base()
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}
