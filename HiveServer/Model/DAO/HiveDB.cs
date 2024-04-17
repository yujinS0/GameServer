namespace HiveServer.Model.DAO; 
public class HdbAccount
{
    public long UserId {get; set;}
    public required string Email {get; set;}
    public required string Password {get; set;}
}