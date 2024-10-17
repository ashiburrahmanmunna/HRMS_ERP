namespace GTERP.Models
{
    public class UserState
    {
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserId { get; set; }
        public string ComId { get; set; }
        public string LastVisited { get; set; }
    }
}
