namespace Service.Interface;

public interface ICommonService
{
    Task RegisterException(Exception ex);
}