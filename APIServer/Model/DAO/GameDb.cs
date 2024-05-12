namespace APIServer.Models.GameDB
{
    public class UserGameData
    {
        public long UserId { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public int Win { get; set; }
        public int Lose { get; set; }
        public int Draw { get; set; }
    }
}