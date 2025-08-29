namespace CashFlow.Exception.ExceptionsBase
{
  public abstract class CashFlowException : SystemException
  {
    protected CashFlowException() { }
    protected CashFlowException(string message) : base(message) { }
  }
}
