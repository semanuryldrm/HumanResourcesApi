namespace HumanResourcesApi.Services;

public class PayrollService
{
    private const decimal DeductionRate = 0.15m;

    public decimal CalculateDeductionAmount(decimal grossSalary)
    {
        return grossSalary * DeductionRate;
    }

    public decimal CalculateNetSalary(decimal grossSalary)
    {
        var deductionAmount = CalculateDeductionAmount(grossSalary);
        return grossSalary - deductionAmount;
    }
}