using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using APIServer.Models.GameDB;

namespace APIServer.DTO;

public class UserGameDataRequest
{
    public long UserId { get; set; }
}

public class UserGameDataResponse
{
    public long UserId { get; set; }
    public string Email { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
    public int Win { get; set; }
    public int Lose { get; set; }
    public int Draw { get; set; }
}