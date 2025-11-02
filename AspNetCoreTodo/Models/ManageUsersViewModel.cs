using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace AspNetCoreTodo.Models
{
    // Este modelo representa la vista para gestionar usuarios (todos y solo los administradores).
    public class ManageUsersViewModel
    {
        public IdentityUser[] Administrators { get; set; }
        public IdentityUser[] Everyone { get; set; }
    }
}

