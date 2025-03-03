using Microsoft.AspNetCore.SignalR;

namespace GreenSpace.Application.SignalR;

public class SignalrHub : Hub
{
    public async Task NewMessage(string user, string message)
    {
        await Clients.All.SendAsync("messageReceived", user, message);
    }
}