using API.Dtos.User;
using API.Models;

namespace API.Hash
{
    public static class CheckHashed
    {
        public static bool checkBcrypt(string string1, string string2)
        {
            return BCrypt.Net.BCrypt.Verify(string1, string2);
        }
    }
}
