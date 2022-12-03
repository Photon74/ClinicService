using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicService.Data
{
    [Table("Consultation")]
    public class Consultation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column]
        public DateTime ConsultationDate { get; set; }

        [Column]
        public string Description { get; set; }

        public Client Client { get; set; }
        public Pet Pet { get; set; }
    }
}
