using DataAccess.Entities;
using BusinessLogic.Interfaces;
namespace BusinessLogic.Interfaces;

public interface IHomeService
{
    public Task<List<Url>> GetUrls();
    public Task DeleteUrlAsync(int id);
    
}