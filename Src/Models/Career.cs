using System.ComponentModel.DataAnnotations;

namespace user_ms.Src.Models
{
    public class Career : BaseModel
    {
        [StringLength(250)]
        public string Name { get; set; } = null!;
    }
}