using System.Runtime.CompilerServices;

namespace Service.Interface;

public interface ICommonService
{
    Task RegisterException(Exception ex, [CallerMemberName] string methodName = "",
    [CallerFilePath] string fileName = "",
    [CallerLineNumber] int lineNumber = 0);
}