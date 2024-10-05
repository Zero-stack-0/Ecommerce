using System.Runtime.CompilerServices;
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

    public async Task RegisterException(Exception ex, [CallerMemberName] string methodName = "",
    [CallerFilePath] string fileName = "",
    [CallerLineNumber] int lineNumber = 0)
    {
        var exception = new LoggingException(ex.Message, methodName, fileName, lineNumber, ex.InnerException.ToString());
        context.LoggingException.Add(exception);
        await context.SaveChangesAsync();
    }
}