using System.ComponentModel.DataAnnotations;

namespace user_ms.Src.Models
{
    public class Subject : BaseModel
    {
        public string Code { get; set; } = null!;

        [StringLength(250)]
        public string Name { get; set; } = null!;

        [StringLength(250)]
        public string Department { get; set; } = null!;

        public int Credits { get; set; }

        public int Semester { get; set; }    
    }
}