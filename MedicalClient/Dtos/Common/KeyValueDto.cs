using GD.Common.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace GD.Dtos.Common
{
    public class KeyValueDto<KeyType, ValueType> : BaseDto
    {
        public KeyType Key { get; set; }

        public ValueType Value { get; set; }
    }
}
