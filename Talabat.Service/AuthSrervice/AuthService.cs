﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.identity;
using Talabat.Core.Service.Contract;

namespace Talabat.Service.AuthSrervice
{
	public class AuthService : IAuthService
	{
		private readonly IConfiguration _configuration;

		public AuthService(IConfiguration configuration)
        {
			_configuration = configuration;
		}
        public async Task<string> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager)
		{
			// private Claims (User-Defined)
			var authClaims = new List<Claim>()
			{
				new Claim(ClaimTypes.Name,user.DisplayName),
				new Claim(ClaimTypes.Email,user.Email),
			};

			var userRole = await userManager.GetRolesAsync(user);

			foreach (var role in userRole)
			{
				authClaims.Add(new Claim(ClaimTypes.Role, role));
			}

			var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:AuthKey"] ?? string.Empty));

			var token = new JwtSecurityToken(

				audience: _configuration["JWT:ValidAudence"],
				issuer: _configuration["JWT:ValidIssuer"],
				expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DuraionInDays"] ?? "0")),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authKey,SecurityAlgorithms.HmacSha256Signature)

				);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
