namespace WnT.Web.Models.DTO
{
    public class AddWalkViewModel
    {
        public AddWalkDTO Walk { get; set; }
        public IEnumerable<RegionDTO> AvailableRegions { get; set; }
    }
}
