namespace WindowsSystem_Backend.BL.DTO
{
    public class GetLibreryDto
    {
        public string? Name { get; set; }
        public string? Keywords { get; set; }
        public List<MediaDto> Media { get; set; } = null!;
    }
}
