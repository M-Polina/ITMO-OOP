using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccess(
        this IServiceCollection collection,
        Action<DbContextOptionsBuilder> configuration)
    {
        collection.AddDbContext<DatabaseContext>(configuration);
        return collection;
    }
}