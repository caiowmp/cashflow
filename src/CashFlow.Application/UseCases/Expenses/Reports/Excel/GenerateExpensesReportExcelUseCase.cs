
using CashFlow.Domain.Enums;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.ChartDrawing;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel
{
  public class GenerateExpensesReportExcelUseCase(IExpensesReadOnlyRepository repository) : IGenerateExpensesReportExcelUseCase
  {
    private const string CURRENCY_SYMBOL = "$";

    public async Task<byte[]> Execute(DateOnly month)
    {
      var expenses = await repository.FilterByMonth(month);
      if (expenses.Count == 0)
        return [];

      using var workbook = new XLWorkbook();

      workbook.Author = "Caio Miranda Pereira";
      workbook.Style.Font.FontSize = 12;
      workbook.Style.Font.FontName = "Arial";

      var worksheet = workbook.Worksheets.Add(month.ToString("Y"));

      InsertHeader(worksheet);

      var line = 2;
      foreach (var expense in expenses)
      {
        worksheet.Cell($"A{line}").Value = expense.Title;
        worksheet.Cell($"B{line}").Value = expense.Date;
        worksheet.Cell($"C{line}").Value = ConvertPaymentType(expense.PaymentType);

        worksheet.Cell($"D{line}").Value = expense.Amount;
        worksheet.Cell($"D{line}").Style.NumberFormat.Format = $"-{CURRENCY_SYMBOL} #,##0.00";
        

        worksheet.Cell($"E{line}").Value = expense.Description;

        line++;
      }

      worksheet.Columns().AdjustToContents();

      var file = new MemoryStream();
      workbook.SaveAs(file);

      return file.ToArray();
    }

    private string ConvertPaymentType(PaymentType payment)
    {
      return payment switch
      {
        PaymentType.Cash => ResourceReportGenerationMessages.PAYMENT_TYPE_CASH,
        PaymentType.CreditCard => ResourceReportGenerationMessages.PAYMENT_TYPE_CREDIT_CARD,
        PaymentType.DebitCard => ResourceReportGenerationMessages.PAYMENT_TYPE_DEBIT_CARD,
        PaymentType.EletronicTransfer => ResourceReportGenerationMessages.PAYMENT_TYPE_ELETRONIC_TRANSFER,
        _ => string.Empty,
      };
    }

    private void InsertHeader(IXLWorksheet worksheet)
    {
      worksheet.Cell("A1").Value = ResourceReportGenerationMessages.TITLE;
      worksheet.Cell("B1").Value = ResourceReportGenerationMessages.DATE;
      worksheet.Cell("C1").Value = ResourceReportGenerationMessages.PAYMENT_TYPE;
      worksheet.Cell("D1").Value = ResourceReportGenerationMessages.AMOUNT;
      worksheet.Cell("E1").Value = ResourceReportGenerationMessages.DESCRIPTION;

      worksheet.Cells("A1:E1").Style.Font.Bold = true;

      worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#8A8A8AC");

      worksheet.Cells("A1:E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
      worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
    }
  }
}
