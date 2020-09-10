using System;
using System.Collections.Generic;
using System.Text;

namespace EssentialUIKit
{
    public class GroupInfo
    {
        public string AdminId { get; set; }
        public string Name { get; set; }

        public GroupInfo(string adminId, string name)
        {
            AdminId = adminId;
            Name = name;
        }
    }
}
