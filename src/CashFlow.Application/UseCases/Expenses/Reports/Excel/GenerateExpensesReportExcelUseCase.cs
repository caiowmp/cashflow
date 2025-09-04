
using ClosedXML.Excel;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel
{
  public class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
  {
    public async Task<byte[]> Execute(DateOnly month)
    {
      var workbook = new XLWorkbook();

      workbook.Author = "Caio Miranda Pereira";
      workbook.Style.Font.FontSize = 12;
      workbook.Style.Font.FontName = "Arial";

      var worksheet = workbook.Worksheets.Add(month.ToString("Y"));
    }
  }
}
