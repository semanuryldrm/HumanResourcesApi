using HumanResourcesApi.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace HumanResourcesApi.Services;

public class PayrollPdfService
{
    public byte[] GeneratePayrollPdf(Payroll payroll)
    {
        var employeeFullName = payroll.Employee != null
            ? $"{payroll.Employee.FirstName} {payroll.Employee.LastName}"
            : "-";

        var employeeEmail = payroll.Employee?.Email ?? "-";

        return Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);

                page.DefaultTextStyle(text => text.FontSize(10));

                page.Header()
                    .Column(column =>
                    {
                        column.Item().Text("İnsan Kaynakları Yönetim Sistemi")
                            .FontSize(18)
                            .Bold();

                        column.Item().Text("Bordro Raporu")
                            .FontSize(14);

                        column.Item().PaddingTop(8).LineHorizontal(1);
                    });

                page.Content()
                    .PaddingVertical(25)
                    .Column(column =>
                    {
                        column.Spacing(12);

                        column.Item().Text("Çalışan Bilgileri")
                            .FontSize(13)
                            .Bold();

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                            });

                            AddRow(table, "Çalışan", employeeFullName);
                            AddRow(table, "E-posta", employeeEmail);
                            AddRow(table, "Bordro Dönemi", payroll.Period);
                            AddRow(table, "Oluşturulma Tarihi", payroll.CreatedAt.ToString("dd.MM.yyyy HH:mm"));
                        });

                        column.Item().PaddingTop(15).Text("Bordro Detayları")
                            .FontSize(13)
                            .Bold();

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                            });

                            AddRow(table, "Brüt Maaş", $"{payroll.GrossSalary:N2} TL");
                            AddRow(table, "Kesinti Tutarı", $"{payroll.DeductionAmount:N2} TL");
                            AddRow(table, "Net Maaş", $"{payroll.NetSalary:N2} TL");
                        });

                        column.Item().PaddingTop(20).Text(
                            "Not: Bu rapor, Web Tabanlı Programlama dersi final projesi kapsamında " +
                            "geliştirilen örnek insan kaynakları yönetim sistemi tarafından oluşturulmuştur."
                        );
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(text =>
                    {
                        text.Span("Sayfa ");
                        text.CurrentPageNumber();
                        text.Span(" / ");
                        text.TotalPages();
                    });
            });
        }).GeneratePdf();
    }

    private static void AddRow(TableDescriptor table, string title, string value)
    {
        table.Cell()
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten2)
            .PaddingVertical(6)
            .Text(title)
            .Bold();

        table.Cell()
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten2)
            .PaddingVertical(6)
            .Text(value);
    }
}
