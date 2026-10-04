using System;

namespace System;

public abstract class Employee
{
    protected string employeeID { get; set; } // Có getter
    protected string fullName;
    protected string department{ get; set;} // Có getter
    protected double monthlyBonus;

    #region Constructor
    public Employee(string employeeID, string fullName, string department)
    {
        if (string.IsNullOrWhiteSpace(employeeID))
            throw new ArgumentException("Mã nhân sự không được để trống.", nameof(employeeID));
        if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Họ tên không được để trống.", nameof(fullName));
        if (string.IsNullOrWhiteSpace(department))
            throw new ArgumentException("Phòng ban không được để trống.", nameof(department));

        this.employeeID = employeeID.Trim();
        this.fullName = fullName.Trim();
        this.department = department.Trim();
        this.monthlyBonus = 0.0;
    }

    public Employee(string employeeID, string fullName)
        : this(employeeID, fullName, "Unassigned")
    {
    }

    #endregion

    #region Overload method
    public void addBonus(double amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount),"Khoản thưởng không được âm");
        monthlyBonus += amount;
    }

    public void addBonus(double amount, string reason)
    {
        if(string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Lý do thưởng không được để trống", nameof(reason));
        addBonus(amount);
        Console.WriteLine($"Đã tăng thưởng '{this.employeeID}' với lý do: " + reason);
    }

    public void addBonus(double rate, double referenceAmount, string reason)
    {
        if (rate <= 0 || rate > 0.5)
            throw new ArgumentOutOfRangeException(nameof(rate), "Tỷ lệ phải nằm trong khoảng lớn hơn 0 và không quá 0.5");
        if (referenceAmount <= 0)
            throw new ArgumentOutOfRangeException("Giá trị tham chiếu phải lớn hơn 0", nameof(referenceAmount));
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Lý do không được rỗng");
        monthlyBonus += rate * referenceAmount;
        Console.WriteLine($"Đã tăng thưởng '{this.employeeID}' với lý do: " + reason);
    }

    #endregion

    #region Public method
    public void resetBonus()
    {
        monthlyBonus = 0.0;
    }

    public string getDepartment()
    {
        return this.department;
    }
    public string getID()
    {
        return this.employeeID;
    }
    public void setDepartment(string department)
    {
        if (string.IsNullOrWhiteSpace(department))
            throw new ArgumentException("Phòng ban không được âm", nameof(department));
        this.department = department;
    }
    #endregion

    #region Abstract and Virtual method
    public abstract double calculateGrossPay();

    public abstract string getEmployeeType();

    public virtual void displayPayrollInfo()
    {
        Console.WriteLine($"Mã: {employeeID} | Họ tên: {fullName} | Phòng ban: {department}");
        Console.WriteLine($"Loại nhân sự: {getEmployeeType()}");
        Console.WriteLine($"Thưởng tháng: {monthlyBonus:N0} VND");
        Console.WriteLine($"Tổng thu nhập: {calculateGrossPay():N0} VND");
    }

    #endregion
}

public class SalariedEmployee: Employee
{
    private double monthlySalary;
    private double responsibilityAllowance;

    #region Constructor
    public SalariedEmployee(string employeeID, string fullName, string department,
                            double monthlySalary, double responsibilityAllowance)
        : base(employeeID, fullName, department)
    {
        if (monthlySalary < 0)
            throw new ArgumentOutOfRangeException(nameof(monthlySalary), "Lương tháng không được âm.");
        if (responsibilityAllowance < 0)
            throw new ArgumentOutOfRangeException(nameof(responsibilityAllowance), "Phụ cấp trách nhiệm không được âm.");

        this.monthlySalary = monthlySalary;
        this.responsibilityAllowance = responsibilityAllowance;
    }

    public SalariedEmployee(string employeeID, string fullName, double monthlySalary)
        :this(employeeID, fullName, "Unassigned", monthlySalary, 0.0)
    {
    }

    #endregion

    #region Override methods
    public override double calculateGrossPay()
    {
        double grossPay;
        grossPay = this.monthlySalary + this.responsibilityAllowance + this.monthlyBonus;
        return grossPay;
    }

