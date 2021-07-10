using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.Authorization;

namespace AVATI.Data
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;

        public CustomAuthStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var state = new AuthenticationState(new ClaimsPrincipal());
            string username = await _localStorageService.GetItemAsStringAsync("Username");
            string empType = await _localStorageService.GetItemAsStringAsync("EmployeeType");
            if (!string.IsNullOrEmpty(username))
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, empType)
                }, "test authentication type");
                
                state = new AuthenticationState(new ClaimsPrincipal(identity));
            }
            
            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }
    }
}