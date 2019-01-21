using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MergeRequestService
{
    public class UserDataInitializer
    {
        private const string AdminRole = "admin";
        private const string NormalRole = "normal";
        private const string DefaultPassword = "Active123!";
        private const string AdminPassword = "ActiveAdmin123!";
        private const string EmailDomain = "@activenetwork.com";

        private static readonly List<string> AdminUsers = new List<string>
        {
            "admin"
        };

        private static readonly List<string> NormalUsers = new List<string>
        {
            "roy.liu",
            "jason.yang",
            "michael.zhang",
            "tracy.cui",
            "michael.song",
            "logan.zhang",
            "chaln.li",
            "eason.yi"
        };

        public static void SeedData(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            InitializeRoles(roleManager);
            InitializeUsers(userManager);
        }

        public static void InitializeRoles(RoleManager<IdentityRole> roleManager)
        {
            CreateRole(roleManager, AdminRole);
            CreateRole(roleManager, NormalRole);
        }

        private static IdentityResult CreateRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            return roleManager.RoleExistsAsync(roleName).Result
                ? null
                : roleManager.CreateAsync(new IdentityRole(roleName)).Result;
        }

        public static void InitializeUsers(UserManager<IdentityUser> userManager)
        {
            AdminUsers.ForEach(userName => { IdentityUser(userManager, userName, AdminPassword, AdminRole); });

            NormalUsers.ForEach(userName => { IdentityUser(userManager, userName, DefaultPassword, NormalRole); });
        }

        private static IdentityResult IdentityUser(UserManager<IdentityUser> userManager, string userName, string password, string role)
        {
            var existUser = userManager.FindByNameAsync(userName).Result;
            if (existUser != null)
            {
                return null;
            }

            var user = new IdentityUser(userName)
            {
                Email = userName + EmailDomain
            };

            var userIdentityResult = userManager.CreateAsync(user, password).Result;

            var userRoleIdentityResult = userManager.AddToRoleAsync(user, role).Result;

            return userIdentityResult;
        }
    }
}