    public override string getEmployeeType()
    {
        return "Nhân sự lương cố định";
    }

    public override void displayPayrollInfo()
    {
        Console.WriteLine("--------------------------------------------------");
        base.displayPayrollInfo();
        Console.WriteLine($" - Lương cố định: {this.monthlySalary:N0} VND");
        Console.WriteLine($" - Phụ cấp trách nhiệm: {this.responsibilityAllowance:N0} VND");
        Console.WriteLine("--------------------------------------------------");
    }

    #endregion

}

public class HourlyEmployee: Employee
{
    private double hourlyRate;
    private double workedHours;

    #region Constructor

    public HourlyEmployee(string employeeID, string fullName, string department,
                        double hourlyRate, double workedHours)
        : base(employeeID, fullName, department)
    {
        if (hourlyRate < 0)
            throw new ArgumentOutOfRangeException("Đơn giá giờ không được âm", nameof(hourlyRate));
        if (workedHours < 0 || workedHours > 250)
            throw new ArgumentOutOfRangeException(nameof(workedHours), "Số giờ làm việc phải từ 0 đến 250");

        this.hourlyRate = hourlyRate;
        this.workedHours = workedHours;
    }

    public HourlyEmployee(string employeeID, string fullName, double hourlyRate)
        : this(employeeID, fullName, "Unassigned", hourlyRate, 0.0)
    {
    }

    #endregion

    #region Public method

    public void setWorkedHours(double hours)
    {
        if (hours < 0 || hours > 250)
            throw new ArgumentOutOfRangeException(nameof(hours), "Số giờ làm việc phải từ 0 đến 250");
        this.workedHours = hours;
    }
    #endregion

    #region  Override method

    public override double calculateGrossPay()
        {
            double basePay;
            if (this.workedHours <= 160)
            {
                basePay = this.workedHours * this.hourlyRate;
            }
            else
            {
                basePay = (160 * this.hourlyRate) + ((this.workedHours - 160) * this.hourlyRate * 1.5);
            }

            return basePay + this.monthlyBonus;
        }

    public override string getEmployeeType()
    {
        return "Nhân sự lương theo giờ";
    }

    public override void displayPayrollInfo()
    {
        Console.WriteLine("--------------------------------------------------");
        base.displayPayrollInfo();
        Console.WriteLine($" - Đơn giá giờ: {this.hourlyRate:N0} VND/h");
        Console.WriteLine($" - Số giờ làm việc: {this.workedHours} giờ");
        if (this.workedHours > 160)
        {
            Console.WriteLine($"   + Giờ tiêu chuẩn: 160 giờ");
            Console.WriteLine($"   + Giờ vượt ngưỡng (x1.5): {this.workedHours - 160} giờ");
        }
        Console.WriteLine("--------------------------------------------------");
    }

    #endregion
}


public class SalesEmployee : Employee
{
    public double baseSalary;
    public double salesRevenue;
    public double commissionRate;

    #region Constructors
    public SalesEmployee(
        string employeeId, 
        string fullName, 
        string department, 
        double baseSalary, 
        double salesRevenue, 
        double commissionRate)
        : base(employeeId, fullName, department)
    {
        if (baseSalary < 0)
            throw new ArgumentOutOfRangeException(nameof(baseSalary), "Lương cơ bản không được âm.");

        if (commissionRate < 0 || commissionRate > 0.3)
            throw new ArgumentOutOfRangeException(nameof(commissionRate), "Tỷ lệ hoa hồng phải nằm trong khoảng từ 0 đến 0.3 (0% - 30%).");

        ValidateAndSetSalesRevenue(salesRevenue);

        this.baseSalary = baseSalary;
        this.commissionRate = commissionRate;
    }

    public SalesEmployee(string employeeId, string fullName, double baseSalary, double commissionRate)
        : this(employeeId, fullName, "Unassigned", baseSalary, 0.0, commissionRate)
    {
    }

    #endregion

    #region Public Methods
    
    public void SetSalesRevenue(double revenue)
    {
        ValidateAndSetSalesRevenue(revenue);
    }

    private void ValidateAndSetSalesRevenue(double revenue)
    {
        if (revenue < 0)
            throw new ArgumentOutOfRangeException(nameof(revenue), "Doanh số bán hàng không được âm.");

        this.salesRevenue = revenue;
    }

