namespace HiveServer.Model.DAO; 
public class HdbAccount
{
    public long UserNum {get; set;}
    public required string UserId {get; set;}
    public required string Password {get; set;}
}