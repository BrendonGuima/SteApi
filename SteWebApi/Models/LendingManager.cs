namespace SteWebApi.Model;

public class LendingManager
{
    
    public string Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public string CategoryId { get; set; }

    public DateTime DateLend { get; set; }

    public DateTime? DateReturn { get; set; }

    public string StudentName { get; set; }

    public string StudentId { get; set; }


}