    #endregion

    #region Override Methods
    public override double calculateGrossPay()
    {
        return this.baseSalary + (this.salesRevenue * this.commissionRate) + this.monthlyBonus;
    }

    public override string getEmployeeType()
    {
        return "Nhân sự kinh doanh";
    }

    public override void displayPayrollInfo()
    {
        Console.WriteLine("--------------------------------------------------");
        base.displayPayrollInfo();
        Console.WriteLine($" - Lương cơ bản: {this.baseSalary:N0} VND");
        Console.WriteLine($" - Doanh số đạt: {this.salesRevenue:N0} VND");
        Console.WriteLine($" - Tỷ lệ hoa hồng: {this.commissionRate * 100}%");
        Console.WriteLine($" - Tiền hoa hồng nhận: {(this.salesRevenue * this.commissionRate):N0} VND");
        Console.WriteLine("--------------------------------------------------");
    }

    #endregion
}

public class Payroll
{
    public string period { get; private set; }
    private readonly List<Employee> _employees;

    public Payroll(string period)
    {
        if (string.IsNullOrWhiteSpace(period))
            throw new ArgumentException("Kỳ lương không được để trống.", nameof(period));

        this.period = period.Trim();
        _employees = new List<Employee>();
    }

    public bool addEmployee(Employee employee)
    {
        if (employee == null)
            throw new ArgumentNullException(nameof(employee), "Đối tượng nhân sự không thể null.");

        // Kiểm tra trùng mã nhân viên
        if (findEmployee(employee.getID()) != null)
        {
            Console.WriteLine($"Mã nhân viên '{employee.getID()}' đã tồn tại trong kỳ lương {this.period}. Bỏ qua thao tác thêm.");
            return false;
        }

        _employees.Add(employee);
        return true;
    }
    public Employee findEmployee(string employeeId)
    {
        if (string.IsNullOrWhiteSpace(employeeId))
            return null;

        // Tìm nhân viên thỏa mãn điều kiện trong list
        return _employees.FirstOrDefault(e => e.getID().Equals(employeeId.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public double calculateTotalPayroll()
    {
        if (_employees.Count == 0)
            return 0.0;

        double total = 0.0;
        foreach (var emp in _employees)
        {
            // Hàm đa hình
            total += emp.calculateGrossPay();
        }
        return total;
    }

    public double CalculatePayrollByDepartment(string department)
    {
        if (string.IsNullOrWhiteSpace(department) || _employees.Count == 0)
            return 0.0;

        double totalDept = 0.0;
        foreach (var emp in _employees)
        {   
            // Nếu tồn tại nhân viên thỏa mãn, thực hiện cộng
            if (emp.getDepartment().Equals(department.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                totalDept += emp.calculateGrossPay();
            }
        }
        return totalDept;
    }

    public Employee FindHighestPaidEmployee()
    {
        if (_employees.Count == 0)
            return null;

        Employee highestEmp = _employees[0];
        double maxGross = highestEmp.calculateGrossPay();

        for (int i = 1; i < _employees.Count; i++)
        {
            double currentGross = _employees[i].calculateGrossPay();
            if (currentGross > maxGross)
            {
                maxGross = currentGross;
                highestEmp = _employees[i];
            }
        }

        return highestEmp;
    }

    public void DisplayPayroll()
    {
        Console.WriteLine("==================================================");
        Console.WriteLine($"         BẢNG LƯƠNG KỲ: {this.period}");
        Console.WriteLine("==================================================");

        if (_employees.Count == 0)
        {
            Console.WriteLine("Danh sách nhân sự hiện đang rỗng.");
            return;
        }

        foreach (var emp in _employees)
        {
            emp.displayPayrollInfo();
        }

        Console.WriteLine("==================================================");
        Console.WriteLine($"TỔNG SỐ LƯỢNG NHÂN VIÊN: {_employees.Count}");
        Console.WriteLine($"TỔNG KINH PHÍ CHI TRẢ: {this.calculateTotalPayroll():N0} VND");
        Console.WriteLine("==================================================");
    }
}
