using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CRMSite.Common
{
    //Hungvx
    public class EnumSystem
    {
        public enum InvestorStatus
        {
            [Description("Đang sử dụng")]
            DangSuDung = 1,
            [Description("Tạm Ngưng")]
            TamNgung = 2,
        }
        public enum AllowanceOrDeductType
        {
            [Description("Các loại thưởng thâm liên")]
            Cac_Loai_Thuong_Tham_nien = 1,
            [Description("Thưởng chuyên cần")]
            Thuong_Chuyen_Can = 2,
            [Description("BHXH")]
            BHXH = 3,
            [Description("Phí công đoàn")]
            Phi_Cong_Doan = 4,
            [Description("Xăng xe")]
            Xang_xe = 5
        }
        public enum AllowanceOrDeductStatus
        {
            [Description("Đang sử dụng")]
            Dang_su_dung = 1,
            [Description("Không còn sử dụng")]
            Khong_Con_Su_Dung = 0
        }
        public enum AllowanceOrDeductCalculation
        {
            [Description("Tính bằng tiền")]
            Tinh_Bang_Tien = 1,
            [Description("Tính bằng phần trăm")]
            Tinh_Bang_Phan_Tram = 0
        }
        public enum AllowanceOrDeductUpOrdown
        {
            [Description("Cộng vào thu nhập hàng tháng")]
            Tinh_Bang_Tien = 1,
            [Description("Trừ vào thu nhập hàng tháng")]
            Tinh_Bang_Phan_Tram = 0
        }
        public enum ProductStatus
        {
            [Description("Sản phẩm đã ngừng hoạt động")]
            San_Pham_Ngung_Hoat_Dong = 0,
            [Description("Sản phẩm đã xây dựng xong, đang hoạt động")]
            San_Pham_XD_Xong = 1,
            [Description("Sản phẩm đang trong quá trình xây dựng")]
            SP_Dang_XD = 2
        }
    }
}
