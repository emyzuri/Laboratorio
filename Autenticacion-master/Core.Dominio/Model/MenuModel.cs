public class MenuModel
{
    public int IdMenu { get; set; }
    public int IdPadre { get; set; }
    public string Nombre { get; set; }
    public string URL { get; set; }
    public DateTime FechaActualizacion { get; set; }
    public List<MenuModel> SubMenus { get; set; } = new List<MenuModel>();
    public MenuModel()
    {
        SubMenus = new List<MenuModel>();
    }
}