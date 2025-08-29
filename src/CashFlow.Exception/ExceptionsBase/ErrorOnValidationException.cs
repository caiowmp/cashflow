using System.Net;

namespace CashFlow.Exception.ExceptionsBase
{
  public class ErrorOnValidationException : CashFlowException
  {
    private readonly List<string> _errorsMessage;

    public ErrorOnValidationException(List<string> errorMessages) : base (string.Empty)
    {
      _errorsMessage = errorMessages;
    }

    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public override List<string> GetErrors()
    {
      return _errorsMessage;
    }
  }
}
