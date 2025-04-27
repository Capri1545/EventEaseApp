using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EventEaseApp.Services;

// WARNING: This is an insecure demonstration using local storage.
// Do NOT use this approach for production applications.
// Use ASP.NET Core Identity or a third-party provider instead.
public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IJSRuntime _jsRuntime;
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
    private const string UserSessionStorageKey = "userSession"; // Key to store user info in local storage

    public CustomAuthStateProvider(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // Use a helper function to get item from local storage
            var userSessionJson = await GetItemAsync<string>(UserSessionStorageKey);

            if (string.IsNullOrWhiteSpace(userSessionJson))
            {
                return new AuthenticationState(_anonymous);
            }

            // In a real scenario, you'd deserialize a proper user session object.
            // Here, we just assume the stored string is the username.
            var claims = new[] { new Claim(ClaimTypes.Name, userSessionJson) };
            var identity = new ClaimsIdentity(claims, "CustomAuth");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }
        catch
        {
            return new AuthenticationState(_anonymous); // Handle potential JS interop errors
        }
    }

    public async Task MarkUserAsAuthenticated(string username)
    {
        // Store the username (or a serialized user session object in a real app)
        await SetItemAsync(UserSessionStorageKey, username);

        var claims = new[] { new Claim(ClaimTypes.Name, username) };
        var identity = new ClaimsIdentity(claims, "CustomAuth");
        var user = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task MarkUserAsLoggedOut()
    {
        // Remove the user session data
        await RemoveItemAsync(UserSessionStorageKey);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }

    // Helper methods for JS Interop with local storage
    private async Task SetItemAsync<T>(string key, T value)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, System.Text.Json.JsonSerializer.Serialize(value));
    }

    private async Task<T?> GetItemAsync<T>(string key)
    {
        var json = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);
        if (json == null) return default;
        return System.Text.Json.JsonSerializer.Deserialize<T>(json);
    }

     private async Task RemoveItemAsync(string key)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }
}
