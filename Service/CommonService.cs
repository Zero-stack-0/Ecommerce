using Data;
using Entities.Models;
using Service.Interface;

namespace Service;

public class CommonService : ICommonService
{
    private readonly LoggingDbContext context;
    public CommonService(LoggingDbContext context)
    {
        this.context = context;
    }

    public async Task RegisterException(Exception ex)
    {
        var exception = new LoggingException(ex.Message);
        context.LoggingException.Add(exception);
        await context.SaveChangesAsync();
    }
}