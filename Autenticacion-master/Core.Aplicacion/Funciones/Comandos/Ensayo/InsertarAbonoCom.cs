using MediatR;

public class InsertarAbonoCom : IRequest<bool>
{
    public int IdEnsayo { get; set; }
    public decimal Monto { get; set; }
    public string Usuario { get; set; } 
}