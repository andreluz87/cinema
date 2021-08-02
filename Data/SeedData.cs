﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Cinema.Data
{
    public static class SeedData
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider, IConfiguration Configuration)
        {
            //incluir perfis customizados
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            //define os perfis em um array de strings
            string[] roleNames = { "Manager", "Collaborator"};
            IdentityResult roleResult;

            //percorre o array de strings 
            //verifica se o perfil já existe
            foreach (var roleName in roleNames)
            {
                // cria perfis e os inclui no banco de dados
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            foreach(var roleName in roleNames)
            {
                // cria um super usuário que pode manter a aplicação web
                var poweruser = new IdentityUser
                { 
                    //obtem o nome e o email do arquivo de configuração
                    UserName = Configuration.GetSection("UserSettings").GetSection(roleName)["UserName"],
                    Email = Configuration.GetSection("UserSettings").GetSection(roleName)["UserEmail"]
                };

                //obtem a senha do arquivo de configuração
                string userPassword = Configuration.GetSection("UserSettings").GetSection(roleName)["UserPassword"];

                //verifica se existe um usuário com o email informado
                var user = await UserManager.FindByEmailAsync(Configuration.GetSection("UserSettings").GetSection(roleName)["UserEmail"]);

                if (user == null)
                {
                    //cria o super usuário com os dados informados
                    var createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);
                    if (createPowerUser.Succeeded)
                    {
                        // atribui o usuário ao perfil Admin
                        await UserManager.AddToRoleAsync(poweruser, roleName);
                    }
                }
            }
        }
    }
}
