using Chess.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Chess.Hubs
{
    public class ChessHub : Hub
    {
        public async Task SendMessage(Board board)
        {
            await Clients.All.SendAsync("Chess", board);
        }
    }
}
