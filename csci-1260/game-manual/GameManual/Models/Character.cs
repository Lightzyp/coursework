namespace GameManual.Models
{
    public class Character
    {
        public int Id { get; set; }                
        public string Name { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string Description { get; set; } = ""; 
        public string Type { get; set; } = "Pokemon";
        public int? BaseHP { get; set; }
        public int? BaseAttack { get; set; }
        public int? BaseDefense { get; set; }
    }
}


