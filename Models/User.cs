using Chatpoc.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace Chatpoc.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password_Hash { get; set; }
    }
}
