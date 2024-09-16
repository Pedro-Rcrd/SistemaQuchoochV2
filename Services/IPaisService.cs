using sistemaQuchooch.Data.QuchoochModels;

public interface IPaisService
{
    Task<IEnumerable<Pai>> ObtenerPaisPaginadosAsync(int pagina, int elementosPorPagina);
    Task<int> ObtenerCantidadTotalRegistrosAsync();

    Task<Pai?> GetById(int id); //Rol? = Indica que devuelve un objeto rol o un null

    Task<Pai> Create (Pai newPais);

    Task Update (int id, Pai pais);
    
    Task Delete (int id);
}
