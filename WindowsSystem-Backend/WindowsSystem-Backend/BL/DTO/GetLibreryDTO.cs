using WindowsSystem_Backend.BL.BO;

namespace WindowsSystem_Backend.BL.DTO
{
    public class GetLibreryDTO
    {
        public string? Name { get; set; }
        public string? Keywords { get; set; }
        public List<Media> Media { get; set; } = null!;
    }
}
