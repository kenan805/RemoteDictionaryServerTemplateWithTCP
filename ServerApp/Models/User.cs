using System;
using System.Collections.Generic;

#nullable disable

namespace ServerApp.DataAccess
{
    public partial class User
    {
        public string Username { get; set; }
        public int? Likes { get; set; }
    }
}
