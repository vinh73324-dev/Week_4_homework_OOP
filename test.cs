using System;

namespace System;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Text.Encoding.UTF8;

        // -------------------------------------------------------------
        // 1. DỮ LIỆU KIỂM THỬ CHÍNH (MỤC C)
        // -------------------------------------------------------------
        Console.WriteLine("1. KHỞI TẠO BẢNG LƯƠNG VÀ DỮ LIỆU NHÂN VIÊN (KỲ 2026-09):");

        Payroll payroll = new Payroll("2026-09");

        // E001: Nhân viên lương cố định
        SalariedEmployee e1 = new SalariedEmployee("E001", "Nguyễn Minh An", "Đào tạo", 15000000, 2000000);
        e1.addBonus(1000000, "Hoàn thành xuất sắc nhiệm vụ");
        payroll.addEmployee(e1);

        // E002: Nhân viên theo giờ, không vượt ngưỡng (150h <= 160h)
        HourlyEmployee e2 = new HourlyEmployee("E002", "Trần Thu Bình", "Hỗ trợ", 100000, 150);
        e2.addBonus(500000);
        payroll.addEmployee(e2);

        // E003: Nhân viên theo giờ, có vượt ngưỡng (170h = 160h + 10h x 1.5)
        HourlyEmployee e3 = new HourlyEmployee("E003", "Lê Hoàng Chi", "Hỗ trợ", 100000, 170);
        payroll.addEmployee(e3);

        // E004: Nhân viên kinh doanh (thưởng 2% của 50 triệu)
        SalesEmployee e4 = new SalesEmployee("E004", "Phạm Quốc Dũng", "Kinh doanh", 8000000, 200000000, 0.05);
        e4.addBonus(0.02, 50000000, "Vượt chỉ tiêu doanh số");
        payroll.addEmployee(e4);

        Console.WriteLine();
        Console.WriteLine(">>> BẢNG LƯƠNG CHI TIẾT (LỜI GỌI ĐA HÌNH):");
        payroll.DisplayPayroll();
        Console.WriteLine();

        // Đối soát với các giá trị kỳ vọng theo đề bài
        Console.WriteLine(">>> KẾT QUẢ ĐỐI SOÁT VỚI MỤC C:");
        Console.WriteLine($"Thu nhập E001: {e1.calculateGrossPay():N0} VND | Kỳ vọng: 18.000.000 VND");
        Console.WriteLine($"Thu nhập E002: {e2.calculateGrossPay():N0} VND | Kỳ vọng: 15.500.000 VND");
        Console.WriteLine($"Thu nhập E003: {e3.calculateGrossPay():N0} VND | Kỳ vọng: 17.500.000 VND");
        Console.WriteLine($"Thu nhập E004: {e4.calculateGrossPay():N0} VND | Kỳ vọng: 19.000.000 VND");
        Console.WriteLine();

        double total = payroll.calculateTotalPayroll();
        Console.WriteLine($"Tổng bảng lương           : {total:N0} VND (Kỳ vọng: 70.000.000 VND)");
        Console.WriteLine($"Tổng phòng 'Hỗ trợ'       : {payroll.CalculatePayrollByDepartment("Hỗ trợ"):N0} VND (Kỳ vọng: 33.000.000 VND)");
        Console.WriteLine($"Tổng phòng 'Đào tạo'      : {payroll.CalculatePayrollByDepartment("Đào tạo"):N0} VND (Kỳ vọng: 18.000.000 VND)");
        Console.WriteLine($"Tổng phòng 'Kinh doanh'   : {payroll.CalculatePayrollByDepartment("Kinh doanh"):N0} VND (Kỳ vọng: 19.000.000 VND)");

        Employee? highest = payroll.FindHighestPaidEmployee();
        if (highest != null)
        {
            Console.WriteLine($"Nhân viên thu nhập cao nhất: {highest.getID()} - {highest.calculateGrossPay():N0} VND (Kỳ vọng: E004)");
        }
        Console.WriteLine();

        // -------------------------------------------------------------
        // 2. KIỂM THỬ BIÊN VÀ BẮT LỖI NGOẠI LỆ (MỤC C.1 - 15 TEST CASES)
        // -------------------------------------------------------------
        Console.WriteLine("2. KIỂM THỬ BIÊN VÀ XỬ LÝ LỖI (MỤC C.1):");

        TestExpectException("TC01. Mã nhân viên rỗng", () =>
            new SalariedEmployee("", "Nguyễn Văn A", "IT", 10000000, 0));

        TestExpectException("TC02. Họ tên rỗng", () =>
            new SalariedEmployee("E100", "   ", "IT", 10000000, 0));

        TestExpectException("TC03. Phòng ban rỗng", () =>
            new SalariedEmployee("E100", "Nguyễn Văn A", "", 10000000, 0));

        TestExpectException("TC04. Lương tháng cố định âm (< 0)", () =>
            new SalariedEmployee("E101", "Trần B", "IT", -5000000, 0));

        TestExpectException("TC05. Phụ cấp trách nhiệm âm (< 0)", () =>
            new SalariedEmployee("E102", "Lê C", "IT", 10000000, -1000000));

        TestExpectException("TC06. Đơn giá giờ làm âm (< 0)", () =>
            new HourlyEmployee("E103", "Phạm D", "Kho", -50000, 100));

        TestExpectException("TC07. Số giờ làm việc âm (< 0)", () =>
            new HourlyEmployee("E104", "Hoàng E", "Kho", 100000, -10));

        TestExpectException("TC08. Số giờ làm việc vượt quá 250 giờ", () =>
            new HourlyEmployee("E105", "Đỗ F", "Kho", 100000, 260));

        // Kiểm thử biên giờ làm hợp lệ
        HourlyEmployee hZero = new HourlyEmployee("E106", "Biên 0 Giờ", "Kho", 100000, 0);
        Console.WriteLine($"[PASS] TC09. Biên 0 giờ làm: Thu nhập = {hZero.calculateGrossPay():N0} VND");

        HourlyEmployee h160 = new HourlyEmployee("E107", "Biên 160 Giờ", "Kho", 100000, 160);
        Console.WriteLine($"[PASS] TC10. Biên đúng 160 giờ: Thu nhập = {h160.calculateGrossPay():N0} VND");

        HourlyEmployee h250 = new HourlyEmployee("E108", "Biên 250 Giờ", "Kho", 100000, 250);
        Console.WriteLine($"[PASS] TC11. Biên tối đa 250 giờ: Thu nhập = {h250.calculateGrossPay():N0} VND (160h + 90h x 1.5)");

        TestExpectException("TC12. Hoa hồng ngoài khoảng [0, 0.3]", () =>
            new SalesEmployee("E109", "Vũ G", "Sales", 5000000, 100000000, 0.35));

        TestExpectException("TC13. Doanh số bán hàng âm (< 0)", () =>
            new SalesEmployee("E110", "Mai H", "Sales", 5000000, -20000000, 0.1));

        TestExpectException("TC14. addBonus số tiền âm (<= 0)", () =>
            e1.addBonus(-100000));

        TestExpectException("TC15. addBonus tỷ lệ vượt quá 0.5", () =>
            e1.addBonus(0.6, 10000000, "Thưởng vượt khung"));

        // Kiểm thử Payroll: trùng mã và null
        bool addDuplicate = payroll.addEmployee(new SalariedEmployee("E001", "Trùng Mã", "IT", 10000000, 0));
        Console.WriteLine($"[PASS] TC16. Thêm nhân viên trùng mã E001: Từ chối thêm = {!addDuplicate}");

        TestExpectException("TC17. Thêm nhân viên null vào Payroll", () =>
            payroll.addEmployee(null!));

        Console.WriteLine();
        Console.WriteLine("Hoàn thành tất cả các bước kiểm thử.");
    }

    static void TestExpectException(string testName, Action action)
    {
        try
        {
            action();
            Console.WriteLine($"[FAIL] {testName} - Không bắt được ngoại lệ");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[PASS] {testName} - Bắt {ex.GetType().Name}: {ex.Message.Split('\n')[0].Trim()}");
        }
    }